using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Assets.SimplePlanesReflection.Assets.Scripts.Gui
{
	public class ShareAircraftScript : ProxyType<ShareAircraftScript>
	{
		private static Property<GameObject> _gameObject = CreateProperty<GameObject>("gameObject");
		private static FieldInfo __initialized = RealType.GetField("_initialized", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo __userScreenshots = RealType.GetField("_userScreenshots", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo __userScreenshotsContainer = RealType.GetField("_userScreenshotsContainer", BindingFlags.Instance | BindingFlags.NonPublic);
		private static FieldInfo __userScreenshotTextures = RealType.GetField("_userScreenshotTextures", BindingFlags.Instance | BindingFlags.NonPublic);


		public void Reset()
		{
			gameObject_ = null;
			_userScreenshots_ = null;
			_userScreenshotTextures_ = null;
		}

		public GameObject gameObject
		{
			get
			{
				if (gameObject_ == null)
					gameObject_ = _gameObject.Get(RealObject);
				return gameObject_;
				//#if !DEBUG
				//                gameObject_可能不是最新
				//#endif
			}
		}



		protected ShareAircraftScript() { }
		private GameObject gameObject_;
		public bool _initialized
		{
			get
			{
				return (bool)__initialized.GetValue(RealObject);
			}
		}
		private List<UITexture> _userScreenshots_ = null; private List<UITexture> _userScreenshots
		{
			get
			{
				if (_userScreenshots_ == null)
				{
					object o = __userScreenshots.GetValue(RealObject);
					_userScreenshots_ = new List<UITexture>();
					IEnumerator i = ((IEnumerable)o).GetEnumerator();
					try
					{
						while (i.MoveNext())
						{
							object current = i.Current;
							_userScreenshots_.Add(UITexture.Wrap(current));
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
				}
				return _userScreenshots_;
				//#if !DEBUG
				//                _userScreenshots_可能不是最新
				//#endif
			}
		}
		private MonoBehaviour _userScreenshotsContainer
		{
			get
			{
				return __userScreenshotsContainer.GetValue(RealObject) as MonoBehaviour;
			}
		}

		private List<Texture2D> _userScreenshotTextures_ = null;
		private List<Texture2D> _userScreenshotTextures
		{
			get
			{
				if (_userScreenshotTextures_ == null)
				{
					object o = __userScreenshotTextures.GetValue(RealObject);
					_userScreenshotTextures_ = (List<Texture2D>)o;
					//_userScreenshotTextures_ = new List<Texture2D>();
					//IEnumerator i = ((IEnumerable)o).GetEnumerator();
					//try
					//{
					//	while (i.MoveNext())
					//	{
					//		object current = i.Current;
					//		_userScreenshotTextures_.Add(current as Texture2D);
					//	}
					//}
					//finally
					//{
					//	IDisposable disposable = i as IDisposable;
					//	if (disposable != null)
					//	{
					//		disposable.Dispose();
					//	}
					//}
				}
				return _userScreenshotTextures_;
				//#if !DEBUG
				//                _userScreenshotTextures_可能不是最新
				//#endif
			}
		}

		public void custom_AddScreenshots(List<Texture2D> list)
		{
			//_userScreenshots.Clear();
			//_userScreenshotTextures.Clear();
			object o = __userScreenshots.GetValue(RealObject);
			IList l = (IList)o;
			if (l.Count >= 1) return;
			for (int i = 2; i >= 0; i--)
			{
				if (i + 1 > list.Count) continue;
				string textureName = string.Format("Screenshot{0}", l.Count);
				UITexture texture = U.FindObjectsMyselfOrChildren(textureName, gameObject)[0];
				texture.mainTexture = list[i];
				//_userScreenshots.Add(texture);
				_userScreenshotTextures.Add(list[i]);
				l.Add(texture.RealObject);
			}
		}

		private void Inject()
		{

		}
	}
	public static class U
	{
		public static List<UITexture> FindObjectsMyselfOrChildren(string name, GameObject gameObject)
		{
			List<UITexture> list = new List<UITexture>();
			if (gameObject != null)
			{
				if (gameObject.name == name || name == null)
				{
					UITexture component = UITexture.Wrap(gameObject.GetComponent("UITexture"));
					if (component != null && component.RealObject != null)
					{
						list.Add(component);
					}
				}
				for (int i = 0; i < gameObject.transform.childCount; i++)
				{
					Transform child = gameObject.transform.GetChild(i);
					List<UITexture> list2 = FindObjectsMyselfOrChildren(name, child.gameObject);
					if (list2 != null)
					{
						list.AddRange(list2);
					}
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			return null;
		}
		static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null)
			{
				// throw Error.ArgumentNull("source");
			}
			IList<TSource> list = source as IList<TSource>;
			if (list != null)
			{
				if (list.Count > 0)
				{
					return list[0];
				}
			}
			else
			{
				using (IEnumerator<TSource> enumerator = source.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			return default(TSource);
		}
	}
}
