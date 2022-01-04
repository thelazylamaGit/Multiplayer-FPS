using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : NetworkBehaviour
{
    public Transform cameraPos;
    [SerializeField] private Camera cam;
    void Update()
    {
        transform.position = cameraPos.position;
    }
}
