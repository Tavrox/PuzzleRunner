using UnityEngine;
using System.Collections;

public class Dracula : MonoBehaviour {


	public enum StateList
	{
		Patrolling,
		Chasing,
		CutScene
	};
	public StateList State;
	public float distToPlayer;

	private Player _plr;
	public WaypointManager currWPM;
	public Waypoint currWp;
	private LevelManager _levman;
	
	public enum DirList
	{
		Up,
		Left,
		Down,
		Right
	};
	public DirList MovingDir;
	public DirList FacingDir;

	private Vector3 pos;
	private Vector3 target;
	private Vector3 direction;
	public float initSpeed = 20f;
	public float speed = 20f;	
	public float modifSpeed = 1f;
	public Vector3 vecMove;
	
	public Vector3 mypos = Vector3.zero;
	private Transform RayDL;
	private Transform RayUL;
	private Transform RayDR;
	private Transform RayUR;
	[SerializeField] private RaycastHit hitInfo;
	[SerializeField] private float halfMyX;
	[SerializeField] private float halfMyY;
	private bool BlockedUp = false;
	private bool BlockedDown = false;
	private bool BlockedLeft = false;
	private bool BlockedRight = false;
	protected int wallMask = 1 << 8;
	public OTAnimatingSprite spr;
	public OTAnimatingSprite pouf;

	
	public float _diffX = 0f;
	public float _diffY = 0f;
	public float _angle = 0f;

	public float threeSoldChasingMin = 10f;
	public float threeSoldChasingMax = 5f;
	public LevelBrick currFloor;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		_levman = _lev;
		_plr = _levman.plr;
		spr = FETool.findWithinChildren(gameObject, "Sprite").GetComponentInChildren<OTAnimatingSprite>();
		pouf = FETool.findWithinChildren(gameObject, "Pouf").GetComponentInChildren<OTAnimatingSprite>();
		randomSpawn();
		
		RayDL = FETool.findWithinChildren(gameObject, "RayOrigin_DL").transform;
		RayUL = FETool.findWithinChildren(gameObject, "RayOrigin_DR").transform;
		RayDR = FETool.findWithinChildren(gameObject, "RayOrigin_UL").transform;
		RayUR = FETool.findWithinChildren(gameObject, "RayOrigin_UR").transform;
		
		GameEventManager.GameOver += GameOver;
		GameEventManager.Respawn += Respawn;
		GameEventManager.GameStart += GameStart;
		GameEventManager.EndGame += EndGame;
	}
	
	// Update is called once per frame
	void Update () 
	{
		vecMove = Vector3.zero;
		checkFloor();

		switch (_levman.Hours.currentTime)
		{
			case HoursManager.DayTime.Day :
			{
				transform.position = new Vector3(500f,500f, 500f);
				break;
			}
			case HoursManager.DayTime.Night :
			{
			switch (State)
			{
			case StateList.Patrolling :
			{
			PlayAnim("patrolling");
			break;
			}
			case StateList.Chasing :
			{
			Move(_plr.transform.position);
			rotateTowardPlayer(_plr.transform.position, transform);
			break;
			}
			case StateList.CutScene :
			{
				vecMove = Vector3.zero ;
				speed = 0f;
				break;
			}
			}
			break;
			}
		}
		wallBlocker();
		gameObject.transform.position += vecMove ;
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

	private void Move(Vector3 target)
	{
		if (currWp != null)
		{
			pos = gameObject.transform.position;
			direction = Vector3.Normalize(target - pos);
			speed = initSpeed * modifSpeed;
			vecMove = new Vector3 ( (speed * direction.x) * Time.deltaTime, (speed * direction.y) * Time.deltaTime, 0f);
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


	private void randomSpawn()
	{
		if (State != StateList.Chasing && _levman.Hours.currentTime != HoursManager.DayTime.Day)
		{
			currWPM = _levman.pathDirector.pickRandomWPM();
			Waypoint _WP = currWPM.pickRandomWP();
			currWp = currWPM.pickRandomWP();
			pouf.transform.parent.transform.position = _WP.transform.position;
			pouf.alpha = 1f;
			pouf.PlayOnce("pouf");
			StartCoroutine(doAppear(_WP));
			new OTTween(spr, 0.01f).Tween("alpha", 0f);
			new OTTween(transform, 0.01f).Tween("position", currWp.transform.position);
			new OTTween(spr, 0.03f).Tween("alpha", 1f);
//			transform.position = currWp.transform.position;
		}
	}
	IEnumerator doAppear(Waypoint __wp)
	{
		yield return new WaitForSeconds(1f);
		transform.position = __wp.transform.position;
		currFloor.objOnFloor.Remove(this.gameObject);
	}

	public void checkFloor()
	{
		if (currFloor != null)
		{
			if (currFloor.objOnFloor.Contains(_plr.gameObject))
			{
				State = StateList.Chasing;
				target = _levman.plr.transform.position;
			}
			else
			{
				State = StateList.Patrolling;
				target = Vector3.zero;
			}
		}
	}

	IEnumerator WaitSec(float _sec)
	{
		yield return new WaitForSeconds(_sec);
		randomSpawn();
	}

	public void GoToWaypoint (Waypoint _wp)
	{
		currWp = _wp.nextWP;
		target = _wp.transform.position;
		direction = Vector3.Normalize(target - pos);
	}

	private void rotateTowardPlayer(Vector3 targ ,Transform _trsf)
	{
		_diffX = targ.x - transform.position.x;
		_diffY = transform.position.y - targ.y;
		_angle = Mathf.Atan2( _diffX, _diffY) * Mathf.Rad2Deg;
		_trsf.rotation = Quaternion.Euler(0f, 0f, _angle - 180);
	}

	public void PlayAnim(string _anim)
	{
		if (spr.animationFrameset != _anim)
		{
			spr.Play (_anim);
		}
	}

	public void findClosestWp()
	{
		foreach (Waypoint wp in _levman.wpList)
		{
			float distWpDracula = Vector3.Distance(transform.position, wp.transform.position);
			wp.distWP = distWpDracula;
		}
		_levman.wpList.Sort(delegate (Waypoint x, Waypoint y)
      	{
			if (x.distWP < y.distWP) return -1;
			if (x.distWP > y.distWP) return 1;
			else return 0;
		});
		currWp = _levman.wpList[0];
		currWPM = currWp.linkedManager;
	}

	
	private void blockDown()
	{
		if (MovingDir != DirList.Up)
		{
			print ("blocked");
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockLeft()
	{
		if (MovingDir != DirList.Right)
		{
			print ("blocked");
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockRight()
	{
		if (MovingDir != DirList.Left)
		{
			print ("blocked");
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockUp()
	{
		if (MovingDir != DirList.Down)
		{
			print ("blocked");
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}


	private void Respawn()
	{
		InvokeRepeating("randomSpawn", 15f, 30f);
	}
	
	private void GameStart()
	{
//		giveWPM();
	}
	
	private void GameOver()
	{
		State = StateList.CutScene;
		CancelInvoke("randomSpawn");
	}
	private void EndGame()
	{
		
	}

	
	


}
