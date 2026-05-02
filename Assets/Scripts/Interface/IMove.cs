using UnityEngine;

public interface IMove
{
    public bool Movable { get; set; }
    public void Move(Vector3 direction, float speed);
}