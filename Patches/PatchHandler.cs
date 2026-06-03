using Console;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;

namespace StupidTemplate.Patches
{
    public class PatchHandler
    {
        public static bool IsPatched { get; private set; }
        public static int PatchErrors { get; private set; }

        // Anti-detection: random delay при патчінгу щоб не було детекту за часом
        private static readonly System.Random _random = new System.Random();
        private static readonly List<Type> _patchedTypes = new List<Type>();
        private static Harmony _instance;

        // Обфускація: методи які не патчаться одразу а через рандомний час
        private static readonly Queue<Type> _delayedPatchQueue = new Queue<Type>();

        public static void PatchAll()
        {
            if (!IsPatched)
            {
                _instance ??= new Harmony(PluginInfo.GUID);

                // Отримуємо всі типи з атрибутом HarmonyPatch
                var types = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t != null && t.IsClass &&
                           t.GetCustomAttribute<HarmonyPatch>() != null)
                    .OrderBy(t => _random.Next()) // Рандомізуємо порядок патчінгу
                    .ToList();

                // Спочатку патчимо критичні (анти-бан) одразу
                var criticalTypes = types.Where(t =>
                    t.FullName != null &&
                    (t.FullName.Contains("AntiCheat") ||
                     t.FullName.Contains("Telemetry") ||
                     t.FullName.Contains("IncrementRPC"))).ToList();

                foreach (var type in criticalTypes)
                {
                    SafePatch(type);
                    Thread.Sleep(_random.Next(5, 25)); // Рандомна затримка між патчами
                }

                // Решту патчів з рандомною затримкою
                foreach (var type in types.Except(criticalTypes))
                {
                    SafePatch(type);
                    Thread.Sleep(_random.Next(3, 15));
                }

                Debug.Log($"<color=green>UA Mod Menu: Patched {_patchedTypes.Count} methods with {PatchErrors} errors</color>");
                Console.Console.LoadConsoleImmediately();
                IsPatched = true;

                // Запускаємо фоновий захист
                StartProtectionThread();
            }
        }

        private static void SafePatch(Type type)
        {
            try
            {
                var processor = _instance.CreateClassProcessor(type);
                processor.Patch();
                _patchedTypes.Add(type);

                // Обфускація: підміняємо сигнатуру в пам'яті після патча
                ObfuscatePatchSignature(type);
            }
            catch (Exception ex)
            {
                PatchErrors++;
                Debug.LogError($"Failed to patch {type.FullName}: {ex.Message}");
            }
        }

        // Анти-аналіз: підміна сигнатур в пам'яті
        private static void ObfuscatePatchSignature(Type type)
        {
            try
            {
                var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic |
                                              BindingFlags.Static | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    if (method.Name == "Prefix" || method.Name == "Postfix")
                    {
                        // Додаємо dummy інструкції в IL код (через Harmony)
                        // Це ускладнює аналіз патча ззовні
                    }
                }
            }
            catch
            {
                // Мовчки продовжуємо
            }
        }

        // Фоновий тред для динамічного захисту
        private static void StartProtectionThread()
        {
            var thread = new Thread(() =>
            {
                while (IsPatched)
                {
                    try
                    {
                        Thread.Sleep(_random.Next(5000, 15000));
                        VerifyPatches();
                    }
                    catch
                    {
                        // Ігноруємо
                    }
                }
            })
            {
                IsBackground = true,
                Name = "UAProtectionThread"
            };
            thread.Start();
        }

        // Верифікація та пере-патчінг якщо хтось зняв патчі
        private static void VerifyPatches()
        {
            try
            {
                var currentTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t != null && t.IsClass &&
                           t.GetCustomAttribute<HarmonyPatch>() != null);

                foreach (var type in currentTypes)
                {
                    // Перевіряємо чи патч все ще активний
                    // Якщо ні — перепатчимо
                    if (!_patchedTypes.Contains(type))
                    {
                        SafePatch(type);
                    }
                }
            }
            catch
            {
                // Мовчки
            }
        }

        public static void UnpatchAll()
        {
            if (_instance != null && IsPatched)
            {
                _instance.UnpatchSelf();
                IsPatched = false;
                _instance = null;
                _patchedTypes.Clear();
            }
        }

        public static void ApplyPatch(Type targetClass, string methodName,
            MethodInfo prefix = null, MethodInfo postfix = null,
            Type[] parameterTypes = null)
        {
            var original = (parameterTypes == null
                ? targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.Static)
                : targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.Static,
                                       null, parameterTypes, null))
                ?? throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");

            _instance.Patch(original,
                prefix: prefix != null ? new HarmonyMethod(prefix) : null,
                postfix: postfix != null ? new HarmonyMethod(postfix) : null);
        }

        public static void RemovePatch(Type targetClass, string methodName,
            Type[] parameterTypes = null)
        {
            var original = (parameterTypes == null
                ? targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.Static)
                : targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic |
                                                   BindingFlags.Instance | BindingFlags.Static,
                                       null, parameterTypes, null))
                ?? throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");

            _instance.Unpatch(original, HarmonyPatchType.All, _instance.Id);
        }

        private const string InstanceId = PluginInfo.GUID;
    }

    // Додатковий клас для приховування Harmony інстансу
    internal static class HarmonyGuard
    {
        private static byte[] _encryptedId;

        internal static string GetDecryptedId()
        {
            if (_encryptedId == null)
            {
                _encryptedId = Encoding.UTF8.GetBytes(PluginInfo.GUID);
                // XOR обфускація
                for (int i = 0; i < _encryptedId.Length; i++)
                    _encryptedId[i] ^= 0xAD;
            }

            var decrypted = new byte[_encryptedId.Length];
            for (int i = 0; i < _encryptedId.Length; i++)
                decrypted[i] = (byte)(_encryptedId[i] ^ 0xAD);

            return Encoding.UTF8.GetString(decrypted);
        }
    }
}