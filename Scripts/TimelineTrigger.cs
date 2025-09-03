using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    [SerializeField] PlayableDirector timeline;
    void Start()
    {
        timeline.Stop();
    }
    void OnEnable()
    {
        timeline.Stop();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(PlayTimeline());
    }
    IEnumerator PlayTimeline()
    {
        timeline.Play();
        yield return 0;
    }
}
