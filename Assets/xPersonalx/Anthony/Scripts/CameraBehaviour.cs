using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform headTransform;
    public Transform cameraTransform;

    public float bobFrequency = 5f;
    public float bobHorizontalAmplitude = 0.01f;
    public float bobVerticalAmplitude = 0.01f;
    [Range(0, 1)] public float headBobSmoothing = 0.1f;

    private bool bIsWalking = true;
    private float walkingTime;
    private Vector3 targetCameraPosition;

    bool bInMenu = true;

    void Update()
    {
        if (!bInMenu)
        {
            if (bIsWalking)
            {
                walkingTime += Time.deltaTime;

                targetCameraPosition = headTransform.position + CalculateHeadBob(walkingTime);

                cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetCameraPosition, headBobSmoothing);
            }
            else
            {
                if ((cameraTransform.position - targetCameraPosition).magnitude <= 0.001f)
                {
                    transform.position = targetCameraPosition;
                }
            }
        }
    }

    Vector3 CalculateHeadBob(float time)
    {
        float horizontalOffset = 0.0f;
        float verticalOffset = 0.0f;
        Vector3 offset = Vector3.zero;

        if (time > 0.0f)
        {
            horizontalOffset = Mathf.Cos(time * bobFrequency) * bobHorizontalAmplitude;
            verticalOffset = Mathf.Sin(time * bobFrequency * 2f) * bobVerticalAmplitude;

            offset = transform.right * horizontalOffset + transform.up * verticalOffset;

            print("OFFSET = " + offset);
        }

        return offset;
    }

    public void SetIsWalking(bool isWalking)
    {
        bIsWalking = isWalking;
    }

    public void SetIsInMenu(bool isInMenu)
    {
        bInMenu = isInMenu;
    }
}
