using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Sensitivity")]
    public float sensX;
    public float sensY;

    [Header("Clamping")]
    public float minY;
    public float maxY;

    [Header("Spectator")]
    public float spectatorMoveSpeed; //izleyici modu

    private float rotX;
    private float rotY;

    private bool isSpectator;

    void Start ()
    {
        // imleci gizle
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate ()
    {
        // fare hareketi girdilerini al
        rotX += Input.GetAxis("Mouse X") * sensX;
        rotY += Input.GetAxis("Mouse Y") * sensY;

        // dikey dönüş
        rotY = Mathf.Clamp(rotY, minY, maxY);

        if(isSpectator)
        {
            // dikey olarak kamerayı aktifleştir
            transform.rotation = Quaternion.Euler(-rotY, rotX, 0);

            
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            float y = 0;

            if(Input.GetKey(KeyCode.E))
                y = 1;
            else if(Input.GetKey(KeyCode.Q))
                y = -1;

            Vector3 dir = transform.right * x + transform.up * y + transform.forward * z;
            transform.position += dir * spectatorMoveSpeed * Time.deltaTime;
        }
        else
        {
            // dikey
            transform.localRotation = Quaternion.Euler(-rotY, 0, 0);

            // yatay
            transform.parent.rotation = Quaternion.Euler(0, rotX, 0);
        }
    }

    //izleyici ise true yap ve ölen oyuncudan ayrıl
    public void SetAsSpectator ()
    {
        isSpectator = true;
        transform.parent = null;
    }
}