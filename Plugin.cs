using BepInEx;
using Fusion;

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
            GorillaTagger.OnPlayerSpawned(OnPlayerSpawned);
            NetworkSystem.Instance.OnReturnedToSinglePlayer += OnDisconnect;
            NetworkSystem.Instance.OnPlayerLeft += OnPlayerLeft;
        }
        private void OnDisconnect()
        {
            Mods.Nametags.modUsers.Clear();
        }

        private void OnPlayerLeft(NetPlayer player)
        {
            if (player?.UserId != null)
                Mods.Nametags.modUsers.Remove(player.UserId);
        }

        public void OnPlayerSpawned() =>
            Patches.PatchHandler.PatchAll();

        void LateUpdate()
        {
            Mods.Nametags.RunNametags();

            if (Menu.Buttons.buttons[13][1].enabled)
                Mods.Visual.RunTrail();
        }
    }
}

