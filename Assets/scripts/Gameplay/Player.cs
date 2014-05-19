using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	public Vector3 vecMove;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		vecMove = new Vector3(0f, 0f, 0f);
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			vecMove.x -= speed;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			vecMove.y -= speed;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			vecMove.y += speed;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			vecMove.x += speed;
			print ("up");
		}
		transform.position += vecMove;
	}
}
