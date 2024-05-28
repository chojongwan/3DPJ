using UnityEngine;

public class JumpPad : MonoBehaviour
{
    // 캐릭터가 점프대에 닿았을 때 가해질 점프 힘
    public float jumpForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // 점프대에 충돌한 오브젝트가 Rigidbody를 가지고 있는지 확인
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 위쪽으로 순간적인 힘(Impulse)을 가함
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
