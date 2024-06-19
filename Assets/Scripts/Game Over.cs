using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public AudioClip gameOverSound;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameOverSound;
        audioSource.Play();

        StartCoroutine(LoadTitleSceneAfterDelay(2.5f));
    }

    IEnumerator LoadTitleSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("Title");
    }
}
