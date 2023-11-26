using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailMove : MonoBehaviour
{
    TrailRenderer _trail;
    EdgeCollider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _trail = transform.GetComponent<TrailRenderer>();
        _collider = transform.GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        SetColliderPointsFromTrail(_trail, _collider);
    }

    void SetColliderPointsFromTrail(TrailRenderer trail, EdgeCollider2D collider)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < trail.positionCount; i++)
        {
            points.Add(trail.GetPosition(i));
        }

        collider.SetPoints(points);
    }
}
