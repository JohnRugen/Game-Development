using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// used for creatures that have been played
public class PlayedCreatureDisplay : MonoBehaviour {

	public CardTemplate cardTemplate;

	public CardDisplay previewManager;

	public Text HealthText;
	public Text AttackText;
	//public Text DescriptionText;
	//public Text NameText;
	//public Text AbilityText;

	public Image CreatureImage;
	public Image CreatureGlowImage;
	//public Text ManaText;
	//public Text AlignmentText;

	public Image AbilityImage;

	void Awake()
	{
		// Load the creature
		if(cardTemplate != null)
		{
			LoadCreature();
		}
	}

	// Default, can't attack
	private bool canAttackNow = false;

	public bool CanAttackNow
	{
		get
		{
			return canAttackNow;
		}
		set
		{
			// change the attack to value being set
			canAttackNow = value;

			// enable the creatures glow
			CreatureGlowImage.enabled = value;
		}
	}

	public void LoadCreature()
	{
		// Load the cards values.
		CreatureImage.sprite = cardTemplate.cardImage;
		AttackText.text = cardTemplate.attack.ToString();
		HealthText.text = cardTemplate.maxHealth.ToString();


		if(previewManager != null)
		{
			// the preview card will need to take the values from our card
			previewManager.card = cardTemplate;
			previewManager.LoadCard();
		}

		// check if the card has an ability
		if(cardTemplate.abilityAction != AbilityActionList.None)
		{
			// enable the ability image on the played card (for players to know this card has an ability)
			AbilityImage.enabled = true;
		}
	}

	public void TakeDamage(int amount, int healthAfter)
	{
		// If card actually takes damage
		if(amount >0)
		{
			// Add effects here
			HealthText.text = healthAfter.ToString();
		}
	}

	
}
