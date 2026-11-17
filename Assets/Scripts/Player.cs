using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] Vector3 centerOffset = Vector3.zero;
    Vector3 Center => transform.TransformPoint(centerOffset);
    bool isMoving = false;
    GridTile currentTile;
    Vector2 moveInput = Vector2.zero;

    #region Input Actions

    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference interactAction;

    #endregion

    readonly List<InteractableObject> interactableObjects = new List<InteractableObject>();
    InteractableObject targetInteractableObject = null;
    [SerializeField] float interactAngleThreshold = 0.7f;

    void Awake()
    {
        moveAction.action.performed += OnMoveAction;
        moveAction.action.canceled += OnMoveAction;

        interactAction.action.performed += OnInteractAction;
        interactAction.action.canceled += OnInteractAction;
    }

    void OnDestroy()
    {
        moveAction.action.performed -= OnMoveAction;
        moveAction.action.canceled -= OnMoveAction;

        interactAction.action.performed -= OnInteractAction;
        interactAction.action.canceled -= OnInteractAction;
    }

    void Update()
    {
        if (moveInput != Vector2.zero && (moveInput.x == 0f || moveInput.y == 0f) && !isMoving)
        {
            int currentTileLayer = currentTile.gameObject.layer;
            currentTile.gameObject.layer = 2;
            if (Physics.Raycast(Center, new Vector3(moveInput.x, 0f, moveInput.y), out RaycastHit hit, Mathf.Infinity, 1)
                && hit.transform.TryGetComponent(out GridTile gridTile) && gridTile.walkable)
            {
                transform.rotation = Quaternion.LookRotation(gridTile.transform.position - transform.position, Vector3.up);
                rb.rotation = transform.rotation;
                StartCoroutine(MoveToTile(gridTile));
            }
            currentTile.gameObject.layer = currentTileLayer;
        }

        if (interactableObjects.Count > 0)
        {
            float closestInteractableDst = float.MaxValue;
            float closestInteractableDot = float.MaxValue;
            int targetInteractableIndex = 0;

            for (int i = 0; i < interactableObjects.Count; i++)
            {
                InteractableObject interactableObject = interactableObjects[i];
                Vector3 dirToInteractable = Vector3.Normalize(interactableObject.transform.position - transform.position);
                float dst = System.MathF.Round(Vector3.SqrMagnitude(dirToInteractable), 2);
                float dot = Vector3.Dot(dirToInteractable, transform.forward);
                interactableObject.SetGraphicsTransparency(dot);
                if (dst < closestInteractableDst)
                {
                    closestInteractableDst = dst;
                    targetInteractableIndex = i;
                }
                else if (dst == closestInteractableDst && dot < closestInteractableDot)
                {
                    closestInteractableDot = dot;
                    targetInteractableIndex = i;
                }
            }
            if (closestInteractableDot < interactAngleThreshold)
            {
                targetInteractableObject = interactableObjects[targetInteractableIndex];
            }
        }
        else
        {
            targetInteractableObject = null;
        }
    }

    #region Input Action Methods

    void OnMoveAction(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnInteractAction(InputAction.CallbackContext context)
    {
        if (targetInteractableObject != null)
        {
            targetInteractableObject.Interact(context, this);
        }
    }

    #endregion

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GridTile gridTile))
        {
            currentTile = gridTile;
        }
        else if (other.TryGetComponent(out InteractableObject interactableObject))
        {
            interactableObjects.Add(interactableObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentTile != null && currentTile.transform == other.transform)
        {
            currentTile = null;
        }
        else if (other.TryGetComponent(out InteractableObject interactableObject))
        {
            interactableObject.SetGraphicsTransparency(0f);
            interactableObjects.Remove(interactableObject);
        }
    }

    IEnumerator MoveToTile(GridTile gridTile)
    {
        isMoving = true;
        Timer timer = new Timer(moveTime);
        // Current Tile will probably change to the new tile during movement, so grab it here
        GridTile currentTile = this.currentTile;
        Vector3 offsetFromCurrentTile = transform.position - currentTile.transform.position;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = gridTile.transform.position + offsetFromCurrentTile;
        while (timer.Running)
        {
            yield return null;
            transform.position = Vector3.Lerp(startPosition, endPosition, timer.FractionOfTimeElapsed);
            rb.position = transform.position;
            timer.Update(Time.deltaTime);
        }
        transform.position = endPosition;
        rb.position = transform.position;
        isMoving = false;
        this.currentTile = gridTile;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Center, 0.05f);
    }
}
