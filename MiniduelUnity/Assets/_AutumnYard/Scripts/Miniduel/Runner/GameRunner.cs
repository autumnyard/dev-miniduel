using AutumnYard.Miniduel.Unity.Display;
using AutumnYard.Miniduel.Unity.Handler;
using System;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Runner
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private UISystem _uiSystem;

        private GameHandler _gameHandler;

        private void Start()
        {
            SetGame();
        }

        private void SetGame()
        {
            _gameHandler = GameHandler.Instance;
            _gameHandler.SetGame(_uiSystem);
        }

        [ContextMenu("Auto play 1")]
        public void TEST_AutoPlay1()
        {
            _gameHandler.SetPiece(0, 0, EPiece.Attack);
            _gameHandler.SetPiece(1, 0, EPiece.Attack);
            _gameHandler.SetPiece(0, 1, EPiece.Attack);
            _gameHandler.SetPiece(1, 1, EPiece.Attack);
            _gameHandler.SetPiece(0, 2, EPiece.Attack);
            _gameHandler.SetPiece(1, 2, EPiece.Attack);
        }

        [ContextMenu("Reset")]
        private void TEST_Reset()
        {
            RoundReset();
        }

        #region Input

        public void RoundPlay()
        {
            bool result = _gameHandler.Game.StartDuel();
            if (result)
                return;

            result = _gameHandler.Game.PlayNextFight();
            if (result)
                return;

            result = _gameHandler.Game.FinishRound();
            if (!result)
                return;

            Debug.Log("FINISHED!!");
        }

        public void RoundReset()
        {
            _gameHandler.SetGame(_uiSystem);
        }

        public void OnSetPiece_0_0(Int32 value) => _gameHandler.SetPiece(0, 0, (EPiece)value);
        public void OnSetPiece_1_0(Int32 value) => _gameHandler.SetPiece(1, 0, (EPiece)value);
        public void OnSetPiece_0_1(Int32 value) => _gameHandler.SetPiece(0, 1, (EPiece)value);
        public void OnSetPiece_1_1(Int32 value) => _gameHandler.SetPiece(1, 1, (EPiece)value);
        public void OnSetPiece_0_2(Int32 value) => _gameHandler.SetPiece(0, 2, (EPiece)value);
        public void OnSetPiece_1_2(Int32 value) => _gameHandler.SetPiece(1, 2, (EPiece)value);

        public void OnPieceSet(int player, int fight, EPiece piece)
        {
            _gameHandler.SetPiece(player, fight, piece);
        }

        #endregion // Input
    }
}
