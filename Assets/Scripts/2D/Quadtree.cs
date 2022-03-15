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
        private Node Root;
        
        public Quadtree(List<Circle> circles, float width, float height)
        {
            Circles = new List<Circle>(circles);
            Width = width;
            Height = height;
        }

        public void BuildQuadtree() 
        {
            Root = new Node(Circles, Width, Height, 0, 0); 
            BuildQuadtree(Root);
        }

        private void BuildQuadtree(Node curNode)
        {

            if (!(curNode.GetCircles().Count < MinCircles || curNode.GetWidth() < MinDimension || curNode.GetHeight() < MinDimension))
            {
                // creating empty children
                for(int i = 0; i < 4; i++)
                {
                    curNode.AddChild(new Node(curNode.GetWidth()/2f, curNode.GetHeight()/2f, curNode.GetX() + Mathf.Pow(-1, i+1) * curNode.GetWidth()/4f,
                                                                         curNode.GetY() + Mathf.Pow(-1, i/2) * curNode.GetWidth()/4f, curNode));
                }

                // placing circles into the correct child using centers only
                foreach (Circle c in curNode.GetCircles()) 
                {
                    int dx = (c.transform.position.x > curNode.GetX()) ? 1 : 0;
                    int dy = (c.transform.position.y > curNode.GetY()) ? 0 : 1;
                    int index = dx + dy * 2;
                    curNode.GetChildren()[index].AddCircle(c);
                }
                
                int[] childNums = new int[4]; 
                int j = 0;
                // building the trees for the children
                foreach(Node child in curNode.GetChildren()){
                    BuildQuadtree(child);
                    childNums[j] = child.GetCircles().Count;
                    j++;
                }

                // Debug.Log(childNums[0] + " : " + childNums[1] + " : " + childNums[2] + " : " + childNums[3]);

                curNode.RemoveCircles();
            }

        }

        // query(node n) : List
        //      check if the current nodes bounds overlap with the circle
        //          if so, add the circles in the node to the list of circles found
        //          if no or node is null, end recursion

        public List<Circle> Query(Circle c) 
        {
            return Query(Root, c);
        }

        private List<Circle> Query(Node n, Circle c) 
        {
            List<Circle> found = new List<Circle>();
            if (n != null && c.IsColliding(n.GetX(), n.GetY(), n.GetWidth(), n.GetHeight())) 
            {
                found = new List<Circle>(n.GetCircles());
                foreach (Node child in n.GetChildren()) 
                {
                    found.AddRange(Query(child, c));
                }
            }
            return found;
        }

        private class Node 
        {
            private List<Circle> Circles;
            private List<Node> Children;
            private Node Parent;
            private float Width, Height;
            private float X, Y;

            public Node(float width, float height, float x, float y) 
            {
                Circles = new List<Circle>();
                Children = new List<Node>();
                Width = width;
                Height = height;
                X = x;
                Y = y;
            }

            public Node(float width, float height, float x, float y, Node parent) : this(width, height, x, y)
            {
                Parent = parent;
            }

            public Node(List<Circle> circles, float width, float height, float x, float y) : this(width, height, x, y)
            {
                Circles = circles;
            }

            public void AddChild(Node child){
                Children.Add(child);
            }

            public void AddCircle(Circle circle){
                Circles.Add(circle);
            }

            public void RemoveCircles() 
            {
                Circles.Clear();
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

