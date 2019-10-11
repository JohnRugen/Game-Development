using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// A list of all the possible places a visual representation of a card could be
// low = bottom player (p1)
// top = top player (p2)
public enum VisualStates
{
	Transition,
	LowHand,
	TopHand,
	LowTable,
	TopTable,
	Dragging
}
public class WhereIsTheCard : MonoBehaviour {


	private HoverOnCard hover;

	// ref to a canvas ont his object to set sorting order
	private Canvas canvas;

	// a value for canvas sorting order when we want to show this object above everything
	private int TopSortingOrder = 500;

	// properties

	private int slot = -1;

	public int Slot
	{
		get{return slot;}
		set
		{
			slot = value;
		}
	}

	[SerializeField]
	private VisualStates state;
	// Stop the card getting highlighted in areas it shouldn't
	public VisualStates VisualState
	{
		get{return state;}
		set
		{
			state = value;
			switch(state)
			{
				case VisualStates.LowHand:
					hover.ThisPreviewEnabled = true;
					gameObject.tag = "BottomCard";
					break;
				case VisualStates.LowTable:
				case VisualStates.TopTable:
					hover.ThisPreviewEnabled = true;
					break;
				case VisualStates.Transition:
					hover.ThisPreviewEnabled = false;
					break;
				case VisualStates.Dragging:
					hover.ThisPreviewEnabled = false;
					break;
				case VisualStates.TopHand:
					hover.ThisPreviewEnabled = true;
					gameObject.tag = "TopCard";
					break;

			}
		}
	}

	void Awake()
	{
		hover = GetComponent<HoverOnCard>();
		// for played cards
		if(hover == null)
		{
			hover = GetComponentInChildren<HoverOnCard>();
		}
		canvas = GetComponentInChildren<Canvas>();
	}


	public void BringToFront()
	{
		canvas.sortingOrder = TopSortingOrder;
		canvas.sortingLayerName = "AboveEverything";
	}

	// not setting order inside of visalStates because when the card is drawn we want to set an index first and set the sorting order only when the card arrives to the hand

	public void SetHandSortingOrder()
	{
		if(slot != -1)
		{
			canvas.sortingOrder = HandSortingOrder(slot);
		}
		canvas.sortingLayerName = "Cards";
	}

	public void SetTableSortingOrder()
	{
		canvas.sortingOrder = 0;
		canvas.sortingLayerName = "Creatures";
	}

	private int HandSortingOrder(int  placeInHand)
	{
		return((placeInHand + 1) * 10);
	}

}
