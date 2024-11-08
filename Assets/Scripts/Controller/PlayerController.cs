using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public interface IMovable
{
    void Move();
}

public class PlayerController : MonoBehaviour, IMovable
{
    [HideInInspector]
    public Rigidbody rb;
    private PlayerCondition condition;

    [Header("Move")]
    [SerializeField] private LayerMask GroundLayerMask;
    private Vector3 inputVector = Vector3.zero;
    public bool canMove = true;
    private float walkSpeed;
    private float runSpeed = 1;
    private float jumpPower;
    private float jumpStamina;
    public float debuff = 1;

    [Header("Look")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    private Vector2 mouseDelta = Vector2.zero;
    private float verticalRotation;
    private float mouseSensitivity;
    private float maxVerticalRotation = 65;
    private float minVerticalRotation = -65;
    public bool canLook = true;


    [HideInInspector] public Action ToggleInventory;
    [HideInInspector] public bool isBuilding;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        condition = CharacterManager.Instance.player.condition;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        if(canLook)
        Look();
    }

    public void Move()
    {
        Vector3 moveDirection = Vector3.one;
        moveDirection = transform.forward * inputVector.y + transform.right * inputVector.x;
        moveDirection.Normalize();

        Vector3 horizontalVector = moveDirection * walkSpeed * runSpeed * debuff;
        rb.velocity = new Vector3(horizontalVector.x, rb.velocity.y, horizontalVector.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        walkSpeed = CharacterManager.Instance.player.playerData.walkSpeed;

        if (context.phase == InputActionPhase.Performed)
        {
            inputVector = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            inputVector = Vector3.Slerp(inputVector, Vector3.zero, 1f);
        }
    }

    private void Look()
    {
        verticalRotation = Mathf.Clamp(-mouseDelta.y, minVerticalRotation, maxVerticalRotation);

        transform.eulerAngles += new Vector3(0, mouseDelta.x, 0);
        virtualCamera.transform.localEulerAngles += new Vector3(verticalRotation, 0, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseSensitivity = CharacterManager.Instance.player.playerData.mouseSensitivity;

        if (context.phase == InputActionPhase.Performed)
        {
            mouseDelta = context.ReadValue<Vector2>() * mouseSensitivity;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            mouseDelta = Vector2.zero;
        }
    }

    public bool IsGround()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(transform.position + Vector3.forward * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + -Vector3.forward* 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + Vector3.right * 0.2f + Vector3.up * 0.01f, Vector3.down),
            new Ray(transform.position + -Vector3.forward * 0.2f + Vector3.up * 0.01f, Vector3.down)
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.1f, GroundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpPower = CharacterManager.Instance.player.playerData.jumpPower;
        jumpStamina = CharacterManager.Instance.player.playerData.jumpStamina;

        if (context.phase == InputActionPhase.Started && IsGround())
        {
            if (condition.UseStamina(jumpStamina))
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            runSpeed = CharacterManager.Instance.player.playerData.runSpeed;
            CharacterManager.Instance.player.equip.curEquip?.OnRunInput(true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            runSpeed = 1;
            CharacterManager.Instance.player.equip.curEquip?.OnRunInput(false);
        }
    }

    public void OnSetting(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            ToggleCursor();
        }
    }

    private void ToggleCursor()
    {
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
