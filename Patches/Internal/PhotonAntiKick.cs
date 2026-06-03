using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace StupidTemplate.Patches.Internal
{
    // Максимальний захист від Photon банів/кіків
    public class PhotonAntiKick
    {
        // Блокуємо будь-які методи які можуть викинути з кімнати
        [HarmonyPatch(typeof(PhotonNetwork), "CloseConnection")]
        public class NoCloseConnection
        {
            private static bool Prefix(Player player) => false;
        }

        [HarmonyPatch(typeof(PhotonNetwork), "Disconnect")]
        public class NoDisconnect
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(PhotonNetwork), "KickPlayer")]
        public class NoKickPlayer
        {
            private static bool Prefix(Player player) => false;
        }

        [HarmonyPatch(typeof(LoadBalancingClient), "OnEvent")]
        public class AntiKickEvent
        {
            private static bool Prefix(LoadBalancingClient __instance, EventData photonEvent)
            {
                // Блокуємо події кіка
                if (photonEvent.Code == 203 || photonEvent.Code == 204)
                    return false;
                return true;
            }
        }

        [HarmonyPatch(typeof(PhotonNetwork), "NetworkStatisticsReset")]
        public class NoStatReset
        {
            private static bool Prefix() => false;
        }

        // Маскування пінгу та пакет-логу для уникнення детекту
        [HarmonyPatch(typeof(PhotonNetwork), "Ping", MethodType.Getter)]
        public class FakedPing
        {
            private static bool Prefix(ref int __result)
            {
                __result = Random.Range(25, 45);
                return false;
            }
        }
    }
}