using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllableEntity : MonoBehaviour
{
    // Start is called before the first frame update
    protected float throttle;
    protected float roll;
    protected float pitch;
    protected float yaw;

    protected abstract void HandleInputs();
    protected abstract void UpdateHUD();
}
