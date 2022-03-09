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
                    Debug.Log($"Collision between {Circles[c1].name} and {Circles[c2].name}");
                }
            }
        }
    }
}
