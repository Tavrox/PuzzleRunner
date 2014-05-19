﻿using UnityEngine;
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
		if(GameStart != null && LevelManager.GAMESTATE != GameState.Live)
		{
			LevelManager.GAMESTATE = GameState.Live;
			GameOver();
		}
	}

	public static void TriggerGameOver(string _gameover)
	{
		if(GameOver != null && LevelManager.GAMESTATE != GameState.GameOver)
		{
			LevelManager.GAMESTATE = GameState.GameOver;
			GameOver();
		}
	}
	
	public static void TriggerRespawn(string _trigger)
	{
		if(Respawn != null)
		{
			LevelManager.GAMESTATE = GameState.Live;
			Respawn();
		}
	}

	public static void TriggerEndGame()
	{
		if(EndGame != null)
		{
			LevelManager.GAMESTATE = GameState.EndGame;
			EndGame();
		}
	}
}
