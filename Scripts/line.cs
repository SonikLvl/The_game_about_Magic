using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(EdgeCollider2D))]
public class line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCollider2D;
    List<Vector2> points;
    Transform CollidedWithTransform;
    LineType type = LineType.sparkle;
    bool playerMinus1;


    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);
            edgeCollider2D.SetPoints(points);
            return;
        }

        if (Vector2.Distance(points.Last(), position) > .1f)
        {
            SetPoint(position);
            edgeCollider2D.SetPoints(points);
        }
    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    public void changeColor(Color start, Color end, LineType lineType)
    {
        lineRenderer.endColor = end;
        lineRenderer.startColor = start;
        type = lineType;
        playerMinus1 = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CollidedWithTransform == null && other.transform.tag != "Player")
        {
            CollidedWithTransform = other.transform;
            //Debug.Log(this.edgeCollider2D.bounds + " hit " + CollidedWithTransform.name);
            Interact(other.transform.gameObject);
        }
        else if (other.transform.tag == "Player")
        {
            if (type == LineType.attack && !playerMinus1) {
                other.transform.gameObject.GetComponent<playerController>().ChangeHealth(-1);
                playerMinus1 = true;
            }
            else if (type == LineType.flame)
            {
                other.transform.gameObject.GetComponent<playerController>().inFlames = true;
                other.transform.gameObject.GetComponent<playerController>().flameParticles.Play();
            }
            else if(type == LineType.water)
            {
                other.transform.gameObject.GetComponent<playerController>().inFlames = false;
                other.transform.gameObject.GetComponent<playerController>().flameParticles.Stop();
            }
        }
    }

    private void Interact(GameObject gameObject)
    {
        try
        {
            gameObject.GetComponent<Interactable>()?.PerformAction(type);
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }
    
}
public enum LineType{sparkle,flame,attack,water}