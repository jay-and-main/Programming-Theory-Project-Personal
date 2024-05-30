using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipController : MonoBehaviour
{
    [Header("Plane Stats")]
    [Tooltip("How much the throttle ramps up or down")]
    public float throttleIncrement = 0.1f;
    [Tooltip("Maximum engine thrust when at 100% throttle")]
    public float maxThrust = 200f;
    [Tooltip("How responsive the plan is when pitching, rolling, and yawing")]
    public float responsiveness = 10f;
    private float throttle;
    private float roll;
    private float pitch;
    private float yaw;
    Rigidbody rb;
    [SerializeField] TextMeshProUGUI hud;
    private float responseModifier
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void HandleInputs()
    {
        roll = Input.GetAxis("Roll");
        pitch = Input.GetAxis("Pitch");
        yaw = Input.GetAxis("Yaw");

        if (Input.GetKey(KeyCode.Space))
        {
            throttle += throttleIncrement;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            throttle -= throttleIncrement;
        }
        throttle = Mathf.Clamp(throttle, 0f, 100f);
    }

    private void Update()
    {
        HandleInputs();
        UpdateHUD();
    }

    private void FixedUpdate()
    {
        rb.AddForce(-transform.right * throttle * maxThrust);
        rb.AddTorque(-transform.up * pitch * responseModifier);
        rb.AddTorque(transform.right * roll * responseModifier);
        rb.AddTorque(transform.forward * yaw * responseModifier);
    }

    private void UpdateHUD()
    {
        hud.text = "Throttle " + throttle.ToString("F0") + "%\n";
        hud.text += "Altitude " + transform.position.y.ToString("F0") + "m";
    }
}
