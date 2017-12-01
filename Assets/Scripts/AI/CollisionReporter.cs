//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class CollisionReporter : MonoBehaviour {
//    public GameObject Dude;
//    private ClickToMove clickToMove;
//    private Animator animator;
//    private bool firstCollision;

//	// Use this for initialization
//	private void Awake () {
//        clickToMove = Dude.GetComponent<ClickToMove>();
//        animator = Dude.GetComponent<Animator>();
//        firstCollision = true;
		
//	}

//    // Update is called once per frame
//    /*
//	void Update () {
		
//	}
//    */
//    void OnTriggerEnter(Collider other)
//    {
        
//        if (firstCollision)
//        {
//            print("collided");
//            animator.SetTrigger("changed");
//            firstCollision = false;
//            clickToMove.behavior = ClickToMove.Behavior.MovingTarget;

//        }
        
//        //clickToMove.behavior = ClickToMove.Behavior.MovingTarget;
//    }
//}
