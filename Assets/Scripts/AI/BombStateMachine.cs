using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class BombStateMachine : MonoBehaviour {

    [SerializeField]
    private DamageTaker damageTaker;

    [SerializeField]
    private ParticleSystem system;

    [SerializeField]
    private AudioSource source;

    private RaycastHit hitInfo = new RaycastHit();
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    public GameObject player;
    private Animator playerAnimator;
    public float futureTLimit = 1.5f;

    private GameObject lookahead;

    //private MovingCubeScript movingCubeScript;
    private AIMovement locomotion;

    public GameObject[] wayPointList;
    private int currWaypointIndex = -1;
    private Boolean dead = false;
    private Boolean isFalling = false;
    private bool isPlayerInside;
    private Coroutine explosion;

    public enum Behavior {
        Patrol,
        Chase
    }

    public Behavior behavior = Behavior.Patrol;

    private void OnTriggerEnter(Collider col) {
        if (col.transform.tag == "Player") {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider col) {
        if (col.transform.tag == "Player") {
            isPlayerInside = false;
        }
    }

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
            print("fall");
        } else if (Vector3.Distance(player.transform.position, this.transform.position) < 1) {
            print("bomb");
            StartCoroutine(waitAndBomb());
            if (explosion == null) {
                explosion = StartCoroutine(bomb());
            }
        }
        transition();
    }

    private IEnumerator waitAndBomb() {
        transform.localScale += new Vector3(.03F, .03f, .03f);
        yield return new WaitForSeconds(1f);
        //		Destroy (this.gameObject);
    }

    private IEnumerator bomb() {
        yield return new WaitForSeconds(2f);
        system.Play();
        source.Play();
        agent.isStopped = true;
        foreach (Renderer rend in GetComponentsInChildren<MeshRenderer>()) {
            rend.enabled = false;
        }
        if (isPlayerInside) {
            damageTaker.Hurt(this.transform.position);
        }
        yield return new WaitWhile(() => system.isPlaying && source.isPlaying);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

    private void transitionToStatePatrol() {
        print("patrol");
        if (currWaypointIndex == -1 || locomotion.reachedWaypoint(wayPointList[currWaypointIndex].transform)) {
            int waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
            while (waypointIndex == currWaypointIndex) {
                waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
            }
            currWaypointIndex = waypointIndex;
            print("patrol waypoint " + currWaypointIndex);
            locomotion.setWayPoint(wayPointList[currWaypointIndex].transform);
        }
    }

    private void transitionToStateChase() {
        //anim.SetBool("walk", false);
        //        locomotion.shouldWalk(false);
        locomotion.setWayPoint(player.transform);
        //        if (!locomotion.isComplete(player.transform.position))
        //        {
        print("chase");
        float dist = (player.transform.position - transform.position).magnitude;
        float futureT = 0.1f * dist;
        futureT = Mathf.Min(futureT, 1000); //limit on how far ahead to look
                                            //extrapolate assuming constant Vel and the futureT intercept estimate
        Vector3 futureMoverPos = player.transform.position + player.GetComponent<Rigidbody>().velocity * futureT;

        GameObject go;
        if (lookahead == null) {
            go = new GameObject();
        } else {
            go = lookahead;
        }
        var newTrans = go.transform;
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

        if (dis < 2) {
            StartCoroutine(waitAndDie());
            return true;
        }
        return false;
    }

    private IEnumerator waitAndDie() {
        yield return new WaitForSeconds(1f);
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