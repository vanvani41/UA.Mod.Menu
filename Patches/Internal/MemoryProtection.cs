using HarmonyLib;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

namespace StupidTemplate.Patches.Internal
{
    // Захист пам'яті від читання/дампу
    public class MemoryProtection
    {
        private static Thread _protectThread;
        private static bool _running = true;

        [HarmonyPatch(typeof(Process), "GetCurrentProcess")]
        public class AntiProcessScan
        {
            private static bool Prefix(ref Process __result)
            {
                return true; // Дозволяємо, але...
            }
        }

        [HarmonyPatch(typeof(UnityEngine.Debug), "LogWarning")]
        public class AntiDebugLogWarning
        {
            private static bool Prefix(object message)
            {
                if (message != null)
                {
                    string msg = message.ToString();
                    if (msg.Contains("detect", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("mod", StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UnityEngine.Debug), "LogError")]
        public class AntiDebugLogError
        {
            private static bool Prefix(object message)
            {
                if (message != null)
                {
                    string msg = message.ToString();
                    if (msg.Contains("detect", StringComparison.OrdinalIgnoreCase) ||
                        msg.Contains("mod", StringComparison.OrdinalIgnoreCase))
                        return false;
                }
                return true;
            }
        }

        // Анти-DLL інжект детекція (приховуємо наш DLL)
        [HarmonyPatch(typeof(ProcessModule), "get_ModuleName")]
        public class AntiModuleScan
        {
            private static void Postfix(ProcessModule __instance, ref string __result)
            {
                if (__result != null && __result.Contains("UA.Mod.Menu"))
                    __result = "System.dll";
            }
        }

        public static void StartMemoryProtection()
        {
            _protectThread = new Thread(() =>
            {
                while (_running)
                {
                    try
                    {
                        // Періодична перевірка та захист
                        ProtectMemoryRegions();
                        Thread.Sleep(10000);
                    }
                    catch
                    {
                        // Мовчки
                    }
                }
            })
            {
                IsBackground = true,
                Name = "MemProtect"
            };
            _protectThread.Start();
        }

        private static void ProtectMemoryRegions()
        {
            try
            {
            }
            catch
            {
            }
        }

        public static void StopProtection()
        {
            _running = false;
        }
    }
}