using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAttack : DraggingActions {

	// Ref to the sprite with a round "target graphic"
	private SpriteRenderer sr;

	// Line renderer that is attached to the child game object to draw an arrow
	private LineRenderer lr;

	// ref to where the card is to track it
	private WhereIsTheCard whereIsTheCard;

	// The pointy end of the arrow
	private Transform triangle;

	// sr of the triangle
	private SpriteRenderer triangleSR;

	private GameObject Target;

	private PlayedCreatureDisplay cardManager;
	

	void Awake()
	{
		// Connecting everything
		sr = GetComponent<SpriteRenderer>();
		lr = GetComponentInChildren<LineRenderer>();
		lr.sortingLayerName = "AboveEverything";
		triangle = transform.Find("ArrowEnd");
		triangleSR = triangle.GetComponent<SpriteRenderer>();

		cardManager = GetComponentInParent<PlayedCreatureDisplay>();
		whereIsTheCard = GetComponentInParent<WhereIsTheCard>();
	}

	public override bool CanDrag
	{
		get
		{
			Debug.Log("CANCARDBEPLAYED:" + base.CanDrag + cardManager.CanAttackNow);
			// Can the card be dragged & can it be played (attack)?
			return base.CanDrag && cardManager.CanAttackNow;
		}
	}

	public override void OnStartDrag()
	{
		Debug.Log("ATTACKING - DRAG");
		// Set the cards state to dragging state
		whereIsTheCard.VisualState = VisualStates.Dragging;
		// enable target graphic
		sr.enabled = true;
		// enable lr
		lr.enabled = true;
	}

// When the player drags in the update
	public override void OnDraggingInUpdate()
	{
		Debug.Log("DRAGGING IN UPDATE");
		Vector3 notNormalized = transform.position - transform.position;
		Vector3 direction = notNormalized.normalized;

		// Find the distance to the target, 2.3 might be off - Change if needed.
		float distanceToTarget = (direction*2.3f).magnitude;
		if(notNormalized.magnitude > distanceToTarget)
		{
			// draw a line from creature - target
			lr.SetPositions(new Vector3[]{ transform.parent.position, transform.position - direction*2.3f});
			lr.enabled = true;

			// position the end of the arrow between near the target
			triangleSR.enabled = true;
			triangle.transform.position = transform.position -1.5f*direction;

			// proper rotation of the arrow end
			float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
			triangleSR.transform.rotation = Quaternion.Euler(0f, 0f, rot_z -90);
		}
		else
		{
			// if the target is far enough from the creature, do not show the arrow
			lr.enabled = false;
			triangleSR.enabled = false;
		}
	}


	public override void OnEndDrag()
	{
		Debug.Log("END ATTACK DRAG");
		Target = null;
		RaycastHit[] hits;
		// TODO: raycast here anyway, store the results in
		hits = Physics.RaycastAll(origin: Camera.main.transform.position,
		direction: (-Camera.main.transform.position + this.transform.position).normalized,
		maxDistance: 30f);

		foreach(RaycastHit h in hits)
		{
			// Check if the player is targeting the opposite player portrait
			if((h.transform.tag == "TopPlayer" && this.tag == "BottomCreature") ||
			  (h.transform.tag == "BottomPlayer" && this.tag == "TopCreature"))
			  {
				  // Save them as the target
				  Target = h.transform.gameObject;
			  }

			  // Check if the player is targeting an enemy 
			else if ((h.transform.tag == "TopCreature" && this.tag == "BottomCreature") ||
					(h.transform.tag == "BottomCreature" && this.tag == "TopCreature"))
			{
				// Save it as a target
				Target = h.transform.parent.gameObject;
			}
		}

		bool targetValid = false;

		if(Target != null)
		{
			int targetID = Target.GetComponent<IDHolder>().UniqueID;
			// DEBUG

			// If the targets ID matches a players ID
			if(targetID == GameManager.Instance.BottomPlayer.PlayerID || targetID == GameManager.Instance.TopPlayer.PlayerID)
			{
				// Attack it - DEBUG
				Debug.Log("Attacking " +Target);
				Debug.Log("TargetID: " + targetID);
				// Grab card & run attack player function
				CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].GoFace();
				targetValid = true;
			}
			// If the targeted creature is alive/real attack it
			else if (CreatureLogic.CreaturesCreatedThisGame[targetID]!= null)
			{
				targetValid = true;
				CreatureLogic.CreaturesCreatedThisGame[GetComponentInParent<IDHolder>().UniqueID].AttackCreatureWithID(targetID);
				Debug.Log("Attacking:" +targetID);
			}
		}

		if(!targetValid)
		{
			// not a valid targe
			if(tag.Contains("Bottom"))
			{
				whereIsTheCard.VisualState = VisualStates.LowTable;
			}
			else
			{
				whereIsTheCard.VisualState = VisualStates.TopTable;
			}
			whereIsTheCard.SetTableSortingOrder();
		}

		// return target and arrow to original pos
		transform.localPosition = Vector3.zero;
		sr.enabled = false;
		lr.enabled = false;
		triangleSR.enabled = false;
	}

	// Not in use
	protected override bool DragSuccessful()
	{
		return true;
	}
}

