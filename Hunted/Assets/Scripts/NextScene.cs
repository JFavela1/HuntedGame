using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] int sceneNumber;
    [SerializeField] int restart;

    public void Scene()
    {
        SceneManager.LoadSceneAsync(sceneNumber);
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(restart);
    }
}
