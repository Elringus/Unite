using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler, IPointerClickHandler
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
			if (value == _position) return;
			myTransform.position = new Vector3(value.x, value.y, 0);
			_position = value;
		}
	}

	private bool _selected;
	public bool Selected
	{
		get { return _selected; }
		set
		{
			if (value == _selected) return;
			sprite.color = value ? Color.yellow : Color.white;
			_selected = value;
		}
	}

	private Transform myTransform;
	private SpriteRenderer sprite;
	private TextMesh textMesh;

	private void Awake ()
	{
		myTransform = transform;
		sprite = GetComponent<SpriteRenderer>();
		textMesh = GetComponentInChildren<TextMesh>();
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		// check that we are actually swiping
		if (Application.isEditor && !Input.GetMouseButton(0)) return;
		if (!Application.isEditor && Input.touchCount > 0 && Input.touches[0].phase != TouchPhase.Moved) return;

		// check that we have already selected some not null node before
		var selectedNumber = GameManager.I.SelectedNumber;
		if (selectedNumber == 0) return;

		// check that this node is properly connected to the previous node
		if (!GameManager.I.Nodes.Exists(n => n.Selected && Vector2.Distance(n.Position, Position) == 1)) return;

		// check that this node number is either null or equal to the previously selected ones
		if (Number != 0 && Number != selectedNumber)
		{
			GameManager.I.UnselectAllNodes();
			return;
		}

		Selected = true;
		if (Number != 0) GameManager.I.LastSelectedNotNullNode = this;
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		GameManager.I.UnselectAllNodes();

		if (Number == 0) return;

		Selected = true;
		GameManager.I.FirstSelectedNode = this;
	}

	public void OnDrag (PointerEventData eventData)
	{

	}

	public void OnDrop (PointerEventData eventData)
	{
		GameManager.I.LastSelectedNode = this;
		if (Number != 0) GameManager.I.LastSelectedNotNullNode = this;
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (Number != 0) HelpButton.Use(this);
	}
}