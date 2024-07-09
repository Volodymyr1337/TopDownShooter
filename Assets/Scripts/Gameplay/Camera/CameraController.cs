using Application;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Camera
{
    public class CameraController : BaseController
    {
        private IUnitPosition _unitPosition;
        private Transform _cameraTransform;
        
        public CameraController(IUnitPosition playerPos)
        {
            _unitPosition = playerPos;
        }
        
        public override void Initialize()
        {
            MonoService.OnUpdate += OnUpdate;
            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        public override void Dispose()
        {
            MonoService.OnUpdate -= OnUpdate;
            
            base.Dispose();
        }

        private void OnUpdate(float data)
        {
            Vector3 playerPos = _unitPosition.GetPosition();
            _cameraTransform.position = new Vector3(playerPos.x, playerPos.y, _cameraTransform.position.z);
        }
    }
}
