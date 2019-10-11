using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour {
	// creating a list of cards
	private DeckHolder instance; // the instance of deck holder
	public List<CardTemplate> cards = new List<CardTemplate>();

	void Awake()
	{
		// Check if there's a Deck Holder object in this scene, if so load the cards from that list into the players deck
		instance = GameManager.FindObjectOfType<DeckHolder>();
		if(instance != null)
		{
			// Set the cards to the deck
			cards = instance.myDeck;
		}
		// If not, don't load anything, it'll take in the premade list I made in the scene just in case a bug happens / for the AI.
		
		
	}

	void Shuffle()
	{
		// shuffle
	}
}
