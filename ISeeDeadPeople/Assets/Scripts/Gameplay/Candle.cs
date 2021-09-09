using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    public bool isOn;
    public Room room;


    private void Start()
    {
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void turnOn()
    {
        isOn = true;
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void turnOff(bool isPlayerOnTheRight)
    {
        if (isOn)
        {
            Transform child = transform.GetChild(0);        
            isOn = false;
            transform.GetChild(1).gameObject.SetActive(false);
            if (!isPlayerOnTheRight)
            {
                child.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            } else child.rotation = Quaternion.Euler(Vector3.zero);
            if(child.childCount > 0)
            {
                foreach (Transform c in child)
                {
                    c.GetComponent<ParticleSystem>().Play();
                }
            } else child.GetComponent<ParticleSystem>().Play();
        }
    }
}
