using UnityEngine;
public interface IPlayerInput
{
    Vector3 MoveDirection { get; }

    Vector3 AimWorldPoint { get; }

}