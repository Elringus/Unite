using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HelpButton : MonoBehaviour
{
	public static HelpType SelectedHelp;

	public HelpType Type;
	public int Cooldown = 10;

	private int cooldownTimer;
	private Button helpButton;
	private Image buttonImage;
	private static Dictionary<HelpType, HelpButton> helpButtons = new Dictionary<HelpType, HelpButton>();

	private void Awake ()
	{
		helpButton = GetComponent<Button>();
		buttonImage = GetComponent<Image>();
	}

	private void OnEnable ()
	{
		helpButton.onClick.AddListener(Select);

		GameManager.OnNextTurn += AtNextTurn;
		GameManager.OnRestartGame += AtRestart;

		helpButtons.Add(Type, this);
	}

	private void OnDisable ()
	{
		helpButton.onClick.RemoveListener(Select);

		GameManager.OnNextTurn -= AtNextTurn;
		GameManager.OnRestartGame -= AtRestart;

		helpButtons.Remove(Type);
	}

	private void Update ()
	{
		buttonImage.color = SelectedHelp == Type ? Color.green : 
			Color.Lerp(buttonImage.color, new Color(1, 1 - (float)cooldownTimer / (float)Cooldown, 1 - (float)cooldownTimer / (float)Cooldown, buttonImage.color.a), Time.deltaTime);
		helpButton.interactable = cooldownTimer == 0;
	}

	private void AtNextTurn ()
	{
		cooldownTimer = (int)Mathf.Clamp(cooldownTimer - 1, 0, Mathf.Infinity);
	}

	private void AtRestart ()
	{
		cooldownTimer = 0;
		Unselect();
	}

	private void Select ()
	{
		if (SelectedHelp == Type) { Unselect(); return; }
		if (cooldownTimer != 0) return;
		Unselect();

		SelectedHelp = Type;
	}

	public static void Unselect ()
	{
		if (SelectedHelp == HelpType.None) return;

		helpButtons[SelectedHelp].buttonImage.color = Color.white;
		SelectedHelp = HelpType.None;
	}

	public static void Use (Node node)
	{
		if (SelectedHelp == HelpType.None) return;

		switch (SelectedHelp)
		{
			case HelpType.Plus:
				node.Number++;
				break;
			case HelpType.Divide:
				node.Number /= 2;
				break;
			case HelpType.Minus:
				node.Number--;
				break;
		}

		helpButtons[SelectedHelp].cooldownTimer = helpButtons[SelectedHelp].Cooldown;
		Unselect();
	}
}