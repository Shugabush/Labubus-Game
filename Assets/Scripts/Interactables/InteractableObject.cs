using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    // Assign these in the inspector
    [SerializeField] InputActionReference interactAction;
    [SerializeField] UnityEvent<Player> onInteracted = new UnityEvent<Player>();
    [SerializeField] Graphic[] interactGraphics = new Graphic[0];

    public void Interact(InputAction.CallbackContext context, Player player)
    {
        onInteracted.Invoke(player);
        OnInteracted(context, player);
    }

    protected virtual void OnInteracted(InputAction.CallbackContext context, Player player) { }

    public void SetGraphicsTransparency(float transparency)
    {
        foreach (Graphic graphic in interactGraphics)
        {
            graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, transparency);
        }
    }
}
