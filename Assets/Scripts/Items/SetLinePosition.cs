using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sets the position of the line used by the gravity inverter object
public class SetLinePosition : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public BoxCollider2D box;
    public GameObject startvfx, endvfx;
    Vector2 endPoint1;
    Vector2 endPoint2;
    void Start()
    {
        int mask = 1 << LayerMask.NameToLayer("Ground");
        RaycastHit2D hit1 = Physics2D.Raycast((Vector2)transform.position, -transform.up, 50f, mask);
        RaycastHit2D hit2 = Physics2D.Raycast((Vector2)transform.position, transform.up, 50f, mask);
        if(hit1.collider){
            Vector3 point = transform.InverseTransformPoint(hit1.point);
            endPoint1 = (Vector2)point;
            lineRenderer.SetPosition(1, new Vector2(point.x, point.y));
            endvfx.transform.position = hit1.point;
        }
        if(hit2.collider){
            Vector3 point = transform.InverseTransformPoint(hit2.point);
            endPoint2 = (Vector2)point;
            lineRenderer.SetPosition(0, new Vector2(point.x, point.y));
            startvfx.transform.position = hit2.point;
        }
        box.size = new Vector2(box.size.x, Vector2.Distance(endPoint2, endPoint1));
        float middlePoint = endPoint2.y - (-endPoint1.y);
        box.transform.localPosition = new Vector2(box.transform.localPosition.x, middlePoint/2);
    }
}
