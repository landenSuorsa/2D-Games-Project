using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public GameObject gun;
    public GameObject playerModel;
    Vector2 movement_vector = Vector2.zero;
    public float x_speed = 5f;
    public float jump_force = 5f;
    public float x_max_speed = 5f;
    public float y_max_speed = 5f;
    public float raycast_depth = 0.1f;
    bool jump = false;
    public int maxJumps = 2;
    bool isSliding = false;
    public float slide_duration = 1f;
    public float slide_cooldown = 2f;
    public float slide_speed = 10f;
    float startedSlide = -1f;
    int numJumps;
    bool isShooting = false;
    public float firerate = 1f;
    float lastFired = -1f;
    public float bulletSpeed = 5f;
    public GameObject bullet;
    Vector2 aim_vector = Vector2.zero;
    public GameObject swing;
    bool isSwinging = false;
    public float swingRate = 1f;
    float lastSwing = -1f;
    Rigidbody2D rb;
    BoxCollider2D col;
    Renderer rend;
    Animator animator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numJumps = maxJumps;
        rb = GetComponent<Rigidbody2D>();
        rend = playerModel.GetComponent<Renderer>();
        animator = playerModel.GetComponent<Animator>();
        col = playerModel.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(new Vector2(rb.position.x - rend.bounds.size.x / 2, col.bounds.min.y), new Vector2(rb.position.x - rend.bounds.size.x / 2, col.bounds.min.y - raycast_depth));
        Debug.DrawLine(new Vector2(rb.position.x, col.bounds.min.y), new Vector2(rb.position.x, col.bounds.min.y - raycast_depth));
        Debug.DrawLine(new Vector2(rb.position.x + rend.bounds.size.x / 2, col.bounds.min.y), new Vector2(rb.position.x + rend.bounds.size.x / 2, col.bounds.min.y - raycast_depth));

        gun.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(aim_vector.y, aim_vector.x) * 180 / Mathf.PI);
    } 

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            numJumps = maxJumps;
        }

        if (isSliding)
        {
            if (movement_vector.x > 0)
            {
                rb.linearVelocityX = slide_speed;
            } else if (movement_vector.x < 0)
            {
                rb.linearVelocityX = -slide_speed;
            }
        } 
        else
        {
            rb.linearVelocityX = movement_vector.x * x_speed;

            if (jump)
            {
                rb.linearVelocityY = jump_force;
                numJumps -= 1;
                jump = false;
            }
        }
        
        if (isSwinging)
        {
            int direction = Mathf.RoundToInt(gun.transform.localEulerAngles.z / 90);
            GameObject newSwing;
            Debug.Log(direction);
            switch (direction)
            {
                case 0:
                case 4:
                    newSwing = Instantiate(swing, new Vector2(rb.position.x + rend.bounds.size.x / 2, rb.position.y), Quaternion.Euler(0, 0, direction * 90));
                    newSwing.transform.parent = gameObject.transform;
                    break;
                case 1:
                    newSwing = Instantiate(swing, new Vector2(rb.position.x, rb.position.y + rend.bounds.size.y / 2), Quaternion.Euler(0, 0, direction * 90));
                    newSwing.transform.parent = gameObject.transform;
                    break;
                case 2:
                    newSwing = Instantiate(swing, new Vector2(rb.position.x - rend.bounds.size.x / 2, rb.position.y), Quaternion.Euler(180, 0, direction * 90));
                    newSwing.transform.parent = gameObject.transform;
                    break;
                case 3:
                    if (!IsGrounded()) { 
                        newSwing = Instantiate(swing, new Vector2(rb.position.x, rb.position.y - rend.bounds.size.y / 2), Quaternion.Euler(0, 180, direction * 90));
                        newSwing.transform.parent = gameObject.transform;
                    }
                    break;
                default:
                    break;
            }
            

            isSwinging = false;
        }

        if (isShooting && Time.time - lastFired > firerate)
        {
            GameObject newBullet = Instantiate(bullet, rb.position + (Vector2)gun.transform.localPosition, Quaternion.Euler(0, 0, gun.transform.localEulerAngles.z));
            newBullet.GetComponent<Rigidbody2D>().linearVelocity = (Vector2)newBullet.transform.right * bulletSpeed + rb.linearVelocity / 3;
            lastFired = Time.time;
        } 
    }

    public void OnMove(InputValue input)
    {
        movement_vector = input.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        jump = input.isPressed && numJumps > 0;
        Debug.Log(input.isPressed + " " + numJumps);
    }

    public void OnSlide(InputValue input)
    {
        if (!isSliding && Time.time - startedSlide > slide_cooldown)
        {
            isSliding = input.isPressed;
            startedSlide = Time.time;
            animator.SetBool("Sliding", true);
            Invoke("EndSlide", slide_duration);
        }
    }

    public void OnAim(InputValue input)
    {
        aim_vector = input.Get<Vector2>();
    }

    public void OnShoot(InputValue input)
    {
        isShooting = input.isPressed;
    }

    public void OnSwing(InputValue input)
    {
        if (!isSwinging && Time.time - lastSwing > swingRate)
        {
            isSwinging = input.isPressed;
            lastSwing = Time.time;
        }
    }

    bool IsGrounded()
    {
        RaycastHit2D left = Physics2D.Raycast(new Vector2(rb.position.x - rend.bounds.size.x / 2, col.bounds.min.y), Vector2.down, raycast_depth, 1 << 6);
        RaycastHit2D center = Physics2D.Raycast(new Vector2(rb.position.x, col.bounds.min.y), Vector2.down, raycast_depth, 1 << 6);
        RaycastHit2D right = Physics2D.Raycast(new Vector2(rb.position.x + rend.bounds.size.x / 2, col.bounds.min.y), Vector2.down, raycast_depth, 1 << 6);

        return left.collider != null || center.collider != null || right.collider != null;
    }

    void EndSlide()
    {
        isSliding = false;
        animator.SetBool("Sliding", false);
    }
}
