using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaypointDirector : MonoBehaviour {


	private LevelManager _lm;
	public List<WaypointManager> waypointsMan = new List<WaypointManager>();
	public WaypointManager _DraculaListSpawnBeforeSendingLetter;
	public WaypointManager _DraculaListSpawnBeforeSaving;

	// Use this for initialization
	public void Setup (LevelManager _levMan)
	{
		_lm = _levMan;
		WaypointManager[] waypointsManagers = GetComponentsInChildren<WaypointManager>();
		foreach (WaypointManager wpm in waypointsManagers)
		{
			waypointsMan.Add(wpm);
			wpm.Setup(_lm);
		}
		_DraculaListSpawnBeforeSendingLetter = waypointsMan[0];
		_DraculaListSpawnBeforeSaving = waypointsMan[1];
	}

	public WaypointManager pickRandomWPM()
	{
		return waypointsMan[Random.Range(0, waypointsMan.Count)];
	}
}
