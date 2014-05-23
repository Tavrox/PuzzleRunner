using UnityEngine;
using System.Collections;

public class MailManPlace : MonoBehaviour {


	public LevelManager levMan;

	// Use this for initialization
	public void Setup (LevelManager _levMan) 
	{
		levMan = _levMan;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter (Collider _oth)
	{
		if (_oth.CompareTag("Player") == true && levMan.plr.haveLetter == Player.letterList.HaveWritten && levMan.MailmanState == LevelManager.MailManStateList.HasArrived)
		{
			levMan.triggerDayEvent(HoursManager.DayEventList.LetterSent);
		}

	}
}
