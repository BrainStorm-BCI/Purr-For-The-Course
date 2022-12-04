using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Events;

public class RotationEvent : UnityEvent<float> { }

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float timeToRotateCompletely = 3f;
    [SerializeField] private float maxDegreeOffset = 30f;
    [SerializeField] private Transform transformToRotateAround;

    private Coroutine rotateCo;
    private bool isCoRunning;
    private bool canRotate = true;

    private RotationEvent StartRotationEvent = new RotationEvent();
    private float angleMultiplier = -1f;

    private Quaternion initialRotation;
    [SerializeField]private MeshRenderer meshRenderer;

    public Quaternion centerRotation;

    private float offsetInFront = 1f;
    public float getOffset() { return offsetInFront; }
    

    // Start is called before the first frame update
    void Awake()
    {
        centerRotation = transform.rotation;
        meshRenderer.enabled = false;

        // TODO: subscribe to event to start rotating
        StartRotationEvent.AddListener(StartRotation);
        //startRotation.Invoke(timeToRotateCompletely/2f);
    }

    public Vector3 getWorldSpaceDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    private void SetPositionRelativeToBall()
    {
        transform.position = transformToRotateAround.position + new Vector3(0f, 0f, offsetInFront);
        transform.rotation = initialRotation;
    }

    public void stopRotation()
    {
        meshRenderer.enabled = false;
        canRotate = false;
        if (isCoRunning)
        {
            StopCoroutine(rotateCo);
        }
    }

    public void StartRotation()
    {
        canRotate = true;
        //SetPositionRelativeToBall();
        meshRenderer.enabled = true;
        StartRotation(timeToRotateCompletely/2f);
    }

    private void StartRotation(float secondsToRotate)
    {
        if (!canRotate)
            return;

        angleMultiplier *= -1f;

        Quaternion currentRotation = centerRotation;


        Quaternion targetRotation = currentRotation * Quaternion.Euler(0, angleMultiplier * maxDegreeOffset, 0);
        Debug.Log("Target Rotation: " + targetRotation.eulerAngles);
        Debug.Log("Current Rotation: " + currentRotation.eulerAngles);


        rotateCo = StartCoroutine(RotateUntil(targetRotation, angleMultiplier * Quaternion.Angle(targetRotation, currentRotation) / secondsToRotate, StartRotationEvent));
    }

    private IEnumerator RotateUntil(Quaternion targetRotation, float degreesPerSecond, RotationEvent toInvoke)
    {
        isCoRunning = true;

        do
        {
            float degreesPerFrame = degreesPerSecond * Time.deltaTime;

            transform.RotateAround(transformToRotateAround.position, Vector3.up, degreesPerFrame);
            yield return null;
        } while (Quaternion.Angle(targetRotation, transform.rotation) > 0.5f);
        

        isCoRunning = false;
        toInvoke.Invoke(timeToRotateCompletely);
    }
}
