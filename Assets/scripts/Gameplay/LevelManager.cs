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
	public Camera currCam;

	public int currH;
	public int currM;
	public WaypointDirector pathDirector;
	public UI GameUI;
	public float writtenPaper;
	public int realWrittenPaper;

	// Use this for initialization
	void Awake () 
	{
		plr = GetComponentInChildren<Player>();
		plr.Setup(this);

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

		
		MailmanState = MailManStateList.Away;
		currCam = FETool.findWithinChildren(gameObject, "Camera").GetComponent<Camera>();
		GameUI = currCam.GetComponentInChildren<UI>();
		GameUI.Setup(this);

//		InvokeRepeating("updateMinute", 0f, 0.01f);

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

	private void updateMinute()
	{
		if (currM == 59)
		{
			currH += 1;
			currM = 0;
		}
		else
		{
			currM += 1;
		}
		string parseH = currH.ToString();
		string parseM = currM.ToString();
		if (currM < 10)
		{
			parseM = "0" + currM;
		}
		if (currH < 10)
		{
			parseH = "0" + currH;
		}
		GameUI.Clock.text = parseH + ":" + parseM;
		triggerDayEvent(Hours.findEvent(currH));
	}
	
	// Update is called once per frame
	void Update () 
	{
		currCam.transform.position =  new Vector3( FETool.Round(plr.transform.position.x, 2), FETool.Round(plr.transform.position.y, 2), -1000f);
		realWrittenPaper = Mathf.RoundToInt(writtenPaper);

		if (Input.GetKey(KeyCode.A))
		{
			GameUI.dialogPop.giveInfos("LOL", PopUp.CharList.Dracula);
		}
		if (Input.GetKey(KeyCode.B))
		{
			GameUI.dialogPop.giveInfos("TROLLA", PopUp.CharList.Johnathan);
		}
		if (Input.GetKey(KeyCode.C))
		{
			GameUI.NotifPop.giveInfos("TROLLA");
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

	public void triggerDayEvent(HoursManager.DayEventList _evt)
	{
		switch (_evt)
		{
		case HoursManager.DayEventList.DraculaEntering :
		{
			GameUI.NotifPop.giveInfos("Dracula is back from hunting");
			GameUI.dialogPop.giveInfos("Dracula is entering the game !", PopUp.CharList.Johnathan);
			new OTTween(GameUI.dialogPop.OutPic, 1f).Tween("alpha", 1f);
			break;
		}
		case HoursManager.DayEventList.DraculaLeaving :
		{
			GameUI.NotifPop.giveInfos("Dracula is gone hunting outside the house");
			GameUI.dialogPop.giveInfos("Dracula is leaving the house !", PopUp.CharList.Johnathan);
			new OTTween(GameUI.dialogPop.OutPic, 1f).Tween("alpha", 0f);
			break;
		}
		case HoursManager.DayEventList.FoodMan :
		{
			spawnFood(FoodSpots);
			GameUI.NotifPop.giveInfos("The Carrier has resplenished food stocks");
			GameUI.dialogPop.giveInfos("I can now eat in the house's kitchens", PopUp.CharList.Johnathan);
			break;
		}
		case HoursManager.DayEventList.MailManIn :
		{
			MailmanState = MailManStateList.HasArrived;
			GameUI.dialogPop.giveInfos("The mail man is waiting for me, I should hurry", PopUp.CharList.Johnathan);
			GameUI.NotifPop.giveInfos("The mail man has arrived");
			break;
		}
		case HoursManager.DayEventList.MailManOut :
		{
			MailmanState = MailManStateList.Away;
			GameUI.dialogPop.giveInfos("The mail man is gone now.", PopUp.CharList.Johnathan);
			GameUI.NotifPop.giveInfos("The mail man is leaving");
			break;
		}

		}
	}

}
