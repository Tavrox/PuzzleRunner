using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public enum PivotList
	{
		Up,
		Down,
		Left,
		Right
	};
	public PivotList Pivot;
	public enum HandleDoor
	{
		Open,
		Closed
	};
	public HandleDoor Handle;
	public DoorBlock blocker;
	public float closedAngle;
	public float openAngle;
	public OTSprite blockSpr;
	private LevelManager levMan;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		levMan = _lev;
		blocker = transform.parent.GetComponentInChildren<DoorBlock>();
		this.blockSpr = FETool.findWithinChildren(this.gameObject, "Block").GetComponentInChildren<OTSprite>();
		getRotation();
		transform.rotation = Quaternion.Euler(0f,0f, closedAngle);
	}

	private void getRotation()
	{
		switch (Pivot)
		{
		case PivotList.Right :
		{
			openAngle = 90f;
			closedAngle = 0f;
			break;
		}
		case PivotList.Left :
		{
			openAngle = 90f;
			closedAngle = 180f;
			break;
		}
		case PivotList.Up :
		{
			openAngle = 0f;
			closedAngle = 270f;
			break;
		}
		case PivotList.Down :
		{
			openAngle = 0f;
			closedAngle = 270f;
			break;
		}
		}
	}

	public void switchDoor()
	{
		print ("open switcvh");
		switch (Handle)
		{
		case HandleDoor.Closed :
		{
			new OTTween(blockSpr , levMan.plr.doorSpeed , OTEasing.CircIn).Tween("rotation" , openAngle);
			Handle = HandleDoor.Open;
//			blocker.gameObject.layer = LayerMask.NameToLayer("Default");
			break;
		}
		case HandleDoor.Open :
		{
			new OTTween(blockSpr , levMan.plr.doorSpeed , OTEasing.CircIn).Tween("rotation" , closedAngle);
			Handle = HandleDoor.Closed;
			break;
		}
		}
	}

	public void Update()
	{
		switch (Handle)
		{
		case HandleDoor.Closed :
		{
			blocker.gameObject.layer = LayerMask.NameToLayer("Wall");
			break;
		}
		case HandleDoor.Open :
		{
			blocker.gameObject.layer = LayerMask.NameToLayer("Default");
			break;
		}
		}

	}

	void OnTriggerStay(Collider _oth)
	{
		if (_oth.CompareTag("Player") && Input.GetKeyDown(KeyCode.Return))
	    {
			this.switchDoor();
		}

		if (_oth.CompareTag("Dracula") == true)
		{
			this.Handle = HandleDoor.Closed;
			this.switchDoor();
		}
	}

	void OnDrawGizmosSelected()
	{
		getRotation();
		transform.rotation = Quaternion.Euler(0f,0f, closedAngle);

	}
}
