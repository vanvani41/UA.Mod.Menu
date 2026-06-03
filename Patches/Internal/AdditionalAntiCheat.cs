using GorillaNetworking;
using HarmonyLib;
using UnityEngine;

namespace StupidTemplate.Patches.Internal
{
    public class AdditionalAntiCheat
    {
        // ─── GorillaScoreBoard — репорти ───
        [HarmonyPatch(typeof(GorillaScoreBoard), "ReportPlayer")]
        public class NoScoreboardReport
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(GorillaScoreBoard), "RPC_ReportPlayer")]
        public class NoRPCScoreboardReport
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(GorillaScoreBoard), "ReportScore")]
        public class NoScoreboardReportScore
        {
            private static bool Prefix() => false;
        }

        // ─── Mothership — бан-дані ───
        [HarmonyPatch(typeof(MothershipBanData), "CheckIfBanned")]
        public class NoMothershipBanCheck
        {
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(MothershipBanData), "IsBanned", MethodType.Getter)]
        public class NoMothershipIsBanned
        {
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }

        // ─── ListBansBulkRequest — батчеві бан-запити ───
        [HarmonyPatch(typeof(ListBansBulkRequest), "CheckBanStatus")]
        public class NoListBansBulkCheck
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(ListBansBulkRequest), "SendBanRequest")]
        public class NoListBansBulkSend
        {
            private static bool Prefix() => false;
        }

        // ─── GTSceneUtils — перевірка сцени ───
        [HarmonyPatch(typeof(GTSceneUtils), "LogSceneViolation")]
        public class NoSceneViolationLog
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(GTSceneUtils), "ValidateSceneIntegrity")]
        public class NoSceneIntegrityCheck
        {
            private static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        // ─── GTUberShaderUtils — перевірка шейдерів ───
        [HarmonyPatch(typeof(GTUberShaderUtils), "ValidateShaders")]
        public class NoShaderValidation
        {
            private static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(GTUberShaderUtils), "DetectModifiedShaders")]
        public class NoModifiedShaderDetect
        {
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }

        // ─── GRUtils — загальні перевірки цілісності ───
        [HarmonyPatch(typeof(GRUtils), "CheckForModifications")]
        public class NoModificationCheck
        {
            private static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }

        [HarmonyPatch(typeof(GRUtils), "LogSuspiciousActivity")]
        public class NoSuspiciousActivityLog
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(GRUtils), "VerifyGameIntegrity")]
        public class NoGameIntegrityCheck
        {
            private static bool Prefix(ref bool __result)
            {
                __result = true;
                return false;
            }
        }

        // ─── GorillaComputer — бан-скрін та перевірки ───
        [HarmonyPatch(typeof(GorillaComputer), "CheckBanStatus")]
        public class NoComputerBanCheck
        {
            private static bool Prefix() => false;
        }

        [HarmonyPatch(typeof(GorillaComputer), "ShowBanScreen")]
        public class NoBanScreen
        {
            private static bool Prefix() => false;
        }
    }
}