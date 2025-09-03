using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static Boss instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    public List<Vector2> spawnPositions;
    public List<GameObject> enemies;
    public GameObject bossKillDamagable;
    string _curentPhrase;
    public TextMeshProUGUI textDisplay;
    private Dictionary<Vector2, bool> Cells = new Dictionary<Vector2, bool>(); //position: isTaken

    float _spawnSpeed = 0;
    float timerSpawn;
    int _maxPositions = 0;
    int posTakenCount = 0;
    bool readyToSpawn = false; //set false by start then trigger zone set true

    int _maxHealth = 0;
    [HideInInspector] public int health;
    int killEnemiesCount = 0;
    int randomSpawnDamagableInt;
    [HideInInspector] public Animator animator;

    [SerializeField] GameObject audioSwitchBG;
    [SerializeField] GameObject BossUI;
    Image[] healthUIs;
    int iteratorhealth=0;
    [SerializeField] Sprite BossUIHealth;
    [SerializeField] Sprite BossUINoHealth;
    [SerializeField] GameObject camSwitchSmall;


    private void Start()
    {
        BossUI.SetActive(false);
        healthUIs = BossUI.GetComponentsInChildren<Image>();
        animator = GetComponent<Animator>();
        foreach (var spawn in spawnPositions)
        {
            Cells.Add(spawn, false);
        }
        PhaseOne();
    }
    private void Update()
    {
        if (readyToSpawn && posTakenCount < _maxPositions)
        {
            timerSpawn -= Time.deltaTime;
            if (timerSpawn < 0)
            {
                readyToSpawn = false;
                timerSpawn = _spawnSpeed;

                StartCoroutine(AddEnemy());
            }
        }
    }
    public void StartBattle()
    {
        StopAllCoroutines();
        BossUI.SetActive(true);
        PhaseOne();
        Debug.Log("START Battle");
        readyToSpawn = true;
        textDisplay.text = _curentPhrase;
    }

    void Phase(float spawnSpeed, int maxPositions, int bossKillDamagableCount, string phrase)
    {
        _spawnSpeed = spawnSpeed;
        timerSpawn = _spawnSpeed;
        _maxPositions = maxPositions;
        _maxHealth = bossKillDamagableCount;
        health = _maxHealth;
        randomSpawnDamagableInt = Random.Range(3, 6);

        _curentPhrase = phrase;

    }
    void PhaseOne() => Phase(3f, 5, 1, "I will end your jorney here!");
    void PhaseTwo() => Phase(2.5f, 6, 2, "You are no match for me!");
    void PhaseThree() => Phase(1.5f, 8, 3, "You are stronger then I first thought.\n But I am not done!");


    IEnumerator AddEnemy()
    {
        if (killEnemiesCount >= randomSpawnDamagableInt)
        {
            yield return StartCoroutine(AddDamagable());
        }
        int randomIndex = Random.Range(0, enemies.Count - 1);

        Instantiate(enemies[randomIndex], GetFreeCell().Key, Quaternion.identity, transform);
        posTakenCount = Cells.Values.Where(values => values == true).Count();
        readyToSpawn = true;

        //Debug.Log("Spawning enemy " + posTakenCount);

        yield return 0;
    }
    IEnumerator AddDamagable()
    {
        killEnemiesCount = 0;
        randomSpawnDamagableInt = Random.Range(3, 6);
        Instantiate(bossKillDamagable, GetFreeCell().Key, Quaternion.identity, transform);
        posTakenCount = Cells.Values.Where(values => values == true).Count();
        yield return 0;
    }
    KeyValuePair<Vector2, bool> GetFreeCell()
    {
        int randomIndex = Random.Range(0, Cells.Count - 1);
        KeyValuePair<Vector2, bool> randomEntry = Cells.ElementAt(randomIndex);
        if (randomEntry.Value == false)
        {
            Cells[randomEntry.Key] = true;
            return randomEntry;
        }
        else
        {
            return GetFreeCell();
        }
    }
    public void EnemyDestroyed(Vector2 position)
    {
        foreach (var cell in Cells)
        {
            if (cell.Key == position)
            {
                Cells[cell.Key] = false;
                posTakenCount = Cells.Values.Where(values => values == true).Count();
                killEnemiesCount++;
                break;
            }
        }
    }
    public void Damage()
    {
        healthUIs[5-iteratorhealth].sprite = BossUINoHealth;
        iteratorhealth++;

        health -= 1;
        animator.SetTrigger("hit");
        if (health <= 0 && _maxHealth == 1)
        {
            Debug.Log("PHASE TWO");

            PhaseTwo();
        }
        else if (health <= 0 && _maxHealth == 2)
        {
            Debug.Log("PHASE THREE");

            PhaseThree();
        }
        else if (health <= 0 && _maxHealth == 3)
        {
            StopAllCoroutines();
            BossUI.SetActive(false);
            _curentPhrase = "I am defeated? But how..";
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Could not properly move on from damage on BOSS");
        }

        textDisplay.text = _curentPhrase;

    }
    public void Restart()
    {
        BossUI.SetActive(false);
        StopAllCoroutines();

        Cells.Clear();
        foreach (var spawn in spawnPositions)
        {
            Cells.Add(spawn, false);
        }
        posTakenCount = Cells.Values.Where(values => values == true).Count();
        
        foreach (var health in healthUIs)
        {
            health.sprite = BossUIHealth;
        }
        iteratorhealth = 0;
        killEnemiesCount = 0;

        readyToSpawn = false;
        PhaseOne();
        textDisplay.text = "";
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        audioSwitchBG.SetActive(true);
        audioSwitchBG.GetComponent<AudioSwitch>().ChangeMusicForRestart();
        camSwitchSmall.GetComponent<CamSwitch>().camSwitch();
    }
}
