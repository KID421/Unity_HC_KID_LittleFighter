using UnityEngine;

public class GM : MonoBehaviour
{
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
