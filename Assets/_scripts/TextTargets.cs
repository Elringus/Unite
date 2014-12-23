using UnityEngine;
using UnityEngine.UI;

public class TextTargets : MonoBehaviour
{
	private Text targetsText;
	private int[] cachedTargets = new int[3];

	private void Awake () 
	{
		targetsText = GetComponent<Text>();
	}

	private void Update () 
	{
		if (cachedTargets != GameManager.I.Targets.ToArray())
		{
			cachedTargets = GameManager.I.Targets.ToArray();
			targetsText.text = string.Format("{0} {1} {2}", cachedTargets[0], cachedTargets[1], cachedTargets[2]);
		}
	}
}