using HarmonyLib;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StupidTemplate.Patches.Internal
{
    public class AntiStacktrace
    {
        [HarmonyPatch(typeof(StackTrace), "ToString")]
        public class HideModCalls
        {
            private static void Postfix(StackTrace __instance, ref string __result)
            {
                if (__result != null)
                {
                    var lines = __result.Split('\n')
                        .Where(l => !l.Contains("StupidTemplate") &&
                                    !l.Contains("UA.Mod.Menu") &&
                                    !l.Contains("Harmony") &&
                                    !l.Contains("MonoMod"))
                        .ToArray();

                    if (lines.Length > 0)
                        __result = string.Join("\n", lines);
                    else
                        __result = "at System.Environment.GetStackTrace(Exception e)";
                }
            }
        }

        [HarmonyPatch(typeof(StackFrame), "GetMethod")]
        public class HideMethodInfo
        {
            private static void Postfix(StackFrame __instance, ref MethodBase __result)
            {
                if (__result != null)
                {
                    var declaringType = __result.DeclaringType;
                    if (declaringType != null &&
                        (declaringType.Namespace != null &&
                         (declaringType.Namespace.Contains("StupidTemplate") ||
                          declaringType.Namespace.Contains("UA.Mod"))))
                    {
                        __result = null;
                    }
                }
            }
        }
    }
}