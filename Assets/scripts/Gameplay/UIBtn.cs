using UnityEngine;
using System.Collections;

public class UIBtn : MonoBehaviour {

	public enum btnTypeList
	{
		Respawn,
		Start,
		Leave
	}

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnMouseDown()
	{
		GameEventManager.TriggerRespawn("btn ui");
	}

	void OnMouseOver()
	{
		if (GetComponent<OTSprite>() != null)
		{
//			if (GetComponent<OTSprite>().frameName
//			GetComponent<OTSprite>();
		}
	}
}
