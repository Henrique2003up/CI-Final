using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string SceneName;

    public void ChangeSceneAction()
    {
        SceneManager.LoadScene(SceneName);
    }
    public void UnloadLoseScene()
    {
        SceneManager.UnloadSceneAsync("LoseScene");
    }
}