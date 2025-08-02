using UnityEngine;

public class enemyFollow : MonoBehaviour
{
    public float speed = 5f;
    public GameObject Player;
    private float distance;
    public float range;
    private Rigidbody2D rb;

    private Vector2 currentVelocity = Vector2.zero;
    [SerializeField] private float smoothTime = 0.2f;

    private float patrolTimer = 0f;
    private int patrolDirection = 1; // 1 for right, -1 for left
    public float patrolDuration = 2f;

    private float delayTimer = 0f;
    public float delayDuration = 1f;
    private SpriteRenderer sp;
    private bool isPlayerInRange = false;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        // Remove freeze position Y constraint to allow movement on y-axis

    }
    

    void Update()
    {

        distance = Vector2.Distance(transform.position, Player.transform.position);
        Vector2 targetVelocity = Vector2.zero;

        if (distance < range)
        {
            isPlayerInRange = true;
            Vector2 direction = Player.transform.position - transform.position;
            direction.Normalize();
            targetVelocity = direction * speed*2;
            // Smoothly change velocity towards target velocity
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
            patrolTimer = 0f; // reset patrol timer when following player
            delayTimer = 0f; // reset delay timer
            // Disable patrol when player detected
            patrolDirection = 0;
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            if (isPlayerInRange)
            {
                // Player just left range, start delay timer
                delayTimer += Time.deltaTime;
                if (delayTimer >= delayDuration)
                {
                    isPlayerInRange = false;
                    delayTimer = 0f;
                    patrolDirection = 1; // reset patrol direction to right
                    patrolTimer = 0f;
                    rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                }
                else
                {
                    // During delay, stop moving
                    rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, Vector2.zero, ref currentVelocity, smoothTime);
                    // Set flipX based on last movement direction when stopping
                    if (rb.linearVelocity.x != 0)
                    {
                        sp.flipX = rb.linearVelocity.x > 0;
                    }
                    return;
                }
            }

            // Patrol behavior: move right for patrolDuration seconds, then left for patrolDuration seconds
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolDuration)
            {
                patrolDirection *= -1;
                patrolTimer = 0f;
            }
            targetVelocity = new Vector2(patrolDirection * speed, 0);
            rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
        }


        // Set flipX based on movement direction (facing direction)
        // If moving right (positive x), flipX should be true (facing right)
        // If moving left (negative x), flipX should be false (facing left)
        if (targetVelocity.x != 0)
        {
            sp.flipX = targetVelocity.x > 0;
        }

    }
    private void AnimationRun()
    {
        // Animation run logic here
        animator.SetBool("Run", true);
    }
}
