using UnityEngine;
using System.Collections;

public class InputManager : ScriptableObject {

	public float BigAxis;
	public float X_AxisPos_Sensibility;
	public float X_AxisNeg_Sensibility;
	public float Y_AxisPos_Sensibility;
	public float Y_AxisNeg_Sensibility;

	public float SmallAxis;

	public float BtnAxis;
	public float X_AxisPos_Btn;
	public float X_AxisNeg_Btn;
	public float Y_AxisPos_Btn;
	public float Y_AxisNeg_Btn;

	public float DeadSens;
	public float X_AxisPos_DeadSens;
	public float X_AxisNeg_DeadSens;
	public float Y_AxisPos_DeadSens;
	public float Y_AxisNeg_DeadSens;

	public string EnterButton = "joystick button 0";
	public string BackButton = "joystick button 1";
	public string TriggerLeftButton = "joystick button 4";
	public string TriggerRightButton = "joystick button 5";

	public KeyCode KeyUp = KeyCode.UpArrow;
	public KeyCode KeyDown = KeyCode.DownArrow;
	public KeyCode KeyLeft = KeyCode.LeftArrow;
	public KeyCode KeyRight = KeyCode.RightArrow;
	public KeyCode KeyEnter =  KeyCode.Return;

}
