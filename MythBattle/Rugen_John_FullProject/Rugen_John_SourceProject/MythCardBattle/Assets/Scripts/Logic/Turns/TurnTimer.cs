using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

// Time for each round & logic found here
public class TurnTimer : MonoBehaviour {
	
	public Text timerText;
	public float TimeForOneTurn;
	
	private float timeUntilZero;
	private bool counting = false;

	public static TurnManager Instance;



	void Awake()
	{
		// assign instance
		Instance = gameObject.GetComponent<TurnManager>();

	}

	public void StartTimer()
	{
		// Setting time until zero to the total round time
		timeUntilZero = TimeForOneTurn;
		counting =true;
	}

	void Update()
	{
		// if timer is on
		if(counting)
		{
			timeUntilZero -= Time.deltaTime;
			if(timerText != null)
			{
				timerText.text = ToString();
			}
		}
		if(timeUntilZero <=0) // if timer has ran out
		{
			counting = false;
			Instance.TurnOver();
		}
	}

	public void StopTimer()
	{
		counting = false;
	}

	// Changing the timer into a suitable format for the countdown timer, overriding ToString for ease

	public override string ToString()
	{
		int inSeconds = Mathf.RoundToInt(timeUntilZero); // How many seconds
		string justSeconds = (inSeconds % 60).ToString();

		if(justSeconds.Length == 1) //There's only 1 digit
		{
			justSeconds = "0" + justSeconds;
		}
		string justMinutes = (inSeconds / 60).ToString();
		if(justMinutes.Length == 1)
		{
			justMinutes = "0" + justMinutes;
		}
		// Format string & return
		return string.Format("{0}:{1}", justMinutes, justSeconds);

	}

}
