using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenToggleController : MonoBehaviour
{
    public void ChangeToggle()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("Toggle screen");
    }
}
