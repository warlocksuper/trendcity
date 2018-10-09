using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

 public class DragHendler : MonoBehaviour ,IBeginDragHandler, IDragHandler,IEndDragHandler {

    public static GameObject itembegingrag;
    Vector3 startposition;
    Transform startparent;

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        itembegingrag = gameObject;
        startposition = transform.position;
        startparent = transform.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        itembegingrag = null;
        if(transform.parent != startparent)
        {
            transform.position = startposition;
        }
        


    }
    #endregion


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
