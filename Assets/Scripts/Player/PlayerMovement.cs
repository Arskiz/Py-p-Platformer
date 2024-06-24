using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerMotor playerMotor;
    Weapon weapon;
    int headingDirection = 1; // 0; Left | 1; Right
    [Header("Player Movement Variables:")]
    [SerializeField] private float _currentSpeed;
    struct cachedSpeed{
        public float cachedPlayerSpeed;
        public float cachedMaxSpeed;
    };
    cachedSpeed cS;
    // Start is called before the first frame update
    void Start()
    {
        playerMotor = FindAnyObjectByType<PlayerMotor>();
        weapon = FindAnyObjectByType<Weapon>();
        cS.cachedPlayerSpeed = playerMotor._playerSpeed;
        cS.cachedMaxSpeed = playerMotor._playerMaxVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleFlipDirection();
    }

    void HandleFlipDirection()
    {
        switch (headingDirection)
        {
            // Left; 0 | Right; 1
            case 0:
                playerMotor._playerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                playerMotor._playerObject.transform.GetChild(2).GetComponent<SpriteRenderer>().flipX = true;
                if(weapon.weapons.Count > 0)
                    weapon.weapons[weapon.selectedWeapon].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                break;

            case 1:
                playerMotor._playerObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                playerMotor._playerObject.transform.GetChild(2).GetComponent<SpriteRenderer>().flipX = false;
                if(weapon.weapons.Count > 0)
                    weapon.weapons[weapon.selectedWeapon].transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
                break;
        }
    }

    void HandleJumping()
    {
        // Player Jumping
        if (VarHelper.CheckKeybindPressed(playerMotor._jumpKeybind) && Grounded())
        {
            playerMotor._playerObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            playerMotor._playerObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, playerMotor._jumpForce), ForceMode2D.Impulse);
        }
    }

    bool Grounded()
    {
        if (GameObject.Find("FloorCheck").GetComponent<FloorCheckInfo>().grounded == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void HandleMovement()
    {
        // Left Movement:
        if (VarHelper.CheckKeybindHold(playerMotor._movementLeftKeybind))
        {
            headingDirection = 0;
            if (_currentSpeed > -playerMotor._playerMaxVelocity)
            {
                _currentSpeed -= playerMotor._playerSpeed * Time.deltaTime;
            }
            else
            {
                _currentSpeed = -playerMotor._playerMaxVelocity;
            }
        }

        // Right Movement
        else if (VarHelper.CheckKeybindHold(playerMotor._defaultMovementRightKeybind))
        {
            headingDirection = 1;
            if (_currentSpeed < playerMotor._playerMaxVelocity)
            {
                _currentSpeed += playerMotor._playerSpeed * Time.deltaTime;
            }
            else
            {
                _currentSpeed = playerMotor._playerMaxVelocity;
            }
        }

        // Deceleration when no key is pressed
        else
        {
            if (_currentSpeed < 0f)
            {
                _currentSpeed += playerMotor._playerSpeed * Time.deltaTime;
                if (_currentSpeed > 0f) _currentSpeed = 0f;  // Stop overshooting to positive
            }
            if (_currentSpeed > 0f)
            {
                _currentSpeed -= playerMotor._playerSpeed * Time.deltaTime;
                if (_currentSpeed < 0f) _currentSpeed = 0f;  // Stop overshooting to negative
            }
        }

        // Apply the translation
        playerMotor._playerObject.transform.Translate(new Vector3(_currentSpeed * Time.deltaTime, 0f, 0f));
        HandleSprint();
    }

    void HandleSprint()
    {
        if(VarHelper.CheckKeybindHold(playerMotor._sprintKeybind)){
            playerMotor._playerSpeed *= 1.5f;
            playerMotor._playerMaxVelocity *= 1.5f;
        }
        else{
            playerMotor._playerSpeed = cS.cachedPlayerSpeed;
            playerMotor._playerMaxVelocity = cS.cachedMaxSpeed;
        }
    }


    
}
