
using Assets.SimplePlanesReflection.Assets.Scripts.Levels;
using System.Reflection;

namespace Assets.SimplePlanesReflection.Assets.Scripts.Airport
{
    public class GuiScript : ProxyType<GuiScript>
    {
        private static MethodInfo m = RealType.GetMethod("ToggleUIVisibility", BindingFlags.Instance | BindingFlags.NonPublic);
        private static PropertyInfo h = RealType.GetProperty("UIHidden", BindingFlags.Static | BindingFlags.Public);

        public void ToggleUIVisibility()
        {
            m.Invoke(RealObject, null);
        }
        public static void custom_HideUI()
        {
            object o;
            try
            {
                o = LevelBase.CurrentLevel.GuiScript.RealObject;
            if (!(bool)h.GetValue(o, null))
                m.Invoke(o, null);
            }
            catch (System.Exception )
            {
                return;
            }
        }
        public static void custom_ShowUI()
        {
            object o;
            try
            {
                o = LevelBase.CurrentLevel.GuiScript.RealObject;
            if ((bool)h.GetValue(o, null))
                m.Invoke(o, null);
            }
            catch (System.Exception )
            {
                return;
            }
        }

    }
}
