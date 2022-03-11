using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThreeDimensions {

    public class Collisions : MonoBehaviour
    {
        private List<Sphere> Spheres;

        [SerializeField]
        private float BoundX, BoundY, BoundZ;
        [Range(0, 1000000), SerializeField]
        private int SphereCount;
        [SerializeField]
        private GameObject SpherePrefab;
        [Range(0, 1000), SerializeField]
        private float Speed;

        [SerializeField]
        private bool UsingUnity;
        private void Start()
        {
            Spheres = new List<Sphere>();
            for (int i = 0; i < SphereCount; i++)
            {
                Spheres.Add(Instantiate(SpherePrefab, new Vector3(Random.Range(-BoundX, BoundX), Random.Range(-BoundY, BoundY), Random.Range(-BoundZ, BoundZ)), Quaternion.identity).GetComponent<Sphere>());
                Spheres[i].GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1)).normalized * Speed);
                Spheres[i].name = "Sphere " + i;
                Spheres[i].SetRadius(Spheres[i].transform.localScale.x / 2);
                Spheres[i].SetMass(Spheres[i].GetComponent<Rigidbody>().mass);
            }
        }

        private void Update()
        {
            if (!UsingUnity)
            {
                for (int s1 = 0; s1 < Spheres.Count - 1; s1++)
                {
                    for (int s2 = s1 + 1; s2 < Spheres.Count; s2++)
                    {
                        if (Spheres[s1].IsColliding(Spheres[s2]))
                        {
                            Vector3 dir = Spheres[s2].transform.position - Spheres[s1].transform.position;
                            if (Vector3.Angle(dir, Spheres[s1].GetComponent<Rigidbody>().velocity) > 180)
                            {
                                //Debug.Log("SKIPPED");
                                continue;
                            }

                            //Debug.Log("HERE");
                            //elastic collision 
                            float invMassSum = Spheres[s1].GetMass() + Spheres[s2].GetMass();
                            invMassSum = 1 / invMassSum;

                            float m1Dif = Spheres[s1].GetMass() - Spheres[s2].GetMass();
                            float m2Dif = -m1Dif;

                            Vector3 vel1 = Spheres[s1].GetComponent<Rigidbody>().velocity;
                            Vector3 vel2 = Spheres[s2].GetComponent<Rigidbody>().velocity;

                            Vector3 newVel1 = (vel1 * m1Dif * invMassSum) + (vel2 * 2 * Spheres[s2].GetMass() * invMassSum);
                            Vector3 newVel2 = (vel1 * 2 * Spheres[s1].GetMass() * invMassSum) + (vel2 * m2Dif * invMassSum);

                            Spheres[s1].GetComponent<Rigidbody>().velocity = newVel1;
                            Spheres[s2].GetComponent<Rigidbody>().velocity = newVel2;

                            //make sure they are not intersecting
                            Vector3 centerDir = Spheres[s2].transform.position - Spheres[s1].transform.position;
                            float overLapMag = (Spheres[s1].GetRadius() + Spheres[s2].GetRadius()) - centerDir.magnitude;
                            centerDir.Normalize();

                            Spheres[s1].transform.position = Spheres[s1].transform.position - centerDir * overLapMag / 2f;
                            Spheres[s2].transform.position = Spheres[s2].transform.position + centerDir * overLapMag / 2f;
                        }
                    }
                }
            }
            else
            {
                foreach (Sphere s in Spheres)
                {
                    Vector3 vel = s.GetComponent<Rigidbody>().velocity;
                    s.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    s.GetComponent<Rigidbody>().AddForce(vel.normalized * Speed);
                }
            }
        }
    }
}
