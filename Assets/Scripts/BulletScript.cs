using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float bulletLifetime = 3f;
    float startTime = -1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        Invoke("DestroySelf", bulletLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        float bulletSize = Mathf.Min((bulletLifetime - Time.time + startTime) / 3 , 0.4f);
        transform.localScale = new Vector3(bulletSize, bulletSize, 1);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            Destroy(gameObject);
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
