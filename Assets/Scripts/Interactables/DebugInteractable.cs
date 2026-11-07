using UnityEngine;
using UnityEngine.InputSystem;

public class DebugInteractable : InteractableObject
{
    protected override void OnInteracted(InputAction.CallbackContext context, Player player)
    {
        base.OnInteracted(context, player);
        Debug.Log("interacted", this);
    }
}
