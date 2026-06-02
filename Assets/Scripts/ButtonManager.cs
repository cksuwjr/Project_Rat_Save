using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private Button targetButton;
    [SerializeField] private KeyCode hotkey;

    void Update()
    {
        // Detects if the chosen key is pressed down
        if (Input.GetKey(hotkey))
        {
            // Simulates a physical button click event
            ExecuteEvents.Execute(
            targetButton.gameObject,
            new PointerEventData(EventSystem.current),
            ExecuteEvents.pointerDownHandler);
            //targetButton.
        }
        else
        {
            ExecuteEvents.Execute(
            targetButton.gameObject,
            new PointerEventData(EventSystem.current),
            ExecuteEvents.pointerUpHandler);
        }
    }

    
}
