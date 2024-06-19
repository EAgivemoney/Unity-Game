using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToTitle : MonoBehaviour
{
    public AudioClip controlsSound;
    private AudioSource audioSource;

    public string levelName;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        PlayControlsSound();
    }

    public void loadLevel()
    {
        PlayerController.lives = 5;

        SceneManager.LoadScene(levelName);
    }

    void PlayControlsSound()
    {
        if (controlsSound != null)
        {
            audioSource.PlayOneShot(controlsSound);
        }
    }
}
