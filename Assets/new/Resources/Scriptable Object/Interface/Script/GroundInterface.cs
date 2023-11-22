using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GroundInterface : UserInterface
{

    public GameObject inventoryPrefab;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();


        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            //EventTriggerType.PointerEnter을 사용한 이유는 아마다른 인터페이스에 올라가면 그 인터페이스의 슬롯으로 obj를 바꾸기위함이다.
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerDown, (eventData) => { if (eventData is PointerEventData wherebuttondata) { Debug.Log("땅PointerDown"); OnDown(obj, wherebuttondata.button); } });
            AddEvent(obj, EventTriggerType.PointerUp, (eventData) => { if (eventData is PointerEventData wherebuttondata) { Debug.Log("땅PointerUp"); OnUp(obj, wherebuttondata.button); } });

            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate {  OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            slotsOnInterface.Add(obj, inventory.Container.Items[i]);

            //Debug.Log(inventory.Container.Items[i]);
        }
    }
    private Vector3 GetPosition(int i)
    {
        //이미지의 위치를 잡아주는 코드
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN)), 0f);
    }


}
