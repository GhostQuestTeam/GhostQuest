using HauntedCity.GameMechanics.Main;
using UnityEngine;

public class PlayerBehaviourFromAccelerometer : MonoBehaviour {

    public GameObject cameraContainer;
    public float moveSpeed = 30;

    private GameObject cameraObject;
    private CharacterController characterController;
    private Camera cam;
    private GyroControl gyroControl;
    private AccelerationCleaner acCleaner;

    public AccelerationCleaner.MovementStepData _debug_md_;
    public Vector3 _debug_output_vector_;

    // Use this for initialization
    void Start()
    {
        characterController = cameraContainer.GetComponent<CharacterController>();
        cameraObject = cameraContainer.transform.GetChild(0).gameObject;
        cam = cameraObject.GetComponent<Camera>();
        gyroControl = cameraContainer.GetComponent<GyroControl>();
        acCleaner = cameraContainer.GetComponent<AccelerationCleaner>();
        acCleaner.Init(gyroControl);
    }

    // Update is called once per frame
    void Update()
    {
        moveWithKeys();
        rotateWithKeys();
       //doAccelerometerMovement();
    }

   
    /*
    *************************************************************************************************************************************
    */


    void doAccelerometerMovement()
    {
        if (!gyroControl.gyroEnabled)
        {
            return;
        }

        acCleaner.insertData(gyroControl);

        AccelerationCleaner.MovementStepData md;
        Vector3 userAcc = acCleaner.getFilteredAcceleration(out md);
        _debug_md_ = md;
        _debug_output_vector_ = userAcc;

        Vector3 moveVector;
        moveVector = -cameraObject.transform.forward * userAcc.z * moveSpeed;
        moveVector *= Time.deltaTime;
        moveVector.z = 0;
        characterController.Move(moveVector);
    }


    /*
    *************************************************************************************************************************************
    */

    //for test on PC
    void moveWithKeys()
    {
        float translation = Input.GetAxis("Vertical"); //value of vert axis
        Vector3 moveVector = cameraObject.transform.forward * translation * moveSpeed; //vector of movement
        moveVector *= Time.deltaTime; //expression in time units instead of frame units
        characterController.Move(moveVector);
    }

    //for test on PC
    void rotateWithKeys()
    {
        float rotationSpeed = 100f;
        float rotation = Input.GetAxis("Horizontal");
        rotation *= rotationSpeed * Time.deltaTime;
        cameraObject.transform.Rotate(0, rotation, 0);
    }

}
