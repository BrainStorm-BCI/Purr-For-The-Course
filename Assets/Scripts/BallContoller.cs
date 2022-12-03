using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallContoller : MonoBehaviour
{
    [SerializeField] private float ImpulseSpeed = 3;
    private Rigidbody rb;

    [SerializeField] private ArrowController arrowController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void AddImpulse()
    {
        Vector3 direction = arrowController.getWorldSpaceDirection();
        rb.AddForce(direction * ImpulseSpeed, ForceMode.Impulse);
    }
}
