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

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

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
                    aiState = AIState.chaseStationaryWaypoint;
                    currWaypoint = 0;
                    navMeshAgent.SetDestination(stationaryWaypoints[0].transform.position);
                }
                else
                    navMeshAgent.SetDestination(movingWaypoint.transform.position);
                break;

            case AIState.chaseStationaryWaypoint:
                if (navMeshAgent.remainingDistance == 0 && !navMeshAgent.pathPending)
                {
                    if (currWaypoint == n - 1)
                    {
                        aiState = AIState.chaseMovingWaypoint;
                        currWaypoint = 0;
                        navMeshAgent.SetDestination(movingWaypoint.transform.position);
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
}
