using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;
using Debug=UnityEngine.Debug;
using System.Deployment.Internal;


public class DragScript : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {

	public float tCardWidth; // table cards width and height to rescale them
	public float tCardHeight; 

	public float hCardWidth;// hand cards width and height to rescale them
	public float hCardHeight;

	public Transform backToParent= null; //parent to return to	
	public Transform parentPlaceholder=null; 

	public GameObject placeholder=null; // to hold place for pulled cards

	public void OnBeginDrag(PointerEventData eventData){
		placeholder = new GameObject ();
		placeholder.transform.SetParent (this.transform.parent); 

		LayoutElement layoutElement = placeholder.AddComponent<LayoutElement> ();
		layoutElement.preferredWidth = this.GetComponent<LayoutElement> ().preferredWidth ;
		layoutElement.preferredHeight = this.GetComponent<LayoutElement> ().preferredHeight;
		layoutElement.flexibleWidth = 0;
		layoutElement.flexibleHeight = 0;

		placeholder.transform.SetSiblingIndex (this.transform.GetSiblingIndex());

		backToParent = this.transform.parent;
		parentPlaceholder=backToParent;

		this.transform.parent = this.transform.parent.parent; // goes one level up parent
		GetComponent<CanvasGroup> ().blocksRaycasts = false; // allows raycast to hit underneath the card
	}

	public void OnDrag(PointerEventData eventData){
		
		this.transform.position = eventData.position; //move with mouse
		//TODO add mouse offset code

		if (placeholder.transform.parent!=parentPlaceholder){ 
			placeholder.transform.SetParent (parentPlaceholder);
		}

		int newSeblingIndex=parentPlaceholder.childCount;

		for(int i =0;i<parentPlaceholder.childCount;i++){ //loop though all cards in the list

			if (this.transform.position.x<parentPlaceholder.transform.GetChild(i).position.x){ // if dragged card is to the left of any card
				newSeblingIndex = i;

				if (placeholder.transform.GetSiblingIndex () > newSeblingIndex) {
				
					newSeblingIndex--;
				}
				break;
			}
		}
		placeholder.transform.SetSiblingIndex (newSeblingIndex);
	
	}

	public void OnEndDrag(PointerEventData eventData){

		//Debug.Log ("OnEndDragHandler");
		this.transform.SetParent(backToParent);
		this.transform.SetSiblingIndex (placeholder.transform.GetSiblingIndex());
		GetComponent<CanvasGroup> ().blocksRaycasts = true;



		if (this.transform.parent.name == "Table") { //set width and height according to the layout
			RectTransform rt = this.GetComponent<RectTransform> ();
			rt.sizeDelta = new Vector2 (tCardWidth, tCardHeight);


		} else {
			RectTransform rt = this.GetComponent<RectTransform> ();
			rt.sizeDelta = new Vector2 (hCardWidth, hCardHeight);

			
		}
		Destroy (placeholder);
	}

}
