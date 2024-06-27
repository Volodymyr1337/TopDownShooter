using System;
using Application;

namespace Gameplay.UI
{
    public class GameOverScreenController : BaseViewController<GameOverView>
    {
        private Action _onRestartBtnClick;

        public GameOverScreenController(Action onRestartBtnClick) : base("Gameplay/UI/GameOverView")
        {
            _onRestartBtnClick = onRestartBtnClick;
        }

        public override void Initialize()
        {
            base.Initialize();
            View.restartBtn.onClick.AddListener(OnRestartBtnClick);
        }

        private void OnRestartBtnClick()
        {
            _onRestartBtnClick?.Invoke();
            Dispose();
        }
    }
}
