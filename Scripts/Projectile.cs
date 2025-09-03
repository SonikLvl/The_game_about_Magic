using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 3f;
    new Rigidbody2D rigidbody2D;

    public void Fire(Vector2 targetPos)
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;
        rigidbody2D.linearVelocity = direction * speed;
    }

    // void Update()
    // {
    //     if (fraction < 1 && targetPos != null)
    //     {
    //         fraction += Time.deltaTime * speed;
    //         transform.position = Vector3.Lerp(gameObject.GetComponentInParent<Transform>().position, targetPos, fraction);
    //     }
    // }
    void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<playerController>().ChangeHealth(-1);
        }
        Destroy(gameObject);
    }
}
