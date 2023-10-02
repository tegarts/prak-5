using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour
{
    public Transform player;
    public Transform viewPoint;
    public Transform aimViewPoint;
    public float rotationSpeed;
    public GameObject tpsCamera, aimCamera;
    public GameObject crosshair;
    bool tpsMode = true, aimMode = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        crosshair.SetActive(false);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        viewPoint.forward = viewDir.normalized;

        if (tpsMode)
        {
            Vector3 inputDir = viewPoint.forward * verticalInput + viewPoint.right * horizontalInput;
            if (inputDir != Vector3.zero)
            {
                Debug.Log(inputDir);
                player.forward = Vector3.Slerp(player.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (aimMode)
        {
            Vector3 dirToCombatLookAt = aimViewPoint.position - new Vector3(transform.position.x, aimViewPoint.position.y, transform.position.z);
            aimViewPoint.forward = dirToCombatLookAt.normalized;

            player.forward = Vector3.Slerp(player.forward, dirToCombatLookAt.normalized, Time.deltaTime * rotationSpeed);
        }
    }

    public void CameraModeChanger(bool TPS, bool AIM)
    {
        tpsMode = TPS;
        aimMode = AIM;

        if (TPS)
        {
            tpsCamera.SetActive(true);
            aimCamera.SetActive(false);
            crosshair.SetActive(false);
        }
        else if (AIM)
        {
            tpsCamera.SetActive(false);
            aimCamera.SetActive(true);
            crosshair.SetActive(true);
        }
    }
}
