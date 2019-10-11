using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : Action {

	private int targetID;
	private int amount;
	private int healthAfter;

	public DamageAction(int targetID, int amount, int healthAfter)
	{
		this.targetID = targetID;
		this.amount = amount;
		this.healthAfter = healthAfter;
	}

	public override void StartActionExecution()
	{
		Debug.Log("Deal damage command");

		GameObject target = IDHolder.GetGameObjectWithID(targetID);
		// Is the target a player?
		if(targetID == GameManager.Instance.BottomPlayer.PlayerID || targetID == GameManager.Instance.TopPlayer.PlayerID)
		{
			Debug.Log("Add hitting player - DealDamageAction");
		}
		// Target is a creature
		else
		{
			target.GetComponent<PlayedCreatureDisplay>().TakeDamage(amount, healthAfter);
		}
		ActionExecutionComplete();
	}





}

