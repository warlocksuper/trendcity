using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerController : MonoBehaviour {

    public GameObject bad;
    public bool isbusy = false;
    public bool isbankrot = false;
    private Transform playerShip;
    private float speed, moveSpeed; // Скорость движения врага
    private Vector3 delta;

    private float randDistance;
    private float randDirection;
    private bool iswall = false;

    private GameObject verstack;
    private CraftOrder slot;
    // Use this for initialization


    public float thrust = 10.0f;

    void Start () {
        playerShip = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void FixedUpdate()
    {
        if (!isbusy)
        {

            float dist = Vector3.Distance(transform.position, bad.transform.position);
            if (dist > 2f)
            {
                //GetComponent<Rigidbody>().isKinematic = false;
                transform.LookAt(bad.transform);
                GetComponent<Animation>().Play("walk");
                GetComponent<NavMeshAgent>().enabled = true;
                GetComponent<NavMeshAgent>().SetDestination(bad.transform.position);

                //GetComponent<NavMeshAgent>().SetDestination(bad.transform.position);
                //delta = bad.transform.position - transform.position; //вычисления разницы между вражеским объектом и кораблём
               // delta.Normalize();
               // moveSpeed = speed * Time.deltaTime;
               // transform.position = transform.position + (delta * moveSpeed);
            }
            if (dist <= 2f)
            {
                transform.LookAt(playerShip);
                GetComponent<Animation>().Play("idle");
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        if (iswall)
        {
            transform.LookAt(verstack.transform);
            GetComponent<Animation>().Play("walk");
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = true;
            agent.SetDestination(verstack.transform.position);

            float dist = Vector3.Distance(transform.position, verstack.transform.position);
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(verstack.transform.position, path);
            Debug.Log("WorkerPath path.status "+ path.status);
            if (path.status == NavMeshPathStatus.PathInvalid)
            {
                gameObject.transform.position = verstack.transform.position;
            }
            

            /*
            speed = Random.Range(3, 6);

            if (dist > 2f)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Animation>().Play("walk");
                delta = verstack.transform.position - transform.position; //вычисления разницы между вражеским объектом и кораблём
                delta.Normalize();
                transform.GetComponent<Rigidbody>().AddForce(0, 0, 0);
                transform.GetComponent<Rigidbody>().AddForce(transform.forward * thrust);
                //moveSpeed = speed * Time.deltaTime;
                //transform.position = transform.position + (delta * moveSpeed);
            }

            */

            if (dist <= 2f)
            {
                GetComponent<Animation>().Play("idle");
                GetComponent<Rigidbody>().isKinematic = true;
                transform.LookAt(verstack.transform);
                //transform.GetComponent<Rigidbody>().AddForce(0, 0, 0);
                iswall = false;
                Craft();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isbankrot)
        {
            float dist = Vector3.Distance(transform.position, bad.transform.position);
            if (dist <= 2f)
            {
                CitySaveAsset save = (CitySaveAsset)Resources.Load("CitySaveAsset");
                save.removeItem(bad);
                Destroy(bad);

                //transform.LookAt(playerShip);
                //GetComponent<Animation>().Play("idle");
                //GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    public void setCraft(GameObject verstack, CraftOrder slot)
    {
        this.verstack = verstack;
        this.slot = slot;

        iswall = true;
    }

    public void Craft()
    {
        //verstack
        transform.LookAt(verstack.transform);
        verstack.GetComponent<verstack>().StartCraft(gameObject, slot);
    }

    public void bankrot()
    {
        iswall = false;
        isbusy = false;
        isbankrot = true;
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<NavMeshAgent>().SetDestination(bad.transform.position);
    }
}


public class Worker : MonoBehaviour
{
        public int workerid;
        public GameObject worker;
}