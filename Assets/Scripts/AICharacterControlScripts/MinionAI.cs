using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAI : MonoBehaviour
{
    public enum AIState
    {
        chaseStationaryWaypoint,
        chaseMovingWaypoint
    };

    public AIState aiState;
    public Animator anim;
    public GameObject[] stationaryWaypoints;
    public GameObject movingWaypoint;
    public int currWaypoint;
    public Rigidbody rb;
    public GameObject destinationTracker;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        destinationTracker.SetActive(false);

        aiState = AIState.chaseStationaryWaypoint;
        currWaypoint = 0;
        navMeshAgent.SetDestination(stationaryWaypoints[0].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("vely", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
        
        int n = stationaryWaypoints.Length;
        
        switch (aiState)
        {
            case AIState.chaseMovingWaypoint:
                if (navMeshAgent.remainingDistance == 0 && !navMeshAgent.pathPending)
                {
                    if (findDistance() <= 1f)
                    {
                        aiState = AIState.chaseStationaryWaypoint;
                        currWaypoint = 0;
                        navMeshAgent.SetDestination(stationaryWaypoints[0].transform.position);
                        destinationTracker.SetActive(false);
                    }
                    else
                    {
                        destinationTracker.transform.position = forecastWaypointPosition();
                        navMeshAgent.SetDestination(destinationTracker.transform.position);
                    }
                }
                break;

            case AIState.chaseStationaryWaypoint:
                if (navMeshAgent.remainingDistance == 0 && !navMeshAgent.pathPending)
                {
                    if (currWaypoint == n - 1)
                    {
                        aiState = AIState.chaseMovingWaypoint;
                        currWaypoint = 0;
                        destinationTracker.transform.position = forecastWaypointPosition();
                        destinationTracker.SetActive(true);
                        navMeshAgent.SetDestination(destinationTracker.transform.position);
                    }
                    else
                    {
                        currWaypoint++;
                        navMeshAgent.SetDestination(stationaryWaypoints[currWaypoint].transform.position);
                    }
                }
                break;
        }
    }
 
    private Vector3 forecastWaypointPosition()
    {
        float dist = findDistance();
        float requiredTime = dist / rb.velocity.magnitude;
        requiredTime = Mathf.Clamp(requiredTime, 0f, 4.5f);

        Vector3 movingWaypointVelocity = movingWaypoint.GetComponent<VelocityReporter>().velocity;
        Vector3 forecastedWaypointPosition = movingWaypoint.transform.position + (requiredTime * movingWaypointVelocity);

        return forecastedWaypointPosition;
    }

    private float findDistance()
    {
        float dist = Vector3.Distance(movingWaypoint.transform.position, transform.position);
        return dist;
    }
}
