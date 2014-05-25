using UnityEngine;
using System.Collections;

public static class GameEventManager {

	public delegate void GameEvent();
	
	public static event GameEvent GameStart, GameOver, Respawn, EndGame;
	public enum GameState
	{
		Live,
		GameOver,
		MainMenu,
		EndGame,
		Trailer
	};
	public static bool gameOver = false;
	
	public static void TriggerGameStart(string _trigger)
	{
		if(GameStart != null && LevelManager.GAMESTATE != GameState.MainMenu)
		{
			Debug.Log ("Start +" + _trigger);
			LevelManager.GAMESTATE = GameState.MainMenu;
			GameStart();
		}
	}

	public static void TriggerGameOver(LevelManager.DeathList _gameover)
	{
		if(GameOver != null && LevelManager.GAMESTATE != GameState.GameOver)
		{
			Debug.Log ("GameOver +" + _gameover.ToString());
			LevelManager.GAMESTATE = GameState.GameOver;
			LevelManager.CAUSEDEATH = _gameover;
			GameOver();
		}
	}
	
	public static void TriggerRespawn(string _trigger)
	{
		if(Respawn != null && LevelManager.GAMESTATE != GameState.Live)
		{
			Debug.Log ("Respawn +" + _trigger);
			LevelManager.GAMESTATE = GameState.Live;
			Respawn();
		}
	}

	public static void TriggerEndGame(string _trigger)
	{
		if(EndGame != null && LevelManager.GAMESTATE != GameState.EndGame)
		{
			Debug.Log ("Victory +" + _trigger);
			LevelManager.GAMESTATE = GameState.EndGame;
			EndGame();
		}
	}
}
