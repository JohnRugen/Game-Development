using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCards : MonoBehaviour {

	// This class belongs in teh deckbuilder scene. It displays cards so that players can chose/unlock them.

	public CardDisplay[] displaySlots; // All the display slots

	public CardTemplate[] cardTemplates; // All card templates currently in the game

	public Text pageDisplay; // Text element to show the user which page they are on
	private int pageNumber; // Private tracker

	private float maxPageNumber; // the total amount of pages

	public DeckCardVisual[] DeckCard;

	private DeckHolder instance; // the instance of deck holder


	public void Start()
	{
		pageNumber = 1; // make sure page starts off at 1
		maxPageNumber = (cardTemplates.Length / displaySlots.Length)+1; // Finding out how many pages are needed to display all the cards. | Add one for safety.

		pageDisplay.text = (pageNumber + "/" + maxPageNumber); // display this

		DisplayNewCards(pageNumber);

		instance = GameObject.FindObjectOfType<DeckHolder>(); // assign deck holder to instance
	}

	public void NextPage()
	{
		if(pageNumber != maxPageNumber)
		{
            pageNumber++; // Increment page number
            pageDisplay.text = (pageNumber + "/" + maxPageNumber); //reflect the new page in the text element
            DisplayNewCards(pageNumber);
		}
	}


	public void PreviousPage()
	{
		if(pageNumber!= 1)
		{
            pageNumber--; // decrease page number
            pageDisplay.text = (pageNumber + "/" + maxPageNumber); // reflect the new page in the text element
            DisplayNewCards(pageNumber);
		}
	}


	private void DisplayNewCards(int pageNo)
	{
		int pageDifference = (pageNo-1)*displaySlots.Length; // Used to split the card template array up into the page difference
		for(int i = 0; i < displaySlots.Length; i++) // loop through to display 6 (or whatever displaySlots length is) cards
		{
			if(i+pageDifference < cardTemplates.Length) // Stops the array becoming out of bounds
			{
				displaySlots[i].gameObject.SetActive(true); // set active, in case it wasn't.
                displaySlots[i].card = cardTemplates[i + pageDifference]; // set the card
                displaySlots[i].LoadCard(); // load the card
            }
			else
			{
				// Hide the cards that don't need to be displayed (ones after the last card)
				displaySlots[i].gameObject.SetActive(false);
			}
			
		}
	}


	// Add the selected card to the next empty deck card visual
	public void AddCard(CardTemplate card)
	{
		// Loop through all the deck cards
		for(int i = 0; i<DeckCard.Length; i++)
		{
			// if there is a deck card with a null template (empty card)
			if(DeckCard[i].cardTemplate == null)
			{
                // add the card to that specific deck card
                DeckCardVisual dcv = DeckCard[i].GetComponent<DeckCardVisual>();
                dcv.cardTemplate = card;
                dcv.LoadCreature(); //reload card
				// Exit loop
				i = DeckCard.Length+1;
            }
		}
	}


	// Stores the templates to an array
	public void UpdateDeck()
	{
		instance.ClearDeck(); // clear the deck

		// Loop through all the deck cards
		for(int i = 0; i<DeckCard.Length; i++)
		{
			// If the current deck card has a valid template, add it to the array
			if(DeckCard[i].cardTemplate != null)
			{
				instance.UpdateDeck(DeckCard[i].cardTemplate); // update the deck with the current card.
			}
		}

	}

}
