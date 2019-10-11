using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


// Visual part of the creature attacking
public class CreatureAttacking : MonoBehaviour {

	private PlayedCreatureDisplay cardManager;

	private WhereIsTheCard whereIsTheCard;

	void Awake()
	{
		// Assigning 
		cardManager = GetComponent<PlayedCreatureDisplay>();
		whereIsTheCard = GetComponent<WhereIsTheCard>();
	}

	public void AttackTarget(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackHealthAfter, int targetHealthAfter)
	{
		Debug.Log(targetUniqueID);
		cardManager.CanAttackNow = false;
		GameObject target = IDHolder.GetGameObjectWithID(targetUniqueID);

		// Bring the creature to the front layer
		whereIsTheCard.BringToFront();

		// Store a temp state for later
		VisualStates tempstate = whereIsTheCard.VisualState;
		// Set state to transition
		whereIsTheCard.VisualState = VisualStates.Transition;

		transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
		{
			// Which player is the target?
			if(targetUniqueID == GameManager.Instance.BottomPlayer.PlayerID)
			{
				// Change players values
				GameManager.Instance.BottomPlayer.Health = targetHealthAfter;
			}
			else if(targetUniqueID == GameManager.Instance.TopPlayer.PlayerID)
			{
				// Change players values
				GameManager.Instance.TopPlayer.Health = targetHealthAfter;
			}
			else
			{
				PlayedCreatureDisplay tempCardTarget = target.GetComponent<PlayedCreatureDisplay>();
				tempCardTarget.HealthText.text = targetHealthAfter.ToString();
			}
			whereIsTheCard.SetTableSortingOrder();
			// revert back to the original state
			whereIsTheCard.VisualState = tempstate;

			cardManager.HealthText.text = attackHealthAfter.ToString();
			Sequence s = DOTween.Sequence();
			s.AppendInterval(1f);
			s.OnComplete(Action.ActionExecutionComplete);
		});
	}
	
}
