using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetup : ScriptableObject {

	public enum LevelList
	{
		None,
		Grensdalur,
		Etna,
		Vesuvio,
		Gedamsa,
		Fuji,
		Whakaari,
		Augustine,
		Lanzarote,
		Amirani, 
		Olympus,
		GameEnding,
		Oblivion,
		Test,
		Trailer
	};
	public enum languageList
	{
		french,
		english
	};
	public enum versionType
	{
		Alpha,
		Demo
	};
	public enum GameModeEnum
	{
		Linear,
		Procedural,
		Maze,
		VerticalScroller
	}
	public versionType GameType;
	public Vector2 GameSize;
	public float OrthelloSize;
	public string gameversion;
	public languageList ChosenLanguage;
	public string twitter_url;
	public string facebook_url;
	public string website_url;
	public DialogSheet TextSheet;
	public bool translated = false;

	public string changeLang( languageList _chosen)
	{
		ChosenLanguage = _chosen;
		return ChosenLanguage.ToString();
	}

	public void startTranslate(languageList _chosen)
	{
		if (TextSheet != null)
		{
			TextSheet.SetupTranslation(_chosen);
			translated = true;
		}
		else
		{
			Debug.Log ("TextSheet is missing");
		}
	}
	public void translateSceneText()
	{
		TextSheet.SetupTranslation(ChosenLanguage);
		TextUI[] allTxt = GameObject.FindObjectsOfType(typeof(TextUI)) as TextUI[];
		TextSheet.TranslateAll(ref allTxt);
	}
}
