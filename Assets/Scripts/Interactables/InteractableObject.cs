using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    // Assign these in the inspector
    [SerializeField] InputActionReference interactAction;
    [SerializeField] UnityEvent<Player> onInteracted = new UnityEvent<Player>();

    public void Interact(InputAction.CallbackContext context, Player player)
    {
        onInteracted.Invoke(player);
        OnInteracted(context, player);
    }

    protected virtual void OnInteracted(InputAction.CallbackContext context, Player player) { }
}
