using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAction : Action{

	private Player p;

	private int DeadCreatureID;

	public DieAction(int CreatureID, Player p)
	{
		this.p = p;
		this.DeadCreatureID = CreatureID;
	}

	public override void StartActionExecution()
	{
		p.pArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
	}
}
