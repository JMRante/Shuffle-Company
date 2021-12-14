using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public abstract class Goal : MonoBehaviour
{
    public string nextSceneName;

    public GameObject blackScreen;

    public abstract bool IsGoalMet();

    void Update()
    {
        if (IsGoalMet() || Input.GetKeyDown(KeyCode.N))
        {
            GoToNextStage();
        }
    }

    protected void GoToNextStage()
    {
        if (blackScreen != null)
        {
            blackScreen.SetActive(true);
        }

        if (nextSceneName == "EXIT")
        {
            Application.Quit();
        }
        else
        {
            StartCoroutine(LoadSceneAsync(nextSceneName));
        }
    }

    IEnumerator LoadSceneAsync(string nextSceneName)
    {
        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

