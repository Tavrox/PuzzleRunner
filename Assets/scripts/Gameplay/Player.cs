using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public GameObject coneParent;
	public BoxCollider coneCollider;
	public LineRenderer coneRenderer;
	public LevelManager levMan;


	public enum letterList
	{
		DontHave,
		Have,
		IsWriting,
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
	public float sleepQty;
	public float hungerQty;
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

	private Transform RayDL;
	private Transform RayUL;
	private Transform RayDR;
	private Transform RayUR;

	// Use this for initialization
	public void Setup (LevelManager _lev) 
	{
		coneParent = FETool.findWithinChildren(gameObject, "ParentCone");
		gameSprite = FETool.findWithinChildren(gameObject, "Sprite");
		levMan = _lev;

		RayDL = FETool.findWithinChildren(gameObject, "RayOrigin_DL").transform;
		RayUL = FETool.findWithinChildren(gameObject, "RayOrigin_DR").transform;
		RayDR = FETool.findWithinChildren(gameObject, "RayOrigin_UL").transform;
		RayUR = FETool.findWithinChildren(gameObject, "RayOrigin_UR").transform;

		coneCollider = coneParent.GetComponentInChildren<BoxCollider>();
		coneRenderer = coneParent.GetComponentInChildren<LineRenderer>();
		halfMyY = 0.25f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		BlockedDown = false;
		BlockedLeft = false;
		BlockedUp = false;
		BlockedRight = false;

		_target = GameObject.Find("LevelManager/Camera").GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
		moveInput();
		rotateTowardMouse(_target ,coneParent.transform);
		rotateTowardMouse(_target , gameSprite.transform);
		changeRenderer();
		wallBlocker();
		writePaper();
		transform.position += vecMove * Time.deltaTime;
	}

	private void writePaper()
	{
		if (Input.GetKey(KeyCode.R))
		{
			haveLetter = letterList.IsWriting;
			levMan.writtenPaper += 0.1f;
		}
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

	public void giveFood()
	{

	}

	public void giveLetter()
	{
		haveLetter = letterList.Have;
	}

	private void wallBlocker()
	{
		mypos = transform.position;
		if (Physics.Raycast(RayDL.position, Vector3.down, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDR.position, Vector3.down, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.blue);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.blue);
			blockDown();
		}
		if (Physics.Raycast(RayUL.position, Vector3.left, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDL.position, Vector3.left, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.black);
			Debug.DrawLine (RayDL.position, hitInfo.point, Color.black);
			blockLeft();
		}
		if (Physics.Raycast(RayUL.position, Vector3.up, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayUR.position, Vector3.up, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUL.position, hitInfo.point, Color.white);
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.white);
			blockUp();
		}
		if (Physics.Raycast(RayUR.position, Vector3.right, out hitInfo, halfMyY, wallMask) ||
		    Physics.Raycast(RayDR.position, Vector3.right, out hitInfo, halfMyY, wallMask))
		{
			Debug.DrawLine (RayUR.position, hitInfo.point, Color.red);
			Debug.DrawLine (RayDR.position, hitInfo.point, Color.red);
			blockRight();
		}
	}

	private void blockDown()
	{
		if (MovingDir != DirList.Up)
		{
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}
	private void blockLeft()
	{
		if (MovingDir != DirList.Right)
		{
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockRight()
	{
		if (MovingDir != DirList.Left)
		{
			vecMove.x = 0f;
			BlockedDown = true;
		}
	}
	private void blockUp()
	{
		if (MovingDir != DirList.Down)
		{
			vecMove.y = 0f;
			BlockedDown = true;
		}
	}

	private void rotateTowardMouse(Vector3 targ ,Transform _trsf)
	{
		_diffX = targ.x - coneParent.transform.position.x;
		_diffY = coneParent.transform.position.y - targ.y;
		_angle = Mathf.Atan2( _diffX, _diffY) * Mathf.Rad2Deg;
		_trsf.rotation = Quaternion.Euler(0f, 0f, _angle - 90);
	}

	private void changeRenderer()
	{
//		coneRenderer.SetPosition(0, new Vector3(0,0f,0f) );
	}

}