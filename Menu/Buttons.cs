/*
 * UA Mod Menu Menu/Buttons.cs
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

using StupidTemplate.Classes;
using StupidTemplate.Mods;
using static StupidTemplate.Menu.Main;
using static StupidTemplate.Settings;

namespace StupidTemplate.Menu
{
    public class Buttons
    {
        public static ButtonInfo[][] buttons = new ButtonInfo[][]
        {
            new ButtonInfo[] { // Main Mods [0]
                new ButtonInfo { buttonText = "Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Opens the main settings page for the menu."},

                new ButtonInfo { buttonText = "Room Mods", method =() => currentCategory = 5, isTogglable = false, toolTip = "Opens the room mods tab."},
                new ButtonInfo { buttonText = "Movement Mods", method =() => currentCategory = 6, isTogglable = false, toolTip = "Opens the movement mods tab."},
                new ButtonInfo { buttonText = "Safety Mods", method =() => currentCategory = 7, isTogglable = false, toolTip = "Opens the safety mods tab."},
                new ButtonInfo { buttonText = "Fun/Challenge Mods", method =() => currentCategory = 8, isTogglable = false, toolTip = "Opens the Fun/Challenge mods tab."},
                new ButtonInfo { buttonText = "Nametags Mods", method =() => currentCategory = 9, isTogglable = false, toolTip = "Opens the Nametags mods tab."},
                //new ButtonInfo { buttonText = "Overpowered Mods", method =() => currentCategory = 10, isTogglable = false, toolTip = "Opens the Overpowered mods tab."},
                new ButtonInfo { buttonText = "Guns", method =() => currentCategory = 10, isTogglable = false, toolTip = "Opens the Guns tab."},
                new ButtonInfo { buttonText = "Master Mods", method =() => currentCategory = 11, isTogglable = false, toolTip = "Opens the Master mods tab."},
                new ButtonInfo { buttonText = "Visual Mods", method =() => currentCategory = 12, isTogglable = false, toolTip = "Opens the Visual mods tab."},
                //new ButtonInfo { buttonText = "Modsided Mods", method = () => currentCategory = 13, isTogglable = false, toolTip = "Opens the mod-sided mods tab." },
            },

            new ButtonInfo[] { // Settings [1]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Menu Settings", method =() => currentCategory = 2, isTogglable = false, toolTip = "Opens the settings for the menu."},
                new ButtonInfo { buttonText = "Movement Settings", method =() => currentCategory = 3, isTogglable = false, toolTip = "Opens the movement settings for the menu."},
                new ButtonInfo { buttonText = "Nametags Settings", method =() => currentCategory = 4, isTogglable = false, toolTip = "Opens the nametags settings for the menu."},
            },

            new ButtonInfo[] { // Menu Settings [2]
                new ButtonInfo { buttonText = "Return to Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Returns to the main settings page for the menu."},

                new ButtonInfo { buttonText = "Right Hand", enableMethod =() => rightHanded = true, disableMethod =() => rightHanded = false, toolTip = "Puts the menu on your right hand."},
                new ButtonInfo { buttonText = "Notifications", enableMethod =() => disableNotifications = false, disableMethod =() => disableNotifications = true, enabled = !disableNotifications, toolTip = "Toggles the notifications."},
                new ButtonInfo { buttonText = "FPS Counter", enableMethod =() => fpsCounter = true, disableMethod =() => fpsCounter = false, enabled = fpsCounter, toolTip = "Toggles the FPS counter."},
                new ButtonInfo { buttonText = "Disconnect Button", enableMethod =() => disconnectButton = true, disableMethod =() => disconnectButton = false, enabled = disconnectButton, toolTip = "Toggles the disconnect button."},
                new ButtonInfo { buttonText = "Reconnect Button", enableMethod =() => reconnectButton = true, disableMethod =() => reconnectButton = false, enabled = reconnectButton, toolTip = "Toggles the reconnect button."},
            },

            new ButtonInfo[] { // Movement Settings [3]
                new ButtonInfo { buttonText = "Return to Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Returns to the main settings page for the menu."},

                new ButtonInfo { buttonText = "Change Fly Speed", overlapText = "Change Fly Speed <color=gray>[</color><color=green>Normal</color><color=gray>]</color>", method =() => Mods.Settings.Movement.ChangeFlySpeed(), isTogglable = false, toolTip = "Changes the speed of the fly mod."},
                new ButtonInfo { buttonText = "Change Speedboost Speed", overlapText = "Change Speedboost Speed <color=gray>[</color><color=green>Normal</color><color=gray>]</color>", method =() => Mods.Settings.Movement.ChangeSpeedboostSpeed(), isTogglable = false, toolTip = "Changes the speed of the speedboost mod."},
                new ButtonInfo { buttonText = "Change WASD Fly Speed", overlapText = "Change WASD Fly Speed <color=gray>[</color><color=green>Normal</color><color=gray>]</color>", method =() => Mods.Settings.Movement.ChangeWASDSpeed(), isTogglable = false, toolTip = "Changes the speed of the WASD fly mod."},
                new ButtonInfo { buttonText = "Change Car Monke Speed", overlapText = "Change Car Monke Speed <color=gray>[</color><color=green>Normal</color><color=gray>]</color>", method =() => Mods.Settings.Movement.ChangeCarMonkeSpeed(), isTogglable = false, toolTip = "Changes the speed of the car monke mod."},
            },

            new ButtonInfo[] { // Nametags Settings [4]
                new ButtonInfo { buttonText = "Return to Settings", method =() => currentCategory = 1, isTogglable = false, toolTip = "Returns to the main settings page for the menu."},

                new ButtonInfo { buttonText = "Change Name Nametags Size", overlapText = "Change Name Nametags Size <color=gray>[</color><color=green>Medium</color><color=gray>]</color>", method =() => Mods.Settings.Nametags.ChangeNametagsSize(), isTogglable = false, toolTip = "Changes the size of the name nametags mod."},
                new ButtonInfo { buttonText = "Change ID Nametags Size", overlapText = "Change ID Nametags Size <color=gray>[</color><color=green>Medium</color><color=gray>]</color>", method =() => Mods.Settings.Nametags.ChangeIdtagsSize(), isTogglable = false, toolTip = "Changes the size of the ID nametags mod."},
            },

            new ButtonInfo[] { // Room Mods [5]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Disconnect", method =() => Room.Disconnect(), isTogglable = false, toolTip = "Disconnects you from the room."},
                new ButtonInfo { buttonText = "Reconnect", method =() => Room.Reconnect(), isTogglable = false, toolTip = "Reconnects you from the room."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> UKRAINE </color><color=gray>]</color>", method =() => Room.JoinRoomUkraine(), isTogglable = false, toolTip = "Connects you to the room UKRAINE."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> UKRAINE1 </color><color=gray>]</color>", method =() => Room.JoinRoomUkraine1(), isTogglable = false, toolTip = "Connects you to the room UKRAINE1."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> UKRAINE2 </color><color=gray>]</color>", method =() => Room.JoinRoomUkraine2(), isTogglable = false, toolTip = "Connects you to the room UKRAINE2."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> UKRAINE3 </color><color=gray>]</color>", method =() => Room.JoinRoomUkraine3(), isTogglable = false, toolTip = "Connects you to the room UKRAINE3."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> UKRAINE4 </color><color=gray>]</color>", method =() => Room.JoinRoomUkraine4(), isTogglable = false, toolTip = "Connects you to the room UKRAINE4."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> UKRAINE5 </color><color=gray>]</color>", method =() => Room.JoinRoomUkraine5(), isTogglable = false, toolTip = "Connects you to the room UKRAINE5."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> VANVANI41 </color><color=gray>]</color>", method =() => Room.JoinRoomVanvani41(), isTogglable = false, toolTip = "Connects you to the room VANVANI41."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> V41FAN </color><color=gray>]</color>", method =() => Room.JoinRoomV41Fan(), isTogglable = false, toolTip = "Connects you to the room V41FAN."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> PBBV </color><color=gray>]</color>", method =() => Room.JoinRoomPBBV(), isTogglable = false, toolTip = "Connects you to the room PBBV."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> DAISY09 </color><color=gray>]</color>", method =() => Room.JoinRoomDAISY09(), isTogglable = false, toolTip = "Connects you to the DAISY09."},
                new ButtonInfo { buttonText = "Connect to Room <color=gray>[</color><color=green> ECHO </color><color=gray>]</color>", method =() => Room.JoinRoomECHO(), isTogglable = false, toolTip = "Connects you to the room ECHO."},
            },

            new ButtonInfo[] { // Movement Mods [6]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Platforms <color=gray>[</color><color=green> G </color><color=gray>]</color>", method =() => Movement.GripPlatforms(), toolTip = "Spawns platforms on your hands when pressing grip."},
                new ButtonInfo { buttonText = "Platforms <color=gray>[</color><color=green> T </color><color=gray>]</color>", method =() => Movement.TriggerPlatforms(), toolTip = "Spawns platforms on your hands when pressing trigger."},
                new ButtonInfo { buttonText = "Sticky Platforms <color=gray>[</color><color=green> G </color><color=gray>]</color>", method =() => Movement.GripStickyPlatforms(), toolTip = "Spawns platforms on your hands when pressing grip."},
                new ButtonInfo { buttonText = "Sticky Platforms <color=gray>[</color><color=green> T </color><color=gray>]</color>", method =() => Movement.TriggerStickyPlatforms(), toolTip = "Spawns platforms on your hands when pressing trigger."},
                new ButtonInfo { buttonText = "Fly <color=gray>[</color><color=green> A </color><color=gray>]</color>", method =() => Movement.Fly(), toolTip = "Sends you forward when holding A."},
                new ButtonInfo { buttonText = "Noclip Fly <color=gray>[</color><color=green> A </color><color=gray>]</color>", method =() => Movement.NoclipFly(), toolTip = "Sends you forward when holding A with Noclip."},
                new ButtonInfo { buttonText = "WASD Fly <color=gray>[</color><color=green> WASD </color><color=gray>]</color>", method =() => Movement.WASDFly(), toolTip = "Fly on WASD!!"},
                new ButtonInfo { buttonText = "Noclip <color=gray>[</color><color=green> RT </color><color=gray>]</color>", method =() => Movement.NoclipRT(), toolTip = "Noclips you when holding right trigger."},
                new ButtonInfo { buttonText = "Noclip <color=gray>[</color><color=green> LT </color><color=gray>]</color>", method =() => Movement.NoclipLT(), toolTip = "Noclips you when holding right trigger."},
                new ButtonInfo { buttonText = "Speedboost", enableMethod =() => Movement.Speedboost(), disableMethod =() => Movement.SpeedboostDisable(), toolTip = "Makes you faster."},
                new ButtonInfo { buttonText = "Car Monke <color=gray>[</color><color=green> G </color><color=gray>]</color>", method =() => Movement.CarMonkeG(), toolTip = "Ride forward when holding right grip and back when holding left grip."},
                new ButtonInfo { buttonText = "Car Monke <color=gray>[</color><color=green> T </color><color=gray>]</color>", method =() => Movement.CarMonkeT(), toolTip = "Ride forward when holding right trigger and back when holding left trigger."},
                new ButtonInfo { buttonText = "Ghost Monke <color=gray>[</color><color=green> XH </color><color=gray>]</color>", method =() => Movement.GhostMonkeXH(), toolTip = "Freezes you when holding X."},
                new ButtonInfo { buttonText = "Ghost Monke <color=gray>[</color><color=green> XT </color><color=gray>]</color>", method =() => Movement.GhostMonkeXT(), toolTip = "Freezes you when pressing X."},
                new ButtonInfo { buttonText = "Invis Monke <color=gray>[</color><color=green> AH </color><color=gray>]</color>", method =() => Movement.InvisMonkeAH(), toolTip = "Making you invisible when holding A."},
                new ButtonInfo { buttonText = "Invis Monke <color=gray>[</color><color=green> AT </color><color=gray>]</color>", method =() => Movement.InvisMonkeAT(), toolTip = "Making you invisible when pressing A."},
                new ButtonInfo { buttonText = "Slow Motion", enableMethod = Movement.SlowMotion, disableMethod = Movement.SlowMotionDisable, toolTip = "Slows down time (client-sided)."},
            },

            new ButtonInfo[] { // Safety Mods [7]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Anti Report <color=gray>[</color><color=green> Disconnect </color><color=gray>]</color>", method =() => Safety.AntiReportDisconnect(), toolTip = "Disconnects you when someone tries to report you."},
                new ButtonInfo { buttonText = "Anti Report <color=gray>[</color><color=green> Reconnect </color><color=gray>]</color>", method =() => Safety.AntiReportReconnect(), toolTip = "Reconnects you when someone tries to report you."},
                new ButtonInfo { buttonText = "No Finger Touch", method =() => Safety.NoFingerTouch(), toolTip = "Disables finger touches."},
                new ButtonInfo { buttonText = "Close Gorilla Tag", method =() => Safety.CloseGorillaTag(), isTogglable = false},
                new ButtonInfo { buttonText = "Restart Gorilla Tag", method =() => Safety.RestartGorillaTag(), isTogglable = false},
                new ButtonInfo { buttonText = "SHUTDOWN PC", method =() => Safety.ShutdownPC(), isTogglable = false},
                new ButtonInfo { buttonText = "RESTART PC", method =() => Safety.RestartPC(), isTogglable = false},
            },

            new ButtonInfo[] { // Fun/Challenge Mods [8]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Anti Report <color=gray>[</color><color=green> Close Gorilla Tag </color><color=gray>]</color>", method =() => FunChallenge.AntiReportCloseGorillaTag(), toolTip = "Closes Gorilla Tag when someone tries to report you."},
                new ButtonInfo { buttonText = "Anti Report <color=gray>[</color><color=green> Restart Gorilla Tag </color><color=gray>]</color>", method =() => FunChallenge.AntiReportRestartGorillaTag(), toolTip = "Restarts Gorilla Tag when someone tries to report you."},
                new ButtonInfo { buttonText = "Anti Report <color=gray>[</color><color=green> Shutdown PC </color><color=gray>]</color>", method =() => FunChallenge.AntiReportShutdownPC(), toolTip = "Shuts down your PC when someone tries to report you."},
                new ButtonInfo { buttonText = "Anti Report <color=gray>[</color><color=green> Restart PC </color><color=gray>]</color>", method =() => FunChallenge.AntiReportRestartPC(), toolTip = "Restarts your PC when someone tries to report you."},

            },

            new ButtonInfo[] { // Nametags Mods [9]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Force nametags to face you", enableMethod = () => Nametags.EnableForceFace(), disableMethod = () => Nametags.DisableForceFace(), toolTip = "Forces nametags to face you"},
                new ButtonInfo { buttonText = "Name Nametags", enableMethod =() => Nametags.EnableNameTags(), disableMethod =() => Nametags.DisableNameTags(), toolTip = "Turns on Name Nametag."},
                new ButtonInfo { buttonText = "ID Nametags", enableMethod =() => Nametags.EnableIdTags(), disableMethod =() => Nametags.DisableIdTags(), toolTip = "Turns on ID Nametag."},
            },

            /*new ButtonInfo[] { // Overpowered Mods [10]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},


            },*/

            new ButtonInfo[] { // Guns [10]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Teleport Gun", method =() => Guns.TeleportGun(), toolTip = "Teleports you to wherever your pointer is when pressing trigger."},
                new ButtonInfo { buttonText = "Tag Gun", method =() => Guns.TagGun(), toolTip = "Teleports you to a player for 0.3s to tag him,  then returns you back."},
            },

            new ButtonInfo[] { // Master Mods [11]
                new ButtonInfo { buttonText = "Return to Main", method =() => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page of the menu."},

                new ButtonInfo { buttonText = "Are you a master client?", method =() => Master.CheckIsMaster(), isTogglable = false, toolTip = "Checks if you are the master client."},
                new ButtonInfo { buttonText = "Kick Gun", method =() => Master.KickGun(), toolTip = "Kicks whoever your pointer is on."},
            },

            new ButtonInfo[] { // Visual Mods [12]
                new ButtonInfo { buttonText = "Return to Main", method = () => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page." },

                new ButtonInfo { buttonText = "ESP", enableMethod = Visual.RunESP, disableMethod = () => Visual.DisableESP(), toolTip = "Shows player lines and distance." },
                new ButtonInfo { buttonText = "Trail", enableMethod =() => Visual.EnableTrail(), disableMethod =() => Visual.DisableTrail(), toolTip = "You will leave a trail behind yourself."}
            },

            /*new ButtonInfo[] { // Modsided Mods [13]
                new ButtonInfo { buttonText = "Return to Main", method = () => currentCategory = 0, isTogglable = false, toolTip = "Returns to the main page." },


            },*/
        };
    }
}