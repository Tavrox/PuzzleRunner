using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class DialogSheet : ScriptableObject {

	private GameSetup.languageList currLanguage;
	private XmlDocument Doc;
	public TextAsset dialog_file; 
	public Dictionary<string, string> translated_texts; // First is ID, Second is Translation
	public bool translateGame;

	public void SetupTranslation(GameSetup.languageList _lang)
	{
		currLanguage = _lang;
		translated_texts = fillDicoText(currLanguage);
//		Debug.Log (translated_texts +""+_lang);
//		Debug.Log (translated_texts.Count);
	}
	public string TranslateSingle(TextUI _txt)
	{
		string result = _txt.text;
		if (translateGame == true && _txt.dontTranslate != true)
		{
			if ( translated_texts != null && translated_texts.ContainsKey(_txt.DIALOG_ID) != false && _txt.DIALOG_ID != null)
			{
				result = translated_texts[_txt.DIALOG_ID];
				_txt.hasBeenTranslated = true;
			}
			else
			{
				Debug.Log(_txt.gameObject.name + "/" +  _txt.DIALOG_ID + " couldn't be foundSINGLE");
				_txt.hasBeenTranslated = false;
				result = "NOT FOUND";
			}
		}
		return (result);
	}
	public void TranslateAll(ref TextUI[] _arrTxt)
	{
		if (translateGame == true)
		{
			foreach (TextUI _tx in _arrTxt)
			{
				if (translated_texts.ContainsKey(_tx.DIALOG_ID) != false && _tx.dontTranslate == false)
				{		
					_tx.text = translated_texts[_tx.DIALOG_ID];
					_tx.hasBeenTranslated = true;
				}
				else
				{
					if (_tx.dontTranslate == false)
					{
						Debug.Log(_tx.gameObject.name + "/" +  _tx.DIALOG_ID + " couldn't be foundALL");
						_tx.text = "NOT FOUND";
						_tx.hasBeenTranslated = false;
					}
				}
			}
		}
	}

	private Dictionary<string, string> fillDicoText(GameSetup.languageList _lang)
	{
		Doc = new XmlDocument();
//		Debug.Log("Loading" +_lang.ToString());
		Doc.LoadXml(dialog_file.text);

		Dictionary<string, string> translate = new Dictionary<string, string>();
		XmlNodeList TextNode = Doc.SelectNodes("texts");

		foreach (XmlNode node in TextNode)
		{
			node.InnerXml = node.InnerXml.Replace("	", "");
			XmlNodeList entries = node.SelectNodes("entry");
			foreach (XmlNode entry in entries)
			{
				string entryID = entry.Attributes.GetNamedItem("id").Value;
				XmlNodeList LangEntry = entry.SelectNodes(_lang.ToString());
				string entryTranslation = LangEntry.Item(0).InnerText;
				entryTranslation = entryTranslation.Replace("/n", "\n");

				if (translate.ContainsKey(entryID) == false)
				{
					translate.Add(entryID, entryTranslation);
				}
			}
		}
		return (translate);
	}
	
	
	public string findTranslation (string _id)
	{
		string result = _id;
		if (translateGame == true)
		{
			if ( translated_texts.ContainsKey(_id) != false)
			{
				result = translated_texts[_id];
			}
			else
			{
				Debug.Log(_id + " couldn't be found");
				result = "NOT FOUND";
			}
		}
		return (result);
	}

}
