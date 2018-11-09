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

    [Header("Bullet Script References")]
    public StartingWeaponBullet startingWepBullet;
    public RocketWeaponBullet rocketWepBullet;
    public SMGWeaponBullet smgWepBullet;

    [Header("Weapon Variables")]
    public Transform[] fireFrom;
    public GameObject[] weaponMesh;

    [Header("Weapon Scriptable Object References")]
    public WeaponsSO startingWepSO;
    public WeaponsSO rocketWepSO;
    public WeaponsSO smgWepSO;

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

    //Private Starting Weapon Variables
    private float bulletSpeed;
    private float timeBetweenShots;
    private float bulletSpread;
    private float shotCounter;
    private float bulletSpreadWidth;

    //Private Rocket Weapon Variables
    private float bulletSpeedRocket;
    private float timeBetweenShotsRocket;
    private float bulletSpreadRocket;
    private float shotCounterRocket;
    private float bulletSpreadWidthRocket;
    private bool setRocketActive;

    //Private SMG Weapon Variables
    private float bulletSpeedSMG;
    private float timeBetweenShotsSMG;
    private float bulletSpreadSMG;
    private float shotCounterSMG;
    private float bulletSpreadWidthSMG;
    private bool setSMGActive;

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

        //Setting all the mesh renderers off
        for (int i = 0; i < weaponMesh.Length; i++)
        {
            weaponMesh[i].GetComponent<MeshRenderer>().enabled = false;
        }

        //Setting the values of the starting weapon variables equal to the scriptable object
        bulletSpeed = startingWepSO.wepBulletSpeed;
        timeBetweenShots = startingWepSO.wepFireRate;
        bulletSpread = startingWepSO.wepBulletSpread;

        //Setting the values of the rocket variables equal to the scriptable object
        bulletSpeedRocket = rocketWepSO.wepBulletSpeed;
        timeBetweenShotsRocket = rocketWepSO.wepFireRate;
        bulletSpreadRocket = rocketWepSO.wepBulletSpread;
        setRocketActive = false;

        //Setting the values of the rocket variables equal to the scriptable object
        bulletSpeedSMG = smgWepSO.wepBulletSpeed;
        timeBetweenShotsSMG = smgWepSO.wepFireRate;
        bulletSpreadSMG = smgWepSO.wepBulletSpread;
        setSMGActive = false;
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
            PlayerShooting();
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

    bool IsFiring()
    {
        //If the inputs are pressed then bool set to true
        //Otherwise if there are no inputs pressed, bool is set to false
        return Device.RightStickX || Device.RightStickY;
    }

    void PlayerShooting()
    {
        StartingWeapon();

        if (setRocketActive == true)
        {
            RocketWeapon();
        }

        if (setSMGActive == true)
        {
            SMGWeapon();
        }
    }

    void StartingWeapon()
    {
        //Setting the weapon mesh render on
        weaponMesh[10].GetComponent<MeshRenderer>().enabled = true;

        //Add bullet spread to the weapon
        bulletSpreadWidth = Random.Range(-bulletSpread, bulletSpread);

        //If is firing
        if (IsFiring())
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                StartingWeaponBullet newBullet = Instantiate(startingWepBullet, fireFrom[10].position, fireFrom[10].rotation) as StartingWeaponBullet;
                newBullet.bulletSpeed = bulletSpeedRocket;
                newBullet.transform.Rotate(0f, bulletSpreadWidthRocket, 0f);
            }
            return;
        }

        //If is not firing
        if (!IsFiring())
        {
            shotCounter = 0;
            return;
        }
    }

    void RocketWeapon()
    {
        //Setting the weapon mesh render on
        weaponMesh[5].GetComponent<MeshRenderer>().enabled = true;

        //Add bullet spread to the weapon
        bulletSpreadWidthRocket = Random.Range(-bulletSpreadRocket, bulletSpreadRocket);

        //If is firing
        if (IsFiring())
        {
            shotCounterRocket -= Time.deltaTime;
            if (shotCounterRocket <= 0)
            {
                shotCounterRocket = timeBetweenShotsRocket;
                RocketWeaponBullet newRocket = Instantiate(rocketWepBullet, fireFrom[5].position, fireFrom[5].rotation) as RocketWeaponBullet;
                newRocket.bulletSpeed = bulletSpeedRocket;
                newRocket.transform.Rotate(0f, bulletSpreadWidthRocket, 0f);
            }
            return;
        }

        //If is not firing
        if (!IsFiring())
        {
            shotCounterRocket = 0;
            return;
        }
    }

    void SMGWeapon()
    {
        //Setting the weapon mesh render on
        weaponMesh[0].GetComponent<MeshRenderer>().enabled = true;

        //Add bullet spread to the weapon
        bulletSpreadWidthSMG = Random.Range(-bulletSpreadSMG, bulletSpreadSMG);

        //If is firing
        if (IsFiring())
        {
            shotCounterSMG -= Time.deltaTime;
            if (shotCounterSMG <= 0)
            {
                shotCounterSMG = timeBetweenShotsSMG;
                SMGWeaponBullet newBullet = Instantiate(smgWepBullet, fireFrom[0].position, fireFrom[0].rotation) as SMGWeaponBullet;
                newBullet.bulletSpeed = bulletSpeedSMG;
                newBullet.transform.Rotate(0f, bulletSpreadWidthSMG, 0f);
            }
            return;
        }

        //If is not firing
        if (!IsFiring())
        {
            shotCounterSMG = 0;
            return;
        }
    }

    void WeaponSwitching()
    {
        //Adding swapping between player primary weapon and custom weapon
    }

    void WeaponAmmo()
    {

    }

    void OnCollisionStay(Collision theCol)
	{
        //Checks whether the player is colliding with either a floor
        //that is inside, or outside
		if (theCol.gameObject.CompareTag("FloorInside"))
		{
			this.moveSpeed = insideSpeed;
		} 
		else if (theCol.gameObject.CompareTag("FloorOutside"))
		{
			this.moveSpeed = outsideSpeed;
		}
	}

    void OnTriggerStay(Collider theCol)
    {
        //Checks whether the player is colliding with the weapon pickup
        //and if an input happens, then a weapon is activated
        if (theCol.gameObject.CompareTag("RocketPickup"))
        {
            if (Device.Action3.WasPressed)
            {
                setRocketActive = true;
                Destroy(theCol.gameObject);
            }
        }

        if (theCol.gameObject.CompareTag("SMGPickup"))
        {
            if (Device.Action3.WasPressed)
            {
                setSMGActive = true;
                Destroy(theCol.gameObject);
            }
        }
    }

    bool IsPlayerMoving()
    {
        return true;
    }
}
