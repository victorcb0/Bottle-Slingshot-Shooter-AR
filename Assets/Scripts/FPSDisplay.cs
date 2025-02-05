using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;
    private float deltaTime = 0.0f;

    void Update()
    {
        // Calculează timpul dintre cadre
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        // Calculează FPS-ul
        float fps = 1.0f / deltaTime;

        // Actualizează textul pentru FPS
        fpsText.text = string.Format("FPS: {0:0.}", fps);

        // Schimbă culoarea textului în funcție de intervalul de FPS
        if (fps >= 25)
        {
            fpsText.color = Color.green;   // peste 25 FPS
        }
        else if (fps >= 15)
        {
            fpsText.color = Color.yellow;  // între 15 și 25 FPS
        }
        else
        {
            fpsText.color = Color.red;     // sub 15 FPS
        }
    }
}