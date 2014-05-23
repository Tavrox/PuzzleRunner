using UnityEngine;
using System.Collections;

public class HoursManager : MonoBehaviour {

	public LevelManager _levMan;

	public enum DayEventList
	{
		None,
		MailManIn,
		MailManOut,
		FoodMan,
		DraculaEntering,
		DraculaLeaving,
		LetterSent
	};
	public enum DayTime
	{
		Day,
		Night
	};
	public DayTime currentTime;

	public int mailManHourIn;
	public int mailManHourOut;
	public int foodManHour;
	public int draculaLeavingHour;
	public int draculaEnteringHour;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		_levMan = _lev;
		mailManHourIn = 8;
		mailManHourOut = 12;
		foodManHour = 15;
		draculaLeavingHour = 18;
		draculaEnteringHour = 6;
	}

	public DayEventList findEvent(int _hour)
	{
		DayEventList dvl = DayEventList.None;
		if (_hour == mailManHourIn && _levMan.plr.haveLetter != Player.letterList.Sent)
		{
			dvl = DayEventList.MailManIn;
		}
		else if (_hour == foodManHour)
		{
			dvl = DayEventList.FoodMan;
		}
		else if (_hour == draculaLeavingHour)
		{
			dvl = DayEventList.DraculaLeaving;
		}
		else if (_hour == draculaEnteringHour)
		{
			dvl = DayEventList.DraculaEntering;
		}
		else if (_hour == mailManHourOut && _levMan.plr.haveLetter != Player.letterList.Sent)
		{
			dvl = DayEventList.MailManOut;
		}
		return dvl;
	}
}
