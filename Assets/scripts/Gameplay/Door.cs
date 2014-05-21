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
	[HideInInspector] public float closedAngle;
	[HideInInspector] public float openAngle;
	public OTSprite blockSpr;
	private LevelManager levMan;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		levMan = _lev;
		this.blockSpr = FETool.findWithinChildren(this.gameObject, "Block").GetComponentInChildren<OTSprite>();
		switch (Pivot)
		{
			case PivotList.Left :
			{
				openAngle = 90f;
				closedAngle = 0f;
				break;
			}
			case PivotList.Right :
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
		switch (Handle)
		{
		case HandleDoor.Closed :
		{
			new OTTween(blockSpr , 0.35f , OTEasing.CircIn).Tween("rotation" , openAngle);
			Handle = HandleDoor.Open;
			break;
		}
		case HandleDoor.Open :
		{
			new OTTween(blockSpr , 0.35f , OTEasing.CircIn).Tween("rotation" , closedAngle);
			Handle = HandleDoor.Closed;
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
	}

	void OnDrawGizmosSelected()
	{
//		switch (Pivot)
//		{
//		case PivotList.Left :
//		{
//			openAngle = 90f;
//			closedAngle = 0f;
//			break;
//		}
//		case PivotList.Right :
//		{
//			openAngle = 90f;
//			closedAngle = 180f;
//			break;
//		}
//		case PivotList.Up :
//		{
//			openAngle = 0f;
//			closedAngle = 270f;
//			break;
//		}
//		case PivotList.Down :
//		{
//			openAngle = 0f;
//			closedAngle = 270f;
//			break;
//		}
//		}
//		transform.rotation = Quaternion.Euler(0f,0f, closedAngle);

	}
}
