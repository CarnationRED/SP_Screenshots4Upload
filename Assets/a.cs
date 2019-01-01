using UnityEngine;
using System.Collections;

public class a : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screenshot4Upload.IsInLevel = true;
	}

	private void OnDestroy()
	{
		Screenshot4Upload.IsInLevel = false;
	}
}
