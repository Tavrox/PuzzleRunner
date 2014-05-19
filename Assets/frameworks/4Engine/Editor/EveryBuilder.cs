using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
 
public class EveryBuilder  : MonoBehaviour {
 
    public static void PerformBuild()
    {
//       // the scenes we want to include in the build
//       string[] scenes = { "Scenes/TitleMenu.unity", 
//          "Scenes/Etna.unity",
//          "Scenes/Grensdalur.unity",
//          "Scenes/Vesuvio.unity"
//         };
// 
//       DateTime currentDate = DateTime.Now;
//       string buildName = "Game-"+currentDate.ToShortDateString();
 
       // build for windows stand alone
//       string windowsStandAloneBuildName = buildName+"-StandAlone.exe";
//       EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.StandaloneWindows);
//        BuildPipeline.BuildPlayer(scenes, "D:\\", BuildTarget.StandaloneWindows, BuildOptions.None);
 
       // build for web player
       // string webplayerBuildName = buildName+"-WebPlayer.exe";
       //EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.WebPlayer);
       // BuildPipeline.BuildPlayer(scenes, webplayerBuildName, BuildTarget.WebPlayer, BuildOptions.None);
    }
}