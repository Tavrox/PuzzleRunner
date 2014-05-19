using UnityEngine;
using System.Collections;

public class FESound : MonoBehaviour {

	public MasterAudioGroup SoundGroup;
	[Range (0,10f)] public float Delay = 0f;
	[Range (0,1f)] public float Volume = 1f;
	[Range (0,1f)] public float Pitch = 1f;
	public float RepeatRate = 0.6f;
	private float distanceToPlayer = 0f;
	public float distanceForFadeOut = 5f;
	private Transform referralDistance;
	private Transform distToTrack;
	private bool playOnce;

	void Start()
	{
		if (SoundGroup == null)
		{
			SoundGroup = GameObject.Find("Frameworks/MasterAudio/" + gameObject.name.Split('/')[1] ).GetComponent<MasterAudioGroup>();
		}
		if (SoundGroup == null)
		{
			Debug.LogWarning("the sound group " + gameObject.transform.parent.transform.parent.gameObject.name + "/" +  gameObject.transform.parent.gameObject.name + "/" + gameObject.name + " hasn't been attributed");
		}
	}

	public void playSound()
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
	}
	public void playSound(int id)
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, id.ToString());
		}
	}
	public void playSound(bool _isOneShot)
	{
		if (SoundGroup != null && playOnce != true)
		{
			playOnce = true;
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
	}
	public void playVariationSound(string _variation)
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _variation);
		}
	}
	public void playModulatedSound(float _var1, float _var2)
	{
		if (SoundGroup != null)
		{
			float percent = (_var1 / _var2);	
			Volume = percent;
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
	}
	public void playLeftSound()
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name + "_L", Volume, Pitch, Delay);
		}
	}
	public void playRightSound()
	{
		if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name + "_R" , Volume, Pitch, Delay);
		}
	}
	public void playDistancedSound(string _var = null)
	{
		referralDistance = this.gameObject.transform;
		distToTrack = GameObject.FindGameObjectWithTag("Player").transform;
		if (_var == null && SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay);
		}
		else if (SoundGroup != null)
		{
			MasterAudio.PlaySound(SoundGroup.name, Volume, Pitch, Delay, SoundGroup.name + "_" + _var);
		}
		InvokeRepeating("checkDistance", 0f, 0.1f); 
	}

	public void stopSound()
	{
		if (SoundGroup != null)
		{
			MasterAudio.StopAllOfSound(SoundGroup.name);
		}
	}

	private void checkDistance()
	{
		Vector2 thisObjPos = new Vector2 (gameObject.transform.position.x, gameObject.transform.position.y);
		Vector2 referralPos = new Vector2 (referralDistance.transform.position.x, referralDistance.transform.position.y);
		Vector2 posToTrack = new Vector2 (distToTrack.position.x, distToTrack.position.y);
		distanceToPlayer = Vector2.Distance(thisObjPos, posToTrack );
		if (distanceToPlayer < 15f)
		{
			if(SoundGroup != null)
			MasterAudio.FadeSoundGroupToVolume(SoundGroup.name, 1f, distanceForFadeOut);
		}
		else
		{
			if(SoundGroup != null)
			MasterAudio.FadeSoundGroupToVolume(SoundGroup.name, 0f, distanceForFadeOut);
		}
	}
}