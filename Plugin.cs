using BepInEx;
using StupidTemplate.Mods;

namespace StupidTemplate
{
    [System.ComponentModel.Description(PluginInfo.Description)]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        public static HarmonyPatches Instance;
        private void Awake()
        {
            Instance = this;
            ModsidedSystem.Init();
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
            NetworkSystem.Instance.OnJoinedRoomEvent += OnJoinedRoom;
            NetworkSystem.Instance.OnReturnedToSinglePlayer += OnDisconnect;
            NetworkSystem.Instance.OnPlayerJoined += OnPlayerJoined;
            NetworkSystem.Instance.OnPlayerLeft += OnPlayerLeft;
        }

        private void OnDestroy()
        {
            ModsidedSystem.Cleanup();
            NetworkSystem.Instance.OnJoinedRoomEvent -= OnJoinedRoom;
            NetworkSystem.Instance.OnReturnedToSinglePlayer -= OnDisconnect;
            NetworkSystem.Instance.OnPlayerJoined -= OnPlayerJoined;
            NetworkSystem.Instance.OnPlayerLeft -= OnPlayerLeft;
        }

        private void OnJoinedRoom()
        {
            Mods.Nametags.modUsers.Clear();
            ModsidedSystem.AnnouncePresence();
        }

        private void OnDisconnect()
        {
            Mods.Nametags.modUsers.Clear();
        }

        private void OnPlayerJoined(NetPlayer player)
        {
            ModsidedSystem.AnnouncePresence();
        }

        private void OnPlayerLeft(NetPlayer player)
        {
            if (player?.UserId != null)
                Mods.Nametags.modUsers.Remove(player.UserId);
        }

        public void OnPlayerSpawned()
        {
            Patches.PatchHandler.PatchAll();
            Patches.Internal.MemoryProtection.StartMemoryProtection();
        }

        void LateUpdate()
        {
            Nametags.RunNametags();

            if (Menu.Buttons.buttons[12][2].enabled)
                Visual.RunTrail();

            Master.GhostGun();
        }
    }
}