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
		this.blockSpr = FETool.findWithinChildren(this.gameObject, "BlockT").GetComponentInChildren<OTSprite>();
		getRotation();
		transform.rotation = Quaternion.Euler(new Vector3(0f,0f, closedAngle));
	}

	private void getRotation()
	{
		switch (Pivot)
		{
		case PivotList.Right :
		{
			openAngle = -180f;
			closedAngle = 90f;
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
			openAngle = 360f;
			closedAngle = -90f;
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
		switch (Handle)
		{
		case HandleDoor.Closed :
		{
			Open();
			break;
		}
		case HandleDoor.Open :
		{
			Close();
			break;
		}
		}
	}

	public void Open()
	{
		transform.rotation = Quaternion.Euler(new Vector3(0f,0f, openAngle));
		Handle = HandleDoor.Open;
	}

	public void Close()
	{
		transform.rotation = Quaternion.Euler(new Vector3(0f,0f, closedAngle));
		Handle = HandleDoor.Closed;
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
		if (_oth.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
	    {
			this.switchDoor();
		}

		if (_oth.CompareTag("Dracula") == true)
		{
			Open();
		}
	}

	void OnDrawGizmosSelected()
	{
		getRotation();
		transform.rotation = Quaternion.Euler(new Vector3(0f,0f, closedAngle));
	}
}
