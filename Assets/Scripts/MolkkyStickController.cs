using UnityEngine;
using System.Collections;

public class MolkkyStickController : MonoBehaviour
{
    public float maxThrowForce = 20f;
    public float pendulumSpeed = 2f;
    public float maxPendulumAngle = 45f;
    public float dragCoefficient = 0.1f;

    private float throwPower;
    private bool isThrown = false;
    private Rigidbody rb;
    private float currentAngle = 0f;
    private bool isThrowComplete = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    public void InitializeThrow(Vector3 direction, float power)
    {
        throwPower = power;
        transform.forward = direction.normalized;
        StartCoroutine(ThrowSequence());
    }

    private IEnumerator ThrowSequence()
    {
        yield return StartCoroutine(PendulumMotion());
        PerformThrow();
    }

    private IEnumerator PendulumMotion()
    {
        float duration = 1f;
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            currentAngle = maxPendulumAngle * Mathf.Sin((Time.time - startTime) * pendulumSpeed);
            transform.rotation = Quaternion.Euler(currentAngle, transform.rotation.eulerAngles.y, 0);
            yield return null;
        }
    }

    private void PerformThrow()
    {
        isThrown = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        Vector3 throwDirection = Quaternion.Euler(currentAngle, transform.rotation.eulerAngles.y, 0) * Vector3.forward;
        rb.AddForce(throwDirection * throwPower, ForceMode.Impulse);
        rb.AddTorque(transform.right * throwPower, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        if (isThrown)
        {
            ApplyAirResistance();

            if (rb.velocity.magnitude < 0.01f && !isThrowComplete)
            {
                StopMotion();
            }
        }
    }

    private void ApplyAirResistance()
    {
        Vector3 dragForce = -dragCoefficient * rb.velocity.magnitude * rb.velocity;
        rb.AddForce(dragForce);
    }

    private void StopMotion()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        isThrown = false;
        isThrowComplete = true;
    }

    public bool IsThrowComplete()
    {
        return isThrowComplete;
    }
}