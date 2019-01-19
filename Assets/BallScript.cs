using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {
    public TextMesh WinText;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Goal")
        {
            WinText.text = "You win! Press Bumper to play again.";
        }
        
    }
    
}
