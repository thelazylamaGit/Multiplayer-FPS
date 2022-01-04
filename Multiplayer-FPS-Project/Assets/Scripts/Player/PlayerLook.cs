using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : NetworkBehaviour
{
    [SerializeField] private float sensX = 10f;
    [SerializeField] private float sensY = 10f;

    public Transform cameraHolder;
    [SerializeField] private Transform orientation;

    private float mouseX;
    private float mouseY;

    private float multiplier = 0.01f;

    private float xRotation;
    private float yRotation;
    // Start is called before the first frame update
    public override void OnStartAuthority()
    {
        enabled = true;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        cameraHolder.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void PlayerInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRotation += mouseX * sensX * multiplier;
        xRotation -= mouseY * sensY * multiplier;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }
}
