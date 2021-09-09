using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum TYPE { FAUTEUIL, CHAISE, TABLE}
    public TYPE type = TYPE.FAUTEUIL;
    Room myRoom = null;

    [HideInInspector]
    public Vector3 posEmote;

    bool isInteracting;
    Animator anim;

    public GameObject itemQTESequence = null;
    private GameObject qteGO = null;
    private QTESequence qteSequence = null;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
        posEmote = transform.GetChild(0).position + new Vector3(0,1,-1);
    }

    private void Update()
    {
        if (Input.GetKeyDown("s")){
            SucceedInteraction();
        }

        if (isInteracting)
        {
            if(qteGO != null)
            {
                if (qteSequence != null)
                {
                    if (qteSequence.sequenceFinished)
                    {
                        SucceedInteraction();
                        Destroy(qteGO);
                        Destroy(qteSequence);
                    }
                }
                else
                {
                    FailInteraction();
                    Destroy(qteGO);
                    Destroy(qteSequence);
                }
            }
            
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
          
            myRoom = other.GetComponent<Room>();

        }
        else if (other.CompareTag("Player"))
        {
            posEmote = transform.GetChild(0).position + new Vector3(0, 1, -1);
            other.GetComponent<CharacterMouvement>().itemsNear.Add(this);
  
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
        anim.SetTrigger("Interact");
        
        qteGO = Instantiate(itemQTESequence);
        qteGO.transform.position = transform.GetChild(0).position;
        qteSequence = qteGO.GetComponent<QTESequence>();
    }

    public void FailInteraction()
    {
        anim.SetTrigger("Fail");
        isInteracting = false;
        // anim
    }

    public void SucceedInteraction()
    {
        anim.SetTrigger("Succeed");
        switch (type)
        {
            case TYPE.FAUTEUIL:


                break;
            case TYPE.CHAISE:

                break;
            case TYPE.TABLE:
                anim.SetBool("isRight", !anim.GetBool("isRight"));
                posEmote = transform.GetChild(0).position;
                break;
        }


        isInteracting = false;

        foreach (Hunter hunter in GameManager.Instance.hunters)
        {
            if(hunter.currentRoom == myRoom)
            {
                hunter.GetFear();
                hunter.lastItemAlert = this;
                hunter.ResetState();
                hunter.ActivateAction(Hunter.ACTION.ALERT);

            }
        }
    }
}
