using System;

namespace StupidTemplate.Classes
{
    public class ButtonInfo
    {
        public string buttonText = "-";
        public string overlapText = null;
        public Action method = null;
        public Action enableMethod = null;
        public Action disableMethod = null;
        public bool enabled = false;
        public bool isTogglable = true;
        public Func<bool> isVisible = null;
        public string toolTip = "This button doesn't have a tooltip/tutorial.";

        public bool IsVisible()
        {
            try
            {
                return isVisible == null || isVisible.Invoke();
            }
            catch
            {
                return false;
            }
        }
    }
}