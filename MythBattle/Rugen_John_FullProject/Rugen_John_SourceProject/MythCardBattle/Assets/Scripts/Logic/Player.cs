using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Holds the logic areas so they can be linked in other scripts
public class Player : MonoBehaviour, ICharacter {
    // Static array to hold both players
    public static Player[] players;
    // The players ID
    public int PlayerID;

    // is this instances turn? (show in editor for debugging)
    [SerializeField]
    private bool isMyTurn = false;

    // Mana Management
    [SerializeField]
    private int maxMana, currentMana, health;
    public PlayArea pArea;
    public Deck deck;
    public Hand hand;
    public Table table;

    private bool isAI = false;
    private AILogic aiLogic;


    // Incrementing damage when the deck is empty
    private int emptyDeckCount = 0;


    // ------------------------------------------------------------------
    // Alignment management
    // ------------------------------------------------------------------
    [SerializeField]
    private int currentAlignment;

    public int CurrentAlignment
    {
        get
        {
            return currentAlignment;
        }
        set
        {
            currentAlignment = value;
            pArea.alignText.text = value.ToString(); // update alignment
        }
    }

    // Propeties part of interface ICharacter
    public int ID
    {
        get{return PlayerID;}
    }


    // ------------------------------------------------------------------
    // Mana management
    // ------------------------------------------------------------------
    public int MaxMana
    {
        get
        {
            return maxMana;
        }
        set
        {
            maxMana = value;
            // Change UI elements here
            pArea.maxMana.text = value.ToString();
            
        }
    }

    public int CurrentMana
    {
        get
        {
            return currentMana;
        }
        set
        {
            currentMana = value;
            // Change UI elements here
            pArea.currentMana.text = value.ToString();
            if(TurnManager.Instance.WhichPlayersTurn == this)
            {
                HighlightCards();
            }
        }
    }

    public void EndTurnManaChanges()
    {
        MaxMana++; // Increase max mana
        CurrentMana = MaxMana; // Restore current mana to new max mana
    }

    // ------------------------------------------------------------------
    // Health management
    // ------------------------------------------------------------------

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            pArea.playerVisual.MyCurrentHealth = value;
            if(value <= 0) // player should die
            {
                Die();
            }
        }
    }

    public void Die()
    {
        // TODO: Game over
        Debug.Log("DEAD" + PlayerID);
        TurnManager.Instance.StopTimer();
        GameObject GM = GameObject.FindGameObjectWithTag("GameManager");
        GM.GetComponent<NavigationManager>().LoadScene("MainMenu"); // Go back to the main menu
    }


    // ------------------------------------------------------------------
    // Turn management & Player assigning & Can the player control anything
    // ------------------------------------------------------------------

    private bool canControl;

    public bool CanControl
    {
        get{return canControl;}
        set
        {
            canControl = value;
            pArea.InPlay = value; // change play area script bool too
        }
    }

    public void Awake()
    {
        // See if this player is an AI
        aiLogic = GetComponent<AILogic>();
        if(aiLogic != null)
        {
            isAI = true;
        }
        else // Just in case somehow the initial false set doesn't work
        {
            isAI = false;
        }

        
        // On awake, set mana to 0 so that when a turn is activated it can increase the mana
        CurrentMana = 0;
        MaxMana = 0;
        Health = 40;

        // Set alignment to 0
        CurrentAlignment = 0;

        // Assign players with ID (using find objects as there will only be two player scripts attached)
        players = GameObject.FindObjectsOfType<Player>();
        // Give the players an ID (1 + 2)
        PlayerID = IDCreator.GetUniqueID();
        // set starting health
        
    }

    // Check which player this instance is and then return the other player
    public Player otherPlayer
    {
        get
        {
            if(players[0] == this)
            {
                return players[1];
            }
            else
            {
                return players[0];
            }
        }
    }
    public void ActivateMyTurn()
    {
        isMyTurn = true;
        EndTurnManaChanges();
        CanControl = true;
        // Reset cards attack limit
        foreach(CreatureLogic cl in table.CreaturesOnTable)
        {
            cl.OnTurnStart();
        }
        // Check if this player is an AI, if so fire the correct function
        if(isAI)
        {
            aiLogic.AITurn(); // It's their turn
        }
    
    }

    public void DeActivateMyTurn()
    {
        isMyTurn = false;
        CanControl = false;
        HighlightCards(true);
    }

    // ------------------------------------------------------------------
    // Drawing a card
    // ------------------------------------------------------------------

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(table.CreaturesOnTable.Count);
        }
    }

    public void DrawASingleCard(bool fast = false)
    {
        if(deck.cards.Count > 0) // If there are still cards in a deck
        {
            if(hand.cardsInHand.Count < pArea.handVisual.slots.Slots.Length)
            {
                // Add card to hand from deck (0 because always the top card on the deck)
                CardLogic newCard = new CardLogic(deck.cards[0]);
                // set the owner of the card
                newCard.owner = this;
                // insert the card into the owners hand
                hand.cardsInHand.Insert(0, newCard);
                // Remove from deck
                deck.cards.RemoveAt(0);
                // command
                new DrawCardAction(hand.cardsInHand[0], this, fast, fromDeck: true).AddToQueue();
            }
            // No room left in hand
            else
            {
                // Change error text to display the current problem
                pArea.changeErrorText("Your hand is full!");
                // Draw card to the middle of the board, and then destroy it.
                CardLogic newCard = new CardLogic(deck.cards[0]);
                // Set owner
                newCard.owner = this;
                // Remove the card from the deck
                deck.cards.RemoveAt(0);
                // Add new action to queue, this one moves the card to the middle of the table and holds it there
                new MaxCardAction(newCard, this, fast, fromDeck: true).AddToQueue();
                // New action to queue, this one destroys the card.
                new DieAction(newCard.UniqueCardID, this).AddToQueue();
            }

        }
        else
        {
            // Change error text so the player knows the problem
            pArea.changeErrorText("You have no cards left in your deck!");
            emptyDeckCount++; // add one to the counter
            this.Health -= emptyDeckCount; // take away health
        }

    }


    // ------------------------------------------------------------------
    // Playing a card
    // ------------------------------------------------------------------


    public void PlayACreatureFromHand(int UniqueID, int tablePos)
    {
        if(CanControl)
        {
            PlayACreatureFromHand(CardLogic.CardsCreatedThisGame[UniqueID], tablePos);
        }
        
    }

    public void PlayACreatureFromHand(CardLogic playedCard, int tablePos)
    {
        // if(CanControl) - Removing this for now
        if (playedCard.CanBePlayed)
        {
            CurrentMana -= playedCard.CurrentManaCost;

            CreatureLogic newCreature = new CreatureLogic(this, playedCard.ct);
            table.CreaturesOnTable.Insert(tablePos, newCreature);

            new PlayCreatureAction(playedCard, this, tablePos, newCreature.UniqueCreatureID).AddToQueue();

            // creature alignment
            CurrentAlignment += newCreature.AlignmentValue;
            // Check if the ability should be activated on play
            AbilityOnPlay(playedCard);

            // Remove the card from the players hand
            hand.cardsInHand.Remove(playedCard);
            // Run the highlight cards function, to refresh highlights.
            HighlightCards();
        }
    }


    // Any abilities that can fire when played should be found here and done.
    public void AbilityOnPlay(CardLogic playedCard)
    {
        if(playedCard.ct.abilityLogic == AbilityLogicList.Preach) // The card has the preach ability.
        {
            otherPlayer.CurrentAlignment += playedCard.ct.abilityValue; // Change the other players alignment depending on the ability value
        }
        else if(playedCard.ct.abilityLogic == AbilityLogicList.Birth) // the card has the birth ability
        {
            if(playedCard.ct.abilityAction == AbilityActionList.AlterOtherHeroHealth) // the cards birth action is to damage the hero
            {
                new AbilityHeroHealthAction(playedCard.ct.abilityValue, otherPlayer).AddToQueue();
            }
            else if(playedCard.ct.abilityAction == AbilityActionList.AlterMyHeroHealth) // the cards birth action is to damage the hero
            {
                new AbilityHeroHealthAction(playedCard.ct.abilityValue, this).AddToQueue();
            }
        }
    }

    public void CanPlayerControl(bool can)
    {
        // Change bool on this script & play area
        CanControl = can;
        pArea.InPlay = can;
    }

    public void HighlightCards(bool removeAllHighlights = false)
    {
        // Loop through cards in hand
        foreach(CardLogic cl in hand.cardsInHand)
        {
            GameObject go = IDHolder.GetGameObjectWithID(cl.UniqueCardID);
            if(go != null)
            {
                go.GetComponent<CardDisplay>().CanCardBePlayedNow = (cl.CurrentManaCost <= CurrentMana) && !removeAllHighlights && isMyTurn; // It can be played if there's enough mana & it's the players turn
            }
        }

        // Loop through cards on table
        foreach(CreatureLogic crl in table.CreaturesOnTable)
        {
            GameObject go = IDHolder.GetGameObjectWithID(crl.UniqueCreatureID);
            if(go!= null)
            {
                go.GetComponent<PlayedCreatureDisplay>().CanAttackNow = (crl.AttacksLeftThisTurn > 0 ) && !removeAllHighlights && isMyTurn;

            }
        }
    }

}
