using System;
using UnityEngine;

namespace Input
{
    public interface IInput
    {
        event Action OnPressed;
        event Action OnReleased;
        
        Vector2 GetDirection();
    }
}
