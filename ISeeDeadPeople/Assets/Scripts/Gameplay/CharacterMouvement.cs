using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Rewired;

public class CharacterMouvement : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed;
    public float SpeedInWall;
    public bool InWall;
    public CharacterController Controller;
    public CinemachineVirtualCamera Camera;
    public FovEffects fovEffects;
    public GameObject Volume;
    private Player rewiredPlayer = null;
    public QTESequence sequenceSouffle;
    void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!InWall)
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
            sequenceSouffle.Play();
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
}