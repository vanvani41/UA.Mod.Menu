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

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Master
    {
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

        public static bool previousKickTrigger;

        private static void KickPlayer(Player player)
        {
            if (!PhotonNetwork.IsMasterClient) return;
            try { PhotonNetwork.DestroyPlayerObjects(player); } catch { }
            try { PhotonNetwork.CloseConnection(player); } catch { }
        }

        public static void KickGun()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f && !previousKickTrigger)
                {
                    if (Physics.Raycast(GorillaTagger.Instance.rightHandTransform.position, GorillaTagger.Instance.rightHandTransform.forward, out RaycastHit hit, 50f))
                    {
                        VRRig rig = hit.collider.GetComponentInParent<VRRig>();
                        if (rig != null && !rig.isLocal && rig.Creator != null)
                        {
                            foreach (var player in PhotonNetwork.PlayerList)
                            {
                                if (player.UserId == rig.Creator.UserId)
                                {
                                    KickPlayer(player);
                                    break;
                                }
                            }
                        }
                    }
                }

                previousKickTrigger = ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f;
            }
        }
    }
}
