using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAddCard : MonoBehaviour {


	private DisplayCards dc; // the display cards ref
	private CardTemplate card; // this cards template.


	void Start()
	{
		 GameObject dcGO = GameObject.FindGameObjectWithTag("DisplayCards"); // grab the gameobject
		 dc = dcGO.GetComponent<DisplayCards>(); // finish getting the ref

		 card = GetComponent<CardDisplay>().card;
	}


	// When the card is clicked on, it gets added to the deck.
	void OnMouseDown()
	{
		// Quickly reload card to make sure we have the right one
		card = GetComponent<CardDisplay>().card;
		dc.AddCard(card);
	}
}
