using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerVisual : MonoBehaviour {

	// Contains the visual aspects of the player!
	public Text myAlignment;
	public Text myHealth;

	private int myCurrentAlignment;
	private int myCurrentHealth;
	private int myID;

	void Awake()
	{
		if(gameObject.tag == "BottomPlayer") // if it's the player
		{
			myID = 1; // set local ID var
			IDHolder myIDHolder = gameObject.AddComponent<IDHolder>(); // add ID holder script
			myIDHolder.UniqueID = myID; // set ID in holder script to lcoal ID
		}
		else if(gameObject.tag == "TopPlayer") // if it's the enemy 
		{
			myID = 2; // Set local ID var
			IDHolder myIDHolder = gameObject.AddComponent<IDHolder>(); // add ID holder script
			myIDHolder.UniqueID = myID; // set ID in holder script to local ID
		}
		
	}

	public int MyCurrentAlignment
	{
        get
        {
            return myCurrentAlignment;
        }


        set
        {
			// set new value & change text
			myCurrentAlignment = value;
			myAlignment.text = myCurrentAlignment.ToString();
        }
	}

	public int MyCurrentHealth
	{
		get
		{
			return myCurrentHealth;
		}
		set
		{
			// Set new value & change text 
			myCurrentHealth = value;
			myHealth.text = myCurrentHealth.ToString();
		}
	}

}
