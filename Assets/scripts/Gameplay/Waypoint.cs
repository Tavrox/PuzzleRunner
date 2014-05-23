using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {

	public enum TypeList
	{
		Normal,
		Initial,
		GoToAndStop
	};
	public TypeList WPType;
	public float distWP;
	[HideInInspector] public int id;
	[HideInInspector] public bool activated = true;
	public Waypoint nextWP;
	[HideInInspector] public WaypointManager linkedManager;
	public bool passedUpon = false;
	public float EDITOR_Resizer;

	public void Setup()
	{
		transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
		linkedManager = transform.parent.GetComponent<WaypointManager>();
		id = int.Parse(name);
	}
	public void lateSetup()
	{
		nextWP = linkedManager.findNextWaypoint(this);
	}

	void OnTriggerEnter(Collider _other)
	{
		if (_other.GetComponent<Dracula>() != null)
		{
			_other.GetComponent<Dracula>().GoToWaypoint(linkedManager.findNextWaypoint(this));
			passedUpon = true;
			delayRetrigger();
		}
	}

	IEnumerator delayRetrigger()
	{
		yield return new WaitForSeconds(0.5f);
		passedUpon = false;
	}

	public void setupCollider(Vector3 resize)
	{
		BoxCollider _coll = GetComponent<BoxCollider>();
		_coll.size = resize;
	}
}
