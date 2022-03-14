using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwoDimensions
{
    public class Quadtree
    {
        private List<Circle> Circles;
        private float Width, Height;
        private const float MinDimension = 5f;
        private const int MinCircles = 5;
        
        public Quadtree(List<Circle> circles, float width, float height)
        {
            Circles = circles;
            Width = width;
            Height = height;
        }

        private class Node 
        {
            private List<Circle> Circles;
            private List<Node> Children;
            private float Width, Height;
            private float X, Y;

            public Node(List<Circle> circles, float width, float height, float x, float y) 
            {
                Circles = circles;
                Children = new List<Node>();
                Width = width;
                Height = height;
                X = x;
                Y = y;
            }

            public void AddChild(Node child){
                Children.Add(child);
            }

            public List<Circle> GetCircles() 
            {
                return Circles;
            }

            public float GetWidth()
            {
                return Width;
            }
        
            public float GetHeight()
            {
                return Height;
            }

            public float GetX()
            {
                return X;
            }

            public float GetY()
            {
                return Y;
            }

            public List<Node> GetChildren()
            {
                return Children;
            }
        }
    }
}

