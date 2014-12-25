using UnityEngine;
using UnityEngine.UI;

public class ButtonRestart : MonoBehaviour
{
	private Button restartButton;

	private void Awake () 
	{
		restartButton = GetComponent<Button>();
		restartButton.onClick.RemoveAllListeners();
		restartButton.onClick.AddListener(() => GameManager.I.ProcessRestart());
	}
}