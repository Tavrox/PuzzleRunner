using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static GameEventManager.GameState GAMESTATE;

	// Use this for initialization
	void Start () 
	{

		GameEventManager.GameOver += GameOver;
	}

	private void GameOver()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
