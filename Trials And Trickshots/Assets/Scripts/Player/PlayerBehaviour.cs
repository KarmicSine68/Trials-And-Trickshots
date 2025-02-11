/******************************************************************************
 * Author: Brad Dixon
 * Creation Date: 1/30/2025
 * File Name: PlayerBehaviour.cs
 * Brief: Allows the player to move
 * ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    InputActionMap actionMap;
    InputAction playerMove, playerLook;
    Rigidbody rb;

    [Header("Movement Values")]
    [Tooltip("The speed of the player")]
    [SerializeField] private float playerSpeed;

    [Header("Camera Values")]
    [Tooltip("The sensitivity of the camera")]
    [SerializeField] private float sensitivity;
    
    [Header("Camera Vertical Turn Angles")]
    private float yRotation = 0f;
    [SerializeField] private float _vertViewHigh = 60f;
    [SerializeField] private float _vertViewLow = -60f;
    [SerializeField] private Transform camera;

    /// <summary>
    /// Enables the action map, input actions, and rigidbody
    /// </summary>
    private void Awake()
    {
        actionMap = GetComponent<PlayerInput>().currentActionMap;
        actionMap.Enable();

        playerMove = actionMap.FindAction("Move");
        playerLook = actionMap.FindAction("Look");

        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Locks the screen
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Calls player movement at a fixed rate
    /// </summary>
    private void FixedUpdate()
    {
        MovePlayer();

        LookAt();
    }

    private void Update()
    {
        Restart();

        Quit();
    }

    /// <summary>
    /// Moves the player
    /// </summary>
    private void MovePlayer()
    {
        Vector2 movementValue = playerMove.ReadValue<Vector2>() * playerSpeed;

        Vector3 movementVector = (movementValue.x * transform.right) + (movementValue.y * transform.forward);

        rb.velocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);
    }

    /// <summary>
    /// Moves the camera to look at the mouse
    /// </summary>
    private void LookAt()
    {
        float lookValueX = playerLook.ReadValue<Vector2>().x * sensitivity;
        float lookValueY = playerLook.ReadValue<Vector2>().y * sensitivity;
        //clamp vertical looking
        yRotation -= lookValueY;
        yRotation = Mathf.Clamp(yRotation, _vertViewLow, _vertViewHigh);
        //applying that lookin of up and down
        camera.transform.rotation = Quaternion.Euler(yRotation, 0f, 0f);
        //apply lookin for left and right
        this.gameObject.transform.Rotate(Vector3.up * lookValueX);
        //locking z rotation so it dont flip sideways
        camera.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, this.gameObject.transform.rotation.eulerAngles.y, 0f);            
        }
    private void Restart()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        actionMap.Disable();
    }
}
