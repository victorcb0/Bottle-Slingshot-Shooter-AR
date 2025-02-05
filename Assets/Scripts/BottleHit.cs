using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    // Referință la obiectul `bottlehit`
    private AudioSource bottleHitAudio;

    private void Start()
    {
        // Găsește obiectul `bottlehit` în scenă și ia componenta AudioSource
        GameObject bottleHitObject = GameObject.Find("BottleHit");
        if (bottleHitObject != null)
        {
            bottleHitAudio = bottleHitObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("Obiectul 'BottleHit' nu a fost găsit în scenă!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Redă sunetul dacă AudioSource este valid
        if (bottleHitAudio != null)
        {
            bottleHitAudio.Play();
        }
    }
}
