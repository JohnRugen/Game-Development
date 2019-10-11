using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckHolder : MonoBehaviour {

	// This holds the users currently selected deck. - Move over to firebase after AI is implemented.

	public List<CardTemplate>  myDeck; // the list containing the players deck

	private static DeckHolder Instance; // instance of this script


	void Awake()
	{
		if(Instance!=null)
		{
			GameObject.Destroy(this);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(this);
		}
	}


	public void UpdateDeck(CardTemplate card)
	{
		myDeck.Add(card);
	}

	public void ClearDeck()
	{
		myDeck.Clear();
	}



}
