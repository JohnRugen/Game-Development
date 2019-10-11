using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Actions are player actions that impact the game, drawing, battling etc.
// They are stored in a queue and then played last in first out
public class Action {

	// Remember - First in last out
	public static Queue<Action> ActionQueue = new Queue<Action>();

	// Is a queue getting  played?
	public static bool playingQueue = false;

	// Add an action to the queue
	public virtual void AddToQueue()
	{
		ActionQueue.Enqueue(this);
		if(!playingQueue)
		{
			PlayFirstActionFromQueue();
		}
	}

	public virtual void StartActionExecution()
	{
		// list of everything that we have to do with this command (draw a card, play a card, play spell effect, etc...)
        // there are 2 options of timing : 
        // 1) use tween sequences and call CommandExecutionComplete in OnComplete()
        // 2) use coroutines (IEnumerator) and WaitFor... to introduce delays, call CommandExecutionComplete() in the end of coroutine
	}

	public static void ActionExecutionComplete()
	{
		if(ActionQueue.Count > 0)
		{
			// Play first
			PlayFirstActionFromQueue();
		}
		else
		{
			playingQueue = false;
			if(TurnManager.Instance.WhichPlayersTurn != null)
			{
				TurnManager.Instance.WhichPlayersTurn.HighlightCards();
			}
		}
	}

	public static void PlayFirstActionFromQueue()
	{
		playingQueue = true;
		ActionQueue.Dequeue().StartActionExecution();
	}

	public static bool CardDrawPending()
	{
		foreach(Action a in ActionQueue)
		{
			if(a is DrawCardAction)
			{
				return true;
			}
		}
		return false;
	}
}
