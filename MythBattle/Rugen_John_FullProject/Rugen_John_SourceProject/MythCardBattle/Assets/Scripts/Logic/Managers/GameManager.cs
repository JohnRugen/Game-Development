using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The game manager, links most things and holds the game togetehr
public class GameManager : MonoBehaviour {

	// Ref both players - just use one below?
	//public Player player1, player2;
	public GameObject CreatureCardPrefab;

	public static GameManager Instance;
	public float CardTransitionTimes = 1f;
	public float CardPreviewTime = 1f;

	public GameObject PlayedCreaturePrefab;

	public Player TopPlayer, BottomPlayer;
	public Dictionary<AreaPositions, Player> Players = new Dictionary<AreaPositions, Player>();



	void Awake()
	{
		Players.Add(AreaPositions.Top, TopPlayer);
		Players.Add(AreaPositions.Bottom, BottomPlayer);
		Instance = this;
	}

	public bool CanControlThisPlayer(AreaPositions owner)
	{
		bool playersTurn = (TurnManager.Instance.WhichPlayersTurn == Players[owner]);
		bool notDrawingCards = !Action.CardDrawPending();
		return Players[owner].pArea.InPlay && Players[owner].pArea.ControlsON && playersTurn && notDrawingCards;
	}

	 public bool CanControlThisPlayer(Player ownerPlayer)
    {
        bool PlayersTurn = (TurnManager.Instance.WhichPlayersTurn == ownerPlayer);
        bool NotDrawingAnyCards = !Action.CardDrawPending();
        return ownerPlayer.pArea.InPlay && ownerPlayer.pArea.ControlsON && PlayersTurn && NotDrawingAnyCards;
    }


}
