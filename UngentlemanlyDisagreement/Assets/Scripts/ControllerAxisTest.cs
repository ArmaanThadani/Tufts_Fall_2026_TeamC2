
using UnityEngine;

public class ControllerAxisTest : MonoBehaviour
{
    [SerializeField] private int controllerSlot = 1;

    private float[] previousValues = new float[10];

    private void Update()
    {
        for (int i = 1; i <= 10; i++)
        {
            string axisName = "TestAxis" + i;
            float value = Input.GetAxisRaw(axisName);

            if (Mathf.Abs(value - previousValues[i - 1]) > 0.05f)
            {
                Debug.Log(
                    axisName + " | Controller " + controllerSlot
                    + " | Value: " + value.ToString("F2")
                );

                previousValues[i - 1] = value;
            }
        }
    }
}
