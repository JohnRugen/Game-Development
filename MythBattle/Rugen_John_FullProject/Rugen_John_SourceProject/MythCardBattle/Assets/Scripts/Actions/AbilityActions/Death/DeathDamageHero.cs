using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathDamageHero : Action {

	private int amount;
	private Player currentPlayer;


	// Amount will be the amount the hero is damaged and will be derived from AbilityLogicValue in the card asset itself.
	public DeathDamageHero(int amount, Player currentPlayer)
	{
		this.amount = amount;
		this.currentPlayer = currentPlayer;
	}

	public override void StartActionExecution()
	{
		Debug.Log("Death damaging hero for: " + amount);
		currentPlayer.otherPlayer.Health -= amount;
		ActionExecutionComplete();

	}

}
