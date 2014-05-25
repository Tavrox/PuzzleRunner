using UnityEngine;
using System.Collections;
using UnityEditor;

public class FEAssetCreator {

	[MenuItem("Assets/Create/Tuning")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<DTTuning>();
	}
}
