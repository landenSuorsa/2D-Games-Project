using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogScript : MonoBehaviour
{
    Tilemap tilemap;
    float alpha = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            StopAllCoroutines();
            StartCoroutine(Clear());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(Cover());
        }
    }

    IEnumerator Clear()
    {
        while (alpha > 0)
        {
            alpha -= 0.01f;
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, Mathf.Max(0, alpha));
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Cover()
    {
        while (alpha < 1)
        {
            alpha += 0.01f;
            tilemap.color = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, Mathf.Min(1, alpha));
            yield return new WaitForSeconds(0.01f);
        }
    }
}
