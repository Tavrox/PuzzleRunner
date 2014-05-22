using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {

	private OTSprite spr;
	private TextUI txtUi;
//	publ
	public enum CharList
	{
		None,
		Dracula,
		Johnathan
	};
	public enum PopTypeList
	{
		NotifHour,
		Dialog
	};
	public PopTypeList PopType;
	public CharList Owner;
	public OTSprite BG;
	public OTSprite DraculaPic;
	public OTSprite JohnPic;
	public OTSprite OutPic;
	public TextUI linkText;
	public TextUI helpTxt;

	// Use this for initialization
	public void Setup () 
	{
		switch ( PopType)
		{
			case PopTypeList.Dialog :
			{
				DraculaPic = FETool.findWithinChildren(gameObject, "DraculaPic").GetComponentInChildren<OTSprite>();
				JohnPic = FETool.findWithinChildren(gameObject, "JohnPic").GetComponentInChildren<OTSprite>();
				OutPic = FETool.findWithinChildren(gameObject, "WillBeBack").GetComponentInChildren<OTSprite>();
				helpTxt = FETool.findWithinChildren(gameObject,"Help").GetComponent<TextUI>();
				break;
			}
			case PopTypeList.NotifHour :
			{
				break;
			}
		}
		BG = FETool.findWithinChildren(gameObject, "BG").GetComponentInChildren<OTSprite>();
		linkText = FETool.findWithinChildren(gameObject,"Text").GetComponent<TextUI>();
	}

	public void giveInfos(string txt, CharList _ownr)
	{
		Setup();
		linkText.text = txt;
		Owner = _ownr;
		switch ( PopType)
		{
		case PopTypeList.Dialog :
		{
			PopDialog();
			break;
		}
		case PopTypeList.NotifHour :
		{
			PopNotif();
			break;
		}
		}
	}
	public void giveInfos(string txt)
	{
		Setup();
		linkText.text = txt;
		switch ( PopType)
		{
		case PopTypeList.Dialog :
		{
			PopDialog();
			break;
		}
		case PopTypeList.NotifHour :
		{
			PopNotif();
			break;
		}
		}
	}
	
	public void checkForInput()
	{
		if (Input.GetKey(KeyCode.Return))
		{
			Skip();
			CancelInvoke("checkForInput");
		}
	}

	public void PopNotif()
	{		
		new OTTween(BG, 0.5f).Tween("alpha", 1f);
		linkText.makeFadeIn();
		StartCoroutine("FadeAway");
	}

	IEnumerator FadeAway()
	{
		yield return new WaitForSeconds(3f);
		new OTTween(BG, 1f).Tween("alpha", 0f);
		linkText.makeFadeOut(0.5f);
	}

	public void PopDialog()
	{
		switch (Owner)
		{
			case CharList.Dracula :
			{
			new OTTween(linkText, 0.5f).Tween("color", Color.red);
			break;
			}
				
			case CharList.Johnathan :
			{
			linkText.makeFadeIn();
			break;
			}
		}
		InvokeRepeating("checkForInput", 2f, 0.1f);
	}

	public void Fade()
	{
		if (BG != null)
		{
			new OTTween(BG, 0.5f).Tween("alpha", 0f);
		}
		linkText.makeFadeOut();
		helpTxt.makeFadeOut();
	}

	public void Skip()
	{

	}
}
