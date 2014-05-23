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
	public enum DifficultyState
	{
		VeryEasy,
		Easy,
		Medium,
		Hard,
		VeryHard
	};

	public Player plr;
	public Dracula Dracu;
	public HoursManager Hours;
	public List<GameObject> PaperSpots;
	public List<GameObject> FoodSpots;
	public List<Door> gameDoors;
	public List<Waypoint> wpList;
	public Camera currCam;
	
	public int currD;
	public int currH;
	public int currM;
	public int saveDay;
	public int remainingDay = 5;
	public WaypointDirector pathDirector;
	public UI GameUI;
	public float writtenPaper;
	public int realWrittenPaper;
	public bool hourChecked = false;

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

		GetComponentInChildren<MailManPlace>().Setup(this);

		InvokeRepeating("updateMinute", 0f, 0.10f);

		Door[] Doors = GetComponentsInChildren<Door>();
		foreach (Door _dr in Doors)
		{
			_dr.Setup(this);
		}

		Waypoint[] Waypo = GetComponentsInChildren<Waypoint>();
		foreach (Waypoint _wp in Waypo)
		{
			wpList.Add(_wp);
		}

		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameStart += GameStart;
		GameEventManager.EndGame += EndGame;

		MasterAudio.PlaySound("ambiance");

