namespace Assets.SimplePlanesReflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;
    using System.Reflection;

    public class UILabel : MonoBehaviourProxyType<UILabel>
    {
        private static Property<string> _text = CreateProperty<string>("text");
        private string text_;
        private static Property<GameObject> _gameObject = CreateProperty<GameObject>("gameObject");
        //private static PropertyInfo __gameObject = RealType.GetProperty("gameObject",BindingFlags.Public | BindingFlags.Instance);
        private GameObject gameObject_;
        protected UILabel()
        {
        }
        
        public string text
        {
            get
            {
                if(text_==null)
                    text_=Get(_text);
                return text_;
            }
        }
        public GameObject gameObject
        {
            get
            {
                 if (gameObject_ == null)
                gameObject_ = _gameObject.Get(RealObject);
                return gameObject_;
            }
        }
    }
    public class UISprite : ProxyType<UISprite>
    {
        public static MethodInfo _enabled = RealType.GetProperty("enabled",BindingFlags.Instance|BindingFlags.Public).GetSetMethod();
        private static Property<int> _depth = CreateProperty<int>("depth");
        private static Property<Color> _color = CreateProperty<Color>("color");
        private Color? color_;
        protected UISprite()
        {
        }
        public bool enabled
        {
             set
            {
                if (RealObject == null) Application.Quit();
                //_enabled.Invoke(RealObject, new object[] { value });
            }
        }

        public int depth
        {
            set
            {
                Set(_depth, value);
            }
        }

        public Color color
        {
            get
            {
                if (color_ == null)
                    color_ = Get(_color);
                return color_.Value;
            }
            set
            {
                Set(_color, value);
                color_ = value;
            }
        }
    }
    public class UITexture:MonoBehaviourProxyType<UITexture>
    {
        protected static Property<Texture> _mainTexture = CreateProperty<Texture>("mainTexture");

        protected UITexture() { }

        public Texture mainTexture
        {
            get
            {
                return Get(_mainTexture);
            }
            set
            {
                Set(_mainTexture, value);
            }
        }
    }
}