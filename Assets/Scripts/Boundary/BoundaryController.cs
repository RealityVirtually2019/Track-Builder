using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{

    public GameObject boundary;
    BoxCollider boxCollider;

    private float MINIMUM_HEIGHT_PRESET = 1f;
    private float minimumHeight = .2f;
    private float minimumWidth;
    public Vector3 boxColliderScale = new Vector3();

    // Use this for initialization
    void Start()
    {
        
    }

    void Awake()
    {
        boxCollider = boundary.GetComponent<BoxCollider>();
    }

    public void setBounds()
    {

    }

    public BoxCollider getBoxCollider() {
        return boxCollider;
    }

    public void setParent(GameObject parent)
    {
        boundary.transform.parent = parent.transform;
    }

    public void encapsulateStartEnd(Vector3 startPoint, Vector3 endPoint) {
        bool willTranslateForward = false;

        Vector3 vecBetween = endPoint - startPoint;

        float newXScale = Mathf.Abs(vecBetween.x * 1.2f);

        float newYScale;

        if (Mathf.Abs(vecBetween.y) < minimumHeight) {
            newYScale = MINIMUM_HEIGHT_PRESET;
        } else {
            newYScale = Mathf.Abs(vecBetween.y * 1.2f);
        }

        print(vecBetween.y);

        var tmp = new Vector2(vecBetween.x, vecBetween.z);
        float newZScale = Mathf.Abs(tmp.magnitude * 1.2f);
        
        transform.position = startPoint;

        float angle1 = Mathf.Atan2(vecBetween.y, vecBetween.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle1, Vector3.right);

        float angle2 = Mathf.Atan2(vecBetween.x, vecBetween.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle2, Vector3.up);

        Vector3 midpoint = startPoint + (startPoint - endPoint) / 2f;

        Vector3 midpointAtoB = new Vector3((startPoint.x + endPoint.x) / 2.0f, (startPoint.y + endPoint.y) / 2.0f, (startPoint.z + endPoint.z) / 2.0f); // midpoint between A B

        transform.position = midpointAtoB;

        print(midpoint);
        boxCollider.size = new Vector3(newXScale, newYScale, newZScale);
        boxColliderScale = boxCollider.size;
        float newXPosition = boxCollider.transform.position.x;
        float newYPosition;
        float newZPosition;

    }

    public void setOriginRelativeToParent(Vector3 origin)
    {
        boundary.transform.localPosition = origin;
    }
}


