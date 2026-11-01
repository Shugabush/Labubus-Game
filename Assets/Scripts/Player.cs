using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    GridTile currentTile;
    [SerializeField] InputActionReference moveAction;

    void Awake()
    {
        moveAction.action.performed += OnMoveAction;
    }

    void OnDestroy()
    {
        moveAction.action.performed -= OnMoveAction;
    }

    void OnMoveAction(InputAction.CallbackContext context)
    {
        
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
}
