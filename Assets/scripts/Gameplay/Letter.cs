using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.CompareTag("Player"))
		{
			print ("ok");
			_oth.GetComponent<Player>().haveLetter = Player.letterList.Have;
		}
	}
}
