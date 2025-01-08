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
    private AudioSource _audioSource;
    
    private void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
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
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            _audioSource.Stop();
        }
    }
    
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        switch (rotationInput)
        {
            case < 0:
                ApplyRotation(rotationSpeed);
                break;
            case > 0:
                ApplyRotation(-rotationSpeed);
                break;
        }
    }

    private void ApplyRotation(float plusOrMinusRotationSpeed)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * (plusOrMinusRotationSpeed * Time.fixedDeltaTime));
        _rigidbody.freezeRotation = false;
    }
}
