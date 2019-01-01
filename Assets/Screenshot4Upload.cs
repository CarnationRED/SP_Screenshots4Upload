using UnityEngine;
using System.Collections;
using Assets.SimplePlanesReflection.Assets.Scripts.Levels.Camera;
using Jundroo.SimplePlanes.ModTools;
using System.Collections.Generic;
using Assets.SimplePlanesReflection.Assets.Scripts.Gui;
using System.Threading;
using System;
using System.Reflection;
using System.Linq;

public class Screenshot4Upload : MonoBehaviour
{
	static bool isAndroid = Application.platform == RuntimePlatform.Android;
	static bool snapRequested;
	static bool usePlaneCam;
	static Camera cam1, cam2;
	static List<Texture2D> screenshots;
	static Thread scriptFinder;
	static ShareAircraftScript script;
	static bool scriptEnabled;

	public static IGameState state;
	public static bool IsInLevel;

	private static FieldInfo FreeCamT;
	private static FieldInfo FreeCamEnabled;
	private static object FreenCam;
	private static Type FreeCamType;

	void FindFreeCam()
	{
		Assembly FreeCamA = AppDomain.CurrentDomain
		   .GetAssemblies()
		   .Where(x => x.GetName().Name.ToLower() == "freecam-assembly-csharp")
		   .FirstOrDefault();
		if (FreeCamA != null)
		{
			Type fc = FreeCamA.GetTypes().Where(x => x.Name ==(isAndroid? "FreeCamMainAndroid" : "FreeCamMainScript")).FirstOrDefault();
			FreeCamType = fc;
			if (fc != null)
			{
				FreeCamT = fc.GetField("FreeCamTransform", BindingFlags.NonPublic | BindingFlags.Instance);
				if (FreeCamT == null)
					FreeCamT = fc.GetField("CamTransform", BindingFlags.NonPublic | BindingFlags.Instance);
				FreeCamEnabled = fc.GetField("Enabled", BindingFlags.NonPublic | BindingFlags.Instance);
			}
		}
	}

	// Use this for initialization
	void Start()
	{
		state = ServiceProvider.Instance.GameState;
		state.LevelEntered += State_LevelEntered;
		state.LevelExited += State_LevelExited;
		cam1 = transform.Find("Camera1").gameObject.GetComponent<Camera>();
		cam2 = transform.Find("Camera2").gameObject.GetComponent<Camera>();
		if (screenshots == null)
			screenshots = new List<Texture2D>();
		FindFreeCam();
	}

	private void State_LevelExited(object sender, Jundroo.SimplePlanes.ModTools.Events.LevelChangedEventArgs e)
	{
		//	IsInLevel = false;
	}

	private void State_LevelEntered(object sender, Jundroo.SimplePlanes.ModTools.Events.LevelChangedEventArgs e)
	{
		//	IsInLevel = true;
	}

	private void OnDestroy()
	{
		scriptFinder.Abort();
		scriptFinder = null;
	}

	private void KillFinder()
	{
		if (scriptFinder != null)
		{
			scriptFinder.Abort();
			scriptFinder = null;
		}
	}

	private void StartFinder()
	{
		scriptFinder = new Thread(FindShareScript);
		scriptFinder.IsBackground = true;
		scriptFinder.Start();
	}

	public static void FindShareScript()
	{
		Thread.Sleep(1000);
		while (state.IsInDesigner)
		{
			Thread.Sleep(500);
			Screenshot4Upload.l++;
			//if (Screenshot4Upload.script != null && Screenshot4Upload.script.RealObject != null)
			//	continue;
			object o = FindObjectOfType(ShareAircraftScript.RealType);
			if (o == null)
			{
				Screenshot4Upload.script = null;
				continue;
			}
			Screenshot4Upload.script = ShareAircraftScript.Wrap(o);
		}
	}

	public Screenshot4Upload()
	{
	}

	private void Update()
	{
		if (state.IsInDesigner)
		{
			if (scriptFinder == null)
				StartFinder();
			else if (!scriptFinder.IsAlive)
				scriptFinder.Start();
		}
		else
			KillFinder();
	}

	static Vector2 oldPosition1, oldPosition2, oldPosition3;
	static Touch t1, t2, t3;
	static float touchStart;

