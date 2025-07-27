using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadStarter : MonoBehaviour
{
    public string targetSceneName = "TargetScene";

    public void LoadScene()
    {
        Debug.Log("Clickec");
        LoadingData.nextSceneName = targetSceneName;
        SceneManager.LoadScene("Loading");
    }
}
