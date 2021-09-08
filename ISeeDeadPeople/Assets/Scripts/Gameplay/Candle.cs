using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public bool isOn;
    public Room room;

    public Material On;
    public Material Off;

    public void turnOn()
    {
        isOn = true;
        GetComponent<MeshRenderer>().material = On;
    }

    public void turnOff(bool isPlayerOnTheRight)
    {
        if (isOn)
        {
            Transform child = transform.GetChild(0);        
            isOn = false;
            GetComponent<MeshRenderer>().material = Off;
            if (!isPlayerOnTheRight)
            {
                child.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            } else child.rotation = Quaternion.Euler(Vector3.zero);
            child.GetComponent<ParticleSystem>().Play();
        }
    }
}
