using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour {


	private DeckHolder instance; // the instance of deck holder
	public List<CardTemplate> cards = new List<CardTemplate>();

	void Awake()
	{
		// Set the cards to the deck
		cards = instance.myDeck;
	}

	void Shuffle()
	{
		// shuffle
	}
}
