using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float min_x = 0;
    public float max_x = 0;
    public float min_y = 0;
    public float max_y = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
