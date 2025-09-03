using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public string[] phrases;
    public TextMeshProUGUI textDisplay;
    public float waitForPhrase = 0.2f;


    [HideInInspector]public int idOfPhrase = 0;
    bool canContinueTalking = true;

    void Start()
    {
        textDisplay.text = phrases[idOfPhrase];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Collided to talk with " + transform.name);
            //textDisplay.text = phrases[idOfPhrase < phrases.Length ? idOfPhrase++ : idOfPhrase = 0];
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (canContinueTalking && collision.transform.CompareTag("Player"))
        {
            StartCoroutine(ShowNextPhrase());
        }
    }

    public IEnumerator ShowNextPhrase() //We first wait for previos phrase and then switch to next
    {
        canContinueTalking = false;
        int numberOfLetters = 0;
        foreach (char c in textDisplay.text) //count letters in phrase
            numberOfLetters++;
        
        yield return new WaitForSeconds((numberOfLetters<8 ? 8 : numberOfLetters) * waitForPhrase); //for example 30 letters *0.05(waitForPhrase) = 1.5f waiting seconds
        if (idOfPhrase < phrases.Length - 1) idOfPhrase++; else idOfPhrase = 0;
        textDisplay.text = phrases[idOfPhrase];

        canContinueTalking = true;
    }
}
