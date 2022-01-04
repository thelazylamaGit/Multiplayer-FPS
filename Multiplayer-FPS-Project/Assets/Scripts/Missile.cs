using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public void Update()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 100, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}