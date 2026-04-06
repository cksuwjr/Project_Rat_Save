using UnityEngine;

public enum KeyInput
{
    Fire1,
    Fire2,
    Fire3,
    Fire4,
    Fire5
}
public interface IInputHandle
{
    public Vector3 GetInput();

    public bool GetKeyInput(KeyInput input);
}