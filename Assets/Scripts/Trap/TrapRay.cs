using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float detectionRange = 10.0f;  // 레이캐스트의 최대 거리
    public LayerMask playerLayer;         // 플레이어가 있는 레이어

    void Update()
    {
        // 레이캐스트를 위한 변수 선언
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // 레이캐스트 수행
        if (Physics.Raycast(ray, out hit, detectionRange, playerLayer))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("플레이어 작동됨");
                // 플레이어를 감지하면 함정을 작동시킴
                Trap trap = GetComponent<Trap>();
                if (trap != null)
                {
                    Debug.Log("함정 작동됨");
                }
            }
        }
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red, 1f);
    }
}
