﻿using UnityEngine;
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

	[HideInInspector]
	public List<Node> Nodes = new List<Node>();
	public float SelectedNumber
	{
		get
		{
			var node = Nodes.Find(n => n.Selected && n.Number != 0);
			return node == null ? 0 : node.Number;
		}
	}
	public Node FirstSelectedNode;
	public Node LastSelectedNode;
	public Node LastSelectedNotNullNode;

	public List<int> Targets = new List<int>(3);

	private int turn = 1;

	private void Awake () 
	{
		Restart();
	}

	public void Restart ()
	{
		Nodes.Clear();
		foreach (Transform node in NodeParent.transform) Destroy(node.gameObject);

		Targets.Clear();
		for (int i = 0; i < 3; i++) Targets.Add(Random.Range(2, 9));

		for (int x = 1; x <= 10; x++)
			for (int y = 1; y <= 10; y++)
			{
				var node = (Instantiate(NodePrefab) as GameObject).GetComponent<Node>();
				if (Random.Range(0, 10) == 0) node.Number = Random.Range(0, 6);
				node.Position = new Vector2(x, y);
				node.transform.parent = NodeParent.transform;

				Nodes.Add(node);
			}
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
			return;
		}

		// case we selected several equal nodes => summ them to the last selected not null, null all nodes except first selected
		LastSelectedNotNullNode.Number = selectedNodes.FindAll(n => n.Number != 0).Sum(n => n.Number);
		if (Targets.Contains(LastSelectedNotNullNode.Number))
		{
			turn++;
			Targets[Targets.IndexOf(LastSelectedNotNullNode.Number)] = Random.Range(2 + turn, 9 + turn);
		}
		else
		{
			selectedNodes.Remove(FirstSelectedNode);
			selectedNodes.Remove(LastSelectedNotNullNode);
		}
		foreach (var node in selectedNodes) node.Number = 0;
		UnselectAllNodes();
	}
}