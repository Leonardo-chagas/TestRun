using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//class that scrolls the view based on the button currently selected
public class SetScrollToButton : MonoBehaviour, ISelectHandler
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    RectTransform rectTransform;
    Vector2 objPosition => (Vector2)scrollRect.transform.InverseTransformPoint(rectTransform.position);
    float scrollHeight;
    float objHeight;

    void Start(){
        rectTransform = GetComponent<RectTransform>();
        scrollHeight = scrollRect.GetComponent<RectTransform>().rect.height;
        objHeight = rectTransform.rect.height;
    }
    public void OnSelect(BaseEventData eventData){
        if(objPosition.y+objHeight/2 > scrollHeight/2){
            float padding = objPosition.y+objHeight/2 - scrollHeight/2;
            contentPanel.localPosition = new Vector2(contentPanel.localPosition.x,
                contentPanel.localPosition.y - objHeight - padding);
        }
        if(objPosition.y-objHeight/2 < -scrollHeight/2){
            float padding = Mathf.Abs(objPosition.y-objHeight/2 + scrollHeight/2);
            contentPanel.localPosition = new Vector2(contentPanel.localPosition.x,
                contentPanel.localPosition.y + objHeight + padding);
        }
    }
}
