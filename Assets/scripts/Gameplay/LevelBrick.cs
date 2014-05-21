using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	public void Setup () 
	{
		tag = "Wall";
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
