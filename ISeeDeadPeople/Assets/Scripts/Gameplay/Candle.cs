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

    public void turnOff()
    {
        isOn = false;
        GetComponent<MeshRenderer>().material = Off;
    }
}
