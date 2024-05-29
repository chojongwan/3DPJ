using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovingPlatform : MonoBehaviour
{
    public LayerMask playerLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, playerLayer))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsInLayerMask(collision.gameObject, playerLayer))
        {
            collision.transform.SetParent(null);
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