//		GameEventManager.TriggerGameStart();
		GameEventManager.TriggerRespawn("init");
	}
	
	// Update is called once per frame
	void Update () 
	{
		currCam.transform.position =  new Vector3( FETool.Round(plr.transform.position.x, 2), FETool.Round(plr.transform.position.y, 2) - 0.8f, -1000f);
		realWrittenPaper = Mathf.RoundToInt(writtenPaper);
		if (currD == saveDay)
		{
			GameEventManager.TriggerEndGame();
		}
	}

	public void doSleep(Bed.hourList _hr)
	{
		switch (_hr)
		{
		case Bed.hourList.Three :
		{
			currH += 1;
			checkHour();
			currH += 1;
			checkHour();
			currH += 1;
			checkHour();
			break;
		}
		case Bed.hourList.Six :
		{
			currH += 1;
			checkHour();
			currH += 1;
			checkHour();
			currH += 1;
			checkHour();

			currH += 1;
			checkHour();
			currH += 1;
			checkHour();
			currH += 1;
			checkHour();
			break;
		}
		}

	}

	private void updateMinute()
	{
		if (currM == 59)
		{
			currH += 1;
			hourChecked = false;
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
		checkHour();
		if (currH < 10)
		{
			parseH = "0" + currH;
		}
		GameUI.Clock.text = parseH + ":" + parseM;
	}
	public void checkForDifficulty()
	{

	}
	public void checkHour()
	{
		if (hourChecked == false)
		{
			print ("check");
			if (currH == 24)
			{
				MasterAudio.PlaySound("clock_bell");
				currH = 1;
				currD += 1;
			}
			
			if (currH > 6 && currH < 18)
			{
				Hours.currentTime = HoursManager.DayTime.Day;
				GameObject[] objLights = GameObject.FindGameObjectsWithTag("Lights");
				foreach (GameObject obj in objLights)
				{
					obj.GetComponent<Light>().color = new Color(217f, 242f, 255f);
				}
				CancelInvoke("laughPlz");
			}
			else
			{
				Hours.currentTime = HoursManager.DayTime.Night;
				GameObject[] objLights = GameObject.FindGameObjectsWithTag("Lights");
				foreach (GameObject obj in objLights)
				{
					obj.GetComponent<Light>().color = new Color(40f, 44f, 121f);
				}
				InvokeRepeating("laughPlz", 20f, 20f);
			}
			hourChecked = true;
			triggerDayEvent(Hours.findEvent(currH));
		}
	}

	public void testLights(HoursManager.DayTime _time)
	{

		switch (_time)
		{
			case HoursManager.DayTime.Day :
			{
				InvokeRepeating("laughPlz", 20f, 20f);
				Hours.currentTime = HoursManager.DayTime.Day;
				GameObject[] objLights = GameObject.FindGameObjectsWithTag("Lights");
				foreach (GameObject obj in objLights)
				{
					obj.GetComponent<Light>().color = new Color(217f, 242f, 255f);
				}
				break;
			}
			case HoursManager.DayTime.Night :
			{
				CancelInvoke("laughPlz");
				Hours.currentTime = HoursManager.DayTime.Night;
				GameObject[] objLights = GameObject.FindGameObjectsWithTag("Lights");
				foreach (GameObject obj in objLights)
				{
					obj.GetComponent<Light>().color = new Color(40f, 44f, 121f);
				}
				break;
			}
		}
	}

	public void respawnPaper(List<GameObject> _ppSpot)
	{
		print ("spawn!");
		int randId = Random.Range(0, _ppSpot.Count);
		GameObject obj = Instantiate(Resources.Load("Objects/Letter")) as GameObject;
		obj.GetComponent<Letter>().Picked = false;
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

	public void resplenishFoods()
	{
		Food[] Foodies = GetComponentsInChildren<Food>();
		foreach (Food _foo in Foodies)
		{
			_foo.Regen();
		}
	}

	public void triggerDayEvent(HoursManager.DayEventList _evt)
	{
		switch (_evt)
		{
		case HoursManager.DayEventList.DraculaEntering :
		{
			GameUI.WillBack.alpha = 0f;
			GameUI.WillBackTxt.makeFadeIn();
			MasterAudio.PlaySound("dracula_back");
			GameUI.NotifPop.giveInfos("Dracula is back\n from hunting");
			GameUI.dialogPop.giveInfos("Dracula is entering the game !");
			new OTTween(GameUI.dialogPop.OutPic, 1f).Tween("alpha", 1f);
			break;
		}
		case HoursManager.DayEventList.DraculaLeaving :
		{
			InvokeRepeating("laughPlz", 20f, 20f);
			GameUI.WillBack.alpha = 1f;
			GameUI.WillBackTxt.makeFadeOut();
			MasterAudio.PlaySound("dacrula_out");
			GameUI.NotifPop.giveInfos("Dracula is gone hunting\n outside the house");
			GameUI.dialogPop.giveInfos("Dracula is leaving the house !");
			new OTTween(GameUI.dialogPop.OutPic, 1f).Tween("alpha", 0f);
			break;
		}
		case HoursManager.DayEventList.FoodMan :
		{
			resplenishFoods();
			MasterAudio.PlaySound("eating_bell");
			GameUI.NotifPop.giveInfos("The Carrier has\n resplenished food stocks");
			GameUI.dialogPop.giveInfos("I can now eat in the house's kitchens");
			break;
		}
		case HoursManager.DayEventList.MailManIn :
		{
			MailmanState = MailManStateList.HasArrived;
			MasterAudio.PlaySound("door_bell");
			GameUI.dialogPop.giveInfos("The mail man is waiting\n for me, I should hurry");
			GameUI.NotifPop.giveInfos("The mail man\n has arrived");
			break;
		}
		case HoursManager.DayEventList.MailManOut :
		{
			MailmanState = MailManStateList.Away;
			GameUI.dialogPop.giveInfos("The mail man is gone now.");
			GameUI.NotifPop.giveInfos("The mail man\n is leaving");
			break;
		}
		case HoursManager.DayEventList.LetterSent :
		{
			saveDay = currD + remainingDay;
			MailmanState = MailManStateList.Away;
			plr.haveLetter = Player.letterList.Sent;
			GameUI.dialogPop.giveInfos("I've asked Mina for some rescue \n against Dracula. I think they'll\n arrive within" + (saveDay - currD)  + " days.");
			GameUI.NotifPop.giveInfos("The mail man\n is leaving\n with the letter");
			InvokeRepeating("recallObjective", 30f, 30f);
			break;
		}

		}
	}
	private void laughPlz()
	{
		MasterAudio.PlaySound("laugh");
	}

	private void recallObjective()
	{
		GameUI.dialogPop.giveInfos("I've asked Mina for some rescue \n against Dracula. I think they'll\n arrive within" + (saveDay - currD)  + " days.");
	}

	private void EndGame()
	{
		
	}
	private void GameOver()
	{

	}
	private void Respawn()
	{
		GameUI.dialogPop.giveInfos("This place doesn't look\n really comfy. I should find a letter and\n ask Mina some help.");
		currD = 1;
		currH = 1;
		currM = 1;
		writtenPaper = 0;
		realWrittenPaper = 0;
		respawnPaper(PaperSpots);
	}
	private void GameStart()
	{
		
	}
}
