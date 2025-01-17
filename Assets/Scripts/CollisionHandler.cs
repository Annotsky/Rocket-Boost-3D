using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
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
    private bool _isCollidable = true;
    private bool _isCrashable = true;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isControllable || !_isCollidable) return;
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
        StartCoroutine(LoadNextLevelAfterDelay(levelLoadDelay));
    }
    private void StartCrashSequence()
    {
        explosionParticles.Play();
        _isControllable = false;
        _audioSource.Stop();
        GetComponent<PlayerMovement>().enabled = false;
        _audioSource.PlayOneShot(crashSound);
        StartCoroutine(ReloadLevelAfterDelay(levelLoadDelay));
    }
    
    private IEnumerator ReloadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    
    private IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

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
            StartCoroutine(LoadNextLevelAfterDelay(0));
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollidable = !_isCollidable;
            Debug.Log("Collision: " + _isCollidable);
        }
    }
}


