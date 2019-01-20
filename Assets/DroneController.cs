using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour, IObstacleCollidable {

    public GameObject drone;
    //public TextMesh[] TextMeshes = FindObjectsOfType<TextMesh>();
    public TextMesh DroneText;





    private void Awake()
    {
        GetComponent<Animator>();
        DroneText = GameObject.FindGameObjectWithTag("DroneText").GetComponent<TextMesh>();
}

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Boundary"))
        {
            onObstacleCollisionExit();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Wall")) {
            onObstacleCollision();
        }
        else if (other.gameObject.tag.Equals("Ball"))
        {
            other.gameObject.transform.position = new Vector3(1000f, 1000f, 1000f);
            DroneText.text = "The ball was taken far away by the drone! Restart to continue.";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    public void startDroneFlight(bool isXOriented) {
        if (isXOriented)
        {
            drone.transform.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(.5f, 0, 0f));
            //drone.transform.GetComponent<Rigidbody>().velocity = new Vector3(.5f, 0f, 0f);
        } else {
            drone.transform.GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(0, .5f, 0f));
            //drone.transform.GetComponent<Rigidbody>().velocity = new Vector3(0f, .5f, 0f);
        }
    }

    public void startAnimation() {
        
    }

    public void onObstacleCollisionExit() {
        var droneVel = drone.transform.GetComponent<Rigidbody>().velocity;
        var newDroneVel = -1 * droneVel;
        drone.transform.GetComponent<Rigidbody>().velocity = newDroneVel;
    }

    public void onObstacleCollision() {
        print("Collided drone");
        var droneVel = drone.transform.GetComponent<Rigidbody>().velocity;
        var newDroneVel = -1 * droneVel;
        drone.transform.GetComponent<Rigidbody>().velocity = newDroneVel;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
