using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    public Transform playerBody;
    public Transform lockLocation;
    public Transform cameraParent;
    private PlayerSettings ps;
    public Vector2 mouseInput;
    public PlayerController playerContr;

    float xRotation = 0f;

    public float normaliseYSens = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        ps = PlayerSettings.Instance;
        //Cursor.visible = false;
    }

    private Vector2 GetMouseLook()
    {
        return mouseInput;
    }

    private void UpdateMouseLook()
    {
        if (!playerContr.GetFrozen())
        {
            mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        } else
        {
            mouseInput = Vector2.zero;
        }
    }

    private Vector2 GetControllerLook()
    {
        return mouseInput;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();

        Vector2 inputVec;
        if (GetMouseLook() == Vector2.zero) {
            inputVec = GetControllerLook();
            inputVec = new Vector2(inputVec.x * ps.GetControllerXSens(), inputVec.y * ps.GetControllerYSens() * normaliseYSens);
        } else
        {
            inputVec = GetMouseLook();
            inputVec = new Vector2(inputVec.x * ps.GetMouseXSens(), inputVec.y * ps.GetMouseYSens() * normaliseYSens);
        }

        xRotation -= inputVec.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * inputVec.x * ps.GetMouseXSens());

        cameraParent.transform.position = Vector3.Lerp(cameraParent.transform.position, lockLocation.position, 0.2f);
        cameraParent.transform.rotation = lockLocation.rotation;
    }
}
