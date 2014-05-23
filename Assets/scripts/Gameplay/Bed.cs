using UnityEngine;
using System.Collections;

public class Bed : MonoBehaviour 
{
	public enum hourList
	{
		Three,
		Six
	};
	public hourList Hour;

	void OnMouseDown()
	{
		GameObject.Find("LevelManager").GetComponent<LevelManager>().doSleep(Hour);
	}
	void OnMouseOver()
	{
		if (GetComponentInChildren<OTSprite>() != null && Hour == hourList.Six)
		{
			GetComponentInChildren<OTSprite>().frameName = "sleep_6h_highlight";
		}
		if (GetComponentInChildren<OTSprite>() != null && Hour == hourList.Three)
		{
			GetComponentInChildren<OTSprite>().frameName = "sleep_3h_highlight";
		}
	}
}
