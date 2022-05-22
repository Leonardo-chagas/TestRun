using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stretching and unstretching animations
public class Animations : MonoBehaviour
{
    CapsuleCollider2D cap;
    Vector2 size;
    void Start(){
        cap = GetComponent<CapsuleCollider2D>();
        size = cap.size;
    }
    public IEnumerator Stretch(bool isRight){
        int d = isRight ? 1 : 1;
        Vector3 newScale =new Vector3(transform.localScale.x + d*0.2f, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;
        cap.size = new Vector2(size.x/transform.localScale.x, size.y/transform.localScale.y);
        yield return new WaitForSeconds(0.01f);
        if (transform.localScale.x < 1.5f && transform.localScale.x > 0.5f)
            StartCoroutine(Stretch(isRight));
    }

    public IEnumerator UnStretch(bool isRight){
        int d = isRight ? -1 : -1;
        Vector3 newScale =new Vector3(transform.localScale.x + d*0.2f, transform.localScale.y, transform.localScale.z);
        transform.localScale = newScale;
        cap.size = new Vector2(size.x/transform.localScale.x, size.y/transform.localScale.y);
        yield return new WaitForSeconds(0.01f);
        if (transform.localScale.x != 1.0f)
            StartCoroutine(UnStretch(isRight));
    }
}
