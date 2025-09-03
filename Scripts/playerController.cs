using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public static playerController instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    public bool INVINSIBLE;
    public float speed = 3.0f;
    public int maxHealth = 6;
    public float timeInvincible = 2.0f;

    public float dashSpeed = 15.0f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    [HideInInspector] public bool inFlames = false;
    float fireTimer = 3.5f;
    int counter = 3;
    public ParticleSystem flameParticlesPrefab;
    [HideInInspector] public ParticleSystem flameParticles;


    public int currentHealth;
    public List<Animator> HealthUIAnim;

    bool isInvincible;
    float invincibleTimer;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    public Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flameParticles = Instantiate(flameParticlesPrefab, transform);
        flameParticles.Stop();
        currentHealth = maxHealth;

    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDirection.Set(move.x, 0);
            lookDirection.Normalize();
        }

        animator.SetFloat("look x", lookDirection.x);
        animator.SetFloat("speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            {
                isInvincible = false;
            }
        }

        if (inFlames) //deals 6 total dmg
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer < 0 && counter != 0)
            {
                counter -= 1;
                fireTimer = 3.5f;
                ChangeHealth(-1);
                if (counter == 0)
                {
                    inFlames = false;
                    counter = 3;
                }
            }
        }
        else flameParticles.Stop();
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        float originalSpeed = speed;
        speed = dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        speed = originalSpeed;
        isDashing = false;
    }

    public void ChangeHealth(int amount)
    {
        if (INVINSIBLE) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth + "/" + amount);
        if (amount <= 0)
        {
            animator.SetTrigger("hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            for (int i = currentHealth; i < HealthUIAnim.Count; i++)
            {
                HealthUIAnim[i].ResetTrigger("Reset");
                HealthUIAnim[i].SetTrigger("Heart");
            }
        }
        else
        {
            inFlames = false;
            for (int i = 0; i < HealthUIAnim.Count; i++)
            {
                HealthUIAnim[i].ResetTrigger("Heart");
                HealthUIAnim[i].SetTrigger("Reset");
            }
        }
            
        if (currentHealth <= 0) GameOver.instance.GameOverScreen();
    }


    
}
