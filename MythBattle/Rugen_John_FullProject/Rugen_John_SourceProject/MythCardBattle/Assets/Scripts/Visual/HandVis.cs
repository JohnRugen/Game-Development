using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// Visual script for the hand, positioning of cards & changing the cards status & where it is.
public class HandVis : MonoBehaviour {

	// Owner of this area
	public AreaPositions owner;

	// Should the player be taking cards?
	public bool TakeCardsOpenly = true;

	// Collection of slots
	public HandCardSpacing slots;

	[Header("Transform refs")]
	// Preview slot
	public Transform DrawPreviewSpot;
	// Wheres the deck (visual)
	public Transform DeckTransform;
	// If a card isn't drawn, where should it appear from
	public Transform OtherCardDrawSourceTransform;
	// Where's the player preview
	public Transform PlayerPreviewSpot;


	// private - a list of all card visual representations
	[SerializeField]
	private List<GameObject> CardsInHand = new List<GameObject>();

	// Adding/Removing cards from hand

	public void AddCard(GameObject card)
	{
		// Always insert a new card at 0th position
		CardsInHand.Insert(0, card);

		// Parent this card to a slot
		card.transform.SetParent(slots.transform);

		// Recalc the pos of the hand
		PlaceCardOnSlot();
		UpdateSlots();
	}

	public void RemoveCard(GameObject card)
	{
		// Remove from list
		CardsInHand.Remove(card);

		// recalc pos
		PlaceCardOnSlot();
		UpdateSlots();
	}

	// remove card with a given index
	public void RemoveAtIndex(int index)
	{
		CardsInHand.RemoveAt(index);
		// recalc pos
		PlaceCardOnSlot();
		UpdateSlots();
	}

	// Get a card at index
	public GameObject GetCardAtIndex(int index)
	{
		return CardsInHand[index];
	}

	// ---------------------------------
	// Managing card pos/slots
	// ---------------------------------

	void UpdateSlots()
	{
		float posX;
		// If there are cards in the players hand
		if(CardsInHand.Count >0)
		{
			posX = (slots.Slots[0].transform.localPosition.x - slots.Slots[CardsInHand.Count -1].transform.localPosition.x) /2f;
		}
		else // No cards in the players hand
		{
			posX = 0f;
		}
		// Tween slots gameObject to new position 0.2secs
		slots.gameObject.transform.DOLocalMoveX(posX, 0.2f);
	}

	// shift cards to new slots
	void PlaceCardOnSlot()
	{
		// For each card in cardsinhand
		foreach(GameObject g in CardsInHand)
		{
			// tween this card to new slot
			g.transform.DOLocalMoveX(slots.Slots[CardsInHand.IndexOf(g)].transform.localPosition.x, 0.2f);

			// Apply correct sorting order and handslot value for later
			  WhereIsTheCard w = g.GetComponent<WhereIsTheCard>();
			 w.Slot = CardsInHand.IndexOf(g);
			 w.SetHandSortingOrder();
		}
	}

	// Card Draw Methods
	// Creates a card and returns a new card as a gameobject
	GameObject CreateACardAtPosition(CardTemplate c, Vector3 position, Vector3 eulerAngles)
	{
		// Instantiate a card
		GameObject card;
		card = GameObject.Instantiate(GameManager.Instance.CreatureCardPrefab, position, Quaternion.Euler(eulerAngles)) as GameObject;

		CardDisplay manager = card.GetComponent<CardDisplay>();
		manager.card = c;
		manager.LoadCard();
		return card;
	}

