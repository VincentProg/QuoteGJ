using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;

public class CharacterMouvement : MonoBehaviour
{
    // Start is called before the first frame update
    private bool canMove = true;
    public float Speed;
    public float SpeedInWall;
    public bool InWall;
    public CharacterController Controller;
    public CinemachineVirtualCamera Camera;
    public FovEffects fovEffects;
    public GameObject Volume;
    private Player rewiredPlayer = null;
    public QTESequence sequenceSouffle;

    Room myRoom;
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
            if(isCandleOn)
            sequenceSouffle.Play();
        }

        if (Input.GetKeyDown("space"))
        {
            Blast();
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
            print("triggerrr");
        }
    }

    void Blast()
    {
       
        foreach(Candle candle in myRoom.myCandles)
        {
            candle.turnOff(candle.transform.position.x - transform.position.x <= 0);
        }

        foreach(Hunter hunter in GameManager.Instance.hunters)
        {

            if(hunter.currentRoom == myRoom)
            {
                hunter.ResetState();
                hunter.ActivateAction(Hunter.ACTION.WATCH_AROUND);
            }

        }


        canMove = true;
    }

}