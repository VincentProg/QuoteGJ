using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ViewCastInfo
{
    public bool hit;
    public Vector3 point;
    public float dist;
    public float angle;

    public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
    {
        hit = _hit;
        point = _point;
        dist = _dist;
        angle = _angle;
    }
}

public struct EdgeInfo
{
    public Vector3 pointA;
    public Vector3 pointB;

    public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
    {
        pointA = _pointA;
        pointB = _pointB;
    }
}

public class FieldofView : MonoBehaviour
{
    [Header("Defeat Conditions")]
    public float timeBeforeLose = 2.5f;

    private float timerLose = 0f;

    [Header("Vision Setup InRuntime")]
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [Header("Target Setup")]
    public List<Transform> visibleTargets = new List<Transform>();
    public bool isSeeing = false;

    public GameObject viewVisualisation;

    [Header("Vision Mesh Setup")]
    public float meshResolution = 2f;
    public int edgeResolveIteration = 3;
    public float edgeDistThreshold = 0.5f;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    GameObject displayDebug = null;
    bool hasBeenDisplayed = false;


    private void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    private void FixedUpdate()
    {
        FindVisibleTarget();
    }

    private void LateUpdate()
    {
            DrawFieldOfView();
            See();
    }

    void FindVisibleTarget()
    {
        visibleTargets.Clear();
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider targetColl in targetInViewRadius)
        {
            Transform target = targetColl.transform;

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    if (!visibleTargets.Contains(target))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }

            else
            {
                visibleTargets.Remove(target);
            }
        }
    }

    void See()
    {
        if (visibleTargets.Count > 0)
        {
            Debug.Log($"Hunter : {transform.name} is seeing the ghost");

            if (!hasBeenDisplayed)
            {
                displayDebug = EmoteManager.instance.PlayEmoteGameObject("ScaredGhost_Emote");
                displayDebug.transform.parent = visibleTargets[0].transform;
                displayDebug.transform.position = new Vector3(visibleTargets[0].position.x, visibleTargets[0].position.y + .7f, -2);
                hasBeenDisplayed = true;
            }

            isSeeing = true;
        }
        else
        {
            isSeeing = false;

            if(displayDebug != null)
            {
                Destroy(displayDebug);
                hasBeenDisplayed = false;
            }

            timerLose = timeBeforeLose;
        }

        if (isSeeing)
        {
            if (timerLose > 0)
            {
                timerLose -= Time.deltaTime;
            }

            else
            {
                viewVisualisation.SetActive(false);
            }
        }
    }
    void UnSee()
    {
        viewVisualisation.SetActive(false);
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;

        List<Vector3> viewPoints = new List<Vector3>();

        ViewCastInfo oldViewCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDistThresholdExceeded = Mathf.Abs(oldViewCast.dist - newViewCast.dist) > edgeDistThreshold;

                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDistThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }

                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }

            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;

        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();
        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIteration; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDistThresholdExceeded = Mathf.Abs(minViewCast.dist - newViewCast.dist) > edgeDistThreshold;


            if (newViewCast.hit == minViewCast.hit && !edgeDistThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }
        return new EdgeInfo(minPoint, maxPoint);
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

