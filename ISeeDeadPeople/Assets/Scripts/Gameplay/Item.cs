using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public enum TYPE { FAUTEUIL, CHAISE, TABLE, PENDULE }
    public TYPE type = TYPE.FAUTEUIL;
    Room myRoom = null;

    [HideInInspector]
    public Vector3 posEmote;

    public bool isInteracting;
    Animator anim;

    public GameObject itemQTESequence = null;
    private GameObject qteGO = null;
    private QTESequence qteSequence = null;

    private float internDestroyFeedbackTimer = 0f;
    private bool hasSpawnFeedback = false;
    public bool isCooldown;
    public float cooldown;
    float t = 0;
    [SerializeField]
    Image circle1, circl2;
    [SerializeField] GameObject pause;




    private void Start()
    {
        pause.SetActive(false);
        anim = GetComponent<Animator>();
        posEmote = transform.GetChild(0).position + new Vector3(0, 1, -1);
    }

    private void Update()
    {

        if (isInteracting)
        {
            if (qteGO != null)
            {
                if (qteSequence != null)
                {
                    if (qteSequence.sequenceFinished)
                    {
                        SucceedInteraction();
                        Destroy(qteGO);
                        Destroy(qteSequence);
                    }

                    else if (qteSequence.sequenceLost)
                    {
                        FailInteraction();
                        Destroy(qteGO);
                        Destroy(qteSequence);
                    }
                }

            }

            if (GameManager.Instance.player.itemsNear.Count < 0)
            {
                Debug.Log("Player left the object");
                FailInteraction();

                if (qteGO != null)
                {
                    if (qteSequence != null)
                    {
                        Destroy(qteGO);
                        Destroy(qteSequence);
                    }
                }

                if (isCooldown)
                {
                    t += Time.deltaTime;
                    circle1.fillAmount = t / cooldown;
                    circl2.fillAmount = t / cooldown;
                    if (t >= cooldown)
                    {
                        isCooldown = false;
                        StartCoroutine(EndPause());
                    }

                }
            }
        }

        //if(internDestroyFeedbackTimer <= 5 && hasSpawnFeedback)
        //{
        //    internDestroyFeedbackTimer += Time.deltaTime;
        //} 
        //else if (internDestroyFeedbackTimer >= 5)
        //{
        //    DestroyInteractFeedback();
        //    hasSpawnFeedback = false;
        //}
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

        if (anim != null)
        {
            anim.SetTrigger("Interact");
        }

        qteGO = Instantiate(itemQTESequence);
        qteGO.transform.position = transform.GetChild(0).position;
        qteSequence = qteGO.GetComponent<QTESequence>();

        //internDestroyFeedbackTimer = 0;
        //hasSpawnFeedback = true;
        DestroyInteractFeedback();
    }

    public void DestroyInteractFeedback()
    {
        if (GameManager.Instance.player.DisplayEmoteInteract != null)
        {
            GameManager.Instance.player.HasBeenDisplayedInteract = false;
            Destroy(GameManager.Instance.player.DisplayEmoteInteract);
        }
    }

    public void FailInteraction()
    {
        anim.SetTrigger("Fail");
        isInteracting = false;
        Debug.Log("Qte failed");
        // anim
        StartCooldown();
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

            case TYPE.PENDULE:
                posEmote = transform.GetChild(0).position;
                break;
        }


        isInteracting = false;
        print(myRoom);
        foreach (Hunter hunter in GameManager.Instance.hunters)
        {
            if (hunter.currentRoom == myRoom)
            {
                hunter.GetFear();
                hunter.lastItemAlert = this;
                hunter.ResetState();
                hunter.ActivateAction(Hunter.ACTION.ALERT);

            }
        }
        StartCooldown();
    }

    void StartCooldown()
    { 

        //anim.ResetTrigger("Fail");

        StartCoroutine(StartPause());
        t = 0;
        
    }
    IEnumerator StartPause()
    {
        yield return new WaitForSeconds(1.5f);
        isCooldown = true;
        pause.SetActive(true);
       
    }
    IEnumerator EndPause()
    {
        pause.GetComponent<Animator>().SetTrigger("Disappear");

        yield return new WaitForSeconds(1f);


        pause.SetActive(false);
    }

}
