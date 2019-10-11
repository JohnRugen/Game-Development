using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
// Using tweening addon

// This will allow cards to be hovered over and enlarged so players can read cards much more easily.

public class HoverOnCard : MonoBehaviour {

	public GameObject originalLayout; // the cards original Layout

	public Vector3 targetPosition;
	public float targetScale;
	public GameObject previewGameObject;
	public bool activateInAwake = false;

	
	private static HoverOnCard currentViewCard = null;


	// Should previews be allowed at this moment
	private static bool _PreviewsAllowed = true;

	// previews allowed constructor
	public static bool PreviewsAllowed
	{
		get
		{
			return _PreviewsAllowed; // return 
		}
		set
		{
			_PreviewsAllowed = value; // set previews to value
			if(!_PreviewsAllowed) // if not allowed
			{
				StopAllPreviews(); // stop all the previews
			}
		}
	}

	private bool _thisPreviewEnabled = false;

	public bool ThisPreviewEnabled
	{
		get
		{
			return _thisPreviewEnabled;
		}

		set
		{
			_thisPreviewEnabled = value; // set to value
			if(!_thisPreviewEnabled)
			{
				StopThisPreview(); // stop this current preview
			}
		}
	}

	public bool OverCollider{get; set;}

	void Awake()
	{
		ThisPreviewEnabled = activateInAwake;
	}

	void OnMouseEnter() // when mouse goes over obj
	{
		OverCollider = true;
		if(PreviewsAllowed && ThisPreviewEnabled)
		{
			PreviewThisObject();
		}
	}

	// when mouse leaves
	void OnMouseExit()
	{
		OverCollider = false;
		if(!PreviewingSomeCard()) // returns false
		{
			StopAllPreviews();
		}
	}

	void PreviewThisObject()
	{
		// clone card
		// disable the previous preview if there is one
		StopAllPreviews();
		// Save this as current
		currentViewCard = this;
		// enable preview GO
		previewGameObject.SetActive(true);
		// disable if needed
		if(originalLayout!= null)
		{
			originalLayout.SetActive(false);
		}
		// tween with tween addon
		previewGameObject.transform.localPosition = Vector3.zero;
		previewGameObject.transform.localScale = Vector3.one;

		previewGameObject.transform.DOLocalMove(targetPosition, 1f).SetEase(Ease.OutQuint);
		previewGameObject.transform.DOScale(targetScale, 1f).SetEase(Ease.OutQuint);
	}


	// stop this specific preview
	void StopThisPreview()
	{
		previewGameObject.SetActive(false);
		previewGameObject.transform.localScale = Vector3.one;
		previewGameObject.transform.localPosition = Vector3.zero;
		if(originalLayout!=null)
		{
			originalLayout.SetActive(true);
		}
	}


	// Stop all previews (highlights)
	private static void StopAllPreviews()
	{
		if(currentViewCard!= null)
		{
			currentViewCard.previewGameObject.SetActive(false);
			currentViewCard.previewGameObject.transform.localScale = Vector3.one;
			currentViewCard.previewGameObject.transform.localPosition = Vector3.zero;
			if(currentViewCard.originalLayout!= null)
			{
				currentViewCard.originalLayout.SetActive(true);
			}
		}
	}

	private static bool PreviewingSomeCard()
	{
		if(!PreviewsAllowed)
		{
			return false;
		}
		HoverOnCard[] allHoverBlowups = GameObject.FindObjectsOfType<HoverOnCard>();

		foreach(HoverOnCard hb in allHoverBlowups)
		{
			if(hb.OverCollider && hb.ThisPreviewEnabled)
			{
				return true;
			}
		}
		return false;

	}

}
