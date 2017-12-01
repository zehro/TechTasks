//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
//public class StateMachine : MonoBehaviour {
//    private RaycastHit hitInfo = new RaycastHit();
//    private UnityEngine.AI.NavMeshAgent agent;
//    private Animator anim;
//    public GameObject player;
//    public float futureTLimit = 1.5f;

//    //private MovingCubeScript movingCubeScript;
//    private AIMovement locomotion;

//    public GameObject[] wayPointList;
//    private int currWaypointIndex = -1;

//    public enum Behavior {
//        Patrol,
//        Chase
//    }

//    public Behavior behavior = Behavior.Patrol;

//    private void Start() {
//        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
//        locomotion = GetComponent<AIMovement>();
//        anim = GetComponent<Animator>();
//        //movingCubeScript = movingWaypoint.GetComponent<MovingCubeScript>();
//    }

//    private void Update() {
//        transition();
//    }

//    private void transitionToStatePatrol() {
//        //        print ("patrol");
//        if (currWaypointIndex == -1 || locomotion.isComplete(wayPointList[currWaypointIndex].transform.position)) {
//            int waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
//            while (waypointIndex == currWaypointIndex) {
//                waypointIndex = UnityEngine.Random.Range(0, wayPointList.Length);
//            }
//            currWaypointIndex = waypointIndex;
//            print("patrol waypoint " + currWaypointIndex);
//            locomotion.setWayPoint(wayPointList[currWaypointIndex].transform.position);
//        }
//    }

//    private void transitionToStateChase() {
//        //anim.SetBool("walk", false);
//        locomotion.shouldWalk(false);
//        //        if (!locomotion.isComplete(player.transform.position))
//        //        {
//        print("chase");
//        float dist = (player.transform.position - transform.position).magnitude;
//        float futureT = 0.1f * dist;
//        futureT = Mathf.Min(futureT, 1000); //limit on how far ahead to look
//                                            //extrapolate assuming constant Vel and the futureT intercept estimate
//        Vector3 futureMoverPos = player.transform.position + player.GetComponent<Rigidbody>().velocity * futureT;

//        //update the target waypoint
//        locomotion.setWayPoint(futureMoverPos);
//        //        }
//    }

//    private void transition() {
//        if (behavior == Behavior.Chase) {
//            //if player is out of the range, start patrol
//            if (!inRange()) {
//                currWaypointIndex = -1;
//                behavior = Behavior.Patrol;
//                transitionToStatePatrol();
//            } else {
//                transitionToStateChase();
//            }
//        } else {
//            //if player is in range, start chase
//            if (inRange()) {
//                behavior = Behavior.Chase;
//                transitionToStateChase();
//            } else {
//                transitionToStatePatrol();
//            }
//        }
//    }

//    private bool inRange() {
//        Vector3 direction = player.transform.position - this.transform.position;
//        float angle = Vector3.Angle(direction, this.transform.forward);
//        float dis = Vector3.Distance(player.transform.position, this.transform.position);
//        //        print ("dis: " + dis);
//        if (Vector3.Distance(player.transform.position, this.transform.position) < 10 && angle < 30) {
//            return true;
//        }
//        return false;
//        //        return true;
//    }
//}