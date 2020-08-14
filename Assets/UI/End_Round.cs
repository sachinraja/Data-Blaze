using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class End_Round : MonoBehaviour
{
    [SerializeField] TMP_InputField consoleInputField = null;
    [SerializeField] TMP_Text processedText = null;
    [SerializeField] TMP_Text acceptedText = null;
    [SerializeField] TMP_Text deniedText = null;
    [SerializeField] TMP_Text correctText = null;

    [HideInInspector] public int processedAmount = 0;
    [HideInInspector] public int acceptedAmount = 0;
    [HideInInspector] public int deniedAmount = 0;
    [HideInInspector] public int correctAmount = 0;
    
    public void SetStats()
    {
        consoleInputField.interactable = false;
        processedText.text = "Processed: " + processedAmount.ToString();
        acceptedText.text = "Accepted: " + acceptedAmount.ToString();
        deniedText.text = "Denied: " + deniedAmount.ToString();
        correctText.text = "Correct: " + correctAmount.ToString() + "/" + processedAmount.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
