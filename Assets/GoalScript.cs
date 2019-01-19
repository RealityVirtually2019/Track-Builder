using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
    private TextMesh WinText;
    private void Awake()
    {
        WinText = gameObject.GetComponentInChildren<TextMesh>();
        WinText.text = "";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            WinText.text = "You win!";
        }
    }

    
}
