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

	private Vector3 pos;
	private Vector3 target;
	private Vector3 direction;
	public float speed = 20f;
	
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
	}
	
	// Update is called once per frame
	void Update () 
	{
		checkForPlayer();
		switch (State)
		{
			case StateList.Patrolling :
			{
				Move(currWp.nextWP.transform.position);
				break;
			}
			case StateList.Chasing :
			{
				Move(_plr.transform.position);
				rotateTowardPlayer(_plr.transform.position, transform);
				break;
			}
		}
	}

	private void Move(Vector3 target)
	{
		if (currWp != null)
		{
			pos = gameObject.transform.position;
			direction = Vector3.Normalize(target - pos);
			gameObject.transform.position += new Vector3 ( (speed * direction.x) * Time.deltaTime, (speed * direction.y) * Time.deltaTime, 0f);
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
		transform.position = currWPM.transform.position;
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

	private void checkForPlayer()
	{
		distToPlayer = Vector3.Distance(transform.position, _plr.transform.position);
		if (distToPlayer < threeSoldChasing)
		{
			State = StateList.Chasing;
		}
		else
		{
			State = StateList.Patrolling;
		}
	}


}
