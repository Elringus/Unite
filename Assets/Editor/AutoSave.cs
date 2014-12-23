using UnityEditor;

[InitializeOnLoad]
public class AutoSave
{
	static AutoSave ()
	{
		EditorApplication.playmodeStateChanged = () =>
		{
			if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
			{
				EditorApplication.SaveScene();
				EditorApplication.SaveAssets();
			}
		};
	}
}