using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 ImpulseDirection;
    [SerializeField] private float ImpulseSpeed;
    private Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddImpulse()
    {
        rb.AddForce(ImpulseDirection * ImpulseSpeed, ForceMode.Impulse);
    }
}
