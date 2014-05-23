using UnityEngine;
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
		Cutscene
	};
	public healthState Health;

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
	public float sleepQty;
	public float hungerQty;
	public Vector3 vecMove;

	public Vector3 mypos = Vector3.zero;
	public Vector3 _target = Vector3.zero;
	public float _diffX = 0f;
	public float _diffY = 0f;
	public float _angle = 0f;
	public OTAnimatingSprite spr;
	
	[SerializeField] private RaycastHit hitInfo;
	[SerializeField] private float halfMyX;
	[SerializeField] private float halfMyY;
	protected int wallMask = 1 << 8;

	private bool BlockedUp = false;
	private bool BlockedDown = false;
	private bool BlockedLeft = false;
	private bool BlockedRight = false;
	private GameObject gameSprite;

	private Transform RayDL;
	private Transform RayUL;
	private Transform RayDR;
	private Transform RayUR;

	public int foodState = 3;
	public int sleepState = 3;
	public float doorSpeed;
	public float modifSpeed;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		coneParent = FETool.findWithinChildren(gameObject, "ParentCone");
		gameSprite = FETool.findWithinChildren(gameObject, "Sprite");
		levMan = _lev;

		RayDL = FETool.findWithinChildren(gameObject, "RayOrigin_DL").transform;
		RayUL = FETool.findWithinChildren(gameObject, "RayOrigin_DR").transform;
		RayDR = FETool.findWithinChildren(gameObject, "RayOrigin_UL").transform;
		RayUR = FETool.findWithinChildren(gameObject, "RayOrigin_UR").transform;
		spr = GetComponentInChildren<OTAnimatingSprite>();
		spr.Play("static");

		coneCollider = coneParent.GetComponentInChildren<BoxCollider>();
		coneRenderer = coneParent.GetComponentInChildren<LineRenderer>();
		InvokeRepeating("consumeFood", 30f, 30f);
		InvokeRepeating("consumeSleep", 30f, 30f);
		halfMyY = 0.25f;

		
		
		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameStart += GameStart;
	}
	
	// Update is called once per frame
	void Update () 
	{
		BlockedDown = false;
		BlockedLeft = false;
		BlockedUp = false;
		BlockedRight = false;
		_target = GameObject.Find("LevelManager/Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

		switch (Health)
		{
		case healthState.Dead :
		{
			PlayAnim("static");
			vecMove = Vector3.zero;
			speed = 0f;
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
		if (sleepState == 1)
		{
			MasterAudio.PlaySound("yawn1");
		}
		else if (sleepState == 2)
		{
			MasterAudio.PlaySound("yawn2");
		}
	}

	private void writePaper()
	{
		if (Input.GetKey(KeyCode.E) && (haveLetter == letterList.Have || haveLetter == letterList.IsWriting))
		{
			MasterAudio.PlaySound("paper_in");
			MasterAudio.PlaySound("writting");
			haveLetter = letterList.IsWriting;
			levMan.writtenPaper += 0.1f;
		}
		else if (Input.GetKeyUp(KeyCode.E) && (haveLetter == letterList.Have || haveLetter == letterList.IsWriting))
		{
			MasterAudio.StopAllOfSound("writting");
			MasterAudio.PlaySound("paper_out");
		}
	}

	private void checkForStats()
	{
		if (foodState == 1)
		{
			modifSpeed = 0.6f;
		}
		else if (foodState == 2)
		{
			modifSpeed = 0.8f;
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

		if ( foodState == 0 || sleepState == 0)
		{
			GameEventManager.TriggerGameOver("Food or Sleep");
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
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			PlayAnim("walk");
			vecMove.x -= speed;
			MovingDir = DirList.Left;
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			PlayAnim("static");
			vecMove.x -= 5f;
		}
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
			PlayAnim("walk");
			vecMove.y -= speed;
			MovingDir = DirList.Down;
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			PlayAnim("static");
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			PlayAnim("walk");
			vecMove.y += speed;
			MovingDir = DirList.Up;
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			PlayAnim("static");
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			PlayAnim("walk");
			vecMove.x += speed;
			MovingDir = DirList.Right;
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			PlayAnim("static");
			vecMove.x += 5f;
		}
		vecMove.x *= 0.1f;
		vecMove.y *= 0.1f;
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
		haveLetter = letterList.DontHave;
	}

	private void GameStart()
	{
		Health = healthState.Alive;
		haveLetter = letterList.DontHave;
	}

	private void GameOver()
	{
		Health = healthState.Dead;
	}

	private void changeRenderer()
	{
//		coneRenderer.SetPosition(0, new Vector3(0,0f,0f) );
	}

}