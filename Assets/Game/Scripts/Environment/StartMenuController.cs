
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void onStart()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void onExit() {
 #if UNITY_Editor
        Unity.Editor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
}
