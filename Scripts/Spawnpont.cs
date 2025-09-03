using UnityEngine;

public class Spawnpont : MonoBehaviour
{
    public int sectionNumber = 2;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerController.instance.ChangeHealth(6);
            GameOver.instance.spawn = (this.transform.position, sectionNumber);
            this.gameObject.SetActive(false);
        }
    }
}
