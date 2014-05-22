using UnityEngine;
using System.Collections;

public class HoursManager : MonoBehaviour {

	public LevelManager _levMan;

	public enum DayEventList
	{
		None,
		MailMan,
		FoodMan,
		DraculaEntering,
		DraculaLeaving
	};

	public int mailManHour;
	public int foodManHour;
	public int draculaLeavingHour;
	public int draculaEnteringHour;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		_levMan = _lev;
		mailManHour = 5;
		foodManHour = 20;
		draculaLeavingHour = 30;
		draculaEnteringHour = 50;
	}

	public DayEventList findEvent(int _hour)
	{
		DayEventList dvl = DayEventList.None;
		if (_hour == mailManHour)
		{
			dvl = DayEventList.MailMan;
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
		return dvl;
	}
}
