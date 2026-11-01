using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] Vector3 centerOffset = Vector3.zero;
    Vector3 Center => transform.TransformPoint(centerOffset);
    bool isMoving = false;
    GridTile currentTile;
    Vector2 moveInput = Vector2.zero;

    void Awake()
    {
        moveAction.action.performed += OnMoveAction;
        moveAction.action.canceled += OnMoveAction;
    }

    void OnDestroy()
    {
        moveAction.action.performed -= OnMoveAction;
        moveAction.action.canceled -= OnMoveAction;
    }

    void Update()
    {
        if (moveInput != Vector2.zero && (moveInput.x == 0f || moveInput.y == 0f) && !isMoving)
        {
            int currentTileLayer = currentTile.gameObject.layer;
            currentTile.gameObject.layer = 2;
            if (Physics.Raycast(Center, new Vector3(moveInput.x, 0f, moveInput.y), out RaycastHit hit) &&
                hit.transform.TryGetComponent(out GridTile gridTile) && gridTile.walkable)
            {
                StartCoroutine(MoveToTile(gridTile));
            }
            currentTile.gameObject.layer = currentTileLayer;
        }
    }

    void OnMoveAction(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out GridTile gridTile))
        {
            currentTile = gridTile;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (currentTile != null && currentTile.transform == other.transform)
        {
            currentTile = null;
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
