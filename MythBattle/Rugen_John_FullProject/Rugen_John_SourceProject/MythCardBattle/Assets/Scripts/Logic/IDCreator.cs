using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDCreator {

	// Creates IDs which allows each card to have a unique ID
	private static int Count;

	public static int GetUniqueID()
	{
		Count++;
		return Count;
	}

	public static void ResetAllIDs()
	{
		Count  = 0;
	}
}
