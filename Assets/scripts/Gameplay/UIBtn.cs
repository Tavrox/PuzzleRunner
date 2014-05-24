using UnityEngine;
using System.Collections;

public class UIBtn : MonoBehaviour {

	public enum btnTypeList
	{
		Respawn,
		Leave,
		LaunchTitle
	};
	public btnTypeList btnType;
	public UI mainUi;
	public OTSprite currSpr;
	public string initName;
	public string HighlightName = "";
	public bool displayed;
	public BoxCollider Coll;

	// Use this for initialization
	public void Setup (UI  _mn)
	{
		mainUi = _mn;
		if (GetComponentInChildren<OTSprite>() != null)
		{
			currSpr = GetComponentInChildren<OTSprite>();
			initName = currSpr.frameName;
			HighlightName = initName + "_hl";
		}
		if (GetComponent<BoxCollider>() != null)
		{
			Coll = GetComponent<BoxCollider>();
		}
		displayed = true;
	}

	void OnMouseDown()
	{
		if (displayed == true)
		{
			switch (btnType)
			{
				case btnTypeList.Respawn :
				{
					GameEventManager.TriggerRespawn("Respawn button");
					break;
				}
				case btnTypeList.LaunchTitle :
				{
					mainUi.DisplayTitleMenu();
					break;
				}
				case btnTypeList.Leave :
				{
					Application.Quit();
					break;
				}
			}
			displayed = false;
			Coll.isTrigger = false;
		}
	}

	void OnMouseOver()
	{
//		currSpr.frameName = HighlightName;
	}

	public void reEnable()
	{
		Coll.isTrigger = true;
		displayed = true;
	}

	void OnMouseExit()
	{
		if (currSpr != null)
		{
			currSpr.frameName = initName;
		}
	}
}