	void LateUpdate()
	{
		if (state.IsInDesigner)
			if (script != null && script.RealObject != null)
			{
				if (!scriptEnabled)
				{
					scriptEnabled = true;
					if (screenshots.Count > 0)
					{
						hackMessage += "adding to menu\r\n";
						script.custom_AddScreenshots(screenshots);
						hackMessage += "added to menu\r\n";
					}
				}
			}
			else
				scriptEnabled = false;
		else
		{
			if (isAndroid)
			{
				if (Input.touchCount == 3)
				{
					if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began || Input.GetTouch(2).phase == TouchPhase.Began)
					{
						touchStart = Time.unscaledTime;
					}
				}
				else if(Input.touchCount==0)
				{
					if (Time.unscaledTime - touchStart < 0.2f)
						snapRequested = true;
				}
			}
			else
			{
				if (Input.GetKeyDown(KeyCode.F12))
					snapRequested = true;
			}
		}
	}

	void GetFreeCamPos()
	{
		if (FreeCamT == null || FreeCamEnabled == null)
			return;
		FreenCam = FindObjectOfType(FreeCamType);
			if (!(bool)FreeCamEnabled.GetValue(FreenCam)) return;
		Transform t = FreeCamT.GetValue(FreenCam) as Transform;
		if (t == null)
			return;
		cam1.transform.position = t.position;
		cam1.transform.rotation = t.rotation;
		if (usePlaneCam)
		{
			cam2.transform.position = t.position;
			cam2.transform.rotation = t.rotation;
		}
	}

	private void trys()
	{
		if (IsInLevel && snapRequested)
		{
			new WaitForEndOfFrame();
			snapMessage += "begin\r\n";
			snapRequested = false;
			usePlaneCam = CameraManagerScript.Instance._planeCamera.gameObject.activeSelf;
			cam1.CopyFrom(CameraManagerScript.Instance._mainCamera);
			GetFreeCamPos();
			//cam1 = UnityEngine.Object.Instantiate<Camera>(Camera.main);
			IEnumerator i = cam1.transform.GetEnumerator();
			try
			{
				while (i.MoveNext())
					((Transform)i.Current).gameObject.SetActive(false);
			}
			finally
			{
				IDisposable disposable = i as IDisposable;
				if (disposable != null)
					disposable.Dispose();
			}

			RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
			RenderTexture targetTexture1 = cam1.targetTexture;
			RenderTexture targetTexture2 = null; ;
			rt.anisoLevel = 8;
			rt.antiAliasing = 8;
			cam1.targetTexture = rt;
			cam1.Render();
			snapMessage += "cam1 rendered\r\n";
			if (usePlaneCam)
			{
				cam2.CopyFrom(CameraManagerScript.Instance._planeCamera);
				targetTexture2 = cam2.targetTexture;
				cam2.targetTexture = rt;
				cam2.Render();
			}
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = rt;
			Texture2D t = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
			t.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
			t.Apply();
			snapMessage += "save to texture2d\r\n";
			if (screenshots.Count == 3)
				screenshots.RemoveAt(0);
			screenshots.Add(t);

			snapMessage += "added:\t" + screenshots.Count + "\r\n";
			cam1.targetTexture = targetTexture1;
			if (usePlaneCam)
			{
				cam2.targetTexture = targetTexture2;
			}
			RenderTexture.active = active;
		}
	}

	static string snapMessage = "";
	static string hackMessage = "";
	public static long l = 0;

	private void OnGUI()
	{
		//GUI.Label(new Rect(550, 250, 250, 350), "in designer: " + state.IsInDesigner + "\r\nin level:" + IsInLevel + "\r\nScript valid:" + (script != null ? ("" + (script.RealObject != null)) : ""));
		trys();
		//if (IsInLevel) return;
		//GUI.Label(new Rect(50, 50, 250, 350), snapMessage);
		//GUI.Label(new Rect(350, 50, 250, 350), hackMessage);
		//GUI.Label(new Rect(550, 50, 250, 350), "l: " + l + "\r\nIsInDesigner:" + state.IsInDesigner + "\r\nscriptFinder valid: " + (scriptFinder != null));
	}
}
