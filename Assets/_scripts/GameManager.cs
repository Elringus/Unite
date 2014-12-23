using UnityEngine;
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

	private void Awake () 
	{
		Reset();
	}

	private void Update () 
	{
    	
	}

	public void Reset ()
	{
		Nodes.Clear();
		foreach (Transform node in NodeParent.transform) Destroy(node.gameObject);

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
	}

	public void UniteSelectedNodes ()
	{

	}
}