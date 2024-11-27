using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageAnimator : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float moveUpSpeed = 1f;
    public float enlargeSpeed = 1f;

    private TextMeshPro textMesh;
    private Color originalColor;

    void Start()
    {
        textMesh = GetComponent<TextMeshPro>();
        originalColor = textMesh.color;
    }

    void Update()
    {
        // Move upward
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Enlarge
        transform.localScale += Vector3.one * enlargeSpeed * Time.deltaTime;

        // Fade
        float fade = Mathf.Clamp01(fadeDuration - Time.time);
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, fade);

        if (fade <= 0f) 
            Destroy(gameObject);
    }
}
