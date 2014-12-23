using UnityEngine;

public class Node : MonoBehaviour
{
	private int _number;
	public int Number
	{
		get { return _number; }
		set 
		{
			if (value == _number) return;
			textMesh.text = value == 0 ? string.Empty : value.ToString();
			_number = value; 
		}
	}

	private Vector2 _position;
	public Vector2 Position 
	{
		get { return _position; }
		set
		{
			if (_position == value) return;
			myTransform.position = new Vector3(value.x, value.y, 0);
			_position = value;
		}
	}

	private Transform myTransform;
	private TextMesh textMesh;

	private void Awake () 
	{
		myTransform = transform;
		textMesh = GetComponentInChildren<TextMesh>();
	}

	private void Update () 
	{
    	
	}
}