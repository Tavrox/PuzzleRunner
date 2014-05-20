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

	// Use this for initialization
	void Setup (string txt, CharList _ownr ) 
	{
		Owner = _ownr;
		txtUi.text = txt;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
