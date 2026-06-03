/*
 * UA Mod Menu Mods/Master.cs
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

using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Master
    {
        private static bool Grabbing => ControllerInputPoller.instance.rightGrab || (Mouse.current != null && Mouse.current.rightButton.isPressed);
        private static bool Triggering => ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f || (Mouse.current != null && Mouse.current.leftButton.isPressed);

        public static void CheckIsMaster()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=purple>MASTER CLIENT</color><color=grey>]</color> You are the master client.");
            }
            else
            {
                Notifications.NotifiLib.SendNotification("<color=grey>[</color><color=purple>MASTER CLIENT</color><color=grey>]</color> You are not the master client.");
            }
        }

        // kick gun
        public static bool previousKickTrigger;

        private static void KickPlayer(Player targetPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            try { PhotonNetwork.DestroyPlayerObjects(targetPlayer); } catch { }
            try { PhotonNetwork.CloseConnection(targetPlayer); } catch { }
        }

        public static void KickGun()
        {
            if (Grabbing)
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (Triggering && !previousKickTrigger)
                {
                    if (Physics.Raycast(
                        GorillaTagger.Instance.rightHandTransform.position,
                        GorillaTagger.Instance.rightHandTransform.forward,
                        out RaycastHit hit, 50f))
                    {
                        VRRig rig = hit.collider.GetComponentInParent<VRRig>();
                        if (rig != null && !rig.isLocal && rig.Creator != null)
                        {
                            foreach (var player in PhotonNetwork.PlayerList)
                            {
                                if (player.UserId == rig.Creator.UserId)
                                {
                                    KickPlayer(player);
                                    Debug.Log($"[KickGun] Кікнув: {player.NickName}");
                                    break;
                                }
                            }
                        }
                    }
                }

                previousKickTrigger = Triggering;
            }
        }
        // lag gun
        public static bool previousLagTrigger;

        private static void LagPlayer(Player targetPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            for (int i = 0; i < 30; i++)
            {
                PhotonNetwork.RaiseEvent(
                    69,
                    new byte[1024],
                    new RaiseEventOptions
                    {
                        TargetActors = new int[] { targetPlayer.ActorNumber },
                        Receivers = ReceiverGroup.Others,
                        CachingOption = EventCaching.DoNotCache
                    },
                    new SendOptions { Reliability = false, Channel = 5 }
                );
            }
        }

        public static void LagGun()
        {
            if (Grabbing)
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (Triggering && !previousLagTrigger)
                {
                    if (Physics.Raycast(
                        GorillaTagger.Instance.rightHandTransform.position,
                        GorillaTagger.Instance.rightHandTransform.forward,
                        out RaycastHit hit, 50f))
                    {
                        VRRig rig = hit.collider.GetComponentInParent<VRRig>();
                        if (rig != null && !rig.isLocal && rig.Creator != null)
                        {
                            foreach (var player in PhotonNetwork.PlayerList)
                            {
                                if (player.UserId == rig.Creator.UserId)
                                {
                                    LagPlayer(player);
                                    Debug.Log($"[LagGun] Залагав: {player.NickName}");
                                    break;
                                }
                            }
                        }
                    }
                }

                previousLagTrigger = Triggering;
            }
        }

        // ghost gun
        public static bool previousGhostTrigger;
        private static readonly List<int> GhostedActors = new List<int>();

        [PunRPC]
        private static void GhostSelfRPC()
        {
            var localRig = GorillaTagger.Instance.offlineVRRig;
            if (localRig != null)
            {
                var bodyRenderer = localRig.GetComponentInChildren<GorillaBodyRenderer>(true);
                if (bodyRenderer != null) GameObject.Destroy(bodyRenderer);

                var renderers = localRig.GetComponentsInChildren<Renderer>(true);
                foreach (var r in renderers) GameObject.Destroy(r);

                var nameTag = localRig.GetComponentInChildren<TextMeshPro>(true);
                if (nameTag != null) GameObject.Destroy(nameTag.gameObject);

                var colliders = localRig.GetComponentsInChildren<Collider>(true);
                foreach (var col in colliders) col.enabled = false;
            }

            var leftHand = GorillaTagger.Instance.leftHandTransform;
            if (leftHand != null)
            {
                var handRenderers = leftHand.GetComponentsInChildren<Renderer>(true);
                foreach (var r in handRenderers) GameObject.Destroy(r);
            }

            var rightHand = GorillaTagger.Instance.rightHandTransform;
            if (rightHand != null)
            {
                var handRenderers = rightHand.GetComponentsInChildren<Renderer>(true);
                foreach (var r in handRenderers) GameObject.Destroy(r);
            }
        }

        private static void GhostPlayer(Player targetPlayer)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            int targetActor = targetPlayer.ActorNumber;
            if (!GhostedActors.Contains(targetActor))
                GhostedActors.Add(targetActor);

            // 1. Ховаємо VRRig на нашому клієнті через GorillaParent
            foreach (var rig in VRRigCache.ActiveRigs)
            {
                if (rig != null && rig.Creator != null && rig.Creator.UserId == targetPlayer.UserId)
                {
                    var bodyRenderer = rig.GetComponentInChildren<GorillaBodyRenderer>(true);
                    if (bodyRenderer != null) GameObject.Destroy(bodyRenderer);

                    var renderers = rig.GetComponentsInChildren<Renderer>(true);
                    foreach (var r in renderers) GameObject.Destroy(r);

                    var nameTag = rig.GetComponentInChildren<TextMeshPro>(true);
                    if (nameTag != null) GameObject.Destroy(nameTag.gameObject);

                    var colliders = rig.GetComponentsInChildren<Collider>(true);
                    foreach (var col in colliders) col.enabled = false;

                    rig.transform.position = new Vector3(0f, -9999f, 0f);
                    break;
                }
            }

            // Якщо хочеш через VRRigCache замість GorillaParent:
            // foreach (var kvp in VRRigCache.Instance.ActiveRigs)
            // {
            //     VRRig rig = kvp.Value; // або kvp якщо це KeyValuePair
            //     ...
            // }

            // 2. RPC на жертву — ховає себе локально
            var view = GorillaTagger.Instance.GetComponent<PhotonView>();
            view.RPC("GhostSelfRPC", targetPlayer);

            // 3. Знищуємо голос і косметику жертви
            try { PhotonNetwork.DestroyPlayerObjects(targetPlayer); } catch { }

            Debug.Log($"[GhostGun] Загостив: {targetPlayer.NickName}");
        }

        public static void GhostGun()
        {
            if (ghostGunEnabled)
            {
                if (Grabbing)
                {
                    var GunData = RenderGun();
                    GameObject NewPointer = GunData.NewPointer;

                    if (Triggering && !previousGhostTrigger)
                    {
                        if (Physics.Raycast(
                            GorillaTagger.Instance.rightHandTransform.position,
                            GorillaTagger.Instance.rightHandTransform.forward,
                            out RaycastHit hit, 50f))
                        {
                            VRRig rig = hit.collider.GetComponentInParent<VRRig>();
                            if (rig != null && !rig.isLocal && rig.Creator != null)
                            {
                                foreach (var player in PhotonNetwork.PlayerList)
                                {
                                    if (player.UserId == rig.Creator.UserId)
                                    {
                                        GhostPlayer(player);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    previousGhostTrigger = Triggering;
                }
            }
        }

        public static bool ghostGunEnabled = false;

        public static void ToggleGhostGun()
        {
            ghostGunEnabled = !ghostGunEnabled;
            previousGhostTrigger = false;
            Debug.Log($"[GhostGun] {(ghostGunEnabled ? "Увімкнено" : "Вимкнено")}");
        }
    }
}
