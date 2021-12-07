using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class Goal : MonoBehaviour
{
    public string nextSceneName;

    public abstract bool IsGoalMet();

    void Update()
    {
        if (IsGoalMet())
        {
            GoToNextStage();
        }
    }

    protected void GoToNextStage()
    {
        if (nextSceneName == "EXIT")
        {
            Application.Quit();
        }

        StartCoroutine(LoadSceneAsync(nextSceneName));
    }

    IEnumerator LoadSceneAsync(string nextSceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

