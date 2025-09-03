using System.Collections;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Disposable")]
    public bool destroyable;
    public bool hideable;
    public int health = 1;
    int maxHealth;

    [Header("Flame and water")]
    public bool isFire;
    public bool flamable;
    public bool imuneToFlame;
    public Sprite noflameSprite;
    Sprite flameSprite;
    public bool inFlames;
    bool startedInFlames;
    public ParticleSystem flameParticles;
    [Header("Boss")]

    public bool bossDamage;
    public bool playerDamage = true;

    float fireTimer = 3.5f;
    int counter = 3;
    ParticleSystem instantiatedEffect;
    SpriteRenderer spriteRenderer;


    void Start()
    {
        startedInFlames = inFlames;
        maxHealth = health;
        if (flameParticles != null) instantiatedEffect = Instantiate(flameParticles, transform);
        if (!inFlames && instantiatedEffect != null) instantiatedEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sR))
        {
            spriteRenderer = sR;
            flameSprite = spriteRenderer.sprite;
        }
    }
    private void Update()
    {
        if (inFlames && !imuneToFlame) //deals 6 total dmg
        {
            fireTimer -= Time.deltaTime;


            if (fireTimer < 0 && counter != 0)
            {
                counter -= 1;
                fireTimer = 3.5f;
                TakeDamage(3 - counter);
                if (counter == 0)
                {
                    inFlames = false;
                    counter = 3;
                    instantiatedEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }

            }
        }
    }

    public void PerformAction(LineType line)
    {
        switch (line)
        {
            case LineType.flame:
                if (flamable)
                {
                    Debug.Log("Set in flames " + name);
                    inFlames = true;
                    instantiatedEffect.Play();
                    var m = instantiatedEffect.main;
                    m.loop = true;
                    if (imuneToFlame) spriteRenderer.sprite = flameSprite;
                }
                else
                {
                    Debug.Log("Not flamable " + name);
                }
                break;
            case LineType.water:
                if (isFire)
                {
                    Debug.Log("Extinguished " + name);
                    TakeDamage();
                }else if (inFlames)
                {
                    Debug.Log("Extinguished " + name);
                    inFlames = false;
                    instantiatedEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    if (imuneToFlame) spriteRenderer.sprite = noflameSprite;
                }
                else
                {
                    Debug.Log("Watered " + name);
                }
                break;
            case LineType.attack:
                Debug.Log("Attacked " + name);
                if (!inFlames && !isFire)
                    TakeDamage();
                break;
            default:
                Debug.Log("No action performed by interactable object " + this.name);
                break;
        }
    }

    void TakeDamage(int damage = 1)
    {
        health -= damage;
        if (gameObject.GetComponent<Animator>() != null) gameObject.GetComponent<Animator>().SetTrigger("hit");
        if (health <= 0)
        {
            if (hideable)
            {
                //Debug.Log("Hidden " + name);
                inFlames = startedInFlames;
                //instantiatedEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                if (inFlames)
                {
                    instantiatedEffect.Play();
                    var n = instantiatedEffect.main;
                    n.loop = true;
                    if (imuneToFlame) spriteRenderer.sprite = flameSprite;
                }
                var m = instantiatedEffect.main;
                m.loop = false;

                health = maxHealth;
                this.transform.gameObject.SetActive(false);
                
            }
            else if (destroyable)
            {
                //Debug.Log("Destroyed " + name);
                if (transform.tag != "Projectile") Boss.instance.EnemyDestroyed(transform.position);
                if (bossDamage) Boss.instance.Damage();
                Destroy(this.transform.gameObject);
            }
            else
            {
                Debug.Log("Nothing happened to " + name);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (inFlames)
            {
                collision.transform.gameObject.GetComponent<playerController>().inFlames = true;
                collision.transform.gameObject.GetComponent<playerController>().flameParticles.Play();
            }
            else if (playerDamage)
                collision.gameObject.GetComponent<playerController>().ChangeHealth(-1);
        }
    }

}
