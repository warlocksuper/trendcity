using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{

    public GameObject workerPrebaf;
    public Worker worker;
    // Use this for initialization
    private GameObject workerobj;
    private GameObject gamelocal;
    private PlayerIO playerIO;
    void Start()
    {
        //Debug.Log("Awake Bad");
        Vector3 positionworker = transform.position;
        positionworker.y += 1;
        workerobj = Instantiate(workerPrebaf, positionworker, Quaternion.identity);
        workerobj.GetComponent<WorkerController>().bad = this.gameObject;
        //worker = new Worker();
        playerIO = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerIO>();
        if (playerIO.isNetwork)
        {
            NetworkLayerClient networkLayer = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
            int homeid = this.gameObject.GetComponent<ItemMy>().homeid;
            networkLayer.workercount++;
            networkLayer.homes.Find(x => x.idtable == homeid).cityWorkers.Add(workerobj);
        }
        else
        {
            gamelocal = GameObject.Find("GameLocal");
            gamelocal.GetComponent<Gamelocal>().AddWorker(workerobj);
        }

        //worker.worker = workerobj;
        //worker = workerobj;


    }

    void OnDestroy()
    {
        if (workerobj != null)
        {
            if (playerIO.isNetwork)
            {
                NetworkLayerClient networkLayer = GameObject.Find("NetworkManager").GetComponent<NetworkLayerClient>();
                int homeid = this.gameObject.GetComponent<ItemMy>().homeid;
                networkLayer.workercount--;
                networkLayer.homes.Find(x => x.idtable == homeid).cityWorkers.Remove(workerobj);
            } else
            {
                gamelocal.GetComponent<Gamelocal>().RemoveWorker(workerobj);
            }
                
            Destroy(workerobj);
        }
        Destroy(gameObject);
    }

}
