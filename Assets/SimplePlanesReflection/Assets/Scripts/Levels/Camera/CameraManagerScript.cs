namespace Assets.SimplePlanesReflection.Assets.Scripts.Levels.Camera
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using System.Reflection;
    using System.Collections;

    public partial class CameraManagerScript : MonoBehaviourProxyType<CameraManagerScript>
    {

        private static Property<Transform> _CameraPosition = CreateProperty<Transform>("CameraPosition");
        private static Property<Transform> _CameraFocalPosition = CreateProperty<Transform>("CameraFocalPosition");
        private static Property<Transform> _CameraTransform = CreateProperty<Transform>("CameraTransform");
        private static Field<object> __cameras = CreateField<object>("_cameras");

        private static Field<Camera> __mainCamera = CreateField<Camera>("_mainCamera");
        private static Field<Camera> __planeCamera = CreateField<Camera>("_planeCamera");
        private static Field<object> __currentCameraController = CreateField<object>("_currentCameraController");


        static PropertyInfo p = RealType.GetProperty("Instance",BindingFlags.Public|BindingFlags.Static);

        private static CameraManagerScript Instance_;
        public static CameraManagerScript Instance
        {
            get
            {
                var a = p.GetValue(null, null);
                if (Instance_ == null||(object)Instance_.RealObject!=a)
                    Instance_ = Wrap(a);
                return Instance_;
                //return LevelBase.Wrap(Get(_CurrentLevel));
            }
            private set
            {
                Instance_ = value;
            }
        }

        public Transform CameraTransform
        {
            get
            {
                return Get<Transform>(_CameraTransform);
            }
            set
            {
                Set<Transform>(_CameraTransform, value);
            }
        }
        public Transform CameraFocalPosition
        {
            get
            {
                return Get(_CameraFocalPosition);
            }
            set
            {
                Set(_CameraFocalPosition, value);
            }
        }
        public Transform CameraPosition
        {
            get
            {
                return Get(_CameraPosition);
            }
            set
            {
                Set(_CameraPosition, value);
            }
        }

        public Camera _mainCamera
        {
            get
            {
                return Get(__mainCamera);
            }
            set
            {
                Set(__mainCamera, value);
            }
        }
        public Camera _planeCamera
        {
            get
            {
                return Get(__planeCamera);
            }
            set
            {
                Set(__mainCamera, value);
            }
        }
        public object _currentCameraController
        {
            get
            {
                return Get(__currentCameraController);
            }
        }
        public List<Camera> _cameras
        {
            get
            {
                object o = Get(__cameras);
                List<Camera> l = new List<Camera>();
                IEnumerator i = ((IEnumerable)o).GetEnumerator();
                try
                {
                    while (i.MoveNext())
                    {
                        object current = i.Current;
                        l.Add((Camera)current);
                    }
                }
                finally
                {
                    IDisposable disposable = i as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                return l;
            }
        }
        
        
    }
}
