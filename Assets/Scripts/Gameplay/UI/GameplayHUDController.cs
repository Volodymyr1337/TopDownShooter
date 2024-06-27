using Application;

namespace Gameplay.UI
{
    public class GameplayHUDController : BaseViewController<GameplayHUDView>
    {
        public GameplayHUDController() : base("Gameplay/UI/HUDView")
        {
        }

        public void UpdateKillCount(int killCount)
        {
            View.killCountTxt.SetText($"Kills: {killCount}");
        }

        public void UpdatePlayerHp(float health)
        {
            View.healthBar.value = health;
        }
    }
}
