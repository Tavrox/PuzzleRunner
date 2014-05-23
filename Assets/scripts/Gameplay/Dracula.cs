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
	public float speed = 20f;	
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
	
	public float _diffX = 0f;
	public float _diffY = 0f;
	public float _angle = 0f;

	public float threeSoldChasing = 20f;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		_levman = _lev;
		_plr = _levman.plr;
		giveWPM();
		
		RayDL = FETool.findWithinChildren(gameObject, "RayOrigin_DL").transform;
		RayUL = FETool.findWithinChildren(gameObject, "RayOrigin_DR").transform;
		RayDR = FETool.findWithinChildren(gameObject, "RayOrigin_UL").transform;
		RayUR = FETool.findWithinChildren(gameObject, "RayOrigin_UR").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkForPlayer();
		vecMove = Vector3.zero;
		switch (State)
		{
			case StateList.Patrolling :
			{
				Move(currWp.nextWP.transform.position);
				rotateTowardPlayer(currWp.nextWP.transform.position, transform);
				break;
			}
			case StateList.Chasing :
			{
				Move(_plr.transform.position);
				rotateTowardPlayer(_plr.transform.position, transform);
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
			print (hitInfo.transform.gameObject.name);
			blockDown();
		}
		if (Physics.Raycast(RayUL.position, Vector3.left, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDL.position, Vector3.left, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.black);
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.black);
			print (hitInfo.transform.gameObject.name);
			blockLeft();
		}
		if (Physics.Raycast(RayUL.position, Vector3.up, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayUR.position, Vector3.up, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.white);
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.white);
			print (hitInfo.transform.gameObject.name);
			blockUp();
		}
		if (Physics.Raycast(RayUR.position, Vector3.right, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDR.position, Vector3.right, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.red);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.red);
			print (hitInfo.transform.gameObject.name);
			blockRight();
		}
	}

	private void Move(Vector3 target)
	{
		if (currWp != null)
		{
			pos = gameObject.transform.position;
			direction = Vector3.Normalize(target - pos);
			vecMove = new Vector3 ( (speed * direction.x) * Time.deltaTime, (speed * direction.y) * Time.deltaTime, 0f);
		}
	}

	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.GetComponent<Player>() != null)
		{
			GameEventManager.TriggerGameOver("Dracula");
		}

	}

	private void turnTowardWp(Waypoint _wp)
	{

	}

	private void giveWPM()
	{
		currWPM = _levman.pathDirector.pickRandomWPM();
		currWp = currWPM.pickRandomWP();
		transform.position = currWp.transform.position;
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
			print ("blcoked");
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockLeft()
	{
		if (MovingDir != DirList.Right)
		{
			print ("blcoked");
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockRight()
	{
		if (MovingDir != DirList.Left)
		{
			print ("blcoked");
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockUp()
	{
		if (MovingDir != DirList.Down)
		{
			print ("blcoked");
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}

	private void checkForPlayer()
	{
//		distToPlayer = Vector3.Distance(transform.position, _plr.transform.position);
//		if (distToPlayer < threeSoldChasing)
//		{
//			State = StateList.Chasing;
//		}
//		else
//		{
//			State = StateList.Patrolling;
//		}
	}


}
