﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject coneParent;
	public BoxCollider coneCollider;
	public LineRenderer coneRenderer;
	public LevelManager levMan;


	public enum letterList
	{
		DontHave,
		Have,
		IsWriting,
		HaveWritten,
		Sent
	};
	public letterList haveLetter;
	public enum healthState
	{
		Alive,
		Dead,
		Paper,
		Cutscene
	};
	public healthState Health;

	public enum ActiviList
	{
		Static,
		Walking
	};
	public ActiviList Activity;

	public enum DirList
	{
		Up,
		Left,
		Down,
		Right
	};
	public DirList MovingDir;
	public DirList FacingDir;
	public Vector3 initpos;
	
	public float speed = 5f;
	public float initSpeed = 5f;
	public Vector3 vecMove;

	public Vector3 mypos = Vector3.zero;
	public Vector3 _target = Vector3.zero;
	public float _diffX = 0f;
	public float _diffY = 0f;
	public float _angle = 0f;
	public OTAnimatingSprite spr;
	private Vector3 direction;
	
	[SerializeField] private RaycastHit hitInfo;
	[SerializeField] private float halfMyX;
	[SerializeField] private float halfMyY;
	protected int wallMask = 1 << 8;
	protected int furnitureMask = 1 << 9;

	private bool BlockedUp = false;
	private bool BlockedDown = false;
	private bool BlockedLeft = false;
	private bool BlockedRight = false;
	private GameObject gameSprite;

	private Transform RayDL;
	private Transform RayUL;
	private Transform RayDR;
	private Transform RayUR;
	private bool stepTriggered = false;

	public int foodState = 3;
	public int sleepState = 3;
	public float doorSpeed;
	public float modifSpeed;
	public LevelBrick currFloor;
	public bool onDoor = false;

	public OTSprite paper;
	public OTSprite bubble;
	public TextUI papertxt;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		coneParent = FETool.findWithinChildren(gameObject, "ParentCone");
		gameSprite = FETool.findWithinChildren(gameObject, "Sprite");
		bubble = FETool.findWithinChildren(gameObject, "Bubble").GetComponentInChildren<OTSprite>();
		levMan = _lev;
		initpos = transform.position;

		RayDL = FETool.findWithinChildren(gameObject, "RayOrigin_DL").transform;
		RayUL = FETool.findWithinChildren(gameObject, "RayOrigin_DR").transform;
		RayDR = FETool.findWithinChildren(gameObject, "RayOrigin_UL").transform;
		RayUR = FETool.findWithinChildren(gameObject, "RayOrigin_UR").transform;

		paper = FETool.findWithinChildren(gameObject, "Paper").GetComponentInChildren<OTSprite>();
		papertxt = FETool.findWithinChildren(gameObject, "Paper").GetComponentInChildren<TextUI>();
		papertxt.color = Color.clear;

		spr = GetComponentInChildren<OTAnimatingSprite>();
		spr.Play("static");

		coneCollider = coneParent.GetComponentInChildren<BoxCollider>();
		coneRenderer = coneParent.GetComponentInChildren<LineRenderer>();
		halfMyY = 0.25f;

		bubble.alpha = 0f;

		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameStart += GameStart;
		GameEventManager.EndGame += EndGame;
	}
	
	// Update is called once per frame
	void Update () 
	{
		BlockedDown = false;
		BlockedLeft = false;
		BlockedUp = false;
		BlockedRight = false;
		_target = GameObject.Find("LevelManager/Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		direction = _target.normalized;

		switch (Health)
		{
		case healthState.Dead :
		{
			PlayAnim("static");
			vecMove = Vector3.zero;
			speed = 0f;
			break;
		}
		case healthState.Paper :
		{
			PlayAnim("static");
			vecMove = Vector3.zero;
			speed = 0f;
			writePaper();
			rotateTowardMouse(_target , transform);
			break;
		}
		case healthState.Alive:
		{
			speed = initSpeed * modifSpeed;
			moveInput();
			rotateTowardMouse(_target , transform);
			changeRenderer();
			wallBlocker();
			writePaper();
			checkForStats();
			break;
		}
		}
		transform.position += vecMove * Time.deltaTime;
	}

	private void consumeFood()
	{
		foodState -= 1;
		displayBubbleInfo("think_eat");
		if (foodState == 1)
		{
			MasterAudio.PlaySound("hungry1");
		}
		else if (foodState == 2)
		{
			MasterAudio.PlaySound("hungry2");
		}
	}
	private void consumeSleep()
	{
		sleepState -= 1;
		displayBubbleInfo("think_sleep");
		if (sleepState == 1)
		{
			MasterAudio.PlaySound("yawn1");
		}
		else if (sleepState == 2)
		{
			MasterAudio.PlaySound("yawn2");
		}
	}

	public void displayBubbleInfo(string _str)
	{
		new OTTween(bubble, 1f).Tween("alpha", 1f).PingPong();
		bubble.frameName = _str;
	}

	private void writePaper()
	{
		if (haveLetter != letterList.HaveWritten)
		{
			if (Input.GetKey(KeyCode.E) && (haveLetter == letterList.Have || haveLetter == letterList.IsWriting) && onDoor == false)
			{
				Health = healthState.Paper;
				MasterAudio.PlaySound("paper_in");
				MasterAudio.PlaySound("writting");
				haveLetter = letterList.IsWriting;
				levMan.writtenPaper += LevelManager.TUNING.writingSpeed;
				paper.alpha = 1f;
				papertxt.color = papertxt.initColor;
				papertxt.text = levMan.realWrittenPaper + "%";
				paper.transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			}
			else if (Input.GetKeyUp(KeyCode.E) && (haveLetter == letterList.Have || haveLetter == letterList.IsWriting) && onDoor == false)
			{
				Health = healthState.Alive;
				MasterAudio.StopAllOfSound("writting");
				MasterAudio.PlaySound("paper_out");
				paper.alpha = 0f;
				papertxt.color = Color.clear;
				paper.transform.parent.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			}
		}
		if (levMan.writtenPaper > 100 && haveLetter != letterList.Sent)
		{
			MasterAudio.StopAllOfSound("writting");
			Health = healthState.Alive;
			haveLetter = letterList.HaveWritten;
			paper.alpha = 0f;
			papertxt.color = Color.clear;
		}
	}

	private void checkForStats()
	{
		if (foodState == 1)
		{
			modifSpeed = LevelManager.TUNING.speedModifierPlayerHunger1;
		}
		else if (foodState == 2)
		{
			modifSpeed = LevelManager.TUNING.speedModifierPlayerHunger2;
		}
		else if (foodState == 3)
		{
			modifSpeed = 1f;
		}

		if (sleepState == 1)
		{
			doorSpeed = 2f;
		}
		else if (sleepState == 2)
		{
			doorSpeed = 1f;
		}
		else if (sleepState == 3)
		{
			doorSpeed = 0.5f;
		}

		if ( foodState == 0)
		{
			GameEventManager.TriggerGameOver(LevelManager.DeathList.Hunger);
		}
		if ( sleepState == 0)
		{
			GameEventManager.TriggerGameOver(LevelManager.DeathList.Exhaust);
		}
	}

	public void PlayAnim(string _anim)
	{
		if (spr.animationFrameset != _anim)
		{
			spr.Play (_anim);
		}
	}


	private void moveInput()
	{
		if (Input.GetKey(KeyCode.Q))
		{
			vecMove.x -= speed;
			MovingDir = DirList.Left;
		}
		else if (Input.GetKeyUp(KeyCode.Q))
		{
			vecMove.x -= 5f;
		}
		
		if (Input.GetKey(KeyCode.S))
		{
			vecMove.y -= speed;
			MovingDir = DirList.Down;
		}
		else if (Input.GetKeyUp(KeyCode.S))
		{
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.Z))
		{
			vecMove.y += speed;
			MovingDir = DirList.Up;
		}
		else if (Input.GetKeyUp(KeyCode.Z))
		{
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.D))
		{
			vecMove.x += speed;
			MovingDir = DirList.Right;
		}
		else if (Input.GetKeyUp(KeyCode.D))
		{
			vecMove.x += 5f;
		}

		if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
		{
			FootSteps();
			PlayAnim("walk");
			Activity = ActiviList.Walking;
		}
		if (!Input.GetKey(KeyCode.Z) && !Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
		{
			Activity = ActiviList.Static;
			PlayAnim("static");
			CancelInvoke("Foots");
			stepTriggered = false;
		}

		vecMove.x *= 0.1f;
		vecMove.y *= 0.1f;
	}

	private void FootSteps()
	{
		if (stepTriggered != true)
		{
			stepTriggered = true;
			InvokeRepeating("Foots", 0f, 0.4f);
		}
	}
	private void Foots()
	{
		MasterAudio.PlaySound("creak");
		MasterAudio.PlaySound("step");
	}


	public void giveFood()
	{
		if (foodState < 3)
		{
			foodState += 1;
		}
	}

	public void giveSleep()
	{
		if (sleepState < 3)
		{
			sleepState += 1;
		}
	}

	public void giveLetter()
	{
		haveLetter = letterList.Have;
	}

	private void wallBlocker()
	{
		mypos = transform.position;
		if (Physics.Raycast(RayDL.position, Vector3.down, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDR.position, Vector3.down, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.blue);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.blue);
			blockDown();
		}
		if (Physics.Raycast(RayUL.position, Vector3.left, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDL.position, Vector3.left, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.black);
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.black);
			blockLeft();
		}
		if (Physics.Raycast(RayUL.position, Vector3.up, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayUR.position, Vector3.up, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.white);
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.white);
			blockUp();
		}
		if (Physics.Raycast(RayUR.position, Vector3.right, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDR.position, Vector3.right, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.red);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.red);
			blockRight();
		}
		//
		if (Physics.Raycast(RayDL.position, Vector3.down, out hitInfo, halfMyY, furnitureMask) ||
		    Physics.Raycast(RayDR.position, Vector3.down, out hitInfo, halfMyY, furnitureMask))
		{
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.blue);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.blue);
			blockDown();
		}
		if (Physics.Raycast(RayUL.position, Vector3.left, out hitInfo, halfMyY, furnitureMask) ||
		    Physics.Raycast(RayDL.position, Vector3.left, out hitInfo, halfMyY, furnitureMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.black);
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.black);
			blockLeft();
		}
		if (Physics.Raycast(RayUL.position, Vector3.up, out hitInfo, halfMyY, furnitureMask) ||
		    Physics.Raycast(RayUR.position, Vector3.up, out hitInfo, halfMyY, furnitureMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.white);
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.white);
			blockUp();
		}
		if (Physics.Raycast(RayUR.position, Vector3.right, out hitInfo, halfMyY, furnitureMask) ||
		    Physics.Raycast(RayDR.position, Vector3.right, out hitInfo, halfMyY, furnitureMask))
		{
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.red);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.red);
			blockRight();
		}
	}

	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.GetComponent<LevelBrick>() != null && _oth.GetComponent<LevelBrick>().brickDef == LevelBrick.brickType.Floor)
		{
			currFloor = _oth.GetComponent<LevelBrick>();
			currFloor.objOnFloor.Add(this.gameObject);
		}
	}
	void OnTriggerExit(Collider _oth)
	{
		if (_oth.GetComponent<LevelBrick>() != null && _oth.GetComponent<LevelBrick>().brickDef == LevelBrick.brickType.Floor)
		{
			_oth.GetComponent<LevelBrick>().objOnFloor.Remove(this.gameObject);
		}
	}


	private void blockDown()
	{
		if (MovingDir != DirList.Up)
		{
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockLeft()
	{
		if (MovingDir != DirList.Right)
		{
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockRight()
	{
		if (MovingDir != DirList.Left)
		{
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockUp()
	{
		if (MovingDir != DirList.Down)
		{
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}

	private void rotateTowardMouse(Vector3 targ ,Transform _trsf)
	{
		_diffX = targ.x - coneParent.transform.position.x;
		_diffY = coneParent.transform.position.y - targ.y;
		_angle = Mathf.Atan2( _diffX, _diffY) * Mathf.Rad2Deg;
		_trsf.rotation = Quaternion.Euler(0f, 0f, _angle - 90);
	}

	private void Respawn()
	{
		Health = healthState.Alive;
		foodState = 3;
		sleepState = 3;
		haveLetter = letterList.DontHave;
		transform.position = initpos;
		InvokeRepeating("consumeFood", LevelManager.TUNING.delaySecConsumeFood, LevelManager.TUNING.everySecConsumeFood);
		InvokeRepeating("consumeSleep", LevelManager.TUNING.delaySecConsumeSleep, LevelManager.TUNING.everySecConsumeSleep);
	}

	private void GameStart()
	{
		Health = healthState.Cutscene;
		haveLetter = letterList.DontHave;
	}

	private void EndGame()
	{
		Health = healthState.Cutscene;
		haveLetter = letterList.DontHave;
	}

	private void GameOver()
	{
		print (LevelManager.CAUSEDEATH);
		Health = healthState.Dead;
		if (LevelManager.CAUSEDEATH == LevelManager.DeathList.Dracula)
		{
			PlayAnim("die");
		}
		CancelInvoke("Foots");
		CancelInvoke("consumeFood");
		CancelInvoke("consumeSleep");
	}

	private void changeRenderer()
	{
//		coneRenderer.SetPosition(0, new Vector3(0,0f,0f) );
	}

	void OnDrawGizmosSelected()
	{
	}

}