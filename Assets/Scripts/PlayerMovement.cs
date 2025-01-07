using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputAction thrust;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float thrustSpeed = 2f;
    [SerializeField] private float rotationSpeed = 2f;
    
    private Rigidbody _rigidbody;
    
    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            _rigidbody.AddRelativeForce(Vector3.up * (thrustSpeed * Time.fixedDeltaTime));
        }
    }
    
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            ApplyRotation(rotationSpeed);
        }
        else if (rotationInput > 0)
        {
            ApplyRotation(-rotationSpeed);
        }
    }

    private void ApplyRotation(float plusOrMinusRotationSpeed)
    {
        transform.Rotate(Vector3.forward * (plusOrMinusRotationSpeed * Time.fixedDeltaTime));
    }
}
