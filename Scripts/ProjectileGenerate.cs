using Unity.VisualScripting;
using UnityEngine;

public class ProjectileGenerate : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float time = 1f;
    float timer;

    bool readyToGenarate = true;
    private void Start() {
        timer = time;
    }
    private void Update()
    {
        if (!readyToGenarate)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                readyToGenarate = true;
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && readyToGenarate)
        {
            timer = time;
            readyToGenarate = false;
            if (!GetComponentInParent<Interactable>().inFlames) projectilePrefab.GetComponent<Interactable>().inFlames = false;
                else projectilePrefab.GetComponent<Interactable>().inFlames = true;
            var projectile = Instantiate(projectilePrefab, gameObject.GetComponentInParent<Transform>().position,Quaternion.identity);
            projectile.GetComponent<Projectile>().Fire(collision.transform.position);
        }
    }
}
