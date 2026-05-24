/*
 * UA Mod Menu Mods/Guns.cs
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

using GorillaLocomotion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Guns
    {
        private static bool Grabbing => ControllerInputPoller.instance.rightGrab || (Mouse.current != null && Mouse.current.rightButton.isPressed);
        private static bool Triggering => ControllerInputPoller.TriggerFloat(XRNode.RightHand) > 0.5f || (Mouse.current != null && Mouse.current.leftButton.isPressed);

        public static bool previousTeleportTrigger;
        public static void TeleportGun()
        {
            if (Grabbing)
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (Triggering && !previousTeleportTrigger)
                {
                    GTPlayer.Instance.TeleportTo(NewPointer.transform.position + Vector3.up, GTPlayer.Instance.transform.rotation);
                    GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                }

                previousTeleportTrigger = Triggering;
            }
        }

        public static bool previousTagTrigger;
        private static float tagReturnTime;
        private static bool waitingToReturn;

        public static void TagGun()
        {
            var rig = GorillaTagger.Instance.offlineVRRig;

            if (waitingToReturn && Time.time >= tagReturnTime)
            {
                rig.enabled = true;
                waitingToReturn = false;
            }

            if (Grabbing)
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (Triggering && !previousTagTrigger && !waitingToReturn)
                {
                    if (Physics.Raycast(NewPointer.transform.position, NewPointer.transform.forward, out RaycastHit hit, 50f))
                    {
                        VRRig target = hit.collider.GetComponentInParent<VRRig>();
                        if (target != null && !target.isLocal)
                        {
                            rig.enabled = false;
                            rig.transform.position = target.transform.position;
                            tagReturnTime = Time.time + 0.3f;
                            waitingToReturn = true;
                        }
                    }
                }

                previousTagTrigger = Triggering;
            }
        }
    }
}