using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]

public class TextUI : MonoBehaviour 
{
	[HideInInspector] public TextMesh _mesh;
	[HideInInspector] public Color initColor;

	public string DIALOG_ID = "NONE";
	public string text = "Superb text";
	public bool dontTranslate = false;
	public bool hasBeenTranslated = false;
	public Color color = Color.white;

	public void Awake()
	{
		_mesh = GetComponent<TextMesh>();
		initColor = color;
		DIALOG_ID = gameObject.name;
	}

	void Start()
	{
		if (hasBeenTranslated == false && dontTranslate == false)
		{
			TranslateThis();
		}
	}
	
	void Update()
	{
		text = text.Replace("/n", "\n");
		_mesh.text = text;
		_mesh.color = color;
	}

	public void makeFadeOut(float _dur = 0.5f)
	{
		new OTTween(this, _dur).Tween("color", Color.clear);
	}

	public void makeFadeIn(float _dur = 0.5f)
	{
		new OTTween(this, _dur).Tween("color", initColor);
	}

	public void Format()
	{
		text = text.Replace("/n", "\n");
	}
	public void TranslateThis()
	{
//		text = SETUP.TextSheet.TranslateSingle(this);
	}
	public void TranslateThis(string _str)
	{
		DIALOG_ID = _str;
//		text = SETUP.TextSheet.TranslateSingle(this);
	}

	public void TranslateVar(string _dialogId)
	{
//		text = SETUP.TextSheet.TranslateSingle(this);
	}

	public void TranslateAllInScene()
	{
//		SETUP.TextSheet.SetupTranslation(SETUP.ChosenLanguage);
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
//		SETUP.TextSheet.TranslateAll(ref allTxt);
	}

	public void resetAllDialogID()
	{
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		foreach (TextUI _tx in allTxt)
		{
			if (_tx.DIALOG_ID == "" || _tx.DIALOG_ID == " ")
			{
				_tx.DIALOG_ID = "NONE";
			}
		}
	}
	public void renameAllTextObject()
	{
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		foreach (TextUI _tx in allTxt)
		{
			if (_tx.DIALOG_ID != "" || _tx.DIALOG_ID != " " || _tx.DIALOG_ID != "NONE")
			{
				_tx.gameObject.name = _tx.DIALOG_ID;
			}
		}	
	}
	public void SetupDialogIDFromGameObject()
	{
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		foreach (TextUI _tx in allTxt)
		{
			_tx.DIALOG_ID = _tx.gameObject.name;
		}			
	}
}
