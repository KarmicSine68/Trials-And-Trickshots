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

        //Debug.Log(playerLook.ReadValue<Vector2>());
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
        Vector2 lookValue = playerLook.ReadValue<Vector2>();

        transform.Rotate(new Vector3(0, lookValue.x * sensitivity, 0), Space.World);
    }
}
