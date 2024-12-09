using UnityEngine;

public class DamageAnimator : MonoBehaviour
{
    // Speed of upward motion
    public float moveUpSpeed = 0.5f;
    // Speed of enlarging effect
    public float enlargeSpeed = 0.1f;
    // How long the text stays visible before disappearing
    public float lifetime = 2f;
    // Tracks how long the object has existed
    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Move upward
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Enlarge
        transform.localScale += Vector3.one * enlargeSpeed * Time.deltaTime;

        // Destroy the object after the set lifetime
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
