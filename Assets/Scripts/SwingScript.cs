using System.Collections;
using UnityEngine;

public class SwingScript : MonoBehaviour
{
    public float lifetime = 0.1f;
    float alpha = 0;
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("FadeIn");
        Invoke("DestroySelf", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }

    IEnumerator FadeIn()
    {
        while (alpha <= 255)
        {
            alpha += 85;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha / 255);
            yield return new WaitForSeconds(lifetime / 5);
        }
        alpha -= 85;
        while (alpha > 0)
        {
            alpha -= 85;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha / 255);
            yield return new WaitForSeconds(lifetime / 5);
        }
    }
}
