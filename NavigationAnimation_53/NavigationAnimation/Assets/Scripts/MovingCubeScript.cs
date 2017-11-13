using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCubeScript : MonoBehaviour
{
    float time = 0f;
    float A;
    float B;
    float C;
    public float SpeedX;
    public float SpeedZ;
    public float t;
    public bool moveCube;


    // Use this for initialization
    void Awake()
    {
        moveCube = true;
        t = 0f;
        A = transform.position.x;
        C = transform.position.z;
        B = transform.position.x + 2;

    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        /*
		if (transform.position.x < B) {
			position.x = Mathf.Lerp (transform.position.x, B, Time.deltaTime);
		} else {
			print ("here");
			position.x = Mathf.Lerp (transform.position.x, A, -Time.deltaTime);
		}
		*/
        //position.x = Mathf.Lerp (A, B, Mathf.Sin(time));
        //print(moveCube);
        float origPosX = transform.position.x;
        float origPosZ = transform.position.z;
        if (moveCube)
        {
            position.x = A + 2f * Mathf.Sin(0.75f * t);
            position.z = C + 2f * Mathf.Cos(0.75f * t);
            SpeedX = (position.x - origPosX) / Time.deltaTime;
            SpeedZ = (position.z - origPosZ) / Time.deltaTime;
            transform.position = position;
            t += 0.01f;
        }


        //print (Mathf.Sin(Time.time));


    }
    void FixedUpdate()
    {
        time += 0.01f;

    }
}
