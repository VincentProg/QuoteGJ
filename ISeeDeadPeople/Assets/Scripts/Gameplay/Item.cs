using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum TYPE { FAUTEUIL, CHAISE, TABLE}
    public TYPE type = TYPE.FAUTEUIL;
    Room myRoom;

    bool isInteracting;

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
        isInteracting = true;
        switch (type)
        {
            case TYPE.FAUTEUIL:


                break;
            case TYPE.CHAISE:

                break;
            case TYPE.TABLE:

                break;
        }

    }


    public void FailInteraction()
    {
        isInteracting = false;
        // anim
    }

    public void SucceedInteraction()
    {
        isInteracting = false;
        // anim
        // HUNTERS
    }
}
