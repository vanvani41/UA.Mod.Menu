/*
 * UA Mod Menu ModsidedSystem.cs
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
using Photon.Pun;
using Photon.Realtime;
using StupidTemplate.Mods;
using UnityEngine;

namespace StupidTemplate
{
    public class ModsidedSystem : MonoBehaviour
    {
        public const byte ModSidedByte = 43;
        private static bool initialized;

        public static void Init()
        {
            if (initialized || PhotonNetwork.NetworkingClient == null) return;
            PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
            initialized = true;
        }

        public static void Cleanup()
        {
            if (!initialized || PhotonNetwork.NetworkingClient == null) return;
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
            initialized = false;
        }

        public static void AnnouncePresence()
        {
            if (!initialized)
                Init();

            if (!PhotonNetwork.InRoom) return;
            PhotonNetwork.RaiseEvent(
                ModSidedByte,
                null,
                new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                SendOptions.SendReliable
            );
        }

        private static void OnEventReceived(EventData data)
        {
            if (data.Code != ModSidedByte) return;
            if (PhotonNetwork.NetworkingClient?.CurrentRoom == null) return;

            Player sender = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender);
            if (sender != null && !string.IsNullOrEmpty(sender.UserId))
                Nametags.modUsers.Add(sender.UserId);
        }
    }
}