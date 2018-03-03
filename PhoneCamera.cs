using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class PhoneCamera : MonoBehaviour {

    private bool flashState = false; 
    // Use this for initialization
    void Start()
    {
        VuforiaARController vuforia = VuforiaARController.Instance;

    }

    public void ToggleCam()
    {
        // turn off one camera
        Vuforia.CameraDevice.Instance.Stop();
        Vuforia.CameraDevice.Instance.Deinit();

        // To rotate the image in the back so that it still be there ;) 
        GameObject.Find("FBImage").transform.Rotate(Vector3.up, 180);
        GameObject.Find("FBImage").transform.Translate(Vector3.left * 10); 

        // turn on another camera
        Vuforia.CameraDevice.Instance.Init(getNextCamera());
        Vuforia.CameraDevice.Instance.Start();
    }

    private Vuforia.CameraDevice.CameraDirection getNextCamera()
    {
        // decide which camera to turn on
        switch (Vuforia.CameraDevice.Instance.GetCameraDirection())
        {
            case Vuforia.CameraDevice.CameraDirection.CAMERA_BACK:
            case Vuforia.CameraDevice.CameraDirection.CAMERA_DEFAULT:
            default:
                return Vuforia.CameraDevice.CameraDirection.CAMERA_FRONT;
            case Vuforia.CameraDevice.CameraDirection.CAMERA_FRONT:
                return Vuforia.CameraDevice.CameraDirection.CAMERA_BACK;
        }
    }

    
    public void ToggleFlash()
    {
        if (!flashState)
        {
            CameraDevice.Instance.SetFlashTorchMode(true);
            flashState = true; 
        }
        else
        {
            CameraDevice.Instance.SetFlashTorchMode(false);
            flashState = false; 
        }
      
    }



}
