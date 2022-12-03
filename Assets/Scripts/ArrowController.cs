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

    private RotationEvent startRotation = new RotationEvent();
    private float angleMultiplier = -1f;

    private Vector3 StartOffset;
    [SerializeField]private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        StartOffset = transform.localPosition;

        // TODO: subscribe to event to start rotating
        startRotation.AddListener(StartRotation);
        startRotation.Invoke(timeToRotateCompletely/2f);
    }

    public void enableRotation()
    {
        transform.localPosition = StartOffset;
        meshRenderer.enabled = false;
    }

    public void stopRotation()
    {
        meshRenderer.enabled = false;
        if (isCoRunning)
        {
            StopCoroutine(rotateCo);
        }
    }

    public Vector3 getWorldSpaceDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    private void StartRotation(float secondsToRotate)
    {
        angleMultiplier *= -1f;

        Quaternion currentRotation = transform.rotation;

        // rotation needed to reach target;
        //Quaternion aQuaternion = Quaternion.identity * Quaternion.Inverse(currentRotation);
        //Quaternion bQuaternion = Quaternion.identity * Quaternion.Inverse(Quaternion.Euler(0, angleMultiplier * maxDegreeOffset, 0));
        //Quaternion deltaQuaternion = bQuaternion * Quaternion.Inverse(aQuaternion);

        Quaternion targetRotation = Quaternion.Euler(0, angleMultiplier * maxDegreeOffset, 0);
        rotateCo = StartCoroutine(RotateUntil(targetRotation, angleMultiplier * Quaternion.Angle(targetRotation, currentRotation) / secondsToRotate, startRotation));
    }

    private IEnumerator RotateUntil(Quaternion targetRotation, float degreesPerSecond, RotationEvent toInvoke)
    {
        isCoRunning = true;


        while(Quaternion.Angle(targetRotation, transform.rotation) > 0.1f)
        {
            float degreesPerFrame = degreesPerSecond * Time.deltaTime;

            transform.RotateAround(transformToRotateAround.position, Vector3.up, degreesPerFrame);
            yield return null;
        }

        isCoRunning = false;
        toInvoke.Invoke(timeToRotateCompletely);
    }
}
