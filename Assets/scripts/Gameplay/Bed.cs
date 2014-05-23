using UnityEngine;
using System.Collections;

public class Bed : MonoBehaviour 
{
	public enum hourList
	{
		Three,
		Six
	};
	public hourList Hour;

	void OnMouseDown()
	{
		GameObject.Find("LevelManager").GetComponent<LevelManager>().doSleep(Hour);
	}
}
