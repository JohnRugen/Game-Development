using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour {

	// Make a list of cards for the table
	 public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();


	 // Place creature at index, using creatureLogic to find out which creature it is (card)
	public void PlaceCreatureAt(int index, CreatureLogic creature)
	{
		CreaturesOnTable.Insert(index, creature);
	}
}
