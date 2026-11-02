
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualCursor : MonoBehaviour
{
    [Header("Cursor Settings")]
    public RectTransform cursor;
    public Canvas canvas;
    public float cursorSpeed = 700f;
    public int controllerSlot = 1;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    void Awake()
    {
        if (canvas != null)
            raycaster = canvas.GetComponent<GraphicRaycaster>();

        eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (cursor == null || canvas == null)
            return;

        float x = Input.GetAxis("Horizontal_P" + controllerSlot);
        float y = Input.GetAxis("Vertical_P" + controllerSlot);

        cursor.anchoredPosition += new Vector2(x, y)
            * cursorSpeed * Time.unscaledDeltaTime;

        RectTransform canvasRect =
            canvas.GetComponent<RectTransform>();

        cursor.anchoredPosition = new Vector2(
            Mathf.Clamp(cursor.anchoredPosition.x,
                canvasRect.rect.xMin, canvasRect.rect.xMax),
            Mathf.Clamp(cursor.anchoredPosition.y,
                canvasRect.rect.yMin, canvasRect.rect.yMax)
        );

        KeyCode aButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0
            + (controllerSlot - 1) * 20
        );

        if (Input.GetKeyDown(aButton))
            ClickUI();
    }

    void ClickUI()
    {
        if (raycaster == null || eventSystem == null)
        {
            Debug.LogWarning(
                "VirtualCursor: Missing GraphicRaycaster or EventSystem."
            );
            return;
        }

        PointerEventData pointer = new PointerEventData(eventSystem);
        pointer.position = RectTransformUtility.WorldToScreenPoint(
            canvas.worldCamera, cursor.position
        );

        var results = new System.Collections.Generic.List<RaycastResult>();
        raycaster.Raycast(pointer, results);

        foreach (RaycastResult result in results)
        {
            Button button = result.gameObject.GetComponentInParent<Button>();

            if (button != null && button.interactable)
            {
                button.onClick.Invoke();
                Debug.Log("Virtual cursor clicked: " + button.name);
                return;
            }
        }

        Debug.Log("Virtual cursor: No button found under cursor.");
    }
}
