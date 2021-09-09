using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;

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
    private Player rewiredPlayer = null;

    Room myRoom;
    public List<Item> itemsNear = new List<Item>();
    int lastItemsNearCount;
    bool canInteract;
    Item itemClose;

    private GameObject displayEmote;
    bool hasBeenDisplayed = false;

    public GameObject blastSequence = null;
    private GameObject blastQTESequence = null;
    private QTESequence blastQTE = null;
    void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !InWall)
        {
            float Axex = Input.GetAxis("Horizontal");
            float Axey = Input.GetAxis("Vertical");

            Vector3 Axes = transform.right * Axex + transform.up * Axey;
            Axes = Axes * Speed * Time.deltaTime;
            Controller.Move(Axes);
        }

        FovApply(fovEffects, InWall);

        if (rewiredPlayer.GetButtonDown("CircleBT"))
        {

            
            bool isCandleOn = false;
            foreach(Candle candle in myRoom.myCandles)
            {
                if (candle.isOn) isCandleOn = true;
            }
            if (isCandleOn)
            {
                CreateBlastQTE();
                blastQTE.Play();
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
                    if (displayEmote != null)
                    {
                        Destroy(displayEmote);
                        hasBeenDisplayed = false;
                    }
                    break;
                default:
                    canInteract = true;
                    break;
            }
        }
        lastItemsNearCount = itemsNear.Count;

        if (canInteract && !myRoom.isOn)
        {
            itemClose = GetCloserItem();

            if (!hasBeenDisplayed) {
                hasBeenDisplayed = true;
                displayEmote = EmoteManager.instance.PlayEmoteGameObject("Interact_Emote");
                displayEmote.transform.position = itemClose.posEmote;
            }

           

            if (rewiredPlayer.GetButtonDown("SquareBT"))
            {
                itemClose.Interact();
                itemsNear.Remove(itemClose);
            }
        } else if (hasBeenDisplayed)
        {
            Destroy(displayEmote);
            hasBeenDisplayed = false;
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
       
         myRoom.TurnOff(transform.position);
        

        foreach(Hunter hunter in GameManager.Instance.hunters)
        {

            if(hunter.currentRoom == myRoom)
            {
                hunter.GetFear();
                hunter.ResetState();
                hunter.ActivateAction(Hunter.ACTION.WATCH_AROUND);

            }

        }


        canMove = true;
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