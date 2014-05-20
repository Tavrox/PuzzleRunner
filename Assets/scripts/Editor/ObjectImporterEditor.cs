using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(ObjectImporter))]
public class ObjectImporterEditor : Editor {

	ObjectImporter import;

	public override void OnInspectorGUI()
	{
		import = (ObjectImporter)target;
		base.DrawDefaultInspector();

		if (GUILayout.Button("Add a brick", GUILayout.ExpandWidth(true)))
		{
			import.setupObjects();
			import.getObjects();
		}
	}
}
