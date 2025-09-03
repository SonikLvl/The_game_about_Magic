using UnityEngine;

public class HideIfActive : MonoBehaviour
{
    public GameObject hide;
    private void Start()
    {
        hide.SetActive(false);
    }
    void OnEnable()
    { 
        hide.SetActive(false);
    }
}
