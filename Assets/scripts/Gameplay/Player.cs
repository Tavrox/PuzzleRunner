using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject coneParent;
	public BoxCollider coneCollider;
	public LineRenderer coneRenderer;

	public float speed = 5f;
	public Vector3 vecMove;

	public Vector3 _target = Vector3.zero;
	public float _diffX = 0f;
	public float _diffY = 0f;
	public float _angle = 0f;

	// Use this for initialization
	public void Setup () 
	{
		coneParent = FETool.findWithinChildren(gameObject, "ParentCone");
		coneCollider = coneParent.GetComponentInChildren<BoxCollider>();
		coneRenderer = coneParent.GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		moveInput();
		rotateTowardMouse();
		changeRenderer();
	}

	private void moveInput()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			vecMove.x -= speed;
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			vecMove.x -= 5f;
		}
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
			vecMove.y -= speed;
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			vecMove.y += speed;
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			vecMove.x += speed;
			print ("up");
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			vecMove.x += 5f;
		}
		vecMove.x *= 0.1f;
		vecMove.y *= 0.1f;
		transform.position += vecMove * Time.deltaTime;
	}

	private void rotateTowardMouse()
	{
		_diffX = _target.x - coneParent.transform.position.x;
		_diffY = coneParent.transform.position.y - _target.y;
		_angle = Mathf.Atan2( _diffX, _diffY) * Mathf.Rad2Deg;
		coneParent.transform.rotation = Quaternion.Euler(0f, 0f, _angle - 90);
	}

	private void changeRenderer()
	{
//		coneRenderer.SetPosition(0, new Vector3(0,0f,0f) );
	}
}