using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum State
{
    Aiming
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector3 ImpulseDirection;
    [SerializeField] private float ImpulseSpeed;
    private Rigidbody rb;

    public UnityEvent StopArrow = new UnityEvent();

    public UnityEvent FireBallNow = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); 
    }

    public void ShootBallTowardsArrow()
    {
        Debug.Log("called by input system");
        // Logic on whether or not to fire the ball
        StopArrow.Invoke();
        FireBallNow.Invoke();
    }
}
