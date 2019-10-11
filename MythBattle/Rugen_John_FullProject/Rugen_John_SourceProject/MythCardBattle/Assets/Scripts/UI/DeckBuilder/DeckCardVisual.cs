using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckCardVisual : MonoBehaviour {

	// This class helps display the correct info for a card in the deck. (Located on the right in the deckbuilding scene)
	// Also, if it's clicked it will remove the card template and reset it's text to show it doesn't hold a card.

	public CardTemplate cardTemplate;

	public CardDisplay previewManager;


	public Text nameText, manaText;


	void Awake()
	{
		// Load the creature
		if(cardTemplate != null)
		{
			LoadCreature();
		}
		else // doesn't have a template, hide the Gameobject
		{
			ShowOrHide();
		}
	}



	public void LoadCreature()
	{
		// Load the cards needed values.
		nameText.text = cardTemplate.name;
		manaText.text = cardTemplate.manaCost.ToString();
		
		if(previewManager != null)
		{
			// the preview card will need to take the values from our card
			previewManager.card = cardTemplate;
			previewManager.LoadCard();
		}

		ShowOrHide();
	}



	void OnMouseDown()
	{
		gameObject.GetComponent<DeckCardVisual>().cardTemplate = null;
		nameText.text = "No Card";
		manaText.text = "-";
		Debug.Log("CLICKED");
		ShowOrHide();
	}


	// Hide or show depending on if it contains an actual card
	void ShowOrHide()
	{
		// if there's no card
		if(cardTemplate == null)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}
	}
}
