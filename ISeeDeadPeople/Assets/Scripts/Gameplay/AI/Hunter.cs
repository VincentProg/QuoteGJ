using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Hunter : MonoBehaviour
{
    public int id;

    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;
    bool isDead;

    public Room currentRoom;
    public enum ACTION {GO_TO_CANDLE,TURN_ON_CANDLE,WATCH_AROUND, ALERT, FLEE}
    private ACTION currentAction;
    Room targetRoom = null;

    public Item lastItemAlert;

    public int fearMax = 3;
    public int currentFear = 0;
    private Image FearImage;

    private bool isDoingAction = false;

    bool isPatroling;
    List<Vector2> points = new List<Vector2>();

    [Header("ACTIONS DURATION")] [SerializeField]
    private float turningOnCandle;
    [SerializeField] private float afterTurningOnCandle, surprisedBeforePatrol, afterPatrol, scaredFrousse2_1, scaredFrousse2_2, scaredFrousse1_1, scaredFrousse1_2, scaredFrousse0_1, scaredFrousse0_2 ;

    [Header("Fear points")]
    public int blast;

    [Header("Animation")]
    public Animator Anim;

    bool isScared;

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        id = GameManager.Instance.hunters.Count;
        GameManager.Instance.hunters.Add(this);
    }

    private void Start()
    {
        currentFear = fearMax;
        currentAction = ACTION.GO_TO_CANDLE;
        ActivateAction(currentAction);
        FearImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        FearImage.color = new Color32(212, 212, 212, 255);
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //    MoveTo(movePositionTransform.position);

        //if (Input.GetKeyDown("a"))
        //{
        //    lastAlertPos = movePositionTransform.position;
        //    RealizeAction(ACTION.ALERT);
        //}
        //if (Input.GetKeyDown("f"))
        //{
        //    AddFear(20);
        //}
        if (!isDead)
        {
            if (!IsMoving())
            {
                Anim.SetBool("Idle", true);
            } else Anim.SetBool("Idle", false);

            if (!IsMoving() && !isDoingAction)
            {
                switch (currentAction)
                {
                    case ACTION.GO_TO_CANDLE:
                        if (targetRoom != null)
                        {
                            if (!targetRoom.isOn)
                            {
                                print("turn on");
                                ActivateAction(ACTION.TURN_ON_CANDLE);
                            }
                            else
                            {
                                print("go away");
                                ActivateAction(ACTION.GO_TO_CANDLE);
       
                            }
                        }

                        else
                        {
                            ActivateAction(ACTION.GO_TO_CANDLE);
                        }
                        break;
                    case ACTION.TURN_ON_CANDLE:
                        ActivateAction(ACTION.GO_TO_CANDLE);
                        break;
                    case ACTION.WATCH_AROUND:
                        ActivateAction(ACTION.GO_TO_CANDLE);
                        break;
                    case ACTION.ALERT:
                        ActivateAction(ACTION.GO_TO_CANDLE);


                        break;
                    case ACTION.FLEE:
                        ;
                        break;
                }
            }


            if (!IsMoving() && isPatroling)
            {
                if (points.Count > 0)
                {
                    MoveTo(points[0]);
                    points.RemoveAt(0);
                }
                else
                {
                    isPatroling = false;
                    StartCoroutine(WaitTimeAfterPatrol());
                }
            }

            if (isScared)
            {
                print(IsMoving());
            }
            if (!IsMoving() && isScared)
            {

                isScared = false;
                StartCoroutine(Scared(false));
            }
        }

    }

    public void MoveTo(Vector3 position)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = position;
    }

    private void OnDrawGizmos()
    {
        if(targetRoom != null)
        Gizmos.DrawLine(targetRoom.transform.position,transform.position);
    }

    public void ActivateAction(ACTION action)
    { 
        if(!isDead)
        navMeshAgent.isStopped = true;

        switch (action)
        {
            case ACTION.GO_TO_CANDLE:
                currentAction = ACTION.GO_TO_CANDLE;
                targetRoom = null;
                List<Room> tempRooms = new List<Room>(GameManager.Instance.rooms);       

                while (targetRoom == null && tempRooms.Count>0)
                {
                    int rand = Random.Range(0, tempRooms.Count);
                    Room room = tempRooms[rand];
                    if(!room.isOn && room != currentRoom)
                    {
                        targetRoom = room;
                    }
                    else
                    {
                        tempRooms.Remove(room);;
                    }

                }

                if (targetRoom != null)
                {
                    MoveTo(targetRoom.transform.position);
                } else
                {
                    MoveToRandomRoom();
                }


                
                break;
            case ACTION.TURN_ON_CANDLE:
                currentAction = ACTION.TURN_ON_CANDLE;
                isDoingAction = true;
                StartCoroutine(TurnOn_Candle());
                break;
            case ACTION.WATCH_AROUND:
                currentAction = ACTION.WATCH_AROUND;
                isDoingAction = true;
                SpriteRenderer candleZone = currentRoom.transform.GetChild(0).GetComponent<SpriteRenderer>();
                float minX = candleZone.bounds.min.x;
                float maxX = candleZone.bounds.max.x;
                float minY = candleZone.bounds.min.y;
                float maxY = candleZone.bounds.max.y;

                Vector3 point0 = new Vector3(minX, (minY + maxY) / 2, candleZone.transform.position.z);
                Vector3 point1 = new Vector3(maxX, (minY + maxY) / 2, candleZone.transform.position.z);

                int rand2 = Random.Range(0, 2);
                if (rand2 == 0){ points.Add(point0); points.Add(point1); }
                else { points.Add(point1); points.Add(point0); }

                StartCoroutine(SurprisedBeforePatrol());
                
                break;
            case ACTION.ALERT:
                currentAction = ACTION.ALERT;
                isDoingAction = true;
                StartCoroutine(Scared(true));
                break;
            case ACTION.FLEE:
                currentAction = ACTION.FLEE;
                StartCoroutine(ResumeNavMesh(2));
                break;
        }

    }

    public void ResetState()
    {
        StopAllCoroutines();
        Anim.Play("Terrified");
        targetRoom = null;
        isDoingAction = false;
        isPatroling = false;
        isScared = false;
    }

    public void GetFear()
    {
        currentFear--;
        byte col = (byte)(((float)currentFear - 1) * 120);
        FearImage.color = FearImage.color = new Color32(164, col, 0, 255);
        FearImage.GetComponent<Animator>().SetTrigger("Trigger");
        if (currentFear <= 0)
        {
            AudioManager.instance.Play("FearMax");
            Death();
        }else
        {
            AudioManager.instance.Play("Fear");
        }
    }

    private void Death()
    {
        print("Dead");
        isDead = true;
        Destroy(gameObject);
        
    }

    IEnumerator ResumeNavMesh( int s)
    {
        yield return new WaitForSeconds(s);
        navMeshAgent.isStopped = false;
    }

    private bool IsMoving()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance || navMeshAgent.isStopped)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return false;
                }
            }
        }

        return true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
           Room room = other.GetComponent<Room>();
            currentRoom = room;
        }
    }

    IEnumerator TurnOn_Candle()
    {
        Anim.SetBool("Interact", true);
        yield return new WaitForSeconds(turningOnCandle);
        targetRoom.TurnOn();
        targetRoom = null;
        Anim.SetBool("Interact", false);
        yield return new WaitForSeconds(afterTurningOnCandle);
        isDoingAction = false;
    }

    private void MoveToRandomRoom()
    {
        List<Room> tempRooms = new List<Room>(GameManager.Instance.rooms);
        tempRooms.Remove(currentRoom);
        int rand = Random.Range(0, tempRooms.Count);
      
        MoveTo(tempRooms[rand].transform.position);
    }

    IEnumerator SurprisedBeforePatrol()
    {
        yield return new WaitForSeconds(surprisedBeforePatrol);
        isPatroling = true;
    }

    IEnumerator WaitTimeAfterPatrol()
    {
      
        yield return new WaitForSeconds(afterPatrol);
        isDoingAction = false;
   
    }

    IEnumerator Scared(bool isFirstTime)
    {
        float time = 0;
        if (isFirstTime)
        {

            switch (currentFear)
            {
                case 2:
                    time = scaredFrousse2_1;
                    break;
                case 1:
                    time = scaredFrousse1_1;
                    break;
                case 0:
                    time = scaredFrousse0_1;

                    break;
            } // get time
            EmoteManager.instance.PlayEmoteWithTransform("Confused_Emote", transform);
            yield return new WaitForSeconds(time);
            MoveTo(lastItemAlert.transform.GetChild(0).position);
            isScared = true;
        } else
        {
            switch (currentFear)
            {
                case 2:
                    time = scaredFrousse2_2;
                    break;
                case 1:
                    time = scaredFrousse1_2;
                    break;
                case 0:
                    time = scaredFrousse0_2;

                    break;
                    
            } // get time
            yield return new WaitForSeconds(time);
            isDoingAction = false;
            print("end");
        }
      
       

    }
}
