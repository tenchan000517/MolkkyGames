using UnityEngine;

public class SkittleController : MonoBehaviour
{
    public int pointValue;
    public float knockOverThreshold = 5f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private bool isKnockedDown = false;

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
        transform.position = position;
        initialRotation = Quaternion.identity;
        transform.rotation = initialRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isKnockedDown && collision.gameObject.GetComponent<MolkkyStickController>() != null)
        {
            float impactForce = collision.impulse.magnitude;
            if (impactForce > knockOverThreshold)
            {
                KnockDown();
            }
        }
    }

    private void KnockDown()
    {
        if (!isKnockedDown)
        {
            isKnockedDown = true;
            GameManager.Instance?.RegisterKnockedDownSkittle(this);
        }
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        isKnockedDown = false;
    }

    public bool IsKnockedDown()
    {
        return isKnockedDown;
    }
}