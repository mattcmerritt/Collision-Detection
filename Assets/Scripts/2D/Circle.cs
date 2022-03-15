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

        public bool IsColliding(float x, float y, float width, float height)
        {

            float closestX = Mathf.Clamp(transform.position.x, x - width/2, x + width/2);
            float closestY = Mathf.Clamp(transform.position.y, y - height/2, y + height/2);

            Vector3 closestPt = new Vector3(closestX, closestY, 0);
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
