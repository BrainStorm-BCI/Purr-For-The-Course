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
}
