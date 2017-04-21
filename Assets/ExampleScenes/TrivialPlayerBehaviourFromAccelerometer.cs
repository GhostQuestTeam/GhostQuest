using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrivialPlayerBehaviourFromAccelerometer : MonoBehaviour {

    public GameObject cameraContainer;
    public float moveSpeed = 30;

    private GameObject cameraObject;
    private CharacterController characterController;
    private Camera cam;
    private GyroControl gyroControl;
    
    void Start()
    {
        characterController = cameraContainer.GetComponent<CharacterController>();
        cameraObject = cameraContainer.transform.GetChild(0).gameObject;
        cam = cameraObject.GetComponent<Camera>();
        gyroControl = cameraContainer.GetComponent<GyroControl>();
    }
    

    void Update()
    {
        doAccelerometerMovement();
    }


    void doAccelerometerMovement()
    {
        if (!gyroControl.gyroEnabled)
        {
            return;
        }

        Vector3 userAcc = gyroControl.gyro.userAcceleration;
        Vector3 moveVector = cameraObject.transform.forward * userAcc.magnitude * moveSpeed;
        moveVector *= Time.deltaTime;
        characterController.Move(moveVector);
    }


}
