using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private float levelLoadDelay = 1f;
    [SerializeField] private AudioClip successSound;
    [SerializeField] private AudioClip crashSound;
    
    private AudioSource _audioSource;

    private bool _isControllable = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_isControllable) return;
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
        _isControllable = false;
        GetComponent<PlayerMovement>().enabled = false;
        _audioSource.PlayOneShot(successSound);
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }
    private void StartCrashSequence()
    {
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
}
