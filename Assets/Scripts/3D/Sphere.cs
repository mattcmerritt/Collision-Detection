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

        public bool IsColliding(float x, float y, float z, float xDepth, float yDepth, float zDepth)
        {

            float closestX = Mathf.Clamp(transform.position.x, x - xDepth/2f, x + xDepth/2f);
            float closestY = Mathf.Clamp(transform.position.y, y - yDepth/2f, y + yDepth/2f);
            float closestZ = Mathf.Clamp(transform.position.z, z - zDepth/2f, z + zDepth/2f);

            Vector3 closestPt = new Vector3(closestX, closestY, closestZ);
            return Vector3.Distance(transform.position, closestPt) < Radius;

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
