using UnityEngine;
using System.Collections;

public class FEAnims : MonoBehaviour {

	public OTAnimation _animations;
	public OTAnimatingSprite _animSprite;

	public void playAnimation(OTAnimationFrameset _anim, float speed = 1f)
	{
		if (_animSprite.animationFrameset != _anim.name)
		{
			_animSprite.animationFrameset = _anim.name;
			_animSprite.speed = speed;
		}
	}
}
