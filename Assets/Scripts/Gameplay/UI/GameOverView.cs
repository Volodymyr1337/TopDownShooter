using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class GameOverView : MonoBehaviour
    {
        public Button restartBtn;

        private void OnDestroy()
        {
            restartBtn.onClick.RemoveAllListeners();
        }
    }
}
