using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour {

	// Used for the AI (top player)
	private Player thisAI;
	private bool CanIDoAnything = true;
	public void Awake()
	{
		// assign player
		thisAI = GetComponent<Player>();
		
	}

	// The start of the AIs turn
	public void AITurn()
	{
		StartCoroutine(AIDecision());
	}


	//Delays are used so that it doesn't break the game, without delays the AI moves to fast for the actions to catch up
	IEnumerator AIDecision()
	{
		CanIDoAnything = true;
		yield return new WaitForSeconds (2f);

		while(CanIDoAnything)
		{
			yield return new WaitForSeconds (1f);
			if(!PlayCard()) // Play cards until it can't
			{
				yield return new WaitForSeconds (1f);
				if(!Attack()) // Attack until it can't
				{
					yield return new WaitForSeconds (1f);
					CanIDoAnything = false; // it finally can't do anything
				}
			}

		}
	}

	bool PlayCard()
	{
		// For all cards in the current AIs hand
		foreach(CardLogic cardLogic in thisAI.hand.cardsInHand)
		{
			if(cardLogic.CanBePlayed == true) // Can the card be played?
			{
				thisAI.PlayACreatureFromHand(cardLogic, 0); // play a card
				return true; // has been played
			}
		}
		return false; // Card cannot be played / no cards in the AIs hand
	}


	// The AI attacking with one of their played creatures
	bool Attack()
	{
		// For all the creatures this AI has got on the table
		foreach(CreatureLogic creature in thisAI.table.CreaturesOnTable)
		{
			if(creature.AttacksLeftThisTurn > 0)
			{
				// Check if there's any creatures on the players side of the board
				if(thisAI.otherPlayer.table.CreaturesOnTable.Count >0)
				{
					int randomPicker = Random.Range(0, thisAI.otherPlayer.table.CreaturesOnTable.Count);
					CreatureLogic chosenEnemy = thisAI.otherPlayer.table.CreaturesOnTable[randomPicker];
					creature.AttackCreature(chosenEnemy);
				}
				else
				{
					creature.GoFace();
				}
				// Has attacked something
				return true;
			}
		}
		// Couldn't attack
		return false;
	}

}
