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
            
            View.joystick.OnPressed += OnJoystickPressed;
            View.joystick.OnReleased += OnJoystickReleased;
        }

        public override void Dispose()
        {
            View.joystick.OnPressed -= OnJoystickPressed;
            View.joystick.OnReleased -= OnJoystickReleased;
            
            base.Dispose();
        }

        public Vector2 GetDirection()
        {
            return View.joystick.Direction;
        }

        private void OnJoystickPressed()
        {
            OnPressed?.Invoke();
        }

        private void OnJoystickReleased()
        {
            OnReleased?.Invoke();;
        }

        public void Release()
        {
            OnReleased?.Invoke();
        }
    }
}
