using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 100f;
    public float maxPowerGaugeTime = 3f;
    public PlayerController playerController;

    private float currentRotation = 0f;
    private bool isAiming = false;
    private float powerGaugeStartTime;
    private float currentPower;

    private bool isUsingGyro = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        isUsingGyro = SystemInfo.supportsGyroscope;
        if (isUsingGyro)
        {
            Input.gyro.enabled = true;
        }
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("PlayerController not found!");
            }
        }
    }

    void Update()
    {
        if (!isAiming)
        {
            Rotate();

            if (Input.GetButtonDown("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Debug.Log("Start aiming in CameraController");
                StartAiming();
            }
        }
        else
        {
            UpdatePowerGauge();

            if (Input.GetButtonUp("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
            {
                Debug.Log("Throwing molkky stick in CameraController");
                ThrowMolkkyStick();
            }
        }
    }

    void Rotate()
    {
        if (isUsingGyro)
        {
            Quaternion gyroRotation = Input.gyro.attitude;
            transform.rotation = Quaternion.Euler(90f, 90f, 0f) * (new Quaternion(-gyroRotation.x, -gyroRotation.y, gyroRotation.z, gyroRotation.w));
        }
        else
        {
            float rotationInput = Input.GetAxis("Mouse X");
            if (Input.touchCount > 0)
            {
                rotationInput = Input.GetTouch(0).deltaPosition.x / Screen.width;
            }

            currentRotation += rotationInput * rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(15f, currentRotation, 0f);
        }

        if (playerController != null)
        {
            playerController.SetAimDirection(transform.forward);
        }
    }

    void StartAiming()
    {
        isAiming = true;
        powerGaugeStartTime = Time.time;
        UIManager.Instance?.ShowPowerGauge();
    }

    void UpdatePowerGauge()
    {
        float elapsedTime = Time.time - powerGaugeStartTime;
        currentPower = Mathf.PingPong(elapsedTime / maxPowerGaugeTime, 1f);
        UIManager.Instance?.UpdatePowerGauge(currentPower);
    }

    void ThrowMolkkyStick()
    {
        if (playerController != null)
        {
            playerController.ThrowMolkkyStick(currentPower * 1000f); // ここで直接投擲力を計算して渡す
        }

        isAiming = false;
        UIManager.Instance?.HidePowerGauge();
    }

    public void ResetAim()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        currentRotation = 0f;
        isAiming = false;
    }
}
