/*
 * UA Mod Menu Mods/Visual.cs
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

using static StupidTemplate.Menu.Main;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace StupidTemplate.Mods
{
    public class Visual
    {
        // ESP
        private static readonly Dictionary<VRRig, (LineRenderer line, LineRenderer dot)> espPool =
            new Dictionary<VRRig, (LineRenderer, LineRenderer)>();
        private static readonly Dictionary<VRRig, LineRenderer> boxEspPool = new Dictionary<VRRig, LineRenderer>();
        private static readonly Dictionary<VRRig, LineRenderer> boneEspPool = new Dictionary<VRRig, LineRenderer>();
        private static readonly Dictionary<VRRig, LineRenderer> headEspPool = new Dictionary<VRRig, LineRenderer>();
        private static readonly Dictionary<VRRig, TextMeshPro> distanceEspPool = new Dictionary<VRRig, TextMeshPro>();

        public static void RunESP()
        {
            if (VRRigCache.ActiveRigs == null) return;

            HashSet<VRRig> seen = new HashSet<VRRig>();

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isLocal || rig.isOfflineVRRig) continue;
                seen.Add(rig);

                if (!espPool.TryGetValue(rig, out var obj))
                {
                    GameObject lineObj = new GameObject("ESP_Line");
                    LineRenderer lr = lineObj.AddComponent<LineRenderer>();
                    lr.material = new Material(Shader.Find("GorillaTag/UberShader"));
                    lr.startWidth = 0.02f;
                    lr.endWidth = 0.02f;
                    lr.positionCount = 2;
                    lr.useWorldSpace = true;

                    GameObject dotObj = new GameObject("ESP_Dot");
                    LineRenderer dot = dotObj.AddComponent<LineRenderer>();
                    dot.material = new Material(Shader.Find("GorillaTag/UberShader"));
                    dot.startWidth = 0.1f;
                    dot.endWidth = 0.1f;
                    dot.positionCount = 2;
                    dot.useWorldSpace = true;

                    obj = (lr, dot);
                    espPool[rig] = obj;
                }

                Vector3 head = GorillaTagger.Instance.headCollider.transform.position;
                Vector3 target = rig.transform.position + Vector3.up * 1.5f;
                float dist = Vector3.Distance(head, target);

                Color c = dist < 5f ? Color.red
                        : dist < 15f ? Color.yellow
                        : Color.green;

                obj.line.startColor = c;
                obj.line.endColor = c;
                obj.line.SetPosition(0, head);
                obj.line.SetPosition(1, target);

                obj.dot.startColor = c;
                obj.dot.endColor = c;
                obj.dot.SetPosition(0, target);
                obj.dot.SetPosition(1, target + Vector3.up * 0.3f);
            }

            List<VRRig> toRemove = new List<VRRig>();
            foreach (var kvp in espPool)
            {
                if (seen.Contains(kvp.Key)) continue;
                Object.Destroy(kvp.Value.line.gameObject);
                Object.Destroy(kvp.Value.dot.gameObject);
                toRemove.Add(kvp.Key);
            }
            foreach (var r in toRemove)
                espPool.Remove(r);
        }

        public static void DisableESP()
        {
            foreach (var kvp in espPool)
            {
                Object.Destroy(kvp.Value.line.gameObject);
                Object.Destroy(kvp.Value.dot.gameObject);
            }
            espPool.Clear();
        }

        public static void RunBoxESP()
        {
            RunLineEsp(boxEspPool, "Box_ESP", 0.025f, 5, (rig, line, color) =>
            {
                Vector3 center = rig.transform.position + Vector3.up * 1.0f;
                Camera cam = Camera.main;
                Vector3 right = cam != null ? cam.transform.right : Vector3.right;
                Vector3 up = Vector3.up;
                float width = 0.65f;
                float height = 1.45f;

                line.startColor = color;
                line.endColor = color;
                line.SetPosition(0, center + up * height * 0.5f - right * width * 0.5f);
                line.SetPosition(1, center + up * height * 0.5f + right * width * 0.5f);
                line.SetPosition(2, center - up * height * 0.5f + right * width * 0.5f);
                line.SetPosition(3, center - up * height * 0.5f - right * width * 0.5f);
                line.SetPosition(4, center + up * height * 0.5f - right * width * 0.5f);
            });
        }

        public static void DisableBoxESP() =>
            ClearLineEsp(boxEspPool);

        public static void RunBoneESP()
        {
            RunLineEsp(boneEspPool, "Bone_ESP", 0.025f, 9, (rig, line, color) =>
            {
                Vector3 root = rig.transform.position;
                Vector3 right = rig.transform.right;
                Vector3 head = root + Vector3.up * 1.55f;
                Vector3 chest = root + Vector3.up * 1.15f;
                Vector3 hips = root + Vector3.up * 0.75f;
                Vector3 leftHand = chest - right * 0.55f;
                Vector3 rightHand = chest + right * 0.55f;
                Vector3 leftFoot = root - right * 0.25f;
                Vector3 rightFoot = root + right * 0.25f;

                line.startColor = color;
                line.endColor = color;
                line.SetPosition(0, leftHand);
                line.SetPosition(1, chest);
                line.SetPosition(2, head);
                line.SetPosition(3, chest);
                line.SetPosition(4, rightHand);
                line.SetPosition(5, chest);
                line.SetPosition(6, hips);
                line.SetPosition(7, leftFoot);
                line.SetPosition(8, rightFoot);
            });
        }

        public static void DisableBoneESP() =>
            ClearLineEsp(boneEspPool);

        public static void RunHeadESP()
        {
            RunLineEsp(headEspPool, "Head_ESP", 0.025f, 25, (rig, line, color) =>
            {
                Vector3 center = rig.transform.position + Vector3.up * 1.55f;
                Camera cam = Camera.main;
                Vector3 right = cam != null ? cam.transform.right : Vector3.right;
                Vector3 up = cam != null ? cam.transform.up : Vector3.up;
                float radius = 0.22f;

                line.startColor = color;
                line.endColor = color;
                for (int i = 0; i < line.positionCount; i++)
                {
                    float angle = (Mathf.PI * 2f * i) / (line.positionCount - 1);
                    Vector3 position = center + right * Mathf.Cos(angle) * radius + up * Mathf.Sin(angle) * radius;
                    line.SetPosition(i, position);
                }
            });
        }

        public static void DisableHeadESP() =>
            ClearLineEsp(headEspPool);

        public static void RunDistanceESP()
        {
            if (VRRigCache.ActiveRigs == null || GorillaTagger.Instance == null) return;

            HashSet<VRRig> seen = new HashSet<VRRig>();
            Vector3 head = GorillaTagger.Instance.headCollider.transform.position;
            Camera cam = Camera.main;

            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isLocal || rig.isOfflineVRRig) continue;
                seen.Add(rig);

                if (!distanceEspPool.TryGetValue(rig, out TextMeshPro text) || text == null)
                {
                    GameObject obj = new GameObject("Distance_ESP");
                    text = obj.AddComponent<TextMeshPro>();
                    text.alignment = TextAlignmentOptions.Center;
                    text.fontSize = 2.5f;
                    text.color = Color.white;
                    distanceEspPool[rig] = text;
                }

                Vector3 target = rig.transform.position + Vector3.up * 1.9f;
                float distance = Vector3.Distance(head, rig.transform.position);
                text.text = $"{distance:F1}m";
                text.transform.position = target;
                text.transform.localScale = Vector3.one * 0.18f;

                if (cam != null)
                    text.transform.rotation = Quaternion.LookRotation(text.transform.position - cam.transform.position);
            }

            List<VRRig> toRemove = new List<VRRig>();
            foreach (var kvp in distanceEspPool)
            {
                if (seen.Contains(kvp.Key)) continue;
                if (kvp.Value != null) Object.Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
            foreach (VRRig rig in toRemove)
                distanceEspPool.Remove(rig);
        }

        public static void DisableDistanceESP()
        {
            foreach (var kvp in distanceEspPool)
                if (kvp.Value != null) Object.Destroy(kvp.Value.gameObject);

            distanceEspPool.Clear();
        }

        private static void RunLineEsp(Dictionary<VRRig, LineRenderer> pool, string objectName, float width, int positionCount, System.Action<VRRig, LineRenderer, Color> updateLine)
        {
            if (VRRigCache.ActiveRigs == null) return;

            HashSet<VRRig> seen = new HashSet<VRRig>();
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isLocal || rig.isOfflineVRRig) continue;
                seen.Add(rig);

                if (!pool.TryGetValue(rig, out LineRenderer line) || line == null)
                {
                    GameObject obj = new GameObject(objectName);
                    line = obj.AddComponent<LineRenderer>();
                    line.material = CreateEspMaterial();
                    line.startWidth = width;
                    line.endWidth = width;
                    line.useWorldSpace = true;
                    pool[rig] = line;
                }

                line.positionCount = positionCount;
                updateLine(rig, line, GetEspColor(rig));
            }

            List<VRRig> toRemove = new List<VRRig>();
            foreach (var kvp in pool)
            {
                if (seen.Contains(kvp.Key)) continue;
                if (kvp.Value != null) Object.Destroy(kvp.Value.gameObject);
                toRemove.Add(kvp.Key);
            }
            foreach (VRRig rig in toRemove)
                pool.Remove(rig);
        }

        private static void ClearLineEsp(Dictionary<VRRig, LineRenderer> pool)
        {
            foreach (var kvp in pool)
                if (kvp.Value != null) Object.Destroy(kvp.Value.gameObject);

            pool.Clear();
        }

        private static Material CreateEspMaterial()
        {
            Shader shader = Shader.Find("GorillaTag/UberShader") ?? Shader.Find("GUI/Text Shader");
            return shader != null ? new Material(shader) : null;
        }

        private static Color GetEspColor(VRRig rig)
        {
            if (GorillaTagger.Instance == null || GorillaTagger.Instance.headCollider == null)
                return Color.white;

            float dist = Vector3.Distance(GorillaTagger.Instance.headCollider.transform.position, rig.transform.position);
            return dist < 5f ? Color.red
                : dist < 15f ? Color.yellow
                : Color.green;
        }

        // Trail
        public static GameObject LeftTrail;
        public static GameObject RightTrail;
        private static readonly Color trailColor = new Color32(0x83, 0x00, 0xFF, 0xFF);
        public static void EnableTrail()
        {
            if (!TryGetTrailHands(out Vector3 leftPosition, out Vector3 rightPosition))
                return;

            if (LeftTrail == null)
                LeftTrail = CreateTrail(leftPosition);

            if (RightTrail == null)
                RightTrail = CreateTrail(rightPosition);
        }

        public static void DisableTrail()
        {
            if (LeftTrail != null) Object.Destroy(LeftTrail);
            if (RightTrail != null) Object.Destroy(RightTrail);

            LeftTrail = null;
            RightTrail = null;
        }

        public static void RunTrail()
        {
            if (!TryGetTrailHands(out Vector3 leftPosition, out Vector3 rightPosition))
                return;

            if (LeftTrail == null || RightTrail == null)
                EnableTrail();

            if (LeftTrail == null || RightTrail == null)
                return;

            LeftTrail.transform.position = leftPosition;
            RightTrail.transform.position = rightPosition;
        }

        private static GameObject CreateTrail(Vector3 startPosition)
        {
            GameObject obj = new GameObject("UA_Mod_Menu_Trail");
            obj.transform.position = startPosition;

            TrailRenderer trail = obj.AddComponent<TrailRenderer>();
            Shader shader = Shader.Find("GorillaTag/UberShader") ?? Shader.Find("GUI/Text Shader");

            if (shader != null)
            {
                trail.material = new Material(shader);
                trail.material.color = trailColor;
            }
            trail.startWidth = 0.045f;
            trail.endWidth = 0f;
            trail.time = 0.75f;
            trail.minVertexDistance = 0.01f;
            trail.numCornerVertices = 8;
            trail.numCapVertices = 8;
            trail.colorGradient = CreateTrailGradient();
            trail.Clear();

            return obj;
        }

        private static Gradient CreateTrailGradient()
        {
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
            new GradientColorKey(trailColor, 0f),
            new GradientColorKey(trailColor, 1f)
                },
                new GradientAlphaKey[]
                {
            new GradientAlphaKey(0.75f, 0f),
            new GradientAlphaKey(0.45f, 0.35f),
            new GradientAlphaKey(0.18f, 0.7f),
            new GradientAlphaKey(0f, 1f)
                }
            );

            return gradient;
        }

        private static bool TryGetTrailHands(out Vector3 leftPosition, out Vector3 rightPosition)
        {
            leftPosition = Vector3.zero;
            rightPosition = Vector3.zero;

            if (GorillaTagger.Instance == null ||
                GorillaTagger.Instance.leftHandTransform == null ||
                GorillaTagger.Instance.rightHandTransform == null)
            {
                return false;
            }

            leftPosition = TrueLeftHand().position;
            rightPosition = TrueRightHand().position;
            return true;
        }
    }
}