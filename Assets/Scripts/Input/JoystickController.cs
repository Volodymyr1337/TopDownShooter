using System;
using Application;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Input
{
    public class JoystickController : BaseViewController<JoystickView>, IInput
    {
        public event Action OnPressed;
        public event Action OnReleased;
        
        public JoystickController(string assetName) : base(assetName)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            
            View.joystick.OnPressed += OnPressed;
            View.joystick.OnReleased += OnReleased;
        }

        public override void Dispose()
        {
            View.joystick.OnPressed -= OnPressed;
            View.joystick.OnReleased -= OnReleased;
            base.Dispose();
        }

        public Vector2 GetDirection()
        {
            return View.joystick.Direction;
        }

        public void Release()
        {
            OnReleased?.Invoke();
        }
    }
}
