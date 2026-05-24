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
using UnityEngine;

namespace StupidTemplate.Mods
{
    public class Visual
    {
        private static readonly Dictionary<VRRig, (LineRenderer line, LineRenderer dot)> espPool =
            new Dictionary<VRRig, (LineRenderer, LineRenderer)>();

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

        public static GameObject leftTrail;
        public static GameObject rightTrail;
        public static void EnableTrail()
        {
            leftTrail = CreateTrail();
            rightTrail = CreateTrail();
        }
        public static void DisableTrail()
        {
            if (leftTrail != null) Object.Destroy(leftTrail);
            if (rightTrail != null) Object.Destroy(rightTrail);
            leftTrail = null;
            rightTrail = null;
        }
        public static GameObject CreateTrail()
        {
            GameObject obj = new GameObject("UA_Trail");
            TrailRenderer trail = obj.AddComponent<TrailRenderer>();
            trail.material = new Material(Shader.Find("GorillaTag/UberShader"));
            trail.startWidth = 0.05f;
            trail.endWidth = 0f;
            trail.time = 0.5f;
            trail.minVertexDistance = 0.01f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.HSVToRGB(0f, 1f, 1f), 0f),
                    new GradientColorKey(Color.HSVToRGB(0.5f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            trail.colorGradient = gradient;
            return obj;
        }
        public static void RunTrail()
        {
            if (leftTrail == null || rightTrail == null) return;
            leftTrail.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            rightTrail.transform.position = GorillaTagger.Instance.rightHandTransform.position;
            float hue = Time.time * 0.5f % 1f;
            TrailRenderer leftRenderer = leftTrail.GetComponent<TrailRenderer>();
            TrailRenderer rightRenderer = rightTrail.GetComponent<TrailRenderer>();
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.HSVToRGB(hue, 1f, 1f), 0f),
                    new GradientColorKey(Color.HSVToRGB((hue + 0.5f) % 1f, 1f, 1f), 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(1f, 0f),
                    new GradientAlphaKey(0f, 1f)
                }
            );
            leftRenderer.colorGradient = gradient;
            rightRenderer.colorGradient = gradient;
        }
    }
}
