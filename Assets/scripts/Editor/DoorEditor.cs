using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(Door))]
public class DoorEditor : Editor {
	
	Door _door;
	
	public override void OnInspectorGUI()
	{
		_door = (Door)target;
		base.DrawDefaultInspector();
		
		if (GUILayout.Button("Open", GUILayout.ExpandWidth(true)))
		{
			_door.Open();
		}
		if (GUILayout.Button("Close", GUILayout.ExpandWidth(true)))
		{
			_door.Close();
		}
	}
}
