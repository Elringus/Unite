﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
	#region SINGLETON
	private static CameraController _instance;
	public static CameraController I
	{
		get
		{
			if (_instance == null) _instance = FindObjectOfType(typeof(CameraController)) as CameraController;
			return _instance;
		}
	}
	private void OnApplicationQuit () { _instance = null; }
	#endregion

	[HideInInspector]
	public Transform Transform;
	public float InitialZoom;
	public Vector2 MinArea;
	public Vector2 MaxArea;
	public float MoveSpeed;
	public float ZoomSpeed;
	public float MinZoom;
	public float MaxZoom;

	private Camera myCamera;
	private Vector3 lastPointerPosition;
	private float prevPinch;

	private void Awake ()
	{
		Transform = transform;
		myCamera = GetComponent<Camera>();

		myCamera.orthographicSize = InitialZoom;
	}

	private void Update ()
	{
		#region MOUSE_INPUT
		if (Application.isEditor)
		{
			if (Input.GetMouseButtonDown(0)) lastPointerPosition = Input.mousePosition; // prevents camera dancing
			if (Input.GetMouseButton(0) && !CameraLocked()) 
				MoveCamera((lastPointerPosition - Input.mousePosition) * Time.deltaTime * MoveSpeed * myCamera.orthographicSize);
			lastPointerPosition = Input.mousePosition;
			ZoomCamera(Input.GetAxis("Mouse ScrollWheel"));
		}
		#endregion
		#region TOUCH_INPUT
		else if (Input.touchCount > 0)
		{
			if (Input.touchCount == 1 && !CameraLocked())
			{
				var touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Moved)
				{
					MoveCamera((lastPointerPosition - (Vector3)touch.position) * Time.deltaTime * MoveSpeed * myCamera.orthographicSize);
					lastPointerPosition = (Vector3)touch.position;
				}
			}
			else if (Input.touchCount == 2)
			{
				Touch touch1 = Input.GetTouch(0), touch2 = Input.GetTouch(1);

				// prevent camera "dancing" when remove one finger while pinching
				if (touch1.phase == TouchPhase.Ended) lastPointerPosition = (Vector3)touch2.position;
				if (touch2.phase == TouchPhase.Ended) lastPointerPosition = (Vector3)touch1.position;

				if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
				{
					float pinch = Vector2.Distance(touch1.position, touch2.position);
					if (prevPinch == 0) prevPinch = pinch;

					if (prevPinch != pinch) ZoomCamera((pinch - prevPinch) * Time.deltaTime / 10);

					prevPinch = pinch;
				}
				else prevPinch = 0;
			}
			lastPointerPosition = (Vector3)Input.GetTouch(0).position;
		}
		#endregion
	}

	private void MoveCamera (Vector3 delta)
	{
		Transform.position = new Vector3(Mathf.Clamp(Transform.position.x + delta.x, MinArea.x, MaxArea.x),
                                         Mathf.Clamp(Transform.position.y + delta.y, MinArea.y, MaxArea.y), Transform.position.z);
	}

	private void ZoomCamera (float delta)
	{
		myCamera.orthographicSize = Mathf.Clamp(myCamera.orthographicSize - delta * ZoomSpeed, MinZoom, MaxZoom);
	}

	private bool CameraLocked ()
	{
		return Application.isEditor ? EventSystem.current.IsPointerOverGameObject() : 
               Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
	}
}