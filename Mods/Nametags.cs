/*
 * UA Mod Menu Mods/Nametags.cs
 * 
 * Copyright (C) 2026 vanvani41
 * https://github.com/vanvani41/UA.Mod.Menu
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://gnu.org>.
*/

using StupidTemplate.Mods.Settings;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static Modio.API.ModioAPI;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace StupidTemplate.Mods
{
    public class Nametags : MonoBehaviour
    {
        public static bool nameTags = false;
        public static bool idTags = false;
        public static bool forceFace = false; // новий флаг

        private static readonly Dictionary<TMP_Text, OriginalNametagData> originalData = new Dictionary<TMP_Text, OriginalNametagData>();

        private struct OriginalNametagData
        {
            public string text;
            public float fontSize;
            public Vector3 localPosition;
            public Vector3 localScale;
            public TextAlignmentOptions alignment;
            public Quaternion localRotation; // зберігаємо оригінальний rotation
        }

        private static void SaveOriginal(TMP_Text tmp)
        {
            if (!originalData.ContainsKey(tmp))
            {
                originalData[tmp] = new OriginalNametagData
                {
                    text = tmp.text,
                    fontSize = tmp.fontSize,
                    localPosition = tmp.transform.localPosition,
                    localScale = tmp.transform.localScale,
                    alignment = tmp.alignment,
                    localRotation = tmp.transform.localRotation, // зберігаємо rotation
                };
            }
        }

        private static void RestoreOriginal(TMP_Text tmp)
        {
            if (originalData.TryGetValue(tmp, out var data))
            {
                tmp.fontSize = data.fontSize;
                tmp.transform.localPosition = data.localPosition;
                tmp.transform.localScale = data.localScale;
                tmp.transform.localRotation = data.localRotation; // відновлюємо rotation
                tmp.alignment = data.alignment;
                tmp.color = Color.white;
                originalData.Remove(tmp);
            }
        }

        public static void EnableNameTags()
        {
            nameTags = true;
        }

        public static void DisableNameTags()
        {
            nameTags = false;
            forceFace = false; // вирубаємо forceFace разом з nameTags

            if (!nameTags && !idTags)
                RestoreAllNametags();
        }

        public static void EnableIdTags()
        {
            idTags = true;
        }

        public static void DisableIdTags()
        {
            idTags = false;
            if (!nameTags && !idTags)
                RestoreAllNametags();
        }

        // ── FORCE FACE ──────────────────────────────────────────────
        public static void EnableForceFace()
        {
            forceFace = true;
        }

        public static void DisableForceFace()
        {
            forceFace = false;

            // Відновлюємо rotation у всіх тегів
            if (GorillaParent.instance == null) return;
            if (VRRigCache.ActiveRigs == null) return;

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isOfflineVRRig) continue;
                if (rig.playerText1 == null) continue;

                if (originalData.TryGetValue(rig.playerText1, out var data))
                {
                    rig.playerText1.transform.localRotation = data.localRotation;
                }
            }
        }

        // Змушує конкретний nametag дивитися на камеру
        public static void ForceNametagFaceCamera(TMP_Text tmp)
        {
            if (tmp == null) return;

            Camera cam = Camera.main;
            if (cam == null) return;

            Vector3 direction = tmp.transform.position - cam.transform.position;

            if (direction != Vector3.zero)
                tmp.transform.rotation = Quaternion.LookRotation(direction);
        }
        // ────────────────────────────────────────────────────────────

        private static void RestoreAllNametags()
        {
            if (GorillaParent.instance == null) return;
            if (VRRigCache.ActiveRigs == null) return;

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isOfflineVRRig) continue;
                if (rig.playerText1 == null) continue;

                RestoreOriginal(rig.playerText1);
            }
        }

        public static void RunNametags()
        {
            if (!nameTags && !idTags && !forceFace) return;
            if (GorillaParent.instance == null) return;
            if (VRRigCache.ActiveRigs == null) return;

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isOfflineVRRig) continue;
                if (rig.playerText1 == null) continue;

                if (nameTags || idTags)
                    UpdateNameOnly(rig);

                if (forceFace)
                    ForceNametagFaceCamera(rig.playerText1); // крутимо до камери
            }
        }

        public static HashSet<string> modUsers = new HashSet<string>();

        static string RainbowText()
        {
            float h = (Time.time * 0.5f) % 1f;
            Color color = Color.HSVToRGB(h, 1f, 1f);
            return ColorUtility.ToHtmlStringRGB(color);
        }

        public static void UpdateNameOnly(VRRig rig)
        {
            try
            {
                TMP_Text tmp = rig.playerText1;
                if (tmp == null) return;

                SaveOriginal(tmp);

                string nameColor =
                    rig.mainSkin != null &&
                    rig.mainSkin.material != null &&
                    rig.mainSkin.material.name.Contains("fected")
                        ? "FF8000"
                        : ColorUtility.ToHtmlStringRGB(rig.playerColor);

                if (nameColor == "000000")
                    nameColor = "FFFFFF";

                string nick = (rig.Creator != null && !string.IsNullOrEmpty(rig.Creator.NickName))
                    ? rig.Creator.NickName
                    : "PLAYER";
                nick = Regex.Replace(nick, "<.*?>", string.Empty);

                string prefix = "";
                string suffix = "";
                string uppix = "";

                if (rig.Creator != null)
                {
                    if (rig.Creator.UserId == "C686727BCD7F2D8E")
                        prefix = "<color=yellow>[OWNER] (Steam)</color>";
                    if (rig.Creator.UserId == "8F406DB4A6CC20B0")
                        prefix = "<color=yellow>[OWNER] (Quest)</color>";
                }

                if (idTags && rig.Creator != null)
                    uppix = $"<color=white><size=0.8>{rig.Creator.UserId}</size></color>\n";

                if (nameTags)
                    tmp.text = $"{uppix}{prefix} <color=#{nameColor}>{nick}</color> {suffix}";
                else if (idTags)
                    tmp.text = $"<color=white><size=0.8>{rig.Creator?.UserId ?? "?"}</size></color>";

                tmp.alignment = TextAlignmentOptions.Center;
                tmp.fontSize = Settings.Nametags.nametagssize;
                tmp.transform.localScale = Vector3.one;
                tmp.transform.localPosition = new Vector3(0f, 0.8f, -0.02f);

                string modTag = "";
                if (!rig.enabled && rig.transform.position.y > 5000f)
                    modTag = "\n<color=#8300ff>GHOST</color> <color=white>INVIS</color>";
                else if (!rig.enabled)
                    modTag = "\n<color=#8300ff>GHOST</color>";
                else if (rig.transform.position.y > 5000f)
                    modTag = "\n<color=white>INVIS</color>";

                string uaTag = (rig.Creator != null && modUsers.Contains(rig.Creator.UserId))
                    ? $"\n<color=#{RainbowText()}>HAS UA MOD MENU</color>"
                    : "";

                if (nameTags)
                    tmp.text = $"{uppix}{prefix} <color=#{nameColor}>{nick}</color>{modTag}{uaTag} {suffix}";
                else if (idTags)
                    tmp.text = $"<color=white><size=0.8>{rig.Creator?.UserId ?? "?"}</size></color>{modTag}{uaTag}";
            }
            catch { }
        }
    }
}