using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Default character, (*REFACTOR* - add more later if desired)
public enum CharacterClass{Basic}

// Characters, the actual player.
[CreateAssetMenu(fileName = "Character", menuName="Character/NewCharacter")]
public class CharacterTemplate : ScriptableObject {

	public CharacterClass Class;
	public string ClassName;
	public int maxHealth = 40;
	public Sprite avatarImage;
	public Color32 avatarBG;
	public Color32 avatarTint;

}
