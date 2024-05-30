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
    public float maxJumpCount; // �߰�: �ִ� ���� Ƚ�� ����
    private int jumpCount; // ����: ���� ���� Ƚ�� ����

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
    private WaitForSeconds cachedWaitForSeconds;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;       //Ŀ���� ����� �ڵ�
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
            jumpCount = 0; // �߰�: ���� ����� �� ���� Ƚ�� �ʱ�ȭ
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)            //�����̴� �κ�
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();                //���� �޾ƿ��� �κ�
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && (IsGrounded() || jumpCount < maxJumpCount)) // ����: IsGrounded()�̰ų� ���� Ƚ���� maxJumpCount���� ������ ���� ����
        {
            rigidbody.AddForce(Vector2.up * jumptForce, ForceMode.Impulse);
            jumpCount++; // �߰�: ������ ������ ���� Ƚ�� ����
        }
        
    }
    public void JumpMaxCount(float value, float time)
    {
        StartCoroutine(JumpUp(value, time));
    }
    public IEnumerator JumpUp(float value, float time)
    {
        maxJumpCount += value;
        yield return new WaitForSeconds(time);
        maxJumpCount -= value;
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
        camCurXRot += mouseDelta.y * lookSensitivity;               //y���� * �ΰ���
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);   //�ּҰ����� �۾����� �ּҰ��� ��ȯ�ϰ� �ִ밪���� Ŀ���� �ִ밪 ��ȯ
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);      //-camCurXRot�� �ϴ� ������ Ʈ�������� -�� ���� ���� +�� �Ʒ��� ���⶧���� ��ȣ ������ ����

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);     //�� �Ʒ�
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