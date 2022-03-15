using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeDimensions
{
    public class Octree
    {
        private List<Sphere> Spheres;
        private float XSize, YSize, ZSize;
        private const float MinDimension = 5f;
        private const int MinSpheres = 5;
        private Node Root;
        
        public Octree(List<Sphere> spheres, float xSize, float ySize, float zSize)
        {
            Spheres = new List<Sphere>(spheres);
            XSize = xSize;
            YSize = ySize;
            ZSize = zSize;
        }

        public void BuildOctree() 
        {
            Root = new Node(Spheres, XSize, YSize, ZSize, 0, 0, 0); 
            BuildOctree(Root);
        }

        private void BuildOctree(Node curNode)
        {

            if (!(curNode.GetSpheres().Count < MinSpheres || curNode.GetXSize() < MinDimension || curNode.GetYSize() < MinDimension || curNode.GetZSize() < MinDimension))
            {
                // creating empty children
                for(int i = 0; i < 8; i++)
                {
                    curNode.AddChild(new Node(curNode.GetXSize()/2f, curNode.GetYSize()/2f, curNode.GetZSize()/2f,
                                                                         curNode.GetX() + Mathf.Pow(-1, i+1) * curNode.GetXSize()/4f,
                                                                         curNode.GetY() + Mathf.Pow(-1, i/2) * curNode.GetYSize()/4f,
                                                                         curNode.GetZ() - Mathf.Pow(-1, i/4) * curNode.GetZSize()/4f, curNode));
                }

                // placing spheres into the correct child using centers only
                foreach (Sphere s in curNode.GetSpheres()) 
                {
                    int dx = (s.transform.position.x > curNode.GetX()) ? 1 : 0;
                    int dy = (s.transform.position.y > curNode.GetY()) ? 0 : 1;
                    int dz = (s.transform.position.z > curNode.GetZ()) ? 1 : 0; 
                    int index = dx + dy * 2 + dz * 4;
                    curNode.GetChildren()[index].AddSphere(s);
                }
                
                // int[] childNums = new int[8]; 
                // int j = 0;
                // building the trees for the children
                foreach(Node child in curNode.GetChildren()){
                    BuildOctree(child);
                    // childNums[j] = child.GetSpheres().Count;
                    // j++;
                }
                curNode.RemoveSpheres();
            }

        }


        public void UpdateTree()
        {

            UpdateTree(Root);

            CleanNodes(Root);

        }

        private void UpdateTree(Node curNode)
        {
            if(curNode.GetSpheres().Count != 0)
            {
                List<Sphere> removed = new List<Sphere>();
                foreach(Sphere s in curNode.GetSpheres())
                {
                    if(!curNode.Contains(s))
                    {
                        FindNode(s).AddSphere(s);
                        removed.Add(s);
                    }
                }
                
                curNode.GetSpheres().RemoveAll((Sphere s) => removed.Contains(s));
            }
            else
            {
                foreach(Node child in curNode.GetChildren())
                {
                    UpdateTree(child);
                }
            }
        }

        private int CleanNodes(Node curNode)
        {
            //clean Children
            int totalSpheres = 0;
            foreach(Node child in curNode.GetChildren())
            {
                totalSpheres += CleanNodes(child);
            }
            // adding new children if node has too many nodes in the parent
            if(totalSpheres > MinSpheres && curNode.GetChildren().Count == 0)
            {
                // creating empty children
                for(int i = 0; i < 8; i++)
                {
                    curNode.AddChild(new Node(curNode.GetXSize()/2f, curNode.GetYSize()/2f, curNode.GetZSize()/2f,
                                                                         curNode.GetX() + Mathf.Pow(-1, i+1) * curNode.GetXSize()/4f,
                                                                         curNode.GetY() + Mathf.Pow(-1, i/2) * curNode.GetYSize()/4f,
                                                                         curNode.GetZ() - Mathf.Pow(-1, i/4) * curNode.GetZSize()/4f, curNode));
                }

                // placing spheres into the correct child using centers only
                foreach (Sphere s in curNode.GetSpheres()) 
                {
                    int dx = (s.transform.position.x > curNode.GetX()) ? 1 : 0;
                    int dy = (s.transform.position.y > curNode.GetY()) ? 0 : 1;
                    int dz = (s.transform.position.z > curNode.GetZ()) ? 1 : 0; 
                    int index = dx + dy * 2 + dz * 4;
                    curNode.GetChildren()[index].AddSphere(s);
                }
            }
            // removing children if they can be all put in a single parent
            if(totalSpheres < MinSpheres && curNode.GetChildren().Count != 0)
            {
                foreach(Node child in curNode.GetChildren())
                {
                    foreach(Sphere s in child.GetSpheres())
                    {
                        curNode.AddSphere(s);
                    }
                }
                curNode.RemoveChildren();
                totalSpheres = 0;
            }
            //check if too few and delete
            return curNode.GetSpheres().Count + totalSpheres;
        }
        
        private Node FindNode(Sphere s)
        {
            return FindNode(s, Root);
        }
        
        private Node FindNode(Sphere s, Node curNode)
        {
            if(curNode.GetChildren().Count == 0)
            {
                return curNode;
            }
            foreach(Node child in curNode.GetChildren())
            {
                if(child.Contains(s))
                {
                    return FindNode(s, child);
                }
            }
            Debug.LogError("RUT ROH " + s.name);
            return null;
        }

        // query(node n) : List
        //      check if the current nodes bounds overlap with the sphere
        //          if so, add the spheres in the node to the list of spheres found
        //          if no or node is null, end recursion

        public List<Sphere> Query(Sphere s) 
        {
            return Query(Root, s);
        }

        private List<Sphere> Query(Node n, Sphere s) 
        {
            List<Sphere> found = new List<Sphere>();
            if (n != null && s.IsColliding(n.GetX(), n.GetY(), n.GetZ(), n.GetXSize(), n.GetYSize(), n.GetZSize())) 
            {
                found = new List<Sphere>(n.GetSpheres());
                foreach (Node child in n.GetChildren()) 
                {
                    found.AddRange(Query(child, s));
                }
            }
            return found;
        }

        private class Node 
        {
            private List<Sphere> Spheres;
            private List<Node> Children;
            private Node Parent;

            private float XSize, YSize, ZSize;
            private float X, Y, Z;

            public Node(float xSize, float ySize, float zSize, float x, float y, float z) 
            {
                Spheres = new List<Sphere>();
                Children = new List<Node>();
 
                XSize = xSize;
                YSize = ySize;
                ZSize = zSize;
                X = x;
                Y = y;
                Z = z;
            }

            public Node(float xSize, float ySize, float zSize, float x, float y, float z, Node parent) : this(xSize, ySize, zSize, x, y, z)
            {
                Parent = parent;
            }

            public Node(List<Sphere> spheres, float xSize, float ySize, float zSize, float x, float y, float z) : this(xSize, ySize, zSize, x, y, z)
            {
                Spheres = spheres;
            }

            public bool Contains(Sphere s)
            {
                if(s.transform.position.x < X - XSize/2f || s.transform.position.x > X + XSize/2f)
                {
                    return false;
                }
                if(s.transform.position.y < Y - YSize/2f || s.transform.position.y > Y + YSize/2f)
                {
                    return false;
                }
                if(s.transform.position.z < Z - ZSize/2f || s.transform.position.z > Z + ZSize/2f)
                {
                    return false;
                }
                return true;
            }

            public void AddChild(Node child){
                Children.Add(child);
            }

            public void AddSphere(Sphere sphere){
                Spheres.Add(sphere);
            }

            public void RemoveSpheres() 
            {
                Spheres.Clear();
            }

            public void RemoveChildren()
            {
                Children.Clear();
            }

            public List<Sphere> GetSpheres() 
            {
                return Spheres;
            }

            public float GetXSize()
            {
                return XSize;
            }
        
            public float GetYSize()
            {
                return YSize;
            }
            
            public float GetZSize()
            {
                return ZSize;
            }

            public float GetX()
            {
                return X;
            }

            public float GetY()
            {
                return Y;
            }
            
            public float GetZ()
            {
                return Z;
            }

            public List<Node> GetChildren()
            {
                return Children;
            }
        }
    }
}

