using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    private float timePassed = 0;

    private void Update()
    {
        timePassed += Time.deltaTime / 2;
        Light light = GetComponent<Light>();

        light.intensity = Mathf.Lerp(2f, 5, Mathf.PingPong(timePassed, 1));
    }
}
