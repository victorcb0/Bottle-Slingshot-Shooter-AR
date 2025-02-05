using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Adăugat pentru TextMeshPro

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText; // Referință la TextMeshPro pentru scor
    [SerializeField] private GameObject scoreObject;   // Obiectul empty "Score"

    private void Start()
    {
        // Verificăm dacă există un scor salvat
        if (!string.IsNullOrEmpty(Timer.FinalScore))
        {
            // Actualizăm textul cu valoarea scorului transmis
            if (scoreText != null)
            {
                scoreText.text = $"{Timer.FinalScore}";
            }

            // Activăm obiectul gol "Score"
            if (scoreObject != null)
            {
                scoreObject.SetActive(true);
            }
        }
        else
        {
            // Dacă nu există un scor, ascundem obiectul "Score"
            if (scoreObject != null)
            {
                scoreObject.SetActive(false);
            }
        }
    }

    public void PlayGame()
    {
        // Începem jocul și încărcăm următoarea scenă (scena 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        #if UNITY_ANDROID
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("finish");
            }
        #else
            Application.Quit();
        #endif
    }
}
