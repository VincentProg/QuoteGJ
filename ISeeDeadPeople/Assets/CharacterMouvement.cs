using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMouvement : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public float SpeedInWall;
    public bool InWall;
    public CharacterController Controller;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!InWall)
        {
        float Axex = Input.GetAxis("Horizontal");
        float Axey = Input.GetAxis("Vertical");

        Vector3 Axes = transform.right * Axex + transform.up * Axey;
        Axes = Axes * Speed * Time.deltaTime;
        Controller.Move(Axes);
        }
    }
}
