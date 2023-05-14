using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StudentFieldDnD : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public TextMeshProUGUI names;
    public float note;
    public bool isAproved;
    public Image state;

    public GameObject itemDragging;

    [SerializeField] private DragAndDropManager manager;

    private Vector3 startPosition;
    Transform startParent;
    Transform dragParent;

    private void Start()
    {
        dragParent = GameObject.FindGameObjectWithTag("DragParent").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemDragging = gameObject;
        manager.actualItem = itemDragging;
        manager.infoText.text = $"Nombres: {names.text}\nCalificación: {note}";
        manager.infoPanel.SetActive(true);

        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(dragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemDragging = null;
        manager.actualItem = null;
        manager.infoPanel.SetActive(false);
        manager.infoText.text = "";

        if (transform.parent == dragParent)
        {
            transform.position = startPosition;
            transform.SetParent(startParent);
        }
    }
}
