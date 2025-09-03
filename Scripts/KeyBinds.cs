using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    public GameObject buttonSparkles;
    public GameObject buttonAttack;
    public GameObject buttonWater;
    public GameObject buttonFire;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            lineGen.instance.ChangeColorSparkle();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && buttonAttack.activeInHierarchy)
        {
            lineGen.instance.ChangeColorPower();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && buttonWater.activeInHierarchy)
        {
            lineGen.instance.ChangeColorWater();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && buttonFire.activeInHierarchy)
        {
            lineGen.instance.ChangeColorFire();
        }
    }
}
