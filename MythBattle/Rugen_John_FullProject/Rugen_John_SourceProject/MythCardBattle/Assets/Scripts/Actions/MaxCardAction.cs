using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The action that is done when the player has a full hand, essentially the same as draw card, except fires a different function in HandVis.
public class MaxCardAction : Action {

	private Player p; // The player which owns this card
	private CardLogic cl; // The card logic itself
	private bool fast; // is it getting drawn fast?
	private bool fromDeck; // is it from the deck?



	// Constructor

	public MaxCardAction(CardLogic cl, Player p, bool fast, bool fromDeck)
	{
		this.cl = cl;
		this.p = p;
		this.fast = fast;
		this.fromDeck = fromDeck;
	}

	public override void StartActionExecution()
	{
		p.pArea.handVisual.VisuallyShowCard(cl.ct, cl.UniqueCardID, fast, fromDeck);
	}
}
