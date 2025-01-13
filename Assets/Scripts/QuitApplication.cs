using UnityEngine;

public class QuitApplication : MonoBehaviour
{
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
