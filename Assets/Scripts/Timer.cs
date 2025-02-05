using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private Text uiText;
    [SerializeField] private TextMeshProUGUI scoreText; // TextMeshPro pentru scor

    public int Duration;

    private int remainingDuration;
    private bool pause;
    private bool hasStarted;

    public static string FinalScore { get; private set; } // Static pentru transferul scorului între scene

    private void Start()
    {
    }

    public void StartTimer()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            Begin(Duration);
        }
    }

    private void Begin(int second)
    {
        remainingDuration = second;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration >= 0)
        {
            if (!pause)
            {
                uiText.text = $"{remainingDuration / 60:00}:{remainingDuration % 60:00}";
                uiFill.fillAmount = Mathf.InverseLerp(0, Duration, remainingDuration);
                remainingDuration--;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                yield return null;
            }
        }
        OnEnd();
    }

    private void OnEnd()
    {
        // Salvăm valoarea scorului
        FinalScore = scoreText.text;

        // Încărcăm scena 0
        SceneManager.LoadScene(0);

        Debug.Log($"Timpul s-a terminat. Scor final: {FinalScore}");
    }
}
