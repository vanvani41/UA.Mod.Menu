/*
 * UA Mod Menu Mods/FunChallenge.cs
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
using static StupidTemplate.Mods.Safety;

namespace StupidTemplate.Mods
{
    public class FunChallenge
    {
        public static void AntiReportCloseGorillaTag()
        {
            AntiReport((vrrig, position) =>
            {
                CloseGorillaTag();
            });
        }
        public static void AntiReportRestartGorillaTag()
        {
            AntiReport((vrrig, position) =>
            {
                RestartGorillaTag();
            });
        }
        public static void AntiReportShutdownPC()
        {
            AntiReport((vrrig, position) =>
            {
                ShutdownPC();
            });
        }
        public static void AntiReportRestartPC()
        {
            AntiReport((vrrig, position) =>
            {
                RestartPC();
            });
        }
    }
}
