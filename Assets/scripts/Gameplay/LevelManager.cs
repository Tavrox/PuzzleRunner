using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static GameEventManager.GameState GAMESTATE;

	public Player plr;
	public Dracula Dracu;
	public HoursManager Hours;

	public int currH;
	public int currS;
	public WaypointDirector pathDirector;

	public GameObject paperSpot;

	// Use this for initialization
	void Awake () 
	{
		plr = GameObject.Find("Player").GetComponent<Player>();
		plr.Setup();

		Hours = gameObject.AddComponent<HoursManager>();
		Hours.Setup(this);

		pathDirector = GetComponentInChildren<WaypointDirector>();
		pathDirector.Setup(this);

		Dracu = GetComponentInChildren<Dracula>();
		Dracu.Setup(this);

		GameEventManager.GameOver += GameOver;
	}

	private void GameOver()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		Camera.main.transform.position =  new Vector3( FETool.Round(plr.transform.position.x, 2), FETool.Round(plr.transform.position.y, 2), 0f);
	}

	public void triggerDayEvent(DayEvent.DayEventList _evt)
	{

	}

	public void triggerPopUp()
	{
		GameObject popup = Instantiate(Resources.Load("Objects/PopUp")) as GameObject;
		PopUp pop = popup.GetComponent<PopUp>();
	}
}
