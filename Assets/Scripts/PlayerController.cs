using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject molkkyStickPrefab;
    public LineRenderer aimLine;

    private bool isAiming = false;
    private Vector3 aimDirection;

    private void Start()
    {
        aimDirection = transform.forward;
        if (aimLine != null)
        {
            aimLine.enabled = false;
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (!isAiming)
            {
                Debug.Log("Start aiming");
                StartAiming();
            }
            else
            {
                Debug.Log("Throwing molkky stick");
            }
        }

        if (isAiming)
        {
            Aim();
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        aimDirection = transform.forward;
        if (aimLine != null)
        {
            aimLine.enabled = true;
        }
    }

    private void Aim()
    {
        if (aimLine != null)
        {
            aimLine.SetPosition(0, transform.position + Vector3.up);
            aimLine.SetPosition(1, transform.position + Vector3.up + aimDirection * 10f);
        }
    }

    public void SetAimDirection(Vector3 direction)
    {
        aimDirection = direction;
    }

    public void ThrowMolkkyStick(float power)
    {
        Debug.Log("Inside ThrowMolkkyStick");
        if (molkkyStickPrefab != null)
        {
            Vector3 spawnPosition = transform.position + transform.forward + Vector3.up;
            Debug.Log("Spawn position: " + spawnPosition);
            Debug.Log("Aim direction: " + aimDirection);
            Debug.Log("Throw power: " + power);

            GameObject molkkyStick = Instantiate(molkkyStickPrefab, spawnPosition, transform.rotation);
            Rigidbody stickRb = molkkyStick.GetComponent<Rigidbody>();
            if (stickRb != null)
            {
                // Ensure isKinematic is set to false and useGravity is set to true before adding force
                stickRb.isKinematic = false;
                stickRb.useGravity = true;
                Debug.Log("Adding force to molkky stick");
                stickRb.AddForce(aimDirection * power, ForceMode.Impulse);
                Debug.Log("Current velocity after AddForce: " + stickRb.velocity);
                Debug.Log("Is Kinematic: " + stickRb.isKinematic);
                Debug.Log("Use Gravity: " + stickRb.useGravity);
            }
            else
            {
                Debug.LogError("Molkky stick prefab does not have a Rigidbody component!");
            }
        }
        else
        {
            Debug.LogError("Molkky stick prefab is not assigned!");
        }

        isAiming = false;
        if (aimLine != null)
        {
            aimLine.enabled = false;
        }
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(0, 1, -10);
        transform.rotation = Quaternion.identity;
    }
}
