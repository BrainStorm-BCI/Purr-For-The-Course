using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class BallContoller : MonoBehaviour
{
    [SerializeField] private float maxImpulseSpeed = 60.0f;
    [SerializeField] private float minImpulseSpeed = 1.0f;
    private Rigidbody rb;
    [SerializeField] private float vEpsilon = 0.1f;
    [SerializeField] private float timeToQuicklySlowDown = 0.2f;

    [SerializeField] private ArrowController arrowController;

    private bool isCoRunning = false;
    private Coroutine co;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public bool getIsCoRunning() { return isCoRunning; }

    public void startShoot()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isCoRunning = true;
        co = StartCoroutine(ShootBallForward());
    }

    public IEnumerator ShootBallForward()
    {
        Vector3 direction = arrowController.getWorldSpaceDirection();
        float percentage = arrowController.getForcePercentage();
        float impulseSpeed = (maxImpulseSpeed - minImpulseSpeed) * percentage + minImpulseSpeed;

        Debug.Log("Impulse speed: " + impulseSpeed);
        Debug.Log("impulse Percentage: " + percentage);
        Debug.Log("max Impulse speed: " + maxImpulseSpeed);
        Debug.Log("min Impulse speed: " + minImpulseSpeed);

        rb.AddForce(direction * impulseSpeed, ForceMode.Impulse);

        do
        {
            yield return null;
        } while (rb.velocity.magnitude < vEpsilon);

        do
        {
            yield return null;
        } while (rb.velocity.magnitude > vEpsilon);

        // slow down quickly
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        isCoRunning = false;
    }

   

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            Debug.Log("Its a speeeed!");
            speedUp(other.transform.forward, 8);
        }

        if (other.gameObject.tag == "Rough")
        {
            Debug.Log("Its a rough!");
           
            slowDown(70);
        }

    }

    public void speedUp(Vector3 direction,int speed)
    {
        rb.AddForce(direction*speed,ForceMode.Impulse);
    }

    public void slowDown(int speed)
    {
        Vector3 direction = Vector3.Normalize(this.transform.forward) * -1;
        //Vector3 test = direction * speed + rb.velocity;
        //if(test.x * rb.velocity.x <= 0 )
        //{//different sign meaning we'd be changing the x direction if we use this change
        //    test.x = 0;
        //}
        //if (test.y * rb.velocity.y <= 0)
        //{//different sign meaning we'd be changing the x direction if we use this change
        //    test.y = 0;
        //}
        //if (test.z * rb.velocity.z <= 0)
        //{//different sign meaning we'd be changing the x direction if we use this change
        //    test.z = 0;
        //}
        
        //rb.velocity = test;
        rb.AddForce(direction*speed, ForceMode.Acceleration);
    }
}
