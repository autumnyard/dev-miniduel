using AutumnYard.Unity.Core;
using UnityEngine;
using UnityEngine.UI;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UIPreparing : Displayable
    {
        public class DTO
        {
            public DisplayBoard.DTO board;
        }

        [SerializeField] private DisplayBoard _board;
        [SerializeField] private Image _blockPlayer1;
        [SerializeField] private Image _blockPlayer2;

        private DTO _dto;

        public void Set(DTO dto)
        {
            _dto = dto;
            _board.Set(_dto.board);

            Refresh();
        }

        private void Refresh()
        {
            _blockPlayer1.gameObject.SetActive(true);
            _blockPlayer2.gameObject.SetActive(true);

            bool hasPlayerfinished;

            hasPlayerfinished = CheckPlayerFinished(_dto.board, 0);
            _blockPlayer1.gameObject.SetActive(hasPlayerfinished);

            // If player1 hasn't finished, no need to check player2 yet
            if (!hasPlayerfinished)
            {
                return;
            }

            hasPlayerfinished = CheckPlayerFinished(_dto.board, 1);
            _blockPlayer2.gameObject.SetActive(hasPlayerfinished);
        }

        private static bool CheckPlayerFinished(DisplayBoard.DTO board, int player)
        {
            for (int i = 0; i < board.locations; i++)
            {
                if (board.board[player, i] == EPiece.None)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
