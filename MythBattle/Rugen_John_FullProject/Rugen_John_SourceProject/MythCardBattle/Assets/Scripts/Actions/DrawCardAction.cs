using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The action that is required when the player draws a card, takes in a few variables and then fires GivePlayerACard in HandVis
public class DrawCardAction : Action {

	private Player p; // The player which owns this card
	private CardLogic cl; // The card logic itself
	private bool fast; // is it getting drawn fast?
	private bool fromDeck; // is it from the deck?



	// Constructor
	public DrawCardAction(CardLogic cl, Player p, bool fast, bool fromDeck)
	{
		this.cl = cl;
		this.p = p;
		this.fast = fast;
		this.fromDeck = fromDeck;
	}

	public override void StartActionExecution()
	{
		p.pArea.handVisual.GivePlayerACard(cl.ct, cl.UniqueCardID, fast, fromDeck);
	}
}
