using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float detectionRange = 10.0f;  // ����ĳ��Ʈ�� �ִ� �Ÿ�
    public LayerMask playerLayer;         // �÷��̾ �ִ� ���̾�

    void Update()
    {
        // ����ĳ��Ʈ�� ���� ���� ����
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // ����ĳ��Ʈ ����
        if (Physics.Raycast(ray, out hit, detectionRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("�÷��̾� �۵���");
                // �÷��̾ �����ϸ� ������ �۵���Ŵ
                Trap trap = GetComponent<Trap>();
                if (trap != null)
                {
                    Debug.Log("���� �۵���");
                }
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
    }
}
