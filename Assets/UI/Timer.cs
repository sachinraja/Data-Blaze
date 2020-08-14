using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] End_Round Stats;
    [SerializeField] TMP_Text timerText = null;
    private float timeCounter = 10;
    private bool timerRunning = true;

    private void Update()
    {
        if (timerRunning)
        {
            if (timeCounter > 0)
            {
                timeCounter -= Time.deltaTime;
                int minutes = Mathf.FloorToInt(timeCounter / 60);
                int seconds = Mathf.FloorToInt(timeCounter % 60);

                if (seconds < 10)
                {
                    timerText.text = minutes + ":0" + seconds;
                }

                else
                {
                    timerText.text = minutes + ":" + seconds;
                }
            }

            else
            {
                timeCounter = 0;
                timerText.text = "0:00";
                Debug.Log("Time is over!");
                timerRunning = false;

                Stats.SetStats();
                Stats.gameObject.SetActive(true);
            }
        }
    }
}
