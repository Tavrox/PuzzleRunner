using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	public LevelManager levMan;
	public TextUI Clock;
	public PopUp dialogPop;
	public PopUp NotifPop;

	public OTSprite Paper;
	public TextUI paperText;
	
	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		levMan = _lev;

		NotifPop = FETool.findWithinChildren(gameObject ,"Ingame/Panels/NotifObj").GetComponent<PopUp>();
		NotifPop.Setup();

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
	}
}
