using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TableVis : MonoBehaviour {

	// which character this table belongs too
	public AreaPositions owner;

	// A ref to the game object that marks positions
	public HandCardSpacing slots;

	// list of al creature cards on the table as gameobjects
	private List<GameObject> CreaturesOnTable = new List<GameObject>();

	// Hovering over the tables collider?
	private bool cursorOverThisTable = false;

	// A 3d collider attatched to the table
	private BoxCollider col;

	// Returns true if not hovering over a players table
	public static bool CursorOverATable
	{
		get
		{
			TableVis[] bothTables = GameObject.FindObjectsOfType<TableVis>();
			return (bothTables[0].CursorOverThisTable || bothTables[1].CursorOverThisTable);
		}
	}

	// returns true if hovering over this tables collider
	public bool CursorOverThisTable
	{
		get{return cursorOverThisTable;}
	}

	void Awake()
	{
		col = GetComponent<BoxCollider>();
	}


	void Update()
	{
		// Raycast & create an array of raycast hits
		RaycastHit[] rayHits;

		// raycast to mouse pos and store results
		rayHits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 30f);

		bool passedThroughTableCollider = false;
		foreach(RaycastHit r in rayHits)
		{
			// check if the collider we hit was the correct one
			if(r.collider == col)
			{
				passedThroughTableCollider = true;
			}
		}
		// set to the result
		cursorOverThisTable = passedThroughTableCollider;
	}

	// method to create a new creature and add it to the table
    public void AddCreatureAtIndex(CardTemplate ct, int UniqueID ,int index)
    {
        // create a new creature from prefab
        GameObject creature = GameObject.Instantiate(GameManager.Instance.PlayedCreaturePrefab, slots.Slots[index].transform.position, Quaternion.identity) as GameObject;

        // apply the look from CardAsset
        PlayedCreatureDisplay manager = creature.GetComponent<PlayedCreatureDisplay>();
        manager.cardTemplate = ct;
        manager.LoadCreature();

        // add tag according to owner
        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
            t.tag = owner.ToString()+"Creature";
        
        // parent a new creature gameObject to table slots
        creature.transform.SetParent(slots.transform);

        // add a new creature to the list
        CreaturesOnTable.Insert(index, creature);

        // let this creature know about its position
        WhereIsTheCard w = creature.GetComponent<WhereIsTheCard>();
        w.Slot = index;
        if (owner == AreaPositions.Bottom)
            w.VisualState = VisualStates.LowTable;
        else
            w.VisualState = VisualStates.TopTable;

        // add our unique ID to this creature
        IDHolder id = creature.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // after a new creature is added update placing of all the other creatures
        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();

        // end command execution
        Action.ActionExecutionComplete();
    }


    // returns an index for a new creature based on mousePosition
    // included for placing a new creature to any positon on the table
    public int TablePosForNewCreature(float MouseX)
    {
        // if there are no creatures or if we are pointing to the right of all creatures with a mouse.
        // right - because the table slots are flipped and 0 is on the right side.
        if (CreaturesOnTable.Count == 0 || MouseX > slots.Slots[0].transform.position.x)
            return 0;
        else if (MouseX < slots.Slots[CreaturesOnTable.Count - 1].transform.position.x) // cursor on the left relative to all creatures on the table
            return CreaturesOnTable.Count;
        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            if (MouseX < slots.Slots[i].transform.position.x && MouseX > slots.Slots[i + 1].transform.position.x)
                return i + 1;
        }
        Debug.Log("Suspicious behavior. Reached end of TablePosForNewCreature method. Returning 0");
        return 0;
    }

    // Destroy a creature
    public void RemoveCreatureWithID(int IDToRemove)
    {
        // TODO: This has to last for some time
        // Adding delay here did not work because it shows one creature die, then another creature die. 
        // 
        //Sequence s = DOTween.Sequence();
        //s.AppendInterval(1f);
        //s.OnComplete(() =>
        //   {
                
        //    });
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        CreaturesOnTable.Remove(creatureToRemove);
        Destroy(creatureToRemove);

        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        Action.ActionExecutionComplete();
    }

    /// <summary>
    /// Shifts the slots game object according to number of creatures.
    /// </summary>
    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float posX;
        if (CreaturesOnTable.Count > 6)
            posX = (slots.Slots[0].transform.localPosition.x - slots.Slots[CreaturesOnTable.Count - 1].transform.localPosition.x) / 2f;
        else
            posX = 0f;

        slots.gameObject.transform.DOLocalMoveX(posX, 0.3f);  
    }

    /// <summary>
    /// After a new creature is added or an old creature dies, this method
    /// shifts all the creatures and places the creatures on new slots.
    /// </summary>
    void PlaceCreaturesOnNewSlots()
    {
        foreach (GameObject g in CreaturesOnTable)
        {
            g.transform.DOLocalMoveX(slots.Slots[CreaturesOnTable.IndexOf(g)].transform.localPosition.x, 0.3f);
            // apply correct sorting order and HandSlot value for later 
            // TODO: figure out if I need to do something here:
            // g.GetComponent<WhereIsTheCardOrCreature>().SetTableSortingOrder() = CreaturesOnTable.IndexOf(g);
        }
    }
}
