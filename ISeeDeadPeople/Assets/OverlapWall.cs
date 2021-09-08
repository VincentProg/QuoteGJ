using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapWall : MonoBehaviour
{
    // Start is called before the first frame update
    public CharacterMouvement Mouvement;
    public OverlapWall OtherOverlap;
    public Vector3 Axes;
    public bool Activate;
    public int Lock;
    public bool Head;
    public Animator Effect;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouvement.InWall && Activate)
        {
            if(Head && Axes.y==0)
            {
                Axes.y = 1;
            }else if(!Head && Axes.x==0)
            {
                Axes.x = 1;
            }
            Mouvement.Controller.Move(Axes*Mouvement.SpeedInWall*Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            if(Lock==0 && Activate)
            {
                Effect.SetBool("InWall", true);
                OtherOverlap.Activate = false;
                Mouvement.InWall = true;
                if(Head)
                {
                    Axes = new Vector3(0, Input.GetAxis("Vertical"), 0).normalized;
                }else
                {
                    Axes = new Vector3(Input.GetAxis("Horizontal"), 0, 0).normalized;
                }
            }
            Lock += 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Wall"))
        {
            Lock -= 1;
            if(Lock==0 && Activate)
            {
                Effect.SetBool("InWall", false);
                OtherOverlap.Activate = true;
                Mouvement.InWall = false;
                Axes = Vector3.zero;
            }
        }
    }
}
