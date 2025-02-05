using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    // Numele tag-ului pe care îl are obiectul ce trebuie distrus la impact
    public string targetTag = "target";
    public string targetTag2 = "real";

    private void OnCollisionEnter(Collision collision)
    {
        // Verificăm dacă obiectul cu care sfera a intrat în coliziune are tag-ul specificat
        if (collision.gameObject.CompareTag(targetTag))
        {
            // Distruge obiectul atins
            Destroy(collision.gameObject);

            // Distruge și sfera (obiectul curent)
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag(targetTag2))
        {
            // Distruge și sfera (obiectul curent)
            Destroy(gameObject);
        }
        
    }
}
