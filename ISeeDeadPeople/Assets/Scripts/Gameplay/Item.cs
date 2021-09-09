using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    Room myRoom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            myRoom = other.GetComponent<Room>();
        }
        else if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterMouvement>().itemsNear.Add(this);
            print("1");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterMouvement>().itemsNear.Remove(this);
        }
    }

    public void Interact() { 

    }
}
