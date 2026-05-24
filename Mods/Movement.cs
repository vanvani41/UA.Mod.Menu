/*
 * UA Mod Menu Mods/Movement.cs
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

using BepInEx;
using GorillaLocomotion;
using StupidTemplate.Classes;
using UnityEngine;
using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods
{
    public class Movement
    {
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public static GameObject platgl;
        public static GameObject platgr;

        public static void GripPlatforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (platgl == null)
                {
                    platgl = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platgl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platgl.transform.position = TrueLeftHand().position - (Vector3.up * 0.1f);
                    platgl.transform.rotation = TrueLeftHand().rotation;
                    FixStickyColliders(platgl);
                    platgl.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (platgl != null) { Object.Destroy(platgl); platgl = null; }
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                if (platgr == null)
                {
                    platgr = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    platgr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platgr.transform.position = TrueRightHand().position - (Vector3.up * 0.1f);
                    platgr.transform.rotation = TrueRightHand().rotation;
                    FixStickyColliders(platgr);
                    platgr.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (platgr != null) { Object.Destroy(platgr); platgr = null; }
            }
        }

        public static GameObject plattl;
        public static GameObject plattr;

        public static void TriggerPlatforms()
        {
            if (ControllerInputPoller.instance.leftControllerTriggerButton)
            {
                if (plattl == null)
                {
                    plattl = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    plattl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    plattl.transform.position = TrueLeftHand().position - (Vector3.up * 0.1f);
                    plattl.transform.rotation = TrueLeftHand().rotation;
                    FixStickyColliders(plattl);
                    plattl.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (plattl != null) { Object.Destroy(plattl); plattl = null; }
            }

            if (ControllerInputPoller.instance.rightControllerTriggerButton)
            {
                if (plattr == null)
                {
                    plattr = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    plattr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    plattr.transform.position = TrueRightHand().position - (Vector3.up * 0.1f);
                    plattr.transform.rotation = TrueRightHand().rotation;
                    FixStickyColliders(plattr);
                    plattr.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (plattr != null) { Object.Destroy(plattr); plattr = null; }
            }
        }

        public static GameObject platsgl;
        public static GameObject platsgr;

        public static void GripStickyPlatforms()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                if (platsgl == null)
                {
                    platsgl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    platsgl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platsgl.transform.position = TrueLeftHand().position;
                    platsgl.transform.rotation = TrueLeftHand().rotation;
                    FixStickyColliders1(platsgl);
                    platsgl.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (platsgl != null) { Object.Destroy(platsgl); platsgl = null; }
            }

            if (ControllerInputPoller.instance.rightGrab)
            {
                if (platsgr == null)
                {
                    platsgr = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    platsgr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platsgr.transform.position = TrueRightHand().position;
                    platsgr.transform.rotation = TrueRightHand().rotation;
                    FixStickyColliders1(platsgr);
                    platsgr.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (platsgr != null) { Object.Destroy(platsgr); platsgr = null; }
            }
        }

        public static GameObject platstl;
        public static GameObject platstr;

        public static void TriggerStickyPlatforms()
        {
            if (ControllerInputPoller.instance.leftControllerTriggerButton)
            {
                if (platstl == null)
                {
                    platstl = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    platstl.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platstl.transform.position = TrueLeftHand().position;
                    platstl.transform.rotation = TrueLeftHand().rotation;
                    FixStickyColliders1(platstl);
                    platstl.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (platstl != null) { Object.Destroy(platstl); platstl = null; }
            }

            if (ControllerInputPoller.instance.rightControllerTriggerButton)
            {
                if (platstr == null)
                {
                    platstr = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    platstr.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    platstr.transform.position = TrueRightHand().position;
                    platstr.transform.rotation = TrueRightHand().rotation;
                    FixStickyColliders1(platstr);
                    platstr.AddComponent<ColorChanger>().colors = StupidTemplate.Settings.backgroundColor;
                }
            }
            else
            {
                if (platstr != null) { Object.Destroy(platstr); platstr = null; }
            }
        }

        public static void Speedboost()
        {
            GTPlayer.Instance.maxJumpSpeed = Settings.Movement.Speedboost;
            GTPlayer.Instance.jumpMultiplier = Settings.Movement.Speedboost;
        }

        public static void SpeedboostDisable()
        {
            GTPlayer.Instance.maxJumpSpeed = 6.5f;
            GTPlayer.Instance.jumpMultiplier = 1.1f;
        }
        public static bool ghostOn = false;
        public static bool ghostPressed = false;

        public static bool invOn = false;
        public static bool invPressed = false;

        public static void GhostMonkeXT()
        {
            bool isDown = ControllerInputPoller.instance.leftControllerPrimaryButton;
            if (isDown && !ghostPressed) ghostOn = !ghostOn;
            ghostPressed = isDown;

            UpdateRigState();
        }

        public static void GhostMonkeXH()
        {
            if (ControllerInputPoller.instance.leftControllerPrimaryButton)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = false;
            }
            else
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
            }
        }

        public static void InvisMonkeAH()
        {
            bool invHold = ControllerInputPoller.instance.rightControllerPrimaryButton;
            UpdateRigState(invHold);
        }

        public static void InvisMonkeAT()
        {
            bool isDown = ControllerInputPoller.instance.rightControllerPrimaryButton;
            if (isDown && !invPressed) invOn = !invOn;
            invPressed = isDown;

            UpdateRigState(false);
        }

        private static void UpdateRigState(bool invHold = false)
        {
            var rig = GorillaTagger.Instance.offlineVRRig;

            bool invisActive = invOn || invHold;

            rig.enabled = !(ghostOn || invisActive);

            if (invisActive)
                rig.transform.position = Vector3.up * 9999f;
            else if (!ghostOn)
                rig.transform.position = GorillaTagger.Instance.headCollider.transform.position;
        }
      
        public static void WASDFly()
        {
            float baseSpeed = Settings.Movement.WASDSpeed;
            float speed = baseSpeed + (UnityInput.Current.GetKey(KeyCode.LeftShift) ? 4f : 0f);

            if (UnityInput.Current.GetKey(KeyCode.W))
            { 
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * speed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
            if (UnityInput.Current.GetKey(KeyCode.S))
            {
                GTPlayer.Instance.transform.position -= GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * speed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
            if (UnityInput.Current.GetKey(KeyCode.D))
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.right * Time.deltaTime * speed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
            if (UnityInput.Current.GetKey(KeyCode.A))
            {
                GTPlayer.Instance.transform.position -= GorillaTagger.Instance.headCollider.transform.right * Time.deltaTime * speed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
            if (UnityInput.Current.GetKey(KeyCode.Space))
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.up * Time.deltaTime * speed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
            if (UnityInput.Current.GetKey(KeyCode.LeftControl))
            {
                GTPlayer.Instance.transform.position -= GorillaTagger.Instance.headCollider.transform.up * Time.deltaTime * speed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public static void NoclipFly()
        {
            MeshCollider[] colliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                foreach (MeshCollider collider in colliders)
                {
                    collider.enabled = false;
                }
            }
            else
            {
                foreach (MeshCollider collider in colliders)
                {
                    collider.enabled = true;
                }
            }
        }

        public static void NoclipRT()
        {
            MeshCollider[] colliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
            if (ControllerInputPoller.instance.rightControllerTriggerButton)
            {
                foreach (MeshCollider collider in colliders)
                {
                    collider.enabled = false;
                }
            }
            else
            {
                foreach (MeshCollider collider in colliders)
                {
                    collider.enabled = true;
                }
            }
        }
        public static void NoclipLT()
        {
            MeshCollider[] colliders = Resources.FindObjectsOfTypeAll<MeshCollider>();
            if (ControllerInputPoller.instance.leftControllerTriggerButton)
            {
                foreach (MeshCollider collider in colliders)
                {
                    collider.enabled = false;
                }
            }
            else
            {
                foreach (MeshCollider collider in colliders)
                {
                    collider.enabled = true;
                }
            }
        }
        public static void CarMonkeG()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                GTPlayer.Instance.transform.position -= GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.CarMonkeSpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.forward;
            }
            if (ControllerInputPoller.instance.rightGrab)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.CarMonkeSpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.forward;
            }
        }
        public static void CarMonkeT()
        {
            if (ControllerInputPoller.instance.leftControllerTriggerButton)
            {
                GTPlayer.Instance.transform.position -= GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.CarMonkeSpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.forward;
            }
            if (ControllerInputPoller.instance.rightControllerTriggerButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * Settings.Movement.CarMonkeSpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.forward;
            }
        }
        public static void SlowMotion()
        {
            Time.timeScale = 0.35f;
        }

        public static void SlowMotionDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
