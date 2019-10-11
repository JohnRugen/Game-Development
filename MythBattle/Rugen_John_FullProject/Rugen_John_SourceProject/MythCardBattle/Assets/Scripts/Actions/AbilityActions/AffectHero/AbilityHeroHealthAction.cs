using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHeroHealthAction : Action {

	private int amount;
	private Player currentPlayer;


	// Amount will be the amount the hero is damaged and will be derived from AbilityLogicValue in the card asset itself.
	public AbilityHeroHealthAction(int amount, Player currentPlayer)
	{
		this.amount = amount;
		this.currentPlayer = currentPlayer;
	}

	public override void StartActionExecution()
	{
		Debug.Log("Adding health: " + amount);
		currentPlayer.Health += amount;
		ActionExecutionComplete();

	}
}
