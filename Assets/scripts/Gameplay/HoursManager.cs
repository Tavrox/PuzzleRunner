using UnityEngine;
using System.Collections;

public class HoursManager : MonoBehaviour {

	public LevelManager _levMan;

	public int mailManMin;
	public int mailManMax;

	public int foodManMin;
	public int foodManMax;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		_levMan = _lev;
	}
	
	// Update is called once per frame
	void Update () 
	{

		// MAIL MAN
		if (_levMan.currH < 0 || _levMan.currH < 10)
		{
			_levMan.triggerDayEvent(DayEvent.DayEventList.MailMan);
		}

		// DRACULA ENTERING
		if (_levMan.currH < 0 || _levMan.currH < 10)
		{
			_levMan.triggerDayEvent(DayEvent.DayEventList.MailMan);
		}

		// DRACULA LEAVING
		if (_levMan.currH < 0 || _levMan.currH < 10)
		{
			_levMan.triggerDayEvent(DayEvent.DayEventList.MailMan);
		}

		// FOOD MAN
		if (_levMan.currH < 0 || _levMan.currH < 10)
		{
			_levMan.triggerDayEvent(DayEvent.DayEventList.MailMan);
		}
		
	}
}
