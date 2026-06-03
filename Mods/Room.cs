/*
 * UA Mod Menu Mods/Room.cs
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
using GorillaNetworking;
using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace StupidTemplate.Mods
{
    public class Room
    {
        public static void Disconnect()
        {
            NetworkSystem.Instance.ReturnToSinglePlayer();
        }
        public static void Reconnect()
        {
            if (!PhotonNetwork.InRoom) return;
            string playersomgcode = PhotonNetwork.CurrentRoom.Name;
            NetworkSystem.Instance.ReturnToSinglePlayer();
            GTPlayer.Instance.StartCoroutine(ReconnectCoroutine(playersomgcode));
        }
        private static IEnumerator ReconnectCoroutine(string room)
        {
            yield return new WaitForSeconds(3f);
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(room, JoinType.Solo);
        }
        public static void JoinRoom(string room)
        {
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(room, JoinType.Solo);
        }
    }
}
