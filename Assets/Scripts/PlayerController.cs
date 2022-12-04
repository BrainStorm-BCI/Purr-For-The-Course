using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using Cinemachine;

enum State
{
    Aiming
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private ArrowController arrowController;
    [SerializeField] private BallContoller ballController;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private Transform transformToLookAt;
    

    private Rigidbody rb;

    public UnityEvent StopArrow = new UnityEvent();

    public UnityEvent FireBallNow = new UnityEvent();

    private Task ArrowTask;

    private bool FireCalled = false;
    private bool isMyTurn = true;

    private Transform FaceTowards;

    private bool isIdle = false;

    private bool isBallDoneMoving;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        StartCoroutine(GameLoopEnum());
    }

    public void SetupAiming()
    {
        // start aiming
        transform.LookAt(FaceTowards);

        arrowController.StartRotation();
    }

    private void RotateCameraToFacePosition(Vector3 position)
    {
        Vector3 diffPosition = (ballController.transform.position - position).normalized;
        diffPosition.y = 0.0f;

        Vector3 currentOffset = cinemachineVirtualCamera.transform.position - ballController.transform.position;
        currentOffset.y = 0f;
        float magnitude = currentOffset.magnitude;

        var transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        var newOffset = diffPosition * magnitude;
        newOffset.y = transposer.m_FollowOffset.y;
        transposer.m_FollowOffset = newOffset;

    }

    private void RotateArrowToFacePosition(Vector3 position)
    {
        float offset = arrowController.getOffset();

        Vector3 diffPosition = (position - ballController.transform.position).normalized;
        diffPosition.y = 0.0f;

        diffPosition *= offset;

        arrowController.transform.position = ballController.transform.position + diffPosition;
        arrowController.transform.rotation = Quaternion.LookRotation(diffPosition);
        arrowController.centerRotation = arrowController.transform.rotation;
    }

    private void SetBallDoneMovingTrue()
    {
        isBallDoneMoving = true;
    }

    private IEnumerator GameLoopEnum()
    {
        // wait until fire is called
        isIdle = true;

        while (isMyTurn)
        {
            SetupAiming();

            isIdle = true;

            while (!FireCalled)
                yield return null;

            FireCalled = false;

            isIdle = false;
            // stop the rotation
            arrowController.stopRotation();

            Debug.Log("before");

            ballController.startShoot();

            do
            {
                Debug.Log("Not yet running");
                yield return new WaitForEndOfFrame();
            } while (!ballController.getIsCoRunning());

            do
            {
                Debug.Log("running");
                yield return new WaitForEndOfFrame();
            } while (ballController.getIsCoRunning());

            Debug.Log("after");

            RotateCameraToFacePosition(transformToLookAt.position);
            RotateArrowToFacePosition(transformToLookAt.position);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            print("space key was pressed");
            Debug.Log("hehe");
            if (isIdle)
                FireCalled = true;
        }
    }

    public void ShootBallTowardsArrow()
    {
        //Debug.Log("called by input system");
        //// Logic on whether or not to fire the ball
        //if (isIdle)
        //    FireCalled = true;

        
        // await set new position
    }
}
