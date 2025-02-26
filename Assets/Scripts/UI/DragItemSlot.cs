using UnityEngine;
using UnityEngine.EventSystems;

public class DragItemSlot : MonoBehaviour, IDropHandler
{
    public DragItem item;

    public void OnDrop(PointerEventData eventData)
    {
        DragItem dragItem = eventData.pointerDrag.GetComponent<DragItem>();
        if (item != null)
        {
            item.transform.SetParent(dragItem.orgParent);
            item.transform.localPosition = Vector3.zero;
        }
        else
        {
            dragItem.orgParent.GetComponent<DragItemSlot>().item = null;
        }
        dragItem.orgParent = transform;
        item = dragItem;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        item = GetComponentInChildren<DragItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