	// gives a player a new card from a given pos
	public void GivePlayerACard(CardTemplate c, int UniqueID, bool fast = false, bool fromDeck = true)
	{
		GameObject card;
		if(fromDeck)
		{
			card = CreateACardAtPosition(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
		}
		else
		{
			card = CreateACardAtPosition(c, OtherCardDrawSourceTransform.position, new Vector3(0f, -179f, 0f));
		}

		// Set a tag to reflect where the card is
		foreach(Transform t in card.GetComponentInChildren<Transform>())
		{
			t.tag = owner.ToString()+"Card";
		}

		// pass this card to hand visual class
		AddCard(card);

		// Bring card to front while it travels fromd raw spot to hand
		WhereIsTheCard w = card.GetComponent<WhereIsTheCard>();
		w.BringToFront();
		w.Slot = 0;
		w.VisualState = VisualStates.Transition;

		IDHolder id = card.AddComponent<IDHolder>();
		id.UniqueID = UniqueID;

		// Move card to the hand
		Sequence s = DOTween.Sequence();
		{
			if(!fast)
			{
				// not fast
				s.Append(card.transform.DOMove(DrawPreviewSpot.position, GameManager.Instance.CardTransitionTimes));
				if(TakeCardsOpenly)
				{
					s.Insert(0f, card.transform.DORotate(Vector3.zero, GameManager.Instance.CardTransitionTimes));
				}
				s.AppendInterval(GameManager.Instance.CardPreviewTime);
				// displace the card so that we can select it in the scene easily
				s.Append(card.transform.DOLocalMove(slots.Slots[0].transform.localPosition, GameManager.Instance.CardTransitionTimes));
			}
			else
			{
				// Displace the card so it can be selected in the scene more easily.
				s.Append(card.transform.DOLocalMove(slots.Slots[0].transform.localPosition, GameManager.Instance.CardTransitionTimes));
				if(TakeCardsOpenly)
				{
					s.Insert(0f, card.transform.DORotate(Vector3.zero, GameManager.Instance.CardTransitionTimes));
				}
			}

			s.OnComplete(()=>ChangeLastCardStatusToInHand(card, w));
		}
	}


	void ChangeLastCardStatusToInHand(GameObject card, WhereIsTheCard w)
	{
		if(owner == AreaPositions.Bottom)
		{
			w.VisualState = VisualStates.LowHand;
		}
		else
		{
			w.VisualState = VisualStates.TopHand;
		}

		w.SetHandSortingOrder();
		// Action complete (allows other actions to be done)
		Action.ActionExecutionComplete();
	}

	// Used when the player has max cards and the drawn card needs to be show getting destroyed
	public void VisuallyShowCard(CardTemplate c, int UniqueID, bool fast = false, bool fromDeck = true)
	{
		GameObject card;
		if(fromDeck)
		{
			card = CreateACardAtPosition(c, DeckTransform.position, new Vector3(0f, -179f, 0f));
		}
		else
		{
			card = CreateACardAtPosition(c, OtherCardDrawSourceTransform.position, new Vector3(0f, -179f, 0f));
		}

		// Bring card to front while it travels fromd raw spot to hand
		WhereIsTheCard w = card.GetComponent<WhereIsTheCard>();
		w.BringToFront();
		w.Slot = 0;
		w.VisualState = VisualStates.Transition;

		IDHolder id = card.AddComponent<IDHolder>();
		id.UniqueID = UniqueID;

		// Move card to the hand
		Sequence s = DOTween.Sequence();
		{
			if(!fast)
			{
				// not fast
				s.Append(card.transform.DOMove(DrawPreviewSpot.position, GameManager.Instance.CardTransitionTimes));
				if(TakeCardsOpenly)
				{
					s.Insert(0f, card.transform.DORotate(Vector3.zero, GameManager.Instance.CardTransitionTimes));
				}
				s.AppendInterval(GameManager.Instance.CardPreviewTime);
				// displace the card so that we can select it in the scene easily
				s.Append(card.transform.DOLocalMove(slots.Slots[0].transform.localPosition, GameManager.Instance.CardTransitionTimes));
			}
			else
			{
				// Displace the card so it can be selected in the scene more easily.
				s.Append(card.transform.DOLocalMove(slots.Slots[0].transform.localPosition, GameManager.Instance.CardTransitionTimes));
				if(TakeCardsOpenly)
				{
					s.Insert(0f, card.transform.DORotate(Vector3.zero, GameManager.Instance.CardTransitionTimes));
				}
			}

			s.OnComplete(()=>ChangeLastCardStatusToInHand(card, w));
		}
	}


}
