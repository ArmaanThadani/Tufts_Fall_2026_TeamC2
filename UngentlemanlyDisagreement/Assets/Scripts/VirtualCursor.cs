
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class VirtualCursor : MonoBehaviour
{
    [SerializeField] private RectTransform cursor;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float cursorSpeed = 700f;
    [SerializeField] private int controllerSlot = 1;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private PointerEventData pointerData;
    private GameObject hoveredObject;

    private void Awake()
    {
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        if (eventSystem != null)
            pointerData = new PointerEventData(eventSystem);
    }

    private void Update()
    {
        if (cursor == null || canvas == null ||
            raycaster == null || eventSystem == null)
            return;

        string horizontalAxis = "Horizontal_P" + controllerSlot;
        string verticalAxis = "Vertical_P" + controllerSlot;

        float x = Input.GetAxisRaw(horizontalAxis);
        float y = Input.GetAxisRaw(verticalAxis);

        Vector2 position = cursor.position;
        position += new Vector2(x, y) *
                    cursorSpeed * Time.unscaledDeltaTime;

        position.x = Mathf.Clamp(position.x, 0, Screen.width);
        position.y = Mathf.Clamp(position.y, 0, Screen.height);

        cursor.position = position;

        UpdateHover();

        KeyCode aButton = (KeyCode)(
            (int)KeyCode.Joystick1Button0 +
            (controllerSlot - 1) * 20
        );

        if (Input.GetKeyDown(aButton))
            ClickHoveredObject();
    }

    private void UpdateHover()
    {
        pointerData.position = cursor.position;

        List<RaycastResult> results =
            new List<RaycastResult>();

        raycaster.Raycast(pointerData, results);

        GameObject newHover = results.Count > 0
            ? results[0].gameObject
            : null;

        if (newHover == hoveredObject)
            return;

        if (hoveredObject != null)
        {
            ExecuteEvents.Execute(
                hoveredObject,
                pointerData,
                ExecuteEvents.pointerExitHandler
            );
        }

        hoveredObject = newHover;

        if (hoveredObject != null)
        {
            ExecuteEvents.Execute(
                hoveredObject,
                pointerData,
                ExecuteEvents.pointerEnterHandler
            );
        }
    }

    private void ClickHoveredObject()
    {
        if (hoveredObject == null)
            return;

        pointerData.button = PointerEventData.InputButton.Left;

        ExecuteEvents.Execute(
            hoveredObject,
            pointerData,
            ExecuteEvents.pointerDownHandler
        );

        ExecuteEvents.Execute(
            hoveredObject,
            pointerData,
            ExecuteEvents.pointerUpHandler
        );

        ExecuteEvents.Execute(
            hoveredObject,
            pointerData,
            ExecuteEvents.pointerClickHandler
        );
    }
}
