using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadTitleSceneAfterDelay(2.5f));
    }

    IEnumerator LoadTitleSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Title");
    }
}
