using UnityEngine;
using Random = UnityEngine.Random;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
	#region SINGLETON
	private static GameManager _instance;
	public static GameManager I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(GameManager)) as GameManager;
			return _instance;
		}
	}

	private void OnApplicationQuit () { _instance = null; }
	#endregion

	public GameObject NodePrefab;
	public GameObject NodeParent;

	public Vector2 GridSize = new Vector2(7, 7);
	public int InitialNumberCount = 5;
	public Vector2 InitialNumberInterval = new Vector2(1, 3);
	public Vector2 InitialTargetInterval = new Vector2(4, 8);
	public float TargetMultiplicator = 1.25f;

	public delegate void NextTurn ();
	public static event NextTurn OnNextTurn;
	public delegate void RestartGame ();
	public static event RestartGame OnRestartGame;

	[HideInInspector]
	public List<int> Targets = new List<int>(3);
	[HideInInspector]
	public List<Node> Nodes = new List<Node>();
	[HideInInspector]
	public int Turn = 1;
	public float SelectedNumber
	{
		get
		{
			var node = Nodes.Find(n => n.Selected && n.Number != 0);
			return node == null ? 0 : node.Number;
		}
	}
	[HideInInspector]
	public Node FirstSelectedNode;
	[HideInInspector]
	public Node LastSelectedNode;
	[HideInInspector]
	public Node LastSelectedNotNullNode;

	private void Awake ()
	{
		ProcessRestart();
	}

	public void ProcessRestart ()
	{
		Nodes.Clear();
		foreach (Transform node in NodeParent.transform) Destroy(node.gameObject);

		Targets.Clear();
		for (int i = 0; i < 3; i++)
			Targets.Add(RandomExtension.RangeExcluded((int)InitialTargetInterval.x, (int)InitialTargetInterval.y, Targets.ToArray()));

		for (int x = 0; x < GridSize.x; x++)
			for (int y = 0; y < GridSize.y; y++)
			{
				var node = (Instantiate(NodePrefab) as GameObject).GetComponent<Node>();
				node.Position = new Vector2(x, y);
				node.transform.parent = NodeParent.transform;

				Nodes.Add(node);
			}

		for (int i = 0; i < InitialNumberCount; i++)
			Nodes.FindAll(n => n.Number == 0).Random().Number = Random.Range((int)InitialNumberInterval.x, (int)InitialNumberInterval.y + 1);

		OnRestartGame();
	}

	public void UnselectAllNodes ()
	{
		foreach (var node in Nodes) node.Selected = false;
		FirstSelectedNode = null;
		LastSelectedNode = null;
		LastSelectedNotNullNode = null;
	}

	public void UniteSelectedNodes ()
	{
		var selectedNodes = Nodes.FindAll(n => n.Selected);

		if (selectedNodes.Count == 0) return;

		// case we selected one number and a null node => duplicate the number
		if (selectedNodes.FindAll(n => n.Number != 0).Count == 1)
		{
			LastSelectedNode.Number = selectedNodes.Find(n => n.Number != 0).Number;
			UnselectAllNodes();
			ProcessTurn();
			return;
		}

		// case we selected several equal nodes => summ them to the last selected not null, null all nodes except first selected
		LastSelectedNotNullNode.Number = selectedNodes.FindAll(n => n.Number != 0).Sum(n => n.Number);
		if (Targets.Contains(LastSelectedNotNullNode.Number)) CompleteTarget(LastSelectedNotNullNode.Number);
		else selectedNodes.Remove(FirstSelectedNode);
		selectedNodes.Remove(LastSelectedNotNullNode); // we now keep last selected number, even if we completed the target

		foreach (var node in selectedNodes) node.Number = 0;
		UnselectAllNodes();

		ProcessTurn();
	}

	private void ProcessTurn ()
	{
		Turn++;
		OnNextTurn();
	}

	public void CompleteTarget (int targetNumber)
	{
		if (!Targets.Contains(targetNumber)) return;

		int newTarget = Mathf.CeilToInt(targetNumber * TargetMultiplicator);
		while (Targets.Exists(t => t == newTarget) || Nodes.Exists(n => n.Number == newTarget))
			newTarget = Mathf.CeilToInt(newTarget * TargetMultiplicator);
		Targets[Targets.IndexOf(targetNumber)] = newTarget;
	}
}