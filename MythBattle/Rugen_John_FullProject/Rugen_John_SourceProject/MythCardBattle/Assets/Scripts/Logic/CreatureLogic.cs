using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CreatureLogic : ICharacter {

	// PUBLIC FIELDS
    public Player owner;
    public CardTemplate ct;
    public int UniqueCreatureID;

    // PROPERTIES
    // property from ICharacter interface
    public int ID
    {
        get{ return UniqueCreatureID; }
    }
        
    // the basic health that we have in CardAsset
    private int baseHealth;
    // health with all the current buffs taken into account
    public int MaxHealth
    {
        get{ return baseHealth;}
    }

    // current health of this creature
    private int health;
    public int Health
    {
        get{ return health; }

        set
        {
            if (value > MaxHealth)
                health = MaxHealth;
            else if (value <= 0)
                Die();
            else
                health = value;
        }
    }
	

    // returns true if we can attack with this creature now
    public bool CanAttack
    {
        get
        {
            bool ownersTurn = false;
            if(TurnManager.Instance.WhichPlayersTurn == owner)
            {
                ownersTurn = true;
            }
            else
            {
                ownersTurn = false;
            }
            return (ownersTurn && (AttacksLeftThisTurn > 0));
        }
    }

    // property for Attack
    private int baseAttack;
    public int Attack
    {
        get{ return baseAttack; }
    }
     
    // number of attacks for one turn
    private int attacksForOneTurn = 1;
    public int AttacksLeftThisTurn
    {
        get;
        set;
    }

    private int alignmentValue;
    public int AlignmentValue
    {
        get {return alignmentValue;}
        set{alignmentValue = value;}
    }

    // CONSTRUCTOR
    public CreatureLogic(Player owner, CardTemplate ct)
    {
        this.ct = ct;
        baseHealth = ct.maxHealth;
        Health = ct.maxHealth;
        baseAttack = ct.attack;
        // if the card has bloodthirst, add 1 to the attacks per turn.
        if(ct.abilityLogic == AbilityLogicList.Bloodthirsty)
        {
            attacksForOneTurn = ct.attacksPerTurn + ct.abilityValue;
        }
        else
        {

            attacksForOneTurn = ct.attacksPerTurn;
        }
        // get alignment
        alignmentValue = ct.alignment;
        
        // If the card has the Bravery attribute
        if(ct.abilityLogic == AbilityLogicList.Bravery)
        {
            AttacksLeftThisTurn = attacksForOneTurn;
        }
        this.owner = owner;
        UniqueCreatureID = IDCreator.GetUniqueID();
        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }

    // METHODS
    public void OnTurnStart()
    {
        AttacksLeftThisTurn = attacksForOneTurn;
    }

    public void Die()
    {   
        if(ct.abilityLogic == AbilityLogicList.Death)
        {
            // Run Death ability function first.
            DeathAbility();
        }
        else
        {
            owner.table.CreaturesOnTable.Remove(this); // Remove the card
            new DieAction(UniqueCreatureID, owner).AddToQueue(); // add remove action to queue
        }
        
    }

    public void DeathAbility()
    {
        if(ct.abilityAction == AbilityActionList.AlterOtherHeroHealth) // if the ability action matches altering the other heroes health
        {
            new AbilityHeroHealthAction(ct.abilityValue, owner.otherPlayer).AddToQueue(); // add the aciton to the queue
            owner.table.CreaturesOnTable.Remove(this); // remove the creature
            new DieAction(UniqueCreatureID, owner).AddToQueue(); // add death action to queue
        }
        else if(ct.abilityAction == AbilityActionList.AlterMyHeroHealth) // Altering owners health
        {
            new AbilityHeroHealthAction(ct.abilityValue, owner).AddToQueue(); // add action to queue
            owner.table.CreaturesOnTable.Remove(this); // remove card
            new DieAction(UniqueCreatureID, owner).AddToQueue(); // add death action to queue
        }
    }

    // Attack enemies portrait
    public void GoFace()
    {
        // 1 less attack
        AttacksLeftThisTurn--;
        // calc health
        int targetHealthAfter = owner.otherPlayer.Health - Attack;
        new CreatureAttack(owner.otherPlayer.PlayerID, UniqueCreatureID, 0, Attack, Health, targetHealthAfter).AddToQueue(); // add action to queue
        owner.otherPlayer.Health -= Attack; // take away health from owner
    }

    // attacking a creature
    public void AttackCreature (CreatureLogic target)
    {
        AttacksLeftThisTurn--;
        // calculate the values so that the creature does not fire the DIE command before the Attack command is sent
        int targetHealthAfter = target.Health - Attack;
        int attackerHealthAfter = Health - target.Attack;
        new CreatureAttack(target.UniqueCreatureID, UniqueCreatureID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter).AddToQueue(); //  add action to queue

        target.Health -= Attack;
        Health -= target.Attack;
    }

    // attack specific creature
    public void AttackCreatureWithID(int uniqueCreatureID)
    {
        // get creatures ID
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target); // attack it
    }

    // STATIC For managing IDs
    public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();
}
