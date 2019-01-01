namespace Assets.SimplePlanesReflection.Assets.Scripts.Levels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using System.Reflection;
    using Airport;

    public partial class LevelBase : MonoBehaviourProxyType<LevelBase>
    {
      //  private static Property<object> _CurrentLevel = CreateProperty<object>("CurrentLevel");
        
        private static Property<object> _GuiScript = CreateProperty<object>("GuiScript");

        protected LevelBase()
        {
           // FreeCamMainScript.OnTargetChanged += FreeCamMainScript_OnTargetChanged;
        }

        private void FreeCamMainScript_OnTargetChanged()
        {
            currentLevel_ = null;
            GuiScript_ = null;
        }

        private static LevelBase currentLevel_;
        private static GuiScript GuiScript_;
        static PropertyInfo p = RealType.GetProperty("CurrentLevel");
        

        public static LevelBase CurrentLevel
        {
            get
            {
                if (currentLevel_ == null)
                {
                    currentLevel_ = LevelBase.Wrap(p.GetValue(null, null));
                }
                return currentLevel_;
                //return LevelBase.Wrap(Get(_CurrentLevel));
            }
        }
        public GuiScript GuiScript
        {
            get
            {
                if (GuiScript_ == null)
                {
                    GuiScript_ = GuiScript.Wrap(Get(_GuiScript));
                }
                return GuiScript_;
                //return LevelBase.Wrap(Get(_CurrentLevel));
            }
        }
        

    }
}