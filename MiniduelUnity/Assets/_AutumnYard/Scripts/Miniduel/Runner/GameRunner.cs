using AutumnYard.Miniduel.Unity.Display;
using System;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Runner
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private DisplayBoard _boardDisplay;
        [SerializeField] private UISystem _uiSystem;

        private Game _game;

        private void Start()
        {
            SetGame();
        }

        private void SetGame()
        {
            _game = new Game(_uiSystem);
            //_game.SetPiece(0, 0, EPiece.Attack);
            //_game.SetPiece(1, 0, EPiece.Attack);
            //_game.SetPiece(0, 1, EPiece.Attack);
            //_game.SetPiece(1, 1, EPiece.Attack);
            //_game.SetPiece(0, 2, EPiece.Attack);
            //_game.SetPiece(1, 2, EPiece.Attack);
        }

        #region Input

        public void PlayRound()
        {
            bool result = _game.StartDuel();
            if (result)
                return;

            result = _game.PlayNextFight();
            if (result)
                return;

            result = _game.FinishRound();
            if (!result)
                return;

            Debug.Log("FINISHED!!");
        }

        public void OnSetPiece_0_0(Int32 value) => _game.SetPiece(0, 0, (EPiece)value);
        public void OnSetPiece_1_0(Int32 value) => _game.SetPiece(1, 0, (EPiece)value);
        public void OnSetPiece_0_1(Int32 value) => _game.SetPiece(0, 1, (EPiece)value);
        public void OnSetPiece_1_1(Int32 value) => _game.SetPiece(1, 1, (EPiece)value);
        public void OnSetPiece_0_2(Int32 value) => _game.SetPiece(0, 2, (EPiece)value);
        public void OnSetPiece_1_2(Int32 value) => _game.SetPiece(1, 2, (EPiece)value);

        #endregion // Input
    }
}
