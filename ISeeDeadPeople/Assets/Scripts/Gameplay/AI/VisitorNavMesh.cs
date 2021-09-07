using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisitorNavMesh : MonoBehaviour
{
    [SerializeField] private Transform movePositionTransform;

    private NavMeshAgent navMeshAgent;

    public enum ACTION { OPEN_DOOR, WATCH_AROUND, ALERT, FLEE}

    public Vector2 lastAlertPos;

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            MoveTo(movePositionTransform.position);

        if (Input.GetKeyDown("a"))
        {
            lastAlertPos = movePositionTransform.position;
            RealizeAction(ACTION.ALERT);
        }
    }

    public void MoveTo(Vector3 position)
    {
        navMeshAgent.destination = position;
    }

    public void RealizeAction(ACTION action)
    { 
        navMeshAgent.isStopped = true;

        switch (action)
        {
            case ACTION.OPEN_DOOR:
                StartCoroutine(ResumeNavMesh(2));
                break;
            case ACTION.WATCH_AROUND:
                StartCoroutine(ResumeNavMesh(4));
                break;
            case ACTION.ALERT:
                navMeshAgent.destination = lastAlertPos;
                StartCoroutine(ResumeNavMesh(2));
                break;
            case ACTION.FLEE:
                StartCoroutine(ResumeNavMesh(2));
                break;
        }

    }

    IEnumerator ResumeNavMesh( int s)
    {
        yield return new WaitForSeconds(s);
        navMeshAgent.isStopped = false;

    }

}
