﻿using System.Collections;
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

    [Header("Player Audio")]
    public List<AudioClip> playerSFX = new List<AudioClip>();
	
	//Private Variables
	private Rigidbody myRB;
	private Vector3 moveInput;
	private Vector3 moveVelocity;
	private float moveSpeed;
    private Camera mainCamera;
    private Vector3 playerDirection;
    private Vector2 playerLookDirection;

    public InputDevice Device
	{
		get;
		set;
	}

	void Start ()
	{
		myRB = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();

        playerLookDirection.x = 0f;
        playerLookDirection.y = 1f;
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
            PlayerRotation();
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
        moveInput = new Vector3(Device.LeftStickX, 0f, Device.LeftStickY);
		moveVelocity = moveInput * moveSpeed;
	}

    void PlayerRotation()
    {
        //The normal direction is where the player is set to being able to move around and rotate
        //whilst shooting
        playerDirection = Vector3.right * Device.RightStickX + Vector3.forward * Device.RightStickY;
        //Checking if the vector3 has got a value inputed
        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            Vector3 tempRotationValue = transform.rotation.eulerAngles;
            tempRotationValue.y = tempRotationValue.y + 17;
            transform.rotation = Quaternion.Euler(tempRotationValue);
            playerLookDirection.x = playerDirection.x;
            playerLookDirection.y = playerDirection.z;
        }
        else
        {
            //The alt direction is where the player is set to being able to move around and rotate
            //whilst not having to shoot
            Vector3 playerDirectionAlt = Vector3.right * Device.LeftStickX + Vector3.forward * Device.LeftStickY;
            if (playerDirectionAlt.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirectionAlt, Vector3.up);
                Vector3 tempRotationValue = transform.rotation.eulerAngles;
                tempRotationValue.y = tempRotationValue.y + 17;
                transform.rotation = Quaternion.Euler(tempRotationValue);
                playerLookDirection.x = playerDirectionAlt.x;
                playerLookDirection.y = playerDirectionAlt.z;
            }
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

    bool IsPlayerMoving()
    {
        return true;
    }
}