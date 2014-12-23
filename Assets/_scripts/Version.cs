using System;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Version : MonoBehaviour
{
	private void Awake () 
	{
		#if UNITY_EDITOR
		if (Application.isEditor && !Application.isPlaying)
			GetComponent<Text>().text = string.Format("pre-alpha ver. {0}\n{1:d.M.yyyy HH:mm:ss}", UnityEditor.PlayerSettings.bundleVersion, DateTime.Now);
		#endif
	}
}
