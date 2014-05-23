using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	public bool Picked = false;

	void OnTriggerStay(Collider _oth)
	{
		if (_oth.CompareTag("Player") && Input.GetKeyDown(KeyCode.E) && Picked == false)
		{
			Picked = true;
			MasterAudio.PlaySound("eat");
			_oth.GetComponent<Player>().giveFood();
			OTSprite spr = GetComponentInChildren<OTSprite>();
			new OTTween(spr, 0.5f).Tween("alpha", 0f);;
		}
	}

	public void Regen()
	{
		Picked = false;
		OTSprite spr = GetComponentInChildren<OTSprite>();
		new OTTween(spr, 0.5f).Tween("alpha", 1f);;
	}
}
