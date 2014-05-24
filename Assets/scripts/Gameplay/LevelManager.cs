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
	public enum DeathList
	{
		Hunger,
		Dracula,
		Exhaust,
		Test
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
	[SerializeField] public int saveDay = 1000;
	public string parseH;
	public string parseM;
	public int remainingDay = 5;
	public WaypointDirector pathDirector;
	public UI GameUI;
	public float writtenPaper;
	public int realWrittenPaper;
	public bool hourChecked = false;
	private bool delaySleep;

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

		//		GAMESTATE = GameEventManager.GameState.MainMenu;
		//		GameEventManager.TriggerRespawn("Rsp");
		GAMESTATE = GameEventManager.GameState.Live;
		GameEventManager.TriggerGameStart("First Init");
	}
	
	// Update is called once per frame
	void Update () 
	{
		currCam.transform.position =  new Vector3( FETool.Round(plr.transform.position.x, 2), FETool.Round(plr.transform.position.y, 2) - 0.8f, -1000f);
		realWrittenPaper = Mathf.RoundToInt(writtenPaper);
		if (currD == saveDay)
		{
			GameEventManager.TriggerEndGame("Last Day");
		}
	}

	public void doSleep(Bed.hourList _hr)
	{
		if (delaySleep == false)
		{
			delaySleep = true;
			plr.foodState -= 1;
			switch (_hr)
			{
			case Bed.hourList.Three :
			{
				currH += 1;
				checkHour();
				hourChecked = false;
				currH += 1;
				checkHour();
				hourChecked = false;
				currH += 1;
				checkHour();
				hourChecked = false;
				break;
			}
			case Bed.hourList.Six :
			{
				currH += 1;
				checkHour();
				hourChecked = false;
				currH += 1;
				checkHour();
				hourChecked = false;
				currH += 1;
				checkHour();
				hourChecked = false;

				currH += 1;
				checkHour();
				hourChecked = false;
				currH += 1;
				checkHour();
				hourChecked = false;
				currH += 1;
				checkHour();
				hourChecked = false;
				break;
			}
			}
			StartCoroutine("delayedSleep");
		}

	}
	IEnumerator delayedSleep()
	{
		yield return new WaitForSeconds(3f);
		delaySleep = false;
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
		parseH = currH.ToString();
		parseM = currM.ToString();
		if (currM < 10)
		{
			parseM = "0" + currM;
		}
		checkHour();
		GameUI.Clock.text = parseH + ":" + parseM;
	}
	public void checkForDifficulty()
	{

	}
	public void checkHour()
	{
		if (hourChecked == false)
		{
			if (currH < 10)
			{
				parseH = "0" + currH;
			}
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
			GameUI.WillBackTxt.makeFadeOut();
			MasterAudio.PlaySound("dracula_back");
			GameUI.NotifPop.giveInfos("Dracula is back\n from hunting");
			GameUI.dialogPop.giveInfos("Dracula is entering the game !");
			new OTTween(GameUI.dialogPop.OutPic, 1f).Tween("alpha", 0f);
			break;
		}
		case HoursManager.DayEventList.DraculaLeaving :
		{
			InvokeRepeating("laughPlz", 20f, 20f);
			GameUI.WillBackTxt.makeFadeIn();
			MasterAudio.PlaySound("dacrula_out");
			GameUI.NotifPop.giveInfos("Dracula is gone hunting\n outside the house");
			GameUI.dialogPop.giveInfos("Dracula is leaving the house !");
			new OTTween(GameUI.dialogPop.OutPic, 1f).Tween("alpha", 1f);
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
			GameUI.dialogPop.giveInfos("The mail man is waiting\n for me, I should hurry", PopUp.CharList.MailMan);
			GameUI.NotifPop.giveInfos("The mail man\n has arrived");
			break;
		}
		case HoursManager.DayEventList.MailManOut :
		{
			MailmanState = MailManStateList.Away;
			GameUI.dialogPop.giveInfos("The mail man is gone now.", PopUp.CharList.Johnathan);
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
		GAMESTATE = GameEventManager.GameState.EndGame;
		CancelInvoke("updateMinute");
		
	}
	private void GameOver()
	{
		GAMESTATE = GameEventManager.GameState.GameOver;
		CancelInvoke("updateMinute");

	}
	private void Respawn()
	{
		GAMESTATE = GameEventManager.GameState.Live;
		InvokeRepeating("updateMinute", 0f, 0.10f);
		GameUI.dialogPop.giveInfos("This place doesn't look\n really comfy. I should find a letter and\n ask Mina some help.");
		currD = 1;
		currH = 0;
		currM = 1;
		writtenPaper = 0;
		realWrittenPaper = 0;
		respawnPaper(PaperSpots);
	}
	private void GameStart()
	{
		GAMESTATE = GameEventManager.GameState.MainMenu;
		
	}
}
