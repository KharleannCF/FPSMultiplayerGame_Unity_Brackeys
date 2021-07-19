using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Player_Motor))]
[RequireComponent(typeof(Animator))]
public class Player_Controller : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float Mouse_Sensitivity = 7f;
    [SerializeField]
    private float thrusterForce = 100f;
    [SerializeField]
    private float thrusterFuelBurnSpeed = 0.9f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.5f;
    private float thrusterFuelAmount = 1f;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Spring Settings")]
    //[SerializeField]
    //private JointDriveMode jointMode =  JointDriveMode.Position;
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;


    private Player_Motor motor;
    private ConfigurableJoint joint;
    private Animator animator;

    void Start()
    {
        motor = GetComponent<Player_Motor>();
        joint = GetComponent<ConfigurableJoint>();
        animator = GetComponent<Animator>();
        SetJointSettings(jointSpring);
    }
    void Update()
    {
        //Setting position for spring
        //This makes the physics work properly when applying gravity flying over objects
        if (Pause_Menu.isOn)
        {
            if (Cursor.lockState != CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            motor.Move(Vector3.zero);
            motor.rotate(Vector3.zero);
            motor.rotateCamera(0);
            return;
        }
        else
        {


            if (Cursor.lockState != CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            RaycastHit _hit;
            if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask))
            {
                joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
            }
            else
            {
                joint.targetPosition = new Vector3(0f, 0f, 0f);
            }

            //Calculate movement as a 3D vector
            float _xMov = Input.GetAxis("Horizontal");
            float _zMov = Input.GetAxis("Vertical");

            Vector3 movHorizontal = transform.right * _xMov; // (x,0,0)
            Vector3 movVertical = transform.forward * _zMov; // (0,0,z)

            //Movement vector
            Vector3 velocity = (movHorizontal + movVertical) * speed;

            //animate movement
            animator.SetFloat("ForwardVelocity", _zMov);
            //Apply movement
            motor.Move(velocity);

            //Calculate rotation as 3DV (Turning Arround)
            float _yRot = Input.GetAxisRaw("Mouse X");

            Vector3 _rotation = new Vector3(0f, _yRot * Mouse_Sensitivity, 0f);

            // Apply Rotation
            motor.rotate(_rotation);

            //Calculate rotation as 3DV (Turning Arround)
            float _xRot = Input.GetAxisRaw("Mouse Y");

            //Vector3 _CameraRotation = new Vector3(_xRot * Mouse_Sensitivity, 0f , 0f);
            float _CameraRotationX = _xRot * Mouse_Sensitivity;

            // Apply Rotation
            motor.rotateCamera(_CameraRotationX);

            Vector3 _thrusterForce = Vector3.zero;
            if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
            {
                thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;
                //Calculate the force based on input
                if (thrusterFuelAmount >= 0.01f)
                {

                    _thrusterForce = Vector3.up * thrusterForce;
                    SetJointSettings(0f);
                }
            }
            else
            {
                thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
                SetJointSettings(jointSpring);
            }

            thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);
            //Apply thruster force
            motor.applyThruster(_thrusterForce);

        }
    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            //mode = jointMode,
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }
}
