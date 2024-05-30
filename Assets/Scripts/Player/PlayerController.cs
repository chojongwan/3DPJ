using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.InputSystem.XR;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumptForce;
    public LayerMask groundLayerMask;
    public float maxJumpCount; // 추가: 최대 점프 횟수 변수
    private int jumpCount; // 변경: 현재 점프 횟수 변수

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;

    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;
    public Action inventory;

    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       //커서를 숨기는 코드
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
        if (IsGrounded())
        {
            jumpCount = 0; // 추가: 땅에 닿았을 때 점프 횟수 초기화
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)            //움직이는 부분
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();                //값을 받아오는 부분
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && (IsGrounded() || jumpCount < maxJumpCount)) // 변경: IsGrounded()이거나 점프 횟수가 maxJumpCount보다 적으면 점프 가능
        {
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
            jumpCount++; // 추가: 점프할 때마다 점프 횟수 증가
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;               //y값과 * 민감도
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);   //최소값보다 작아지면 최소값을 반환하고 최대값보다 커지면 최대값 반환
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);      //-camCurXRot를 하는 이유는 트랜스폼이 -면 위를 보고 +면 아래를 보기때문에 부호 반전을 위해

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);     //위 아래
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.3f , groundLayerMask))
            {
                return true;
            }
            Debug.DrawRay(rays[i].origin, rays[i].direction * 0.1f, Color.red, 1f);
        }
        

        return false;
    }
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
    public void SpeedUp(float value, float time)
    {
        StartCoroutine(SpeedBoost( value, time));
    }

    public IEnumerator SpeedBoost(float value, float time)
    {
        moveSpeed += value;
        yield return new WaitForSeconds(time);
        moveSpeed -= value;
    }
}