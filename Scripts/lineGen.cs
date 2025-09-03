using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineGen : MonoBehaviour
{
    public static lineGen instance { get; private set; }

    void Awake()
    {
        instance = this;
    }
    public GameObject linePrefab;
    public ParticleSystem[] effectPrefabs;
    ParticleSystem currentEffectPrefab;
    float particleDestroy = 0.4f;

    line activeLine;
    Color startColor = Color.black;

    Color endColor = Color.black;
    LineType lineType = LineType.sparkle;

    GameObject newLine;
    public float fadeDuration = 1.5f;

    bool generatePartiles = true;
    void Start()
    {
        ChangeColorSparkle();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            newLine = Instantiate(linePrefab, this.transform);
            activeLine = newLine.GetComponent<line>();
            ChangeColor(startColor, endColor, lineType);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Destroy(activeLine.GetComponent<line>());
            activeLine = null;
            StartCoroutine(AutoDestroy(newLine));
        }

        if (activeLine != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            activeLine.UpdateLine(mousePos);

            if (lineType == LineType.attack) fadeDuration = 0.02f;
            else  if (lineType == LineType.flame) fadeDuration = 0.3f;
            else fadeDuration = 0.6f;
            if (generatePartiles)
            {
                generatePartiles = false;
                ParticleSystem instantiatedEffect = Instantiate(currentEffectPrefab, mousePos, Quaternion.identity);
                StartCoroutine(EffectPlay(instantiatedEffect));
            }
        }
    }

    private IEnumerator EffectPlay(ParticleSystem instantiatedEffect)
    {
        instantiatedEffect.Play();

        yield return new WaitForSeconds(0.03f);
        generatePartiles = true;

        yield return new WaitForSeconds(particleDestroy / 2);
        instantiatedEffect.Stop();

        Destroy(instantiatedEffect.gameObject, particleDestroy);

        yield return new WaitForSeconds(particleDestroy / 2);
    }
    private IEnumerator AutoDestroy(GameObject line)
    {
        LineRenderer lineRenderer = newLine.GetComponent<LineRenderer>();

        float timer = 0f;
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        yield return new WaitForSeconds(fadeDuration * 15f);

        while (timer < fadeDuration)
        {
            if (lineRenderer == null) break; //if line is destroyed before couroutine ended then exit
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration); // Lerp alpha from 1 to 0

            startColor.a = alpha;
            endColor.a = alpha;

            lineRenderer.startColor = startColor;
            lineRenderer.endColor = endColor;

            yield return null; // Wait for the next frame
        }
        yield return new WaitForSeconds(fadeDuration);

        Destroy(line);
    }
    public void ChangeColor(Color start, Color end, LineType lineType)
    {
        activeLine.changeColor(start, end, lineType);
    }

    public void ChangeColorPower()
    {
        endColor = new Color32(143, 5, 255, 255);
        startColor = new Color32(255, 82, 238, 255);
        lineType = LineType.attack;
        currentEffectPrefab = effectPrefabs[0];
        particleDestroy = 0.4f;
    }
    public void ChangeColorFire()
    {
        endColor = new Color32(240, 117, 29, 255);
        startColor = new Color32(230, 8, 0, 255);
        lineType = LineType.flame;
        currentEffectPrefab = effectPrefabs[1];
        particleDestroy = 1f;
    }
    public void ChangeColorSparkle()
    {
        startColor = new Color32(255, 192, 20, 255);
        endColor = new Color32(255, 244, 214, 255);
        lineType = LineType.sparkle;
        currentEffectPrefab = effectPrefabs[0];
        particleDestroy = 0.4f;
    }
    public void ChangeColorWater()
    {
        endColor = new Color32(2, 52, 201, 255);
        startColor = new Color32(91, 178, 245, 255);
        lineType = LineType.water;
        currentEffectPrefab = effectPrefabs[2];
        particleDestroy = 0.8f;
    }
    public void ChangeColorNothing()
    {
        endColor = new Color32(0, 0, 0, 0);
        startColor = new Color32(0, 0, 0, 0);
        lineType = LineType.sparkle;
        currentEffectPrefab = effectPrefabs[0];
        particleDestroy = 0.4f;
    }
    public void DestroyCurrentLine() => Destroy(newLine);
}


