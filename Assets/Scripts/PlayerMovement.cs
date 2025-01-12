using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputAction thrust;
    [SerializeField] private float thrustSpeed = 2f;
    [SerializeField] private InputAction rotation;
    [SerializeField] private float rotationSpeed = 2f;
    [Header("Audio")]
    [SerializeField] private AudioClip mainEngineSound;
    [Header("Particles")]
    [SerializeField] private ParticleSystem mainEngineParticles;
    [SerializeField] private ParticleSystem leftEngineParticles;
    [SerializeField] private ParticleSystem rightEngineParticles;
    
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
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        _rigidbody.AddRelativeForce(Vector3.up * (thrustSpeed * Time.fixedDeltaTime));
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(mainEngineSound);
        }

        if (!mainEngineParticles.isPlaying)
        {
            mainEngineParticles.Play();
        }
    }
    private void StopThrusting()
    {
        _audioSource.Stop();
        mainEngineParticles.Stop();
    }

    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        switch (rotationInput)
        {
            case < 0:
                RotateLeft();
                break;
            case > 0:
                RotateRight();
                break;
            default:
                StopRotation();
                break;
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        if (leftEngineParticles.isPlaying) return;
        rightEngineParticles.Stop();
        leftEngineParticles.Play();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        if (rightEngineParticles.isPlaying) return;
        leftEngineParticles.Stop();
        rightEngineParticles.Play();
    }
    
    private void StopRotation()
    {
        leftEngineParticles.Stop();
        rightEngineParticles.Stop();
    }

    private void ApplyRotation(float plusOrMinusRotationSpeed)
    {
        _rigidbody.freezeRotation = true;
        transform.Rotate(Vector3.forward * (plusOrMinusRotationSpeed * Time.fixedDeltaTime));
        _rigidbody.freezeRotation = false;
    }
}
