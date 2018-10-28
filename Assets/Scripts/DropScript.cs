using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Diagnostics;
using Debug=UnityEngine.Debug;

public class DropScript : MonoBehaviour,IDropHandler, IPointerEnterHandler,IPointerExitHandler {

	public void OnPointerEnter(PointerEventData eventData){
		if (eventData.pointerDrag == null) { // do nothing if nothing is being dragged
			return;
		}
		DragScript draggedCard = eventData.pointerDrag.GetComponent<DragScript> ();

		if (draggedCard != null) {
			draggedCard.parentPlaceholder = this.transform; //change parent of the dragged card
		}
	}

	public void OnPointerExit(PointerEventData eventData){
		if (eventData.pointerDrag == null) {
			return;
		}
		DragScript draggedCard = eventData.pointerDrag.GetComponent<DragScript> ();

			if (draggedCard != null && draggedCard.parentPlaceholder == this.transform) {
				draggedCard.parentPlaceholder = draggedCard.backToParent;    //change parent of the dragged card
			}
	}


	public void OnDrop(PointerEventData eventData){
		//Debug.Log ("OnDrop");
		DragScript draggedCard = eventData.pointerDrag.GetComponent<DragScript> ();

		if (draggedCard != null) {
			draggedCard.backToParent = this.transform;
		}

	
	}
}
