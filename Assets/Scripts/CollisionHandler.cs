using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;
    [Header("Sounds")]
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip crashSound;
    [Header("Particles")]
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private ParticleSystem successParticles;
    
    private AudioSource _audioSource;

    private bool _isControllable = true;
    private bool _isCollideble = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isControllable || !_isCollideble) return;
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        successParticles.Play();
        _isControllable = false;
        GetComponent<PlayerMovement>().enabled = false;
        _audioSource.PlayOneShot(successSound);
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }
    private void StartCrashSequence()
    {
        explosionParticles.Play();
        _isControllable = false;
        _audioSource.Stop();
        GetComponent<PlayerMovement>().enabled = false;
        _audioSource.PlayOneShot(crashSound);
        Invoke(nameof(ReloadLevel), levelLoadDelay);
    }
    
    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    
    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollideble = !_isCollideble;
            Debug.Log("Collision: " + _isCollideble);
        }
    }
}


