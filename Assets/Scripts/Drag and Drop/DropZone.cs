using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject swapCard;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null)
            {
                if (name == "Play Panel" && transform.childCount > 1) //Return if Playzone is occupied
                {
                    swapCard = GetComponentInChildren<Draggable>().gameObject;
                    swapCard.transform.SetParent(d.parentToReturnTo);
                    swapCard.transform.SetSiblingIndex(d.placeholder.transform.GetSiblingIndex());
                    d.parentToReturnTo = this.transform;
                }
                else if (name == "Deck Build" && transform.childCount > 20) //Return if Deck is full
                {
                    Destroy(d.placeholder);
                    Destroy(d.gameObject);
                }
                else
                {
                    d.parentToReturnTo = this.transform;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) //When the mouse enters a Drop Zone
    {
        if(eventData.pointerDrag == null) //Check if we are dragging a card 
        {
            return;
        }


        Draggable d = eventData.pointerDrag.GetComponent<Draggable>(); //Set d to our dragged card
        
        if(d != null)
        {
            d.placeholderParent = this.transform; //Set the placeholderParent of the card to the current Drop Zone
        }
    }

    public void OnPointerExit(PointerEventData eventData) //When the mouse leaves a Drop Zone
    {
        if(eventData.pointerDrag == null) //Check if we are dragging a card
        {
            return;
        }

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>(); //Set d to our dragged card

        if (d != null && d.placeholderParent == this.transform)
        {
            d.placeholderParent = d.parentToReturnTo; //Set the placeholderParent to our default parent
        }
    }
}
