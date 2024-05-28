using UnityEngine;

public class JumpPad : MonoBehaviour
{
    // ĳ���Ͱ� �����뿡 ����� �� ������ ���� ��
    public float jumpForce = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        // �����뿡 �浹�� ������Ʈ�� Rigidbody�� ������ �ִ��� Ȯ��
        Rigidbody rb = collision.collider.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // �������� �������� ��(Impulse)�� ����
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
