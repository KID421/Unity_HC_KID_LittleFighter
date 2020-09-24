using UnityEngine;

public class GM : MonoBehaviour
{
    private void Start()
    {
#if (UNITY_WEBPLAYER || UNITY_WEBGL) && !UNITY_EDITOR
    try {
        Application.ExternalCall("GameControlReady");
    } catch (System.Exception e) {
        Debug.LogError("GameControlReady function not on webpage"+e);
    }
#endif
    }

    // This function will be called from the webpage
    public void FocusCanvas(string p_focus)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
    if (p_focus == "0") {
        WebGLInput.captureAllKeyboardInput = false;
    } else {
        WebGLInput.captureAllKeyboardInput = true;
    }
#endif
    }
}
