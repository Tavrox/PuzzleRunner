using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(OTSprite))]
public class SpriteResizer : Editor
{
	private OTSprite sprite;
	
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		if (GUILayout.Button("Resize"))
		{
			sprite = (OTSprite)target;
			sprite.ResizeOT();
		}
	}
}