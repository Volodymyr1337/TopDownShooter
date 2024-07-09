using Gameplay;
using Services.Mono;
using UnityEngine;

namespace Application
{
    public class Client : MonoBehaviour
    {
        private MonoService _monoService;
        private ControllerFactory _controllerFactory;

        void Start()
        {
            _monoService = new MonoService();
            _monoService.Initialize();
            _controllerFactory = new ControllerFactory(_monoService);
        
            // In real project I would make a FSM with states Login, Lobby, Gameplay.. and create all the controllers inside a corresponding state
            // so that after the game switched to another state, the controllers from the previous state would be disposed

            GameplayController gameplayController = _controllerFactory.CreateController<GameplayController>();
            gameplayController.Initialize();
        }
    }
}
