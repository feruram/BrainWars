using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private ControllerInputManager input;
    float MoveSpeed = 0.1f;
    private GameObject Head;
    public float sideSpeedDiv=3;

    private void Start()
    {
        Head = GameObject.FindGameObjectWithTag("MainCamera");
        input=GameObject.FindGameObjectWithTag("Input").GetComponent<ControllerInputManager>();

    }

    private void Update()
    {
        Vector2 pos = input.move;
        transform.position += Head.transform.forward * pos.y * MoveSpeed;
        transform.position += Head.transform.right * pos.x * MoveSpeed / sideSpeedDiv;
        Vector3 loc = transform.position;
        loc.y = 0;
        transform.position = loc;
    }
}
