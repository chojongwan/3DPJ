using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRay : MonoBehaviour
{
    private float distance;
    private RaycastHit hit;
    private Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray();
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = transform.position;
        ray.direction = transform.forward;
    }
}
