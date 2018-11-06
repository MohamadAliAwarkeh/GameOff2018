using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public enum PlayerState
{
	Idle,
	WalkingAndActions,
}

public class PlayerController : MonoBehaviour 
{

	//Public Variables
	[Header("Player Variables")]
	[Range(0, 10)] public float insideSpeed;
	[Range(0, 10)] public float outsideSpeed;
	public PlayerState playerState = PlayerState.Idle;
	public bool KeyboardTesting = false;
	
	//Private Variables
	private Rigidbody myRB;
	private Vector3 moveInput;
	private Vector3 moveVelocity;
	private float moveSpeed;
	
	public InputDevice Device
	{
		get;
		set;
	}
	
	void Start ()
	{
		myRB = GetComponent<Rigidbody>();
	}
	
	void Update () 
	{
		switch (playerState)
		{
			case PlayerState.Idle:
				//The states a player can have during idle
				Idle();
				break;
			
			case PlayerState.WalkingAndActions:
				//The states a player can have during Walking and Actions
				PlayerMovement();
				break;
		}
	}

	void FixedUpdate()
	{
		myRB.velocity = moveVelocity;
	}

	void Idle()
	{
		//Code for idle in here
	}

	void PlayerMovement()
	{
		//Adding velocity to the player with moveSpeed to make him move
		if (KeyboardTesting)
		{
			moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
			moveVelocity = moveInput * moveSpeed;
		}

		if (!KeyboardTesting)
		{
			moveInput = new Vector3(Device.LeftStickX, 0f, Device.LeftStickY);
			moveVelocity = moveInput * moveSpeed;
		}	
	}

	void OnCollisionStay(Collision theCol)
	{
		if (theCol.gameObject.CompareTag("FloorInside"))
		{
			this.moveSpeed = insideSpeed;
		} 
		else if (theCol.gameObject.CompareTag("FloorOutside"))
		{
			this.moveSpeed = outsideSpeed;
		}
	}
}
