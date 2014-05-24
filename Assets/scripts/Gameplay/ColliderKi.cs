using UnityEngine;
using System.Collections;

public class ColliderKi : MonoBehaviour {

	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.GetComponent<Player>() != null)
		{
			GameEventManager.TriggerGameOver(LevelManager.DeathList.Dracula);
		}
		
	}
}
