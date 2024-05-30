using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlaneController : ControllableEntity // Inheritance
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
    [SerializeField] GameObject laser;
    public AudioClip Shoot;
    private AudioSource playerAudioShoot;
    private float responseModifier // Encapsulation
    {
        get
        {
            return (rb.mass / 10f) * responsiveness;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAudioShoot = GetComponent<AudioSource>();
    }

    protected override void HandleInputs() // Polymorphism
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
        HandleInputs(); // Abstraction
        UpdateHUD();
        ProcessFiring();
    }

    private void FixedUpdate()
    {
        rb.AddForce(-transform.right * throttle * maxThrust);
        rb.AddTorque(-transform.up * pitch * responseModifier);
        rb.AddTorque(transform.right * roll * responseModifier);
        rb.AddTorque(transform.forward * yaw * responseModifier);
    }

    protected override void UpdateHUD()
    {
        hud.text = "Throttle " + throttle.ToString("F0") + "%\n";
        hud.text += "Altitude " + transform.position.y.ToString("F0") + "m";
    }

    void ProcessFiring()
    {
        if (Input.GetButton("Fire1"))
        {
            SetLaserActive(true);
            if (!playerAudioShoot.isPlaying)
            {
                playerAudioShoot.PlayOneShot(Shoot, 1.0f);
            }
        }
        else
        {
            SetLaserActive(false);
            if(playerAudioShoot.isPlaying)
            {
                playerAudioShoot.Stop();
            }
        }
    }

    private void SetLaserActive(bool isActive)
    {
        var emissionModule = laser.GetComponent<ParticleSystem>().emission;
        emissionModule.enabled = isActive;
    }
}
