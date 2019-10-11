using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureAttack : Action {

	// pos of creature on enemies table that will be attacked
	// if enemy index is =1, attacking enemy char

	private int TargetUniqueID;
	private int AttackerUniqueID;
	private int AttackerHealthAfter;
	private int TargetHealthAfter;
	private int DamageTakenByAttacker;
	private int DamageTakenByTarget;


 	public CreatureAttack(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter)
    {
        this.TargetUniqueID = targetID;
        this.AttackerUniqueID = attackerID;
        this.AttackerHealthAfter = attackerHealthAfter;
        this.TargetHealthAfter = targetHealthAfter;
        this.DamageTakenByTarget = damageTakenByTarget;
        this.DamageTakenByAttacker = damageTakenByAttacker;
    }


	public override void StartActionExecution()
	{
		GameObject Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

		Attacker.GetComponent<CreatureAttacking>().AttackTarget(TargetUniqueID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter);
	}
}
