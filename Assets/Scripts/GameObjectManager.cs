using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

    public BoundaryController boundaryController;
    private List<GameObject> listObstacle = new List<GameObject>();
    private GameObject goTest;
    public float RateOfSpawn = 5;
    private float nextSpawn = 1;
    private bool isMade = false;


	// Use this for initialization
	void Start () {
        
	}

    void Awake()
    {
        goTest = (GameObject)Resources.Load(ResourcePath.TEST_CUBE); 

    }

    public void instantiateGameObject(GameObject prefab) {
        BoxCollider boundary = boundaryController.getBoxCollider();
        Vector3 randomPositionWithin;

        float xRand = Random.Range(-.8f, .8f);
        float yRand = Random.Range(-.8f, .8f);
        float zRand = Random.Range(-.8f, .8f);

        randomPositionWithin = new Vector3(xRand, yRand, zRand);
        randomPositionWithin = boundary.transform.TransformPoint(randomPositionWithin * .5f);

        var obstacle = Instantiate(prefab, randomPositionWithin, transform.rotation);
        listObstacle.Add(obstacle);
    }

    public void destroyLevelObstacles() {
        for (int i = 0; i < listObstacle.Count; i++)
        {
            Destroy(listObstacle[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + RateOfSpawn;
            instantiateGameObject(goTest);
            if (!isMade)
            {
                boundaryController.encapsulateStartEnd(new Vector3(2f, 2f, 1f), new Vector3(10f, 2f, 5f));
                //new Vector3(10f, 2f, 5f)
            }
        }

	}
}
