using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCreatureAction : Action {

	private CardLogic cl;
	private int tablePos;
	private Player p;
	private int creatureID;

	public PlayCreatureAction(CardLogic cl, Player p, int tablePos, int creatureID)
	{
		this.p = p;
		this.cl = cl;
		this.tablePos = tablePos;
		this.creatureID = creatureID;
	}

	public override void StartActionExecution()
	{
		// remove and destroy the card in hand
		HandVis playerHand = p.pArea.handVisual;
		GameObject card = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
		playerHand.RemoveCard(card);
		GameObject.Destroy(card);
		// enable hover previews back
		HoverOnCard.PreviewsAllowed = true;

		// Move this card to the spot
		p.pArea.tableVisual.AddCreatureAtIndex(cl.ct, creatureID, tablePos);

	}

}
