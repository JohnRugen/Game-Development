using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CardLogic : IIdentifiable {

	// Owning player stored here
	public Player owner;
	// This cards unique ID
	public int UniqueCardID;
	public CardTemplate ct;

	private int baseManaCost;
	//public SpellEffect effect;

	// Managing IDs, creating a dictionary that takes in ID and card log and stores it
	public static Dictionary<int, CardLogic> CardsCreatedThisGame = new Dictionary<int, CardLogic>();

	public int ID
	{
		get{return UniqueCardID;}
	}

	public int CurrentManaCost{get; set;}

	public bool CanBePlayed
	{
		get
		{
			bool ownersTurn = (TurnManager.Instance.WhichPlayersTurn == owner);  // assign the owner of the card
			return ownersTurn && (CurrentManaCost <= owner.CurrentMana);		 // check if the owner has enough mana
		}
	}

	// Constructor
	public CardLogic(CardTemplate ct)
	{
		// Set the refs
		this.ct = ct;
		// Get unique int ID
		UniqueCardID = IDCreator.GetUniqueID();
		ResetManaCost();
		// Add this card to the dictionary with its ID as key
		CardsCreatedThisGame.Add(UniqueCardID, this);
	}

	// Reset mana cost
	public void ResetManaCost()
	{
		CurrentManaCost = ct.manaCost;
	}

	//public int CurrentManaCost{get; set}


}
