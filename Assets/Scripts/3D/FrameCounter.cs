using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameCounter : MonoBehaviour
{
    private Text Textbox;

    private float UpdateTimer = 0;

    [SerializeField, Range(0, 1)]
    private float UpdateDelay;

    private void Start()
    {
        Textbox = GetComponent<Text>();
    }

    private void Update()
    {
        UpdateTimer += Time.deltaTime;

        if (UpdateTimer > UpdateDelay)
        {
            Textbox.text = "FPS: " + (1 / Time.smoothDeltaTime) + "\nTime between Frames: " + Time.smoothDeltaTime;
            UpdateTimer = 0;
        }
    }
}
