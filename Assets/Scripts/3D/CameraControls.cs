using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float RotateSpeed;
    [SerializeField] private float MovementRemaining;
    private void Update()
    {
        // right
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.eulerAngles += new Vector3(0, RotateSpeed * Time.deltaTime, 0);
        }
        // left
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.eulerAngles -= new Vector3(0, RotateSpeed * Time.deltaTime, 0);
        }
        // up
        if (Input.GetAxisRaw("Vertical") > 0 && MovementRemaining >= -90f)
        {
            transform.eulerAngles -= new Vector3(RotateSpeed * Time.deltaTime, 0, 0);
            MovementRemaining -= RotateSpeed * Time.deltaTime;
            if(MovementRemaining <= -90f)
            {
                transform.eulerAngles = new Vector3(-90f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        // down
        else if (Input.GetAxisRaw("Vertical") < 0 && MovementRemaining <= 90f)
        {
            transform.eulerAngles += new Vector3(RotateSpeed * Time.deltaTime, 0, 0);
            MovementRemaining += RotateSpeed * Time.deltaTime;
            if (MovementRemaining >= 90f)
            {
                transform.eulerAngles = new Vector3(90f, transform.eulerAngles.y, transform.eulerAngles.z);
            }
        }
        // move forward
        if((Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl) || Input.GetMouseButton(0)))
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
        // move backward
        else if (Input.GetMouseButton(1) || (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.LeftControl)))
        {
            transform.position -= transform.forward * MoveSpeed * Time.deltaTime;
        }
    }
}
