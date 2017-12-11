using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class GoombaStateMachine : MonoBehaviour {
    private RaycastHit hitInfo = new RaycastHit();
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    public GameObject player;
    private Animator playerAnimator;
    public float futureTLimit = 1.5f;

    //private MovingCubeScript movingCubeScript;
    private AIMovement locomotion;

    public GameObject[] wayPointList;
    private int currWaypointIndex = -1;
    private Boolean dead = false;
    private Boolean isFalling = false;

    public enum Behavior {
        Patrol,
        Chase
    }

    public Behavior behavior = Behavior.Patrol;
    private GameObject nextPoint;

    [SerializeField]
    private ParticleSystem ps;

    [SerializeField]
    private AudioClip flattened;

    [SerializeField]
    private AudioClip die;

    [SerializeField]
    private AudioSource source;

    private Coroutine death;

    private void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        locomotion = GetComponent<AIMovement>();
        anim = GetComponent<Animator>();
        playerAnimator = player.GetComponent<Animator>();
        //movingCubeScript = movingWaypoint.GetComponent<MovingCubeScript>();
    }

    private void Update() {
        if (playerAnimator.GetBool("isFalling")) {
            jumpOn();
        }
        transition();
    }

    private void transitionToStatePatrol() {
        if (currWaypointIndex == -1 || locomotion.reachedWaypoint(wayPointList[currWaypointIndex].transform)) {
            int waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
            while (waypointIndex == currWaypointIndex) {
                waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
            }
            currWaypointIndex = waypointIndex;
            locomotion.setWayPoint(wayPointList[currWaypointIndex].transform);
        }
    }

    private void transitionToStateChase() {
        //anim.SetBool("walk", false);
        //        locomotion.shouldWalk(false);
        locomotion.setWayPoint(player.transform);
        //        if (!locomotion.isComplete(player.transform.position))
        //        {
        float dist = (player.transform.position - transform.position).magnitude;
        float futureT = 0.1f * dist;
        futureT = Mathf.Min(futureT, 1000); //limit on how far ahead to look
                                            //extrapolate assuming constant Vel and the futureT intercept estimate
        Vector3 futureMoverPos = player.transform.position + player.GetComponent<Rigidbody>().velocity * futureT;
        if (nextPoint == null) {
            nextPoint = new GameObject();
        }
        var newTrans = nextPoint.transform;
        newTrans.position = futureMoverPos;
        //update the target waypoint
        locomotion.setWayPoint(newTrans);
        //        }
    }

    private void transition() {
        if (behavior == Behavior.Chase) {
            //if player is out of the range, start patrol
            if (!inRange()) {
                currWaypointIndex = -1;
                behavior = Behavior.Patrol;
                transitionToStatePatrol();
            } else {
                transitionToStateChase();
            }
        } else {
            //if player is in range, start chase
            if (inRange()) {
                behavior = Behavior.Chase;
                transitionToStateChase();
            } else {
                transitionToStatePatrol();
            }
        }
    }

    private bool jumpOn() {
        float dis = Vector3.Distance(player.transform.position, this.transform.position);
        //		print ("dis: " + dis);

        if (dis < 2 && death == null) {
            death = StartCoroutine(waitAndDie());
            return true;
        }
        return false;
    }

    private IEnumerator waitAndDie() {
        float oldY = transform.localScale.y;
        transform.localScale = new Vector3(transform.localScale.x, 0.1f, transform.localScale.z);
        source.clip = flattened;
        source.Play();
        yield return new WaitForSeconds(1);
        transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);
        agent.isStopped = true;
        GetComponent<Renderer>().enabled = false;
        this.tag = "Untagged";
        foreach (Collider col in GetComponents<Collider>()) {
            col.enabled = false;
        }
        ps.Play();
        source.clip = die;
        source.Play();
        yield return new WaitWhile(() => ps.isPlaying);
        Destroy(this.gameObject);
    }

    private bool inRange() {
        Vector3 direction = player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.forward);
        float dis = Vector3.Distance(player.transform.position, this.transform.position);
        //		print ("dis: " + dis);

        if (dis < 10 || (dis < 15 && angle < 30)) {
            return true;
        }
        return false;
        //        return true;
    }

    //	void OnCollisionEnter(Collision collision) {
    //		if (collision.gameObject.CompareTag ("Player")) {
    //
    //			Debug.Log ("COLLIDE");
    //			if (playerAnimator.GetBool ("isFalling")) {
    //				Debug.Log ("fall");
    //				Destroy (this.gameObject);
    //			}
    //		}
    //	}
}