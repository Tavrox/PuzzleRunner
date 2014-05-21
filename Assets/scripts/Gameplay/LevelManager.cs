using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static GameEventManager.GameState GAMESTATE;

	public enum MailManStateList
	{
		HasArrived,
		Away
	};
	public MailManStateList MailmanState;

	public Player plr;
	public Dracula Dracu;
	public HoursManager Hours;
	public List<GameObject> PaperSpots;
	public List<GameObject> FoodSpots;

	public int currH;
	public int currS;
	public WaypointDirector pathDirector;

	public PopUp thePop;

	// Use this for initialization
	void Awake () 
	{
		plr = GetComponentInChildren<Player>();
		plr.Setup();

		Hours = gameObject.AddComponent<HoursManager>();
		Hours.Setup(this);

		pathDirector = GetComponentInChildren<WaypointDirector>();
		pathDirector.Setup(this);

		Dracu = GetComponentInChildren<Dracula>();
		Dracu.Setup(this);

		PaperSpots = new List<GameObject>();
		GameObject pSpot = FETool.findWithinChildren(gameObject, "Spots/PaperSpot");
		Spot[] papSpot = pSpot.GetComponentsInChildren<Spot>();
		foreach (Spot obj in papSpot)
		{
			obj.spotType = Spot.spotList.Paper;
			PaperSpots.Add(obj.gameObject);
		}

		FoodSpots = new List<GameObject>();
		GameObject fSpot = FETool.findWithinChildren(gameObject, "Spots/FoodSpot");
		Spot[] fooSpot = fSpot.GetComponentsInChildren<Spot>();
		foreach (Spot obj in fooSpot)
		{
			obj.spotType = Spot.spotList.Food;
			FoodSpots.Add(obj.gameObject);
		}

		respawnPaper(PaperSpots);
		spawnFood(FoodSpots);

		thePop = GetComponentInChildren<PopUp>();
		thePop.Setup();
		thePop.Fade();
		
		MailmanState = MailManStateList.Away;

//		InvokeRepeating("

		Door[] Doors = GetComponentsInChildren<Door>();
		foreach (Door _dr in Doors)
		{
			_dr.Setup(this);
		}

		GameEventManager.GameOver += GameOver;
	}

	private void GameOver()
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		Camera.main.transform.position =  new Vector3( FETool.Round(plr.transform.position.x, 2), FETool.Round(plr.transform.position.y, 2), 0f);

		if (Input.GetKey(KeyCode.A))
		{
			thePop.giveInfos("LOL", PopUp.CharList.Dracula);
			thePop.Pop();
		}
		if (Input.GetKey(KeyCode.B))
		{
			thePop.giveInfos("TROLLA", PopUp.CharList.Johnathan);
		}

	}

	public void respawnPaper(List<GameObject> _ppSpot)
	{
		int randId = Random.Range(0, _ppSpot.Count);
		GameObject obj = Instantiate(Resources.Load("Objects/Letter")) as GameObject;
		obj.transform.position = _ppSpot[randId].transform.position;
		obj.transform.parent = _ppSpot[randId].transform;
	}

	public void spawnFood(List<GameObject> _fdSpot)
	{
		foreach (GameObject obj in _fdSpot)
		{
			GameObject food = Instantiate(Resources.Load("Objects/FoodCrate")) as GameObject;
			food.transform.position = obj.transform.position;
			food.transform.parent = obj.transform;
		}

	}

	public void triggerDayEvent(DayEvent.DayEventList _evt)
	{
		switch (_evt)
		{
		case DayEvent.DayEventList.DraculaEntering :
		{

			break;
		}
		case DayEvent.DayEventList.DraculaLeaving :
		{
			
			break;
		}
		case DayEvent.DayEventList.FoodMan :
		{
			// SAY FOOD HAS ARRIVED
			// 
			break;
		}
		case DayEvent.DayEventList.MailMan :
		{
			// SAY MAIL HAS ARRIVED
			// LET PLAYER 
			break;
		}

		}
	}

	public void triggerPopUp()
	{
		GameObject popup = Instantiate(Resources.Load("Objects/PopUp")) as GameObject;
		PopUp pop = popup.GetComponent<PopUp>();
	}
}
