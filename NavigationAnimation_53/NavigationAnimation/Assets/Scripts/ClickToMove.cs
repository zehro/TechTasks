// ClickToMove.cs
using System;
using UnityEngine;

[RequireComponent (typeof (UnityEngine.AI.NavMeshAgent))]
public class ClickToMove : MonoBehaviour {
	RaycastHit hitInfo = new RaycastHit();
	UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    public GameObject wayPoint;
    public GameObject wayPoint1;
    public GameObject wayPoint2;
    public GameObject movingWaypoint;
    public float futureTLimit = 1.5f;
    private MovingCubeScript movingCubeScript;
    private LocomotionSimpleAgent locomotion;
    public GameObject[] wayPointList;
    private String patrolState = "wayPoint1";
    private int currWaypointIndex = -1;
    public enum Behavior
    {
        ClickToMove, 
        Patrol,
        Random,
        MovingTarget,
        Idle,
        PickARandomWaypoint
    }
    public Behavior behavior = Behavior.Patrol;

	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        locomotion = GetComponent<LocomotionSimpleAgent>();
        anim = GetComponent<Animator>();
        movingCubeScript = movingWaypoint.GetComponent<MovingCubeScript>();

    }
	void Update () {

        if (behavior == Behavior.ClickToMove)
        {
            locomotion.shouldWalk(false);
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out hitInfo))
                    agent.destination = hitInfo.point;
            }

        } else if (behavior == Behavior.Patrol)
        {
            locomotion.shouldWalk(true);
            if (patrolState == "wayPoint1")
            {
                print("wayPoint1");
                locomotion.setWayPoint(wayPoint1.transform.position);
                patrolState = "waiting to complete 1";
                //print(Vector3.Magnitude(transform.position - wayPoint2.transform.position));

            } else if (patrolState == "waiting to complete 1")
            {
                if (locomotion.isComplete(wayPoint1.transform.position))
                {
                    print("wayPoint2");
                    locomotion.setWayPoint(wayPoint2.transform.position);
                    patrolState = "waiting to complete 2";

                }
                

            } else if (patrolState == "waiting to complete 2")
            {
                if (locomotion.isComplete(wayPoint2.transform.position))
                {
                    print("wayPoint1 (new state)");
                    locomotion.setWayPoint(wayPoint1.transform.position);
                    patrolState = "waiting to complete 1";

                }

            }
            
            

        } else if (behavior == Behavior.MovingTarget)
        {
            //anim.SetBool("walk", false);
            locomotion.shouldWalk(false);

            if (!locomotion.isComplete(movingWaypoint.transform.position))
            {
                
                //dist between moving object and NPC 
                float dist = (movingWaypoint.transform.position - transform.position).magnitude;

                //a super simple mapping of dist to time in sec:
                //feel free to come up with something better
                //you can also use the equations from the Gamasutra projectile article
                //detailed in the assignment PDF for a more accurate method
                float futureT = 0.1f * dist;
                //print(futureT);

                futureT = Mathf.Min(futureT, futureTLimit); //limit on how far ahead to look
                                                            //timeBetweenPredictions = futureT * 900f;
                                                            //extrapolate assuming constant Vel and the futureT intercept estimate
                Vector3 futureMoverPos = movingWaypoint.transform.position + new Vector3(movingCubeScript.SpeedX, 0, movingCubeScript.SpeedZ) * futureT;

                //update the target waypoint
                locomotion.setWayPoint(futureMoverPos);
  

            } else
            {
                behavior = Behavior.Idle;
            }


        } else if (behavior == Behavior.PickARandomWaypoint)
        {
            if (currWaypointIndex == -1 || locomotion.isComplete(wayPointList[currWaypointIndex].transform.position))
            {
                int waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
                while (waypointIndex == currWaypointIndex)
                {
                    waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);

                }
                currWaypointIndex = waypointIndex;
                locomotion.setWayPoint(wayPointList[currWaypointIndex].transform.position);

            }
            
        } 


    }
}
