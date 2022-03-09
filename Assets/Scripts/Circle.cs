using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    private float Radius;

    public bool IsColliding(Circle circle)
    {
        return Vector3.Distance(transform.position, circle.transform.position) < Radius + circle.GetRadius();
    }

    public float GetRadius()
    {
        return Radius;
    }

    public void SetRadius(float rad)
    {
        Radius = rad;
    }
}
