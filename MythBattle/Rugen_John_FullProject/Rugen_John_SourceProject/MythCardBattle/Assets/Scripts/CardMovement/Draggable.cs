using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Cards are dragabble when this is attatched
public class Draggable : MonoBehaviour {

	private bool dragging = false;


	// This allows the card to be dragged without the cursor being snapped to the middle
	private Vector3 pointerDisplacement;

	// Distance from camera to mouse (z)
	private float zDisplacement;
	
	 private DraggingActions da;

	private static Draggable _draggingThis;

	public static Draggable DraggingThis
	{
		get{return _draggingThis;}
	}
	
	void Awake()
	{
		da = GetComponent<DraggingActions>();
	}

	void OnMouseDown()
	{
		if(da!=null && da.CanDrag)
		{
		dragging = true;
		//when we are dragging something all previews should be off
		HoverOnCard.PreviewsAllowed = false;
		_draggingThis = this;
		da.OnStartDrag();
		zDisplacement = -Camera.main.transform.position.z + transform.position.z;
		pointerDisplacement = -transform.position + MouseInWorldCoords();
		}
	}

	void Update()
	{
		if(dragging)
		{
			Vector3 mousePos = MouseInWorldCoords();
			transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y, transform.position.z);
			da.OnDraggingInUpdate();
		}
	}

	void OnMouseUp()
	{
		if(dragging)
		{
			dragging = false;
			// turn previews back on
			HoverOnCard.PreviewsAllowed = true;
			_draggingThis = null;
			da.OnEndDrag();
		}
	}

	// returns mouse pos in world coords
	private Vector3 MouseInWorldCoords()
	{
		var screenMousePos = Input.mousePosition;
		screenMousePos.z = zDisplacement;
		return Camera.main.ScreenToWorldPoint(screenMousePos);
	}
}
