using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeDimensions
{
    public class Sphere : MonoBehaviour
    {
        private float Radius;
        private float Mass;

        public bool IsColliding(Sphere sphere)
        {
            return Vector3.Distance(transform.position, sphere.transform.position) < Radius + sphere.GetRadius();
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
