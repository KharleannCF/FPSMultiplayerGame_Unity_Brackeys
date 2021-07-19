using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player_Motor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private float cameraRotationX = 0;
    private float currentCameraRotationX = 0;
    private Vector3 thrusterForce = Vector3.zero;

    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    public void Move (Vector3 _velocity){
        velocity = _velocity;
    }
    
    public void rotate (Vector3 _rotation){
        rotation = _rotation;
    }

    public void rotateCamera (float _rotationX){
        cameraRotationX = _rotationX;
    }

    public void applyThruster (Vector3 _thrusterForce){
        thrusterForce = _thrusterForce;
    }

    // Update is called once per frame
    //Run every physics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
    void PerformMovement(){
        if (velocity != Vector3.zero){
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        if (thrusterForce != Vector3.zero){
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }
    void PerformRotation(){
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (cam != null){
            //cam.transform.Rotate(-cameraRotationX);
            //now rotation calc
            currentCameraRotationX-=cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
         }

    }
    
}
