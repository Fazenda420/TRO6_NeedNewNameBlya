using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashLightTEST : MonoBehaviour
{
    public Light2D lg;

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard != null)
        {
            if (keyboard.fKey.isPressed)
            {
                lg.enabled = false;
            }
        }
    }
}
