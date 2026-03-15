using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private ConveyorManager conveyorManager;

    private List<ItemController> items = new List<ItemController>();
    private bool isMoving = false;

    private void Start()
    {
        if (conveyorManager != null)
        {
            conveyorManager.SetConveyorController(this);
        }
        else
        {
            Debug.LogError("ConveyorManager reference not set in ConveyorController!");
        }
    }

    [SerializeField] private float moveTime = 0.3f;

    public void MoveAllItems(float direction, Action onComplete = null)
    {
        if (isMoving || items.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        isMoving = true;

        int itemsMoving = items.Count;

        foreach (var item in items)
        {
            if (item != null)
            {
                item.StartItemMovement(direction, () =>
                {
                    itemsMoving--;
                    if (itemsMoving <= 0)
                    {
                        isMoving = false;
                        onComplete?.Invoke();
                    }
                });
            }
            else
            {
                itemsMoving--;
            }
        }

        if (itemsMoving <= 0)
        {
            isMoving = false;
            onComplete?.Invoke();
        }
    }

    public void RegisterItem(ItemController item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            Debug.Log($"Item registered: {item.name}, Total items: {items.Count}");
        }
    }

    public void UnregisterItem(ItemController item)
    {
        items.Remove(item);
        Debug.Log($"Item unregistered: {item.name}, Total items: {items.Count}");
    }

    public bool IsMoving => isMoving;
    public int ItemCount => items.Count;
}
