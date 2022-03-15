using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwoDimensions
{
    public class Circle : MonoBehaviour
    {
        private float Radius;
        private float Mass;

        public bool IsColliding(Circle circle)
        {
            return Vector3.Distance(transform.position, circle.transform.position) < Radius + circle.GetRadius();
        }

        public float GetRadius()
        {
            return Radius;
        }

        public float GetMass()
        {
            return Mass;
        }

        public void SetMass(float mass)
        {
            Mass = mass;
        }

        public void SetRadius(float rad)
        {
            Radius = rad;
        }
    }

}
