using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    // Variabilă statică în care reținem timpul ultimei coliziuni
    // (se aplică pentru toate obiectele de tipul SpawnedObject)
    private static float lastCollisionTime = 0f;

    // Setările scorului
    private float maxScore = 100f;        // Scor maxim
    private float minScore = 10f;         // Scor minim

    // Pragurile de timp
    private float timeForMaxScore = 1f;   // Coliziune la sub 1 secundă față de ultima coliziune -> scor maxim
    private float timeForMinScore = 3f;   // Coliziune la peste 3 secunde -> scor minim

    void OnCollisionEnter(Collision collision)
    {
        float currentTime = Time.time;

        // Calculăm diferența de timp față de ultima coliziune
        float timeDifference = currentTime - lastCollisionTime;

        int scoreToAdd;

        // 1) Dacă noua coliziune are loc la sub 1 secundă de la ultima coliziune -> scor maxim
        if (timeDifference <= timeForMaxScore)
        {
            scoreToAdd = Mathf.RoundToInt(maxScore);
        }
        // 2) Dacă noua coliziune are loc după mai mult de 3 secunde de la ultima coliziune -> scor minim
        else if (timeDifference >= timeForMinScore)
        {
            scoreToAdd = Mathf.RoundToInt(minScore);
        }
        // 3) Dacă se află între 1 și 3 secunde -> scor scade proporțional (interpolare liniară)
        else
        {
            // la 1s => factor 0, la 3s => factor 1
            float t = (timeDifference - timeForMaxScore) / (timeForMinScore - timeForMaxScore);
            float interpolatedScore = Mathf.Lerp(maxScore, minScore, t);
            scoreToAdd = Mathf.RoundToInt(interpolatedScore);
        }

        // Actualizăm timpul ultimei coliziuni pentru următoarea interacțiune
        lastCollisionTime = currentTime;

        // Trimitem scorul către un ScoreManager
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.AddScore(scoreToAdd);
        }

        // Distrugem obiectul după coliziune
        Destroy(gameObject);
    }
}
