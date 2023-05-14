using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class DropItem : MonoBehaviour, IDropHandler
{
    public DragAndDropManager manager;
    [SerializeField] private GameObject waitListPanel;
    [SerializeField] private Color color;

    public void OnDrop(PointerEventData eventData)
    {
        if (manager.actualItem != null)
        {
            manager.actualItem.transform.SetParent(transform);
            manager.actualItem.GetComponent<StudentFieldDnD>().state.color = color;
            bool showCheckButton = true;
            for (int i = 0; i < waitListPanel.transform.childCount; i++)
            {
                if (waitListPanel.transform.GetChild(i).gameObject.activeSelf)
                {
                    showCheckButton = false;
                }
            }
            if (showCheckButton)
            {
                manager.CheckDnD();
            }
        }
    }
}
