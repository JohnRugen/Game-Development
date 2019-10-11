using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to rotate the cards so the back can be shown. Useful for drawing cards


// Allows this script to be run in editor mode
[ExecuteInEditMode]
public class CardRotation : MonoBehaviour {
	// Front and back of the card
	public RectTransform front, back;
	//Offset for the card
	public Transform cardOffset;
	// 3d collider that engulfs the card
	public Collider cardCollider;
	// Which side of the card is showing?
	private bool cardOnBack = false;

	void Update()
	{
		
		RaycastHit[] rayHits;

		rayHits = Physics.RaycastAll(Camera.main.transform.position, (-Camera.main.transform.position + cardOffset.position).normalized, (-Camera.main.transform.position + cardOffset.position).magnitude);

		bool hasPassedCollider = false;

		foreach(RaycastHit r in rayHits)
		{
			if(r.collider == cardCollider)
			{
				hasPassedCollider = true;
			}
		}

		if(hasPassedCollider != cardOnBack)
		{
			cardOnBack = hasPassedCollider;
			if(cardOnBack)
			{
				// Show back side
				front.gameObject.SetActive(false);
				back.gameObject.SetActive(true);
			}
			else
			{
				// Show front side
				back.gameObject.SetActive(false);
				front.gameObject.SetActive(true);
			}
		}
	}
}

