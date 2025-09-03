using Unity.VisualScripting;
using UnityEngine;

public class ProjectileEverywhere: MonoBehaviour
{
    public GameObject projectilePrefab;
    float offset = 0f;
    public float time = 1f;
    float aimRadius;
    float timer;

    bool readyToGenerate = true;
    private void Start()
    {
        timer = time;
        CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            aimRadius = circleCollider.radius;
        }
        else
        {
            // Fallback value if no CircleCollider2D is found.
            Debug.LogWarning("No CircleCollider2D found on the GameObject. Setting aimRadius to a default value.");
            aimRadius = 10f; 
        }
    }
    private void Update()
    {
        if (!readyToGenerate)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                readyToGenerate = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && readyToGenerate)
        {
            timer = time;
            readyToGenerate = false;
            offset = offset == 10f? 0f:10f;

            Vector3 parentPos = gameObject.GetComponentInParent<Transform>().position;

            // Loop six times to create six projectiles.
            for (int i = 0; i < 6; i++)
            {
                // 360 degrees divided by 6 projectiles gives us a 60-degree spread.
                float angle = i * 60f * Mathf.Deg2Rad;

                Vector3 targetPos = new Vector3(
                    parentPos.x + aimRadius * Mathf.Cos(angle+offset),
                    parentPos.y + aimRadius * Mathf.Sin(angle+offset),
                    parentPos.z
                );
                if (!GetComponentInParent<Interactable>().inFlames) projectilePrefab.GetComponent<Interactable>().inFlames = false;
                else projectilePrefab.GetComponent<Interactable>().inFlames = true;
                var projectile = Instantiate(projectilePrefab, parentPos, Quaternion.identity);

                projectile.GetComponent<Projectile>().Fire(targetPos);
            }
        }
    }
}
