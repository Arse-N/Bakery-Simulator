using UnityEngine;
using System.Collections;
using System;


public class ItemController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float t;
    public float moveSpeed = 0.5f;

    private bool isMoving = false;
    private Coroutine currentMovement;

    void Start()
    {
        RegisterWithConveyor();
    }

    void OnDestroy()
    {
        UnregisterFromConveyor();
    }

    private void RegisterWithConveyor()
    {
        ConveyorController conveyorController = FindObjectOfType<ConveyorController>();
        if (conveyorController != null)
        {
            conveyorController.RegisterItem(this);
        }
        else
        {
            Debug.LogError("ConveyorController not found for item: " + name);
        }
    }

    private void UnregisterFromConveyor()
    {
        ConveyorController conveyorController = FindObjectOfType<ConveyorController>();
        if (conveyorController != null)
        {
            conveyorController.UnregisterItem(this);
        }
    }

    public void StartItemMovement(float targetPositionX, Action onComplete = null)
    {
        if (isMoving && currentMovement != null)
        {
            StopCoroutine(currentMovement);
        }

        currentMovement = StartCoroutine(MoveItem(targetPositionX, onComplete));
    }

    IEnumerator MoveItem(float targetPositionX, Action onComplete = null)
    {
        isMoving = true;
        startPosition = transform.position;
        targetPosition = startPosition + new Vector3(targetPositionX, 0, 0);

        t = 0f;
        while (t <= 1.0f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            t += Time.deltaTime * moveSpeed;
            yield return null;
        }

        // Ensure final position is exact
        transform.position = targetPosition;
        isMoving = false;
        currentMovement = null;
        
        onComplete?.Invoke();

    }

}
