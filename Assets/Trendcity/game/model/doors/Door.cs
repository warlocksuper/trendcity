using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    // Use this for initialization
    public float doorOpenAngle = 90.0f;
    public float doorCloseAngle = 0.0f;
    public float doorAnimSpeed = 2.0f;
    private Quaternion doorOpen = Quaternion.identity;
    private Quaternion doorClose = Quaternion.identity;
    private Transform playerTrans = null;
    public bool doorStatus = false; //false is close, true is open
    private bool doorGo = false; //for Coroutine, when start only one


    private void Awake()
    {
        doorStatus = false; //door is open, maybe change
                            //Initialization your quaternions
        doorOpen = Quaternion.Euler(0, doorOpenAngle, 0);
        doorClose = Quaternion.Euler(0, doorCloseAngle, 0);
        //Find only one time your player and get him reference
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        Quaternion newrot = transform.localRotation;
    }

    void Start () {
        doorStatus = false; //door is open, maybe change
                            //Initialization your quaternions
        doorOpen = Quaternion.Euler(0, doorOpenAngle, 0);
        doorClose = Quaternion.Euler(0, doorCloseAngle, 0);
        //Find only one time your player and get him reference
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        //GameObject.Find("Player").transform;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.E) && !doorGo) //&& !doorGo
        {
            Debug.Log("Open door");
            if (Vector3.Distance(playerTrans.position, this.transform.position) < 2f)
            {
                if (doorStatus)
                { //close door
                    StartCoroutine(this.moveDoor(doorClose));
                }
                else
                { //open door
                    StartCoroutine(this.moveDoor(doorOpen));
                }
            }
        }

    }


    public IEnumerator moveDoor(Quaternion dest)
    {
        Debug.Log("Open door");
       // transform.RotateAround(Vector3.zero, Vector3.up, 0.5f * Time.deltaTime);
       // transform.localRotation = Quaternion.RotateTowards(transform.localRotation, dest, Time.deltaTime * doorAnimSpeed);
       
        doorGo = true;
        //Check if close/open, if angle less 4 degree, or use another value more 0
        while (Quaternion.Angle(transform.localRotation, dest) > 4.0f)
        {

            transform.localRotation = Quaternion.Slerp(transform.localRotation, dest, Time.deltaTime * doorAnimSpeed);
            //UPDATE 1: add yield
            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, dest, Time.deltaTime * doorAnimSpeed);
            yield return null;
        }
        //Change door status
        doorStatus = !doorStatus;
        doorGo = false;
        //UPDATE 1: add yield
        yield return null;
      
        yield return null;
    }

}
