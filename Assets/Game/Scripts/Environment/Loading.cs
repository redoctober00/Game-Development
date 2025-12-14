
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LoadingScreen;



    public void LoadScreen(int sceneID)
    {
        StartCoroutine(LoadSceneAsync(sceneID));
     

    }
    IEnumerator LoadSceneAsync(int sceneID)
    {
     
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

        LoadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");
            yield return null;
        }
    }
}

