using UnityEngine;
using System.Collections;

public class DetectionCollider : MonoBehaviour {

	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.CompareTag("Player") == true)
		{
			print (_oth);
			transform.parent.GetComponent<Dracula>().State = Dracula.StateList.Chasing;
		}
	}

	void OnTriggerExit(Collider _oth)
	{
		if (_oth.CompareTag("Player") == true)
		{
			transform.parent.GetComponent<Dracula>().State = Dracula.StateList.Patrolling;
			transform.parent.GetComponent<Dracula>().findClosestWp();
		}
	}
}
