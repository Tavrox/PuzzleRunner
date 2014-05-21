using UnityEngine;
using System.Collections;

public class PopUp : MonoBehaviour {

	private OTSprite spr;
	private TextUI txtUi;
	public enum CharList
	{
		Dracula,
		Johnathan
	};
	public CharList Owner;
	public OTSprite BG;
	public OTSprite Picture;
	public TextUI linkText;
	public TextUI helpTxt;

	// Use this for initialization
	public void Setup () 
	{
		BG = FETool.findWithinChildren(gameObject, "BG").GetComponentInChildren<OTSprite>();
		Picture = FETool.findWithinChildren(gameObject, "Picture").GetComponentInChildren<OTSprite>();
		linkText = FETool.findWithinChildren(gameObject,"Text").GetComponent<TextUI>();
		helpTxt = FETool.findWithinChildren(gameObject,"Help").GetComponent<TextUI>();
	}

	public void giveInfos(string txt, CharList _ownr)
	{
		Setup();
		linkText.text = txt;
		Owner = _ownr;
	}

	public void checkForInput()
	{
		if (Input.GetKey(KeyCode.Return))
		{
			Fade();
		}
	}

	public void Pop()
	{
		new OTTween(BG, 0.5f).Tween("alpha", 1f);
		new OTTween(Picture, 0.5f).Tween("alpha", 1f);
		switch (Owner)
		{
		case CharList.Dracula :
		{
//			Picture.frameName = "Dracula_pic";
			new OTTween(linkText, 0.5f).Tween("color", Color.red);
			break;
		}
			
		case CharList.Johnathan :
		{
//			Picture.frameName = "John_pic";
			new OTTween(linkText, 0.5f).Tween("color", Color.red);
			break;
		}
		}
		new OTTween(helpTxt, 0.5f).Tween("color", Color.white);
		InvokeRepeating("checkForInput", 2f, 0.1f);
	}

	public void Fade()
	{
		new OTTween(BG, 0.5f).Tween("alpha", 0f);
		new OTTween(Picture, 0.5f).Tween("alpha", 0f);
		new OTTween(linkText, 0.5f).Tween("color", Color.clear);
		new OTTween(helpTxt, 0.5f).Tween("color", Color.clear);

	}
}
