using UnityEngine;
using System.Collections;

public class Letter : MonoBehaviour 
{
	void OnTriggerEnter(Collider _oth)
	{
		if (_oth.CompareTag("Player"))
		{
			_oth.GetComponent<Player>().giveLetter();
			MasterAudio.PlaySound("paper_drop");
			GameObject.Find("LevelManager").GetComponent<LevelManager>().GameUI.NotifPop.giveInfos("I can now write\n a letter");
			GameObject.Find("LevelManager").GetComponent<LevelManager>().GameUI.dialogPop.giveInfos("I need to write a letter \n to Mina, or Dracula will kill me.");
			OTSprite spr = GetComponentInChildren<OTSprite>();
			new OTTween(spr, 0.5f).Tween("alpha", 0f);;
		}
	}
}
