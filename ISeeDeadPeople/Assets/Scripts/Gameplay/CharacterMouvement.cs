using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;
using UnityEngine.UI;

public class CharacterMouvement : MonoBehaviour
{
    // Start is called before the first frame update
    //[HideInInspector]
    public bool canMove = true;
    public float Speed;
    public float SpeedInWall;
    public bool InWall;
    public CharacterController Controller;
    public CinemachineVirtualCamera Camera;
    public FovEffects fovEffects;
    public GameObject Volume;
    public CharacterFlip Flip;
    private Player rewiredPlayer = null;

    Room myRoom;
    public List<Item> itemsNear = new List<Item>();
    int lastItemsNearCount;
    bool canInteract;
    Item itemClose;


    public GameObject DisplayEmoteInteract { get { return displayEmoteInteract; } set{ displayEmoteInteract = value; } }
    private GameObject displayEmoteInteract;
    private GameObject displayEmoteNoLight;

    public bool HasBeenDisplayedInteract { get { return hasBeenDisplayedInteract; } set { hasBeenDisplayedInteract = value; } }
    bool hasBeenDisplayedInteract = false;
    bool hasBeenDisplayedNoLight = false;

    public GameObject blastSequence = null;
    private GameObject blastQTESequence = null;
    private QTESequence blastQTE = null;

    [Header("MANA")]
    [SerializeField] float manaLoadSpeed;
    [SerializeField] int manaMax, currentMana, costBlast, costInteract;
    Animator animTxt;

    float t;
    float t1;
    [SerializeField] Text txt_mana, txt_removeMana;


    void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Player");
        t1 = t + 1 / manaLoadSpeed;
        animTxt = txt_mana.transform.parent.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        t += Time.deltaTime;

        if (t >= t1)
        {
            currentMana++;
            currentMana = Mathf.Clamp(currentMana, 0, manaMax);
            txt_mana.text = currentMana.ToString();
            t1 += 1 / manaLoadSpeed;
        }


        if (canMove && !InWall)
        {
            float Axex = Input.GetAxis("Horizontal");
            float Axey = Input.GetAxis("Vertical");

            Vector3 Axes = transform.right * Axex + transform.up * Axey;
            Axes = Axes * Speed * Time.deltaTime;
            Controller.Move(Axes);
            Flip.Flip(Axes);
        }

        FovApply(fovEffects, InWall);

        if (rewiredPlayer.GetButtonDown("CircleBT"))
        {
           
            if (myRoom.isOn)
            {
                if (currentMana >= costBlast)
                {
                    CreateBlastQTE();
                    blastQTE.Play();
                }

                else if (animTxt != null) 
                {
                    animTxt.SetTrigger("Trigger"); 
                }
            }
        }
        if (blastQTE != null && blastQTE.sequenceFinished)
        {
            Destroy(blastQTESequence);
        }

        if(itemsNear.Count != lastItemsNearCount)
        {
            switch (itemsNear.Count)
            {
                case 0:
                    canInteract = false;
                    if (displayEmoteInteract != null)
                    {
                        Destroy(displayEmoteInteract);
                        hasBeenDisplayedInteract = false;
                    }

                    if(displayEmoteNoLight != null)
                    {
                        Destroy(displayEmoteNoLight);
                        hasBeenDisplayedNoLight = false;
                    }
                    break;
                default:
                    canInteract = true;
                    break;
            }
        }
        lastItemsNearCount = itemsNear.Count;

        if (canInteract)
        {
           
                if (!myRoom.isOn)
                {
                    itemClose = GetCloserItem();

                    if (!itemClose.isInteracting && !itemClose.isCooldown)
                    {
                        if (!hasBeenDisplayedInteract)
                        {
                            if (hasBeenDisplayedNoLight)
                            {
                                Destroy(displayEmoteNoLight);
                                hasBeenDisplayedNoLight = false;
                            }

                            hasBeenDisplayedInteract = true;
                            displayEmoteInteract = EmoteManager.instance.PlayEmoteGameObject("Interact_Emote");
                            displayEmoteInteract.transform.position = itemClose.posEmote;
                        }



                        if (rewiredPlayer.GetButtonDown("SquareBT"))
                        {
                            if (currentMana >= costInteract)
                            {
                                itemClose.Interact();
                                Destroy(displayEmoteInteract);
                                hasBeenDisplayedInteract = false;
                            currentMana -= costInteract + 1;
                            txt_removeMana.text = costInteract.ToString();
                            animTxt.SetTrigger("Remove");
                        }
                            else animTxt.SetTrigger("Trigger");
                        }
                    }
                
                }

            else if (myRoom.isOn)
            {
                if (!hasBeenDisplayedNoLight)
                {
                    if (hasBeenDisplayedInteract)
                    {
                        Destroy(displayEmoteInteract);
                        hasBeenDisplayedInteract = false;
                    }

                    hasBeenDisplayedNoLight = true;
                    displayEmoteNoLight = EmoteManager.instance.PlayEmoteGameObject("LightOn_Emote");
                    displayEmoteNoLight.transform.position = itemClose.posEmote;
                }
            }
        } 
        else if (hasBeenDisplayedInteract)
        {
            Destroy(displayEmoteInteract);
            hasBeenDisplayedInteract = false;
        }
    }

    public void FovApply(FovEffects Stats, bool Condition)
    {
        if (Condition)
        {
            Camera.m_Lens.FieldOfView = Mathf.Lerp(Camera.m_Lens.FieldOfView, Stats.MaxFov, Stats.FovSpeedUp * Time.deltaTime);
        }
        else
        {
            Camera.m_Lens.FieldOfView = Mathf.Lerp(Camera.m_Lens.FieldOfView, Stats.MinFov, Stats.FovSpeedDown * Time.deltaTime);
        }
    }

    [System.Serializable]
    public struct FovEffects
    {
        public float MaxFov;
        public float MinFov;
        public float FovSpeedUp;
        public float FovSpeedDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            myRoom = other.GetComponent<Room>();
        }
    }

    public void Blast()
    {
        currentMana -= costBlast +1;
        txt_removeMana.text = costBlast.ToString();
        animTxt.SetTrigger("Remove");
        AudioManager.instance.Play("Breath");
         myRoom.TurnOff(transform.position);
        canMove = true;
        

        foreach(Hunter hunter in GameManager.Instance.hunters)
        {
            if(hunter.currentRoom == myRoom)
            {
                hunter.GetFear();
                hunter.ResetState();
                hunter.ActivateAction(Hunter.ACTION.WATCH_AROUND);

            }

        }


    }

    private Item GetCloserItem()
    {
        Item item = itemsNear[0];
        float distance1 = Mathf.Abs(item.transform.position.x - transform.position.x);
        for (int i = 1; i < itemsNear.Count; i++)
        {
            float distance2 = Mathf.Abs(itemsNear[i].transform.position.x - transform.position.x);
            if (distance2 < distance1)
            {
                item = itemsNear[i];
                Item temp = itemsNear[0];
                itemsNear[0] = item;
                itemsNear[i] = temp;
            }
        }

        return item;
    }

    private void CreateBlastQTE()
    {
        if (blastQTESequence == null)
        {
            blastQTESequence = Instantiate(blastSequence);
            blastQTE = blastQTESequence.GetComponent<QTESequence>();
            blastQTESequence.transform.parent = transform;
        }
    }

}