using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Switching turns and counting down time until the turn expires
public class TurnManager : MonoBehaviour {

	public static TurnManager Instance;

	private TurnTimer timer;

	// Store both players into variable
	private GameObject p1, p2;



	// Used to randomise start
	private int randomDecider = 0;


	[SerializeField]
	// Who's turn is it?
	private Player whichPlayersTurn;
	public Player WhichPlayersTurn
	{
		get
		{
			return whichPlayersTurn;
		}
		set
		{
			whichPlayersTurn = value;
			whichPlayersTurn.HighlightCards();
		}
	}
	void Awake()
	{
		Instance = this;
		timer = GetComponent<TurnTimer>();
	}
	void Start()
	{
		StartCoroutine(OnGameStart());
	}

	IEnumerator OnGameStart()
	{
		

		// Clear history of cards, this will be useful later for rematches
		CardLogic.CardsCreatedThisGame.Clear();
		CreatureLogic.CreaturesCreatedThisGame.Clear();

		// assign player to variables
		p1 = GameObject.FindGameObjectWithTag("P1");
		p2 = GameObject.FindGameObjectWithTag("P2");

		// set an int to 0/1 to decide who starts (0=p1|1=p2)
		randomDecider = Random.Range(0,2);

		// Assign player to variables
		Player whoGoesFirst = Player.players[randomDecider];
		Player whoGoesSecond = whoGoesFirst.otherPlayer;
		
		// Set whogoesfirst to first player
		whichPlayersTurn = whoGoesFirst;

		// Delay start by 2 seconds
		yield return new WaitForSeconds(2f);
	
		// Both Draw 4 cards
		for(int i = 0; i <4 ; i++)
		{
			whoGoesFirst.DrawASingleCard(true);
			whoGoesSecond.DrawASingleCard(true);
		}
		
		// First persons first extra card is drawn
		whoGoesFirst.DrawASingleCard(true);
		whoGoesFirst.ActivateMyTurn();

		

		timer.StartTimer();
	}

	public void StopTimer()
	{
		timer.StopTimer();
	}

	public void TurnOver()
	{
		// stop timer & switch current player (whichPlayersTurn)
		timer.StopTimer();
		if(WhichPlayersTurn == p1.GetComponent<Player>())
		{
			timer.StartTimer(); // start timer again
			WhichPlayersTurn.DeActivateMyTurn(); // Stop P1s turn bool
			WhichPlayersTurn = p2.GetComponent<Player>(); // Change to P2s turn
			whichPlayersTurn.ActivateMyTurn(); // actiave their turn bool
			whichPlayersTurn.DrawASingleCard();
		}
		else if(WhichPlayersTurn == p2.GetComponent<Player>())
		{
			timer.StartTimer(); // start timer again
			whichPlayersTurn.DeActivateMyTurn(); // Stop P2s turn bool
			WhichPlayersTurn = p1.GetComponent<Player>(); // Change to P1s turn
			whichPlayersTurn.ActivateMyTurn(); // Turn on their turn bool
			whichPlayersTurn.DrawASingleCard();
		}

	}



}
