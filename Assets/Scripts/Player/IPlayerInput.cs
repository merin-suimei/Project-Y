using System;
using UnityEngine;
public interface IPlayerInput
{
    Vector2 MoveDirection { get; }

    Vector2 AimDirection { get; }

    event Action OnInteract;
}
