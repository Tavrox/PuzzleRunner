using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelTestor : Editor
{
	private LevelManager test;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		test = (LevelManager)target;
		if (GUILayout.Button("Enter Dracula"))
		{
			test.triggerDayEvent(HoursManager.DayEventList.DraculaEntering);
		}
		
		if (GUILayout.Button("Leave Dracula"))
		{
			test.triggerDayEvent(HoursManager.DayEventList.DraculaLeaving);
		}
		
		if (GUILayout.Button("Food Man"))
		{
			test.triggerDayEvent(HoursManager.DayEventList.FoodMan);
		}
		
		if (GUILayout.Button("Send Letter"))
		{
			test.triggerDayEvent(HoursManager.DayEventList.LetterSent);
		}
		
		if (GUILayout.Button("Mail In"))
		{
			test.triggerDayEvent(HoursManager.DayEventList.MailManIn);
		}
		
		if (GUILayout.Button("Mail Out"))
		{
			test.triggerDayEvent(HoursManager.DayEventList.MailManOut);
		}

		if (GUILayout.Button("GoNight"))
		{
			test.testLights(HoursManager.DayTime.Night);
		}
		if (GUILayout.Button("GoDay"))
		{
			test.testLights(HoursManager.DayTime.Day);
		}
		if (GUILayout.Button("Respawn"))
		{
			GameEventManager.TriggerRespawn("test");
		}
		if (GUILayout.Button("GameOver"))
		{
			GameEventManager.TriggerGameOver("test");
		}
	}
	
}