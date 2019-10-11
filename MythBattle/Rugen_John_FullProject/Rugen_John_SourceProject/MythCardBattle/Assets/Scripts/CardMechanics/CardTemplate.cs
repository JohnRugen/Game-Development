using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum is a collection of data, all the targeting options for cards listed here
public enum TargetOptions
{
	None,
	AllCreatures,
	EnemyCreatures,
	YourCreatures,
	AllCharacters,
	EnemyCharacters,
	YourCharacters

}

public enum AbilityLogicList
{
	None,
	Preach,
	Bloodthirsty,
	Bravery,
	Birth,
	Death
}

public enum AbilityActionList
{
	None,
	BirthDamageHero,
	AlterOtherHeroHealth,
	AlterMyHeroHealth
}

// The template for the card, cards can be created in unity with this
[CreateAssetMenu(fileName = "Card", menuName="Cards/NewCard")]
public class CardTemplate : ScriptableObject {
	// Headers tidy up Unity editor
	[Header("General")]
	public CharacterTemplate characterTemplate;
	// text area for easy Description typing (minLines, maxLines)
	// text area for easy Description typing (minLines, maxLines)
	[TextArea(2,3)]
	public string description; // Cards info
	public Sprite cardImage;
	public int manaCost;

	[Header("Ability (Visual then Logic)")]
	// text area for easy Description typing (minLines, maxLines)
	[TextArea(2,3)]
	public string ability;
	public AbilityLogicList abilityLogic;
	public int abilityValue;
	public AbilityActionList abilityAction;
	
	

	[Header("Creature Information")]
	public int maxHealth;
	public int attack;
	// Keeping this int, because if I add silence mechanic then bloodthirsty cards will be silenced and reverted back to 1.
	public int attacksPerTurn = 1;
	public string creatureScriptName;
	public int specialCreatureAmount;
	public int alignment;

	[Header("SpellInfo")]
	public string spellScriptName;
	public int specialSpellAmount;
	public TargetOptions Targets;
	
}
