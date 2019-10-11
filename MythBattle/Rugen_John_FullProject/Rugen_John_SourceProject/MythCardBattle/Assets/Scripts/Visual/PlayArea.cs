using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script groups the play areas together & includes error text functionality
// Top = player 2 | bottom Player 1
public enum AreaPositions{Top, Bottom}
public class PlayArea : MonoBehaviour {
	// Links the play area together so that logic scripts can access
	public Text currentMana, maxMana, alignText;
	public HandVis handVisual;
	public TableVis tableVisual;

	public Text errorText;

	public PlayerVisual playerVisual;

	public bool ControlsON = true;

	private bool inPlay;

	public bool InPlay
	{
		get
		{
			return inPlay;
		}
		set
		{
			inPlay = value;
		}
	}

	public void changeErrorText(string newText)
	{
		StartCoroutine(ChangeErrorText(newText));
	
	}

	IEnumerator ChangeErrorText(string newText)
	{
		errorText.text = newText;
		errorText.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.2f);
		errorText.gameObject.SetActive(false);
	}


}
