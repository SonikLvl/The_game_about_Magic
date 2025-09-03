using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver instance { get; private set; }
    public GameObject gameOverScreen;
    public Transform parentFirst;
    public Transform parentSecond;
    public Transform parentThird;
    public Transform parentFourth;
    public Transform parentBoss;
    public Transform girls;
    public Transform tilemap;
    [HideInInspector] public (Vector2 savepointPos, int sectionNumber) spawn;
    void Awake()
    {
        instance = this;
        gameOverScreen.SetActive(false);
        spawn = (new Vector2(-4.03f, -0.81f), 0);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        lineGen.instance.ChangeColorNothing();
        playerController.instance.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        lineGen.instance.ChangeColorSparkle();

        spawn = (new Vector2(-4.03f, -0.81f), 0);

        gameOverScreen.SetActive(false);

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var p in projectiles)
            Destroy(p);


        playerController.instance.transform.position = new Vector2(-4.03f, -0.81f);
        playerController.instance.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerController.instance.ChangeHealth(6);
        playerController.instance.inFlames = false;
        foreach (Transform g in girls)
        {
            var d = g.gameObject.GetComponentInChildren<Dialogue>();
            d.idOfPhrase = 0;
            d.textDisplay.text = d.phrases[0];
        }
        foreach (Transform child in tilemap)
        {
            child.gameObject.SetActive(true);
        }

        foreach (Transform child in parentFirst)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in parentSecond)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in parentThird)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in parentFourth)
        {
            child.gameObject.SetActive(true);
        }
        foreach (Transform child in parentBoss)
        {
            child.gameObject.SetActive(true);
        }
        Boss.instance.Restart();

    }
    public void LastSavePoint()
    {
        Time.timeScale = 1;
        lineGen.instance.ChangeColorSparkle();
        

        gameOverScreen.SetActive(false);

        playerController.instance.transform.position = spawn.savepointPos;
        playerController.instance.rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerController.instance.ChangeHealth(6);
        playerController.instance.inFlames = false;
        foreach (Transform g in girls)
        {
            var d = g.gameObject.GetComponentInChildren<Dialogue>();
            d.idOfPhrase = 0;
            d.textDisplay.text = d.phrases[0];
        }
        
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (var p in projectiles)
            Destroy(p);

        Boss.instance.Restart();
        foreach (Transform child in tilemap)
            child.gameObject.SetActive(true);
        switch (spawn.sectionNumber)
        {
            case 1:
                foreach (Transform child in parentFirst)
                    child.gameObject.SetActive(true);

                break;
            case 2:
                foreach (Transform child in parentSecond)
                    child.gameObject.SetActive(true);
                break;
            case 3:
                foreach (Transform child in parentThird)
                    child.gameObject.SetActive(true);
                break;
            case 4:
                foreach (Transform child in parentFourth)
                    child.gameObject.SetActive(true);
                break;
            case 5:
                foreach (Transform child in parentBoss)
                    child.gameObject.SetActive(true);
                break;
            default:
                foreach (Transform child in parentFirst)
                {
                    child.gameObject.SetActive(true);
                }
                foreach (Transform child in parentSecond)
                {
                    child.gameObject.SetActive(true);
                }
                foreach (Transform child in parentThird)
                {
                    child.gameObject.SetActive(true);
                }
                foreach (Transform child in parentFourth)
                {
                    child.gameObject.SetActive(true);
                }
                foreach (Transform child in parentBoss)
                {
                    child.gameObject.SetActive(true);
                }
                break;
        }
    }
    public void Pause()
    {
        lineGen.instance.ChangeColorNothing();
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
        lineGen.instance.ChangeColorSparkle();
    } 
}
