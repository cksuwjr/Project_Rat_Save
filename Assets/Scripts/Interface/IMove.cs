using UnityEngine;

public interface IMove
{
    public Vector3 Direction { get; set; }
    public bool Movable { get; set; }
    public void Move(Vector3 direction, float speed);
}