using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam;
    public GameObject camdisable;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            camSwitch();
        }
    }
    public void camSwitch()
    {
        cam.SetActive(true);
        camdisable.SetActive(false);
    }
}
