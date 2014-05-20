using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject coneParent;
	public BoxCollider coneCollider;
	public LineRenderer coneRenderer;

	public enum letterList
	{
		DontHave,
		Have,
		HaveWritten
	};
	public letterList haveLetter;

	public enum DirList
	{
		Up,
		Left,
		Down,
		Right
	};
	public DirList MovingDir;
	public DirList FacingDir;

	public float speed = 5f;
	public Vector3 vecMove;

	public Vector3 mypos = Vector3.zero;
	public Vector3 _target = Vector3.zero;
	public float _diffX = 0f;
	public float _diffY = 0f;
	public float _angle = 0f;
	
	[SerializeField] private RaycastHit hitInfo;
	[SerializeField] private float halfMyX;
	[SerializeField] private float halfMyY;
	protected int wallMask = 1 << 8;

	private bool BlockedUp = false;
	private bool BlockedDown = false;
	private bool BlockedLeft = false;
	private bool BlockedRight = false;
	private GameObject gameSprite;

	// Use this for initialization
	public void Setup () 
	{
		coneParent = FETool.findWithinChildren(gameObject, "ParentCone");
		coneCollider = coneParent.GetComponentInChildren<BoxCollider>();
		coneRenderer = coneParent.GetComponentInChildren<LineRenderer>();
		gameSprite = FETool.findWithinChildren(gameObject, "Sprite");
		halfMyY = 0.5f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		BlockedDown = false;
		BlockedLeft = false;
		BlockedUp = false;
		BlockedRight = false;

		_target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		moveInput();
		rotateTowardMouse(coneParent.transform);
		rotateTowardMouse(gameSprite.transform);
		changeRenderer();
		wallBlocker();
		transform.position += vecMove * Time.deltaTime;
	}

	private void moveInput()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			vecMove.x -= speed;
			MovingDir = DirList.Left;
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			vecMove.x -= 5f;
		}
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
			vecMove.y -= speed;
			MovingDir = DirList.Down;
		}
		else if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
			vecMove.y += speed;
			MovingDir = DirList.Up;
		}
		else if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			vecMove.y += 5f;
		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			vecMove.x += speed;
			MovingDir = DirList.Right;
		}
		else if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			vecMove.x += 5f;
		}
		vecMove.x *= 0.1f;
		vecMove.y *= 0.1f;
	}

	private void wallBlocker()
	{
		mypos = transform.position;
		if (Physics.Raycast(mypos, Vector3.down, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (transform.position, hitInfo.point, Color.red);
			blockDown();
		}
		if (Physics.Raycast(mypos, Vector3.left, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (transform.position, hitInfo.point, Color.red);
			blockLeft();
		}
		if (Physics.Raycast(mypos, Vector3.up, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (transform.position, hitInfo.point, Color.red);
			blockUp();
		}
		if (Physics.Raycast(mypos, Vector3.right, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (transform.position, hitInfo.point, Color.red);
			blockRight();
		}
	}

	private void blockDown()
	{
		if (MovingDir == DirList.Down)
		{
			vecMove.x = 0f;
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockLeft()
	{
		if (MovingDir == DirList.Left)
		{
			vecMove.x = 0f;
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockRight()
	{
		if (MovingDir == DirList.Right)
		{
			vecMove.x = 0f;
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockUp()
	{
		if (MovingDir == DirList.Up)
		{
			vecMove.x = 0f;
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}

	private void rotateTowardMouse(Transform _trsf)
	{
		_diffX = _target.x - coneParent.transform.position.x;
		_diffY = coneParent.transform.position.y - _target.y;
		_angle = Mathf.Atan2( _diffX, _diffY) * Mathf.Rad2Deg;
		_trsf.rotation = Quaternion.Euler(0f, 0f, _angle - 90);
	}

	private void changeRenderer()
	{
//		coneRenderer.SetPosition(0, new Vector3(0,0f,0f) );
	}

}