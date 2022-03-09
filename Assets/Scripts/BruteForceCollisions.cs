using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteForceCollisions : MonoBehaviour
{
    private List<Circle> Circles;

    [SerializeField]
    private float BoundX, BoundY;
    [Range(0, 1000000), SerializeField]
    private int CircleCount;
    [SerializeField]
    private GameObject CirclePrefab;
    [Range(0, 1000), SerializeField]
    private float Speed;


    private void Start()
    {
        Circles = new List<Circle>();
        for (int i = 0; i < CircleCount; i++)
        {
            Circles.Add(Instantiate(CirclePrefab, new Vector3(Random.Range(-BoundX, BoundX), Random.Range(-BoundY, BoundY), 0f), Quaternion.identity).GetComponent<Circle>());
            Circles[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Speed);
            Circles[i].name = "Circle " + i;
            Circles[i].SetRadius(Circles[i].transform.localScale.x / 2);
            Circles[i].SetMass(Circles[i].GetComponent<Rigidbody2D>().mass);
        }
    }

    private void Update()
    {
        for (int c1 = 0; c1 < Circles.Count - 1; c1++)
        {
            for (int c2 = c1 + 1; c2 < Circles.Count; c2++)
            {
                if (Circles[c1].IsColliding(Circles[c2]))
                {
                    Vector3 dir = Circles[c2].transform.position - Circles[c1].transform.position;
                    if (Vector3.Angle(dir, Circles[c1].GetComponent<Rigidbody2D>().velocity) > 180) {
                        Debug.Log("SKIPPED");
                        continue;
                    }

                    Debug.Log("HERE");
                    //elastic collision 
                    //MIGHT NEED TO MAKE SURE THAT THINGS ARE ZERO MASS 
                    float invMassSum = Circles[c1].GetMass() + Circles[c2].GetMass();
                    invMassSum = 1 / invMassSum;

                    float m1Dif = Circles[c1].GetMass() - Circles[c2].GetMass();
                    float m2Dif = -m1Dif;

                    Vector2 vel1 = Circles[c1].GetComponent<Rigidbody2D>().velocity;
                    Vector2 vel2 = Circles[c2].GetComponent<Rigidbody2D>().velocity;

                    Vector2 newVel1 = (vel1 * m1Dif * invMassSum) + (vel2 * 2 * Circles[c2].GetMass() * invMassSum);
                    Vector2 newVel2 = (vel1 * 2 * Circles[c1].GetMass() * invMassSum) + (vel2 * m2Dif * invMassSum);

                    Circles[c1].GetComponent<Rigidbody2D>().velocity = newVel1;
                    Circles[c2].GetComponent<Rigidbody2D>().velocity = newVel2;

                }
            }
        }
    }
}