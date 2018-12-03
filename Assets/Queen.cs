using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : MonoBehaviour {
    public GameObject workerPrefab;
    public PheromoneMap pheromone;
    public List<Worker> workers;
    public float food;
	// Use this for initialization
	void Start () {
        InvokeRepeating("Breed", 0, 5);
        InvokeRepeating("SetPheromone", 0, 1);
        workers = new List<Worker>();
        
    }
	
	// Update is called once per frame
	void Update () {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        if (new Vector3(x, 0, y) != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(x, 0, y));
            transform.position += transform.forward * Time.deltaTime;
        }
        

    }

    private void SetPheromone()
    {

        var pos = PheromoneMap.GetPos(transform.position);
        var p = pheromone.GetPreomone(pos);
        //Debug.Log(pos[0] + "," + pos[1]);
        p.scale[PheromoneType.Queen] += 400;
        if (p.scale[PheromoneType.Queen] >600)
        {
            p.scale[PheromoneType.Queen] =600;
        }

        

    }
    void Breed()
    {
        for (int i = 0; i <3&&workers.Count<60; i++)
        {
            GameObject obj = Instantiate(workerPrefab, transform.position, Quaternion.identity);
            obj.GetComponent<Worker>().pheromone = pheromone;
            obj.GetComponent<Worker>().queen = this;
            workers.Add(obj.GetComponent<Worker>());
        }
       
    }
    private void FixedUpdate()
    {
        
    }
}
