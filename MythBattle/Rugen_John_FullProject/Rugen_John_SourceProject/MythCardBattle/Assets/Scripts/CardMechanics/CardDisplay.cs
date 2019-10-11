using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This handles the card display, cards load in a card type from my assets and then displays all correct info using this
// (*REFACTOR* - change to JSON if needed at a later date, may improve file size & speed*)
public class CardDisplay : MonoBehaviour {
	// All the refs, self explanatory
	public CardTemplate card;
	// preview, give cards a basic preview in case they don't load instantly / in editor
	public CardDisplay previewManager;
	public Text nameText;
	public Text manaCostText;
	public Text descriptionText;
	public Text abilityText;
	public Text healthText;
	public Text attackText;
	public Text alignmentText;
	[Header("Image Refs")]
	public Image cardLayout;
	public Image cardBackGlow;
	public Image cardFrontGlow;
	public Image cardFrontGraphic;



	// When gameobject is created / onstart
	void Awake()
	{
		// if there is a card set to this, load it.
		if(card != null)
		{
			LoadCard();
		}
	}

	// Play system
	private bool canCardBePlayedNow = false;


	// Constructor for other scripts, allowing the card to be set to playable
	// when set to playable, enable glow to show it can be played
	public bool CanCardBePlayedNow
	{
		get
		{
			return canCardBePlayedNow;
		}
		set
		{
			canCardBePlayedNow = value;
			cardFrontGlow.enabled = value;
		}
	}

	public void LoadCard()
	{
		// Load the card, done on awake
		// Name
		nameText.text = card.name;
		//mana
		manaCostText.text = card.manaCost.ToString();
		// Description
		descriptionText.text = card.description;
		// Ability Text
		abilityText.text = card.ability;
		// add image
		cardFrontGraphic.sprite = card.cardImage;
		// Check if creature (spells don't have health/atck/alignment)
		if(card.maxHealth != 0)
		{
			attackText.text = card.attack.ToString();
			healthText.text = card.maxHealth.ToString();
			alignmentText.text = card.alignment.ToString();
		}

		if(previewManager != null)
		{
			// this is a card not a preview
			previewManager.card = card;
			previewManager.LoadCard();
		}
	}
	
}
