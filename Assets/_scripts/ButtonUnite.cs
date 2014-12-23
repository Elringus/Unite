using UnityEngine;
using UnityEngine.UI;

public class ButtonUnite : MonoBehaviour
{
	private Button uniteButton;

	private void Awake () 
	{
		uniteButton = GetComponent<Button>();
		uniteButton.onClick.RemoveAllListeners();
		uniteButton.onClick.AddListener(() => GameManager.I.UniteSelectedNodes());
	}
}