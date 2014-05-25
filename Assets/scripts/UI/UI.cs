using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public OTSprite WillBack;
	public OTSprite Controls;
	public TextUI WillBackTxt;

	public enum menuTypes
	{
		Ingame,
		Start,
		Victory,
		Title, 
		Death
	};
	public menuTypes Menu;

	public GameObject VictoryGO;
	public GameObject DeathGO;
	public GameObject StartGO;
	public GameObject TitleGO;

	public OTSprite DarkBG;

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
		Controls = FETool.findWithinChildren(gameObject, "Ingame/Controls").GetComponentInChildren<OTSprite>();

		dialogPop = FETool.findWithinChildren(gameObject, "Ingame/Dialog").GetComponent<PopUp>();
		WillBack = FETool.findWithinChildren(gameObject, "Ingame/Dialog/WillBeBack").GetComponentInChildren<OTSprite>();
		WillBackTxt = FETool.findWithinChildren(gameObject, "Ingame/Dialog/WillBeBack").GetComponentInChildren<TextUI>();
		dialogPop.Setup();

		VictoryGO = FETool.findWithinChildren(gameObject, "Ingame/Victory");
		DeathGO = FETool.findWithinChildren(gameObject, "Ingame/Death");
		StartGO = FETool.findWithinChildren(gameObject, "Ingame/Start");
		TitleGO = FETool.findWithinChildren(gameObject, "Ingame/Title");

		StartCoroutine("fadeControls");
//		dialogPop.Fade();

		Paper = FETool.findWithinChildren(gameObject, "Ingame/Paper/SeekPaper").GetComponentInChildren<OTSprite>();
		paperText = FETool.findWithinChildren(gameObject, "Ingame/Paper/State").GetComponentInChildren<TextUI>();

		Clock = FETool.findWithinChildren(gameObject, "Ingame/Panels/Clock/Hours").GetComponent<TextUI>();
		DarkBG = FETool.findWithinChildren(gameObject, "Ingame/Black").GetComponentInChildren<OTSprite>();

		UIBtn[] listBtn = GetComponentsInChildren<UIBtn>();
		foreach ( UIBtn btn in listBtn)
		{
			btn.Setup(this);
		}
		
		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameStart += GameStart;
		GameEventManager.EndGame += EndGame;
	}

	public void AppearTextsAndSprite(GameObject _targ)
	{
		new OTTween(DarkBG, 1f).Tween("alpha", 1f);
		if (_targ.GetComponentsInChildren<OTSprite>() != null)
		{
			foreach (OTSprite _spr in _targ.GetComponentsInChildren<OTSprite>())
			{
				_spr.alpha = 1f;
			}
		}
		if (_targ.GetComponentsInChildren<TextUI>() != null)
		{
			foreach (TextUI _txt in _targ.GetComponentsInChildren<TextUI>())
			{
				_txt.makeFadeIn();
			}
		}
		if (_targ.GetComponentsInChildren<UIBtn>() != null)
		{
			foreach (UIBtn _btn in _targ.GetComponentsInChildren<UIBtn>())
			{
				_btn.reEnable();
			}	
		}
	}

	public void ClearTextsAndSprite(GameObject _targ)
	{
		new OTTween(DarkBG, 1f).Tween("alpha", 0f);
		if (_targ.GetComponentsInChildren<OTSprite>() != null)
		{
			foreach (OTSprite _spr in _targ.GetComponentsInChildren<OTSprite>())
			{
				_spr.alpha = 0f;
			}
		}
		if (_targ.GetComponentsInChildren<TextUI>() != null)
		{
			foreach (TextUI _txt in _targ.GetComponentsInChildren<TextUI>())
			{
				_txt.makeFadeOut();
			}
		}
		if (_targ.GetComponentsInChildren<UIBtn>() != null)
		{
			foreach (UIBtn _btn in _targ.GetComponentsInChildren<UIBtn>())
			{
				_btn.reDisable();
			}
		}
	}

	public void DisplayTitleMenu()
	{
		ClearTextsAndSprite(TitleGO);
		AppearTextsAndSprite(StartGO);
	}

	
	public void triggerPopUp()
	{
		GameObject popup = Instantiate(Resources.Load("Objects/PopUp")) as GameObject;
		PopUp pop = popup.GetComponent<PopUp>();
	}
	
	IEnumerator fadeControls()
	{
		yield return new WaitForSeconds(30f);
		Controls.alpha = 0f;
	}

	// Update is called once per frame
	void Update () 
	{
		HandClock.rotation = 360 - (24* levMan.currH);

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
			paperText.text = "Hold E To Write";
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

	public void discoverCharacter(PopUp.CharList _char)
	{
		if ( _char == PopUp.CharList.Dracula)
		{
			dialogPop.DraculaPic.frameName = "portrait_dracula";
		}
		else if (_char == PopUp.CharList.MailMan)
		{
			dialogPop.DraculaPic.frameName = "portrait_mailman";
		}
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

	private void GameStart()
	{
		Menu = menuTypes.Title;
		ClearTextsAndSprite(VictoryGO);
		ClearTextsAndSprite(DeathGO);
		ClearTextsAndSprite(StartGO);
		DarkBG.alpha = 1f;
		AppearTextsAndSprite(TitleGO);
	}
	private void GameOver()
	{
		Menu = menuTypes.Death;

		MasterAudio.PlaySound("death");
		StartCoroutine("sayDied");
	}
	private void EndGame()
	{
		Menu = menuTypes.Victory;
		StartCoroutine("WinPlz");
	}
	private void Respawn()
	{
		Menu = menuTypes.Ingame;
		MasterAudio.FadeAllPlaylistsToVolume(1f, 3f);
		ClearTextsAndSprite(VictoryGO);
		ClearTextsAndSprite(DeathGO);
		ClearTextsAndSprite(StartGO);
		ClearTextsAndSprite(TitleGO);
	}

	IEnumerator WinPlz()
	{
		MasterAudio.FadeAllPlaylistsToVolume(0f, 3f);
		yield return new WaitForSeconds(3f);
		MasterAudio.PlaySound("win");
		AppearTextsAndSprite(VictoryGO);
	}

	IEnumerator sayDied()
	{
		MasterAudio.FadeAllPlaylistsToVolume(0f, 3f);
		yield return new WaitForSeconds(3f);
		MasterAudio.PlaySound("game_over");
		ClearTextsAndSprite(VictoryGO);
		ClearTextsAndSprite(DeathGO);
		ClearTextsAndSprite(StartGO);
		AppearTextsAndSprite(DeathGO);
	}


}
