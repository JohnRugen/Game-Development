using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spreads the hand slots out so I don't have to keep doing it in the editor
public class HandCardSpacing : MonoBehaviour {

	// collection of card slots
	public Transform[] Slots;

	void Awake()
	{
		// assign first postion the first slot (starts at 0)
		Vector3 firstPos = Slots[0].transform.position;
		// last pos is n-1 as it starts at 0
		Vector3 lastPos = Slots[Slots.Length-1].transform.position;

		// Distance between last slots x & first slots x / the length of the slots
		float xDistance = (lastPos.x - firstPos.x) / (float)(Slots.Length);
		float yDistance = (lastPos.y - firstPos.y) / (float)(Slots.Length-1);
		float zDistance = (lastPos.z - firstPos.z) / (float)(Slots.Length-1);

		// distance between cards
		Vector3 distance = new Vector3(xDistance, yDistance, zDistance);

		for(int i = 1; i<Slots.Length; i++)
		{
			Slots[i].transform.position = Slots[i-1].transform.position + distance;
		}
	}
}
