using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputHandle : MonoBehaviour, IInputHandle
{
    public Vector3 GetInput()
    {
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    public bool GetKeyInput(KeyInput input)
    {
        return Input.GetButtonDown(input.ToString());
    }
}