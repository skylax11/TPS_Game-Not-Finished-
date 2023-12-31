using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_IMAGES : MonoBehaviour, IDragHandler
{
    [Header("Image Props")]
    public Canvas canvas;
    private RectTransform rectTransform;
    public bool Touching;
    public bool HaveDraggedEver;
    public bool IsDraggingNow;
    public int id;
    [SerializeField] Vector2 initial_Transform;
    Collider2D detected_coll;
    Vector2 temp_vector2d;
    void Start()
    {

        if (transform.parent.tag == "Inventory")
        {
            id = 9 + transform.GetSiblingIndex();
            Inventory.instance.Slot_Images[9 + transform.GetSiblingIndex()] = gameObject;
        }
        else
        {
            id = transform.GetSiblingIndex();
            Inventory.instance.Slot_Images[transform.GetSiblingIndex()] = gameObject;
        }
        Touching = false;
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Touching = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Touching = true;
        if (HaveDraggedEver == false && IsDraggingNow)
        {
            detected_coll = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Touching =false;
    }
    public void DropUI_Element()
    {
        if (HaveDraggedEver)
        {
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(gameObject.GetComponent<RectTransform>().anchoredPosition, initial_Transform, Time.deltaTime * 20f);
        }
    }
    public void EventTrigger_Drop()
    {
        if (Touching)
        {
            HaveDraggedEver = false;
            SetAndGetParent();
            initial_Transform = detected_coll.GetComponent<UI_IMAGES>().initial_Transform;
            detected_coll.GetComponent<UI_IMAGES>().initial_Transform = temp_vector2d;
            IsDraggingNow = false;
            HaveDraggedEver = true;
            detected_coll.GetComponent<UI_IMAGES>().HaveDraggedEver = true;
            SwitchItems(gameObject.transform.parent.tag, detected_coll.transform.parent.tag, id, detected_coll.transform.GetComponent<UI_IMAGES>().id);

        }
        else
        {
            HaveDraggedEver = true;
        }
    }
    int idChecker(int currentItem_id, int swapItem_id)
    {
        if (currentItem_id == swapItem_id)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    public int previous_index;
    public int nxt_index;
    public void SwitchItems(string my_tag, string detectedcoll_tag, int my_index, int detectedcoll_index)
    {
        SlotItemInfos temp = Inventory.instance.All_Items[my_index];
        Inventory.instance.All_Items[my_index] = Inventory.instance.All_Items[detectedcoll_index];   // SWAPPING ITEMS...
        Inventory.instance.All_Items[detectedcoll_index] = temp;

        id = detectedcoll_index;                                                 // ...All_Items[my_index] is one of the leaving one through current slot.
        detected_coll.transform.GetComponent<UI_IMAGES>().id = my_index;         // ...All_Items[detectedcoll_index] is one of the incoming one through current slot.
        int id2 = detected_coll.transform.GetComponent<UI_IMAGES>().id;

        Inventory.instance.All_Items[id2].id = id2;
        Inventory.instance.All_Items[id].id = id;

        if (idChecker(Inventory.instance.current_item.id, Inventory.instance.All_Items[id].id) == 1)  // Runs when you dragged your current item.
        {
            Inventory.instance.SwitchItems(Inventory.instance.All_Items[id2].id);
        }
        else if (idChecker(Inventory.instance.current_item.id, Inventory.instance.All_Items[id2].id) == 1) // Runs when you swapped your item without dragging
        {
            Inventory.instance.SwitchItems(Inventory.instance.All_Items[id].id);
        }
        Inventory.instance.SwapIcon(id, id2);

    }
    public void SetAndGetParent()
    {
        Transform temp_parent = gameObject.transform.parent;
        int temp_index = gameObject.transform.GetSiblingIndex();
        temp_vector2d = initial_Transform;

        gameObject.transform.SetParent(detected_coll.transform.parent);
        gameObject.transform.SetSiblingIndex(detected_coll.transform.GetSiblingIndex());

        detected_coll.transform.SetParent(temp_parent);
        detected_coll.transform.SetSiblingIndex(temp_index);
    }
    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        IsDraggingNow = true;
        HaveDraggedEver = false;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    private void Update() => DropUI_Element();

}