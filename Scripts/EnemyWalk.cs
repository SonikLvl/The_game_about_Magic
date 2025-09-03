using UnityEngine;

public class EnemyWalk : MonoBehaviour
{

    public float speed = 3.0f;
    public int harm = -1;
    public bool vertical;


    Rigidbody2D rigidbody2d;
    Animator animator;
    int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y += speed * Time.deltaTime * direction;
            if(animator != null) animator.SetFloat("direction", 0);
        }
        else
        {
            position.x += speed * Time.deltaTime * direction;
            if(animator != null) animator.SetFloat("direction", direction);

        }
        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            other.gameObject.GetComponent<playerController>().ChangeHealth(harm);
            
        }
        direction = -direction;
    }

}
