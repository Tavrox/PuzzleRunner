using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public static GameEventManager.GameState GAMESTATE;

	public Player PLAYER;

	// Use this for initialization
	void Start () 
	{
		PLAYER = GameObject.Find("Player").GetComponent<Player>();
		PLAYER.Setup();

		GameEventManager.GameOver += GameOver;
	}

	private void GameOver()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
