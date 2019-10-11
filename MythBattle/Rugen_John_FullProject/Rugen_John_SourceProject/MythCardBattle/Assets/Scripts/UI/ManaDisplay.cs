using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Show correct mana (can be used in editor for debugging)
[ExecuteInEditMode]
public class ManaDisplay : MonoBehaviour {

	// used for editor (*REFACTOR* - remove later)
	public int testManaMax;
	public int testManaCurrent;


	public Text currentMana;
	public Text maxMana;

	private int totalMana;
	public int TotalMana
	{
		get
		{
			return totalMana;
		}
		set
		{
			// if mana exceeds max, set to max
			if(value > 10) // hard coded value, change (*REFACTOR* grab value from player settings when implemented)
			{
				totalMana = 10;
			}
			else if(value < 0) // if value falls below 0, set to 0
			{
				totalMana = 0;
			}
			else // update mana to value of this
			{
				totalMana = value;
			}
			// update text
			currentMana.text = availableMana.ToString();
			maxMana.text = totalMana.ToString();
		}
	}

	private int availableMana;

	public int AvailableMana
	{
		get
		{
			return availableMana;
		}

		set
		{
			// if bigger than total mana, reset to total 
			if(value > totalMana)
			{
				availableMana = totalMana;
			}
			else if(value < 0) // reset to 0 if it falls below 0
			{
				availableMana = 0;
			}
			else // update mana to value
			{
				availableMana = value;
			}

			// update text
			currentMana.text = availableMana.ToString();
			maxMana.text = totalMana.ToString();
		}
	}

	//(*REFACTOR* - move out of update if possible)
	void Update()
	{
		if(Application.isEditor && !Application.isPlaying)
		{
			TotalMana = testManaMax;
			AvailableMana = testManaCurrent;
		}
	}
}
