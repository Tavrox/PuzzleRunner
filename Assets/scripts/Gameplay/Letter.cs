﻿using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour 
{
	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.CompareTag("Player"))
		{
			_oth.GetComponent<Player>().giveLetter();
			OTSprite spr = GetComponentInChildren<OTSprite>();
			new OTTween(spr, 0.5f).Tween("alpha", 0f);;
		}
	}
}
