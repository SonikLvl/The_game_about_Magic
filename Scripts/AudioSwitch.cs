using System.Collections;
using UnityEngine;

public class AudioSwitch : MonoBehaviour
{
    public AudioSource current;
    public AudioSource changeTo;
    public GameObject enableComponent;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            enableComponent.SetActive(true);
            enableComponent.SetActive(true);
            current.volume = 0;
            current.Stop();

            changeTo.Play();
            changeTo.volume = 1;
            gameObject.SetActive(false);

        }
    }

    public void ChangeMusicForRestart()
    {
        enableComponent.SetActive(true);
        current.volume = 0;
        current.Stop();

        changeTo.Play();
        changeTo.volume = 1;
    }
}
