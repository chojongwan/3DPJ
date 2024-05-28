using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public PlayerController playerController;
    public float speedIncreaseAmount = 5f;
    public LayerMask Player;

    private void OnTriggerEnter(Collider other)
    {
        if ((Player.value & (1 << other.gameObject.layer)) > 0)
        {
            playerController.moveSpeed += speedIncreaseAmount;
        }
    }
}
