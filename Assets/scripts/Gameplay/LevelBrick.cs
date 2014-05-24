using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBrick : MonoBehaviour {

	public enum brickType
	{
		Wall,
		Floor,
		Room_Food,
		Room_Sleep,
		Room_Paper,
		Room_Mail
	}
	public brickType brickDef;
	public List<GameObject> objOnFloor;

	// Use this for initialization
	public void Setup () 
	{
		if (brickDef == brickType.Wall)
		{
			tag = "Wall";
		}
		if (brickDef == brickType.Floor)
		{
			objOnFloor = new List<GameObject>();
		}
	}

	public void giveInfos()
	{
		switch (brickDef)
		{
		case brickType.Wall :
		{
			gameObject.layer = LayerMask.NameToLayer("Wall");
			break;
		}
		}
	}

	void OnDrawGizmos()
	{
		if (GetComponent<BoxCollider>() != null)
		{
			BoxCollider coll = GetComponent<BoxCollider>();
			switch (brickDef)
			{
				case brickType.Wall :
				{
				Gizmos.color = Color.red;
				break;
				}
				default :
				{
					Gizmos.color = Color.blue;
					break;
				}
			}
			Gizmos.DrawWireCube( coll.center - coll.transform.position,coll.size);
		}
	}
}
