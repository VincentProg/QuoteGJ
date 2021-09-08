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

    public Room currentRoom;
    public enum ACTION {GO_TO_CANDLE,TURN_ON_CANDLE,WATCH_AROUND, ALERT, FLEE}
    private ACTION currentAction;
    Candle targetCandle = null;

    public Vector2 lastAlertPos;

    public int fearMax = 100;
    public int currentFear = 0;
    private Image FearImage;

    private bool isDoingAction = false;

    bool isPatroling;
    List<Vector2> points = new List<Vector2>();

    [Header("ACTIONS DURATION")] [SerializeField]
    private float turningOnCandle;
    [SerializeField] private float afterTurningOnCandle, surprisedBeforePatrol, afterPatrol; 

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        id = GameManager.Instance.hunters.Count;
        GameManager.Instance.hunters.Add(this);
    }

    private void Start()
    {
        currentAction = ACTION.GO_TO_CANDLE;
        ActivateAction(currentAction);
        FearImage = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        FearImage.fillAmount = 0;
        
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

        if (!IsMoving() && !isDoingAction)
        {
            switch (currentAction )
            {
                case ACTION.GO_TO_CANDLE:
                    if (targetCandle != null)
                    {
                        if (!targetCandle.isOn)
                        {
                            ActivateAction(ACTION.TURN_ON_CANDLE);
                        }
                        else
                        {
                            Candle newTarget = null;
                            for (int i = 0; i < currentRoom.myCandles.Count; i++)
                            {
                                if (!currentRoom.myCandles[i].isOn)
                                {
                                    newTarget = currentRoom.myCandles[i];
                                    break;
                                }
                            }
                            if (newTarget != null)
                            {
                                targetCandle = newTarget;
                                MoveTo(targetCandle.transform.position);
                            }
                            else
                            {
                                targetCandle = null;
                                ActivateAction(ACTION.GO_TO_CANDLE);
                            }
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

                    break;
                case ACTION.ALERT:

                    break;
                case ACTION.FLEE:
;
                    break;
            }
        }

        if (!IsMoving() && isPatroling )
        {
            if (points.Count>0)
            {
                MoveTo(points[0]);
            } else
            {
                isPatroling = false;
                StartCoroutine(WaitTimeAfterPatrol());
            }
        }

    }

    public void MoveTo(Vector3 position)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.destination = position;
    }

    public void ActivateAction(ACTION action)
    { 
        navMeshAgent.isStopped = true;

        switch (action)
        {
            case ACTION.GO_TO_CANDLE:
                currentAction = ACTION.GO_TO_CANDLE;
                List<Candle> tempCandles = new List<Candle>(GameManager.Instance.allCandles);       

                while (targetCandle == null && tempCandles.Count>0)
                {
                    int rand = Random.Range(0, tempCandles.Count);
                    Candle candle = tempCandles[rand];
                    if(!candle.isOn && candle.room != currentRoom)
                    {
                        targetCandle = candle;
                    }
                    else
                    {
                        tempCandles.Remove(candle);;
                    }

                }

                if (targetCandle != null)
                {
                    MoveTo(targetCandle.transform.position);
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
                if(rand2 == 0)
                {
                    points.Add(point0); points.Add(point1); points.Add(point0);
                }
                else points.Add(point1); points.Add(point0); points.Add(point1);

                StartCoroutine(SurprisedBeforePatrol());
                
                break;
            case ACTION.ALERT:
                currentAction = ACTION.ALERT;
                navMeshAgent.destination = lastAlertPos;
                StartCoroutine(ResumeNavMesh(2));
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
        targetCandle = null;
        isDoingAction = false;
        isPatroling = false;
    }

    public void AddFear(int fear)
    {
        currentFear += fear;
        FearImage.fillAmount = (float)currentFear / fearMax;
        print (currentFear);
        if (currentFear >= fearMax)
        {
            Death();
        }
    }

    private void Death()
    {
        print("Dead");
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
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
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
        
        yield return new WaitForSeconds(turningOnCandle);
        targetCandle.turnOn();
        targetCandle = null;
        yield return new WaitForSeconds(afterTurningOnCandle);
        print("rewalk");
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
}
