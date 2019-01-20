using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectManager : MonoBehaviour {

    public BoundaryController boundaryController;
    private List<GameObject> listObstacle = new List<GameObject>();
    private GameObject goDrone;
    private GameObject goWall;
    public float RateOfSpawn = 20;
    private float nextSpawn = 1;
    private bool isMade = false;
    private bool isSpawned = false;


	// Use this for initialization
	void Start () {
        
	}

    void Awake()
    {
        goDrone = (GameObject)Resources.Load(ResourcePath.DAWN_DRONE); 
        goWall = (GameObject)Resources.Load(ResourcePath.WALL); 
    }

    public void instantiateGameObjects() {
        instantiateGameObject(goDrone, true, true);
        instantiateGameObject(goDrone, true, true);
        instantiateGameObject(goDrone, true, false);
        instantiateGameObject(goWall, false, false);
        instantiateGameObject(goWall, false, false);
        instantiateGameObject(goWall, false, false);
    }

    public void instantiateGameObject(GameObject prefab, bool isDrone, bool isXOriented) {
        BoxCollider boundary = boundaryController.getBoxCollider();
        Vector3 randomPositionWithin;

        float xRand = Random.Range(-.8f, .8f);
        float yRand = Random.Range(-.8f, .8f);
        float zRand = Random.Range(-.8f, .8f);

        xRand *= boundaryController.boxColliderScale.x;
        yRand *= boundaryController.boxColliderScale.y;
        zRand *= boundaryController.boxColliderScale.z;

        randomPositionWithin = new Vector3(xRand, yRand, zRand);
        randomPositionWithin = boundary.transform.TransformPoint(randomPositionWithin * .5f);

        var obstacle = Instantiate(prefab, randomPositionWithin, boundaryController.boundaryQuat);
        obstacle.transform.rotation = Quaternion.AngleAxis(boundaryController.mAngle1, Vector3.forward);
        obstacle.transform.rotation = Quaternion.AngleAxis(boundaryController.mAngle2, Vector3.up);
        if (isDrone) {
            obstacle.transform.SetParent(boundary.transform);
        if (isXOriented) {
            obstacle.GetComponent<DroneController>().startDroneFlight(true);    
            } else {
                obstacle.GetComponent<DroneController>().startDroneFlight(false);    
            }
        } else {
            obstacle.transform.Rotate(Vector3.up * 90f);   
        }

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

        /*if (Time.time > nextSpawn)
        {
            nextSpawn = Time.time + RateOfSpawn;
            if (!isMade)
            {
                boundaryController.encapsulateStartEnd(new Vector3(1f , 2f, 1f), new Vector3(10f, 4f, 7f));
            }
            if (!isSpawned) {
                instantiateGameObjects();
                isSpawned = true;
            }
        }*/

	}
}
