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
