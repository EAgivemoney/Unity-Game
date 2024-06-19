using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    void Start()
    {
        if (livesText == null)
        {
            Debug.LogError("Lives Text is not assigned in the LivesDisplay script.");
        }
    }

    void Update()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + PlayerController.lives.ToString();
        }
        else
        {
            Debug.LogError("Lives Text is null in the Update method.");
        }
    }
}