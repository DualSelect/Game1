using UnityEngine;

public class WindowSize //: MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(450, 800, false);

    }
}
