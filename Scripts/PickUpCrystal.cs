using UnityEngine;

public class PickUpCrystal : MonoBehaviour
{
    public GameObject button;
    private void Start()
    {
        button.SetActive(false);
    }
    void OnEnable()
    { 
        button.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            button.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
