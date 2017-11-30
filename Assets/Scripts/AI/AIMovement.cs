using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class AIMovement : MonoBehaviour {
    NavMeshAgent navagent;
    Animator aiAnimator;

    public float agentCaptureRadius = 1.5f;

    public bool isComplete(Vector3 target)
    {
        get { return Vector3.Magnitude(transform.position - target) < agentCaptureRadius; }
    }

    Vector2 smoothDeltaPosition = Vector2.zero;
	Vector2 velocity = Vector2.zero;
    public bool shouldMove;
    public bool walk;
    public float isCompleteDistance = 1.5f;

	void Start () {
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.updatePosition = false;
        anim.SetBool("walk", walk);
    }

    public void setWayPoint(Transform newWaypoint)
    {
        navagent.destination = newWaypoint.position;
    }

    public void setWaypoints(Transform[] newWaypoints)
    {
        if (newWaypoints == null)
        {
            return;
        }

        waypoints = new Transform[newWaypoints.Length];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = newWaypoints[i];
        }
    }




    void Update () {
        anim.SetBool("walk", walk);
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

		// Map 'worldDeltaPosition' to local space
		float dx = Vector3.Dot (transform.right, worldDeltaPosition);
		float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
		Vector2 deltaPosition = new Vector2 (dx, dy);

		// Low-pass filter the deltaMove
		float smooth = Mathf.Min(1.0f, Time.deltaTime/0.5f);
		smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

		// Update velocity if delta time is safe
		if (Time.deltaTime > 1e-5f)
			velocity = smoothDeltaPosition / Time.deltaTime;

		shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;
        velocity.x = Mathf.Clamp(velocity.x, -0.5f, 0.5f);
        velocity.y = Mathf.Clamp(velocity.y, 0f, 2.7f);
        velocity.x = Mathf.Lerp(velocity.x, agent.velocity.x,5f * Time.deltaTime);
        velocity.y = Mathf.Lerp(velocity.y, agent.velocity.y, 5 * Time.deltaTime);


        // Update animation parameters
        
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", velocity.x);

       
        anim.SetFloat("vely", velocity.y);
        

		LookAt lookAt = GetComponent<LookAt> ();
		if (lookAt)
			lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;


        //		// Pull character towards agent
       // if (worldDeltaPosition.magnitude > agent.radius)
        //	transform.position = agent.nextPosition - 0.9f*worldDeltaPosition;

        //		// Pull agent towards character
        if (worldDeltaPosition.magnitude > agent.radius)
			agent.nextPosition = transform.position + 0.9f*worldDeltaPosition;
	}

	void OnAnimatorMove () {
        Quaternion rotation = transform.rotation;
        // Update postion to agent position
        //		transform.position = agent.nextPosition;

        // Update position based on animation movement using navigation surface height
        Vector3 position = anim.rootPosition;
		position.y = agent.nextPosition.y;
        transform.position = position;
		
	}
}
