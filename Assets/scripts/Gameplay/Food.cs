using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.CompareTag("Player"))
		{
			_oth.GetComponent<Player>().giveFood();
			OTSprite spr = GetComponentInChildren<OTSprite>();
			new OTTween(spr, 0.5f).Tween("alpha", 0f);;
		}
	}
}
