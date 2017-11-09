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
    private LocomotionSimpleAgent locomotion;
    private String patrolState = "wayPoint1";
    public enum Behavior
    {
        ClickToMove, 
        Patrol,
        Random
    }
    public Behavior behavior = Behavior.Patrol;

	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
        locomotion = GetComponent<LocomotionSimpleAgent>();
        anim = GetComponent<Animator>();

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
                print(Vector3.Magnitude(transform.position - wayPoint2.transform.position));

            } else if (patrolState == "waiting to complete 1")
            {
                if (Vector3.Magnitude(transform.position - wayPoint1.transform.position) < 1.5f)
                {
                    print("wayPoint2");
                    locomotion.setWayPoint(wayPoint2.transform.position);
                    patrolState = "waiting to complete 2";

                }
                

            } else if (patrolState == "waiting to complete 2")
            {
                if (Vector3.Magnitude(transform.position - wayPoint2.transform.position) < 1.5f)
                {
                    print("wayPoint1 (new state)");
                    locomotion.setWayPoint(wayPoint1.transform.position);
                    patrolState = "waiting to complete 1";

                }

            }
            
            

        }


	}
}
