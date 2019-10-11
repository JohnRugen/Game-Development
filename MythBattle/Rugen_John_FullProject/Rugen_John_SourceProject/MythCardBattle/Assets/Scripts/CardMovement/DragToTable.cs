using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Used to drag creatures to the play table
public class DragToTable : DraggingActions {

	private int savedHandSlot;
	private WhereIsTheCard whereIsTheCard;
	private IDHolder idScript;
	private VisualStates tempSTate;
	private CardDisplay manager;

	private int manaConversion;

	public override bool CanDrag
	{
		get
		{
			return true;
		}
	}

	void Awake()
	{
		whereIsTheCard = GetComponent<WhereIsTheCard>();
		manager = GetComponent<CardDisplay>();
	}

	public override void OnStartDrag()
	{
		savedHandSlot = whereIsTheCard.Slot;
		tempSTate = whereIsTheCard.VisualState;
		whereIsTheCard.VisualState = VisualStates.Dragging;
		whereIsTheCard.BringToFront();
	}

	public override void OnDraggingInUpdate()
	{
		// testing
	}


	// Once the player has dragged a card over a valid table slot, play it. Unless the player can't play that card (mana/not their turn)
	public override void OnEndDrag()
	{
		// Store the potential mana outcome if the player were to play the current card that's getting dragged around
		manaConversion = playerOwner.CurrentMana - manager.card.manaCost;
		// Check if the drag was succesful (struck a place on the board) && mana would be valid && if it's the players turn / has control.
		if(DragSuccessful() && manaConversion >= 0 && playerOwner.CanControl)
		{
			// determine the table pos
			 int tablePos = playerOwner.pArea.tableVisual.TablePosForNewCreature(Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z - Camera.main.transform.position.z)).x);
			
			// Play the card
			playerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().UniqueID, tablePos);
		}
		else // Return the card to the players hand - REFACTOR / ADD: UI elements to explain why that card can't be added to the board.
		{
			// set old sorting order
			whereIsTheCard.SetHandSortingOrder();
			whereIsTheCard.VisualState = tempSTate;
			// move this card back to its slot pos
			HandVis PlayerHand = playerOwner.pArea.handVisual;
			Vector3 oldCardPos = PlayerHand.slots.Slots[savedHandSlot].transform.localPosition;
			transform.DOLocalMove(oldCardPos, 1f);
			// Check why the card couldn't be played
			if(!(playerOwner.CanControl))
			{
				playerOwner.pArea.changeErrorText("It isn't your turn");
			}
			else if(manaConversion < 0)
			{
				playerOwner.pArea.changeErrorText("You don't have enough mana to play this card");
			}
			else if(!DragSuccessful())
			{
				playerOwner.pArea.changeErrorText("Please drag the card to a valid position");
			}
			
		}
	}

	protected override bool DragSuccessful()
	{
		bool TableNotFull = true;
		return TableVis.CursorOverATable && TableNotFull;
	}

}
