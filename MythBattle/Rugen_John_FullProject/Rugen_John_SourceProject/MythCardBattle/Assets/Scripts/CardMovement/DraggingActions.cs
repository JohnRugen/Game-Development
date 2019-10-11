using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DraggingActions : MonoBehaviour {

	public abstract void OnStartDrag();

	public abstract void OnEndDrag();

	public abstract void OnDraggingInUpdate();

	public virtual bool CanDrag
	{
		get
		{
			return true;
		}
	}

	protected virtual Player playerOwner
	{
		get
		{
			if(tag.Contains("Bottom"))
			{
				return GameManager.Instance.BottomPlayer;
			}
			else if(tag.Contains("Top"))
			{
				return GameManager.Instance.TopPlayer;
			}
			else
			{
				Debug.Log("Untagged card/creature" + transform.parent.name);
				return null;
			}
		}
	}

	protected abstract bool DragSuccessful();
}
