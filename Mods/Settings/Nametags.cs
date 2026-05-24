using static StupidTemplate.Menu.Main;

namespace StupidTemplate.Mods.Settings
{
    public class Nametags
    {
        public static int nametagsindex = 3;
        public static float nametagssize = 1.2f;

        public static void ChangeNametagsSize()
        {
            string[] nametagsNames = new string[] { "Extra Small", "Very Small", "Small", "Medium", "Large", "Very Large", "Extra Large" };
            float[] nametagsValues = new float[] { 0.5f, 0.8f, 1f, 1.2f, 1.7f, 2f, 2.6f };

            nametagsindex++;
            nametagsindex %= nametagsNames.Length;
            nametagssize = nametagsValues[nametagsindex];

            GetIndex("Change Name Nametags Size").overlapText = $"Change Name Nametags Size <color=gray>[</color><color=green>{nametagsNames[nametagsindex]}</color><color=gray>]</color>";
        }

        public static int idtagsindex = 3;
        public static float idtagssize = 1.7f;

        public static void ChangeIdtagsSize()
        {
            string[] idtagsNames = new string[] { "Extra Small", "Very Small", "Small", "Medium", "Large", "Very Large", "Extra Large" };
            float[] idtagsValues = new float[] { 0.8f, 1f, 1.2f, 1.7f, 2f, 2.6f, 3f };

            idtagsindex++;
            idtagsindex %= idtagsNames.Length;
            idtagssize = idtagsValues[idtagsindex];

            GetIndex("Change ID Nametags Size").overlapText = $"Change ID Nametags Size <color=gray>[</color><color=green>{idtagsNames[idtagsindex]}</color><color=gray>]</color>";
        }
    }
}