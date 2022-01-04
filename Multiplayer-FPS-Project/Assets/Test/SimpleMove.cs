using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (isLocalPlayer)
        {
            float m = Input.GetAxisRaw("Horizontal");
            float x = Input.GetAxisRaw("Vertical");

            Vector3 movee = new Vector3(m * 0.1f, x * 0.1f, 0);
            transform.position = transform.position + movee;
        }
    }
}
