using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public LevelManager levMan;
	public TextUI Clock;
	public PopUp dialogPop;
	public PopUp NotifPop;

	public OTSprite Paper;
	public TextUI paperText;
	public OTSprite HandClock;
	public OTSprite FoodState;
	public OTSprite SleepState;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		levMan = _lev;

		NotifPop = FETool.findWithinChildren(gameObject ,"Ingame/Panels/NotifObj").GetComponent<PopUp>();
		NotifPop.Setup();
		NotifPop.Fade();
	
		HandClock = FETool.findWithinChildren(gameObject, "Ingame/Panels/Clock/Aiguille").GetComponentInChildren<OTSprite>();
		FoodState = FETool.findWithinChildren(gameObject, "Ingame/Panels/PanFood/Bar").GetComponentInChildren<OTSprite>();
		SleepState = FETool.findWithinChildren(gameObject, "Ingame/Panels/PanSleep/Bar").GetComponentInChildren<OTSprite>();

		dialogPop = FETool.findWithinChildren(gameObject, "Ingame/Dialog").GetComponent<PopUp>();
		dialogPop.Setup();
//		dialogPop.Fade();

		Paper = FETool.findWithinChildren(gameObject, "Ingame/Paper/SeekPaper").GetComponentInChildren<OTSprite>();
		paperText = FETool.findWithinChildren(gameObject, "Ingame/Paper/State").GetComponentInChildren<TextUI>();

		Clock = FETool.findWithinChildren(gameObject, "Ingame/Panels/Clock/Hours").GetComponent<TextUI>();
	}

	
	public void triggerPopUp()
	{
		GameObject popup = Instantiate(Resources.Load("Objects/PopUp")) as GameObject;
		PopUp pop = popup.GetComponent<PopUp>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		HandClock.rotation = ((360 / 24 ) * levMan.currH) -1;
		switch (levMan.plr.haveLetter)
		{
		case Player.letterList.DontHave :
		{
			Paper.frameName = "look_for_paper";
			paperText.text = "Find a paper";
			break;
		}
		case Player.letterList.Have :
		{
			Paper.frameName = "paper_found";
			Paper.alpha = 0.1f;
			paperText.text = "Hold R To Write";
			break;
		}
		case Player.letterList.IsWriting :
		{
			Paper.frameName = "paper_found";
			Paper.alpha = 1f;
			paperText.text = levMan.realWrittenPaper + "% Written";
			break;
		}
		case Player.letterList.HaveWritten :
		{
			Paper.frameName = "paper_found";
			paperText.text = "Deliver Mailman";
			break;
		}
		}

		attributeStateSprite(levMan.plr.foodState, FoodState);
		attributeStateSprite(levMan.plr.sleepState, SleepState);
	}

	private void attributeStateSprite(int attribute, OTSprite _spr)
	{
		if (attribute == 0)
		{
			_spr.frameName = "bar_00";
		}
		else if (attribute == 1)
		{
			_spr.frameName = "bar_01";
		}
		else if (attribute == 2)
		{
			_spr.frameName = "bar_02";
		}
		else if (attribute == 3)
		{
			_spr.frameName = "bar_03";
		}
	}
}
