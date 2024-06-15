using AutumnYard.Unity.Core;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayBoard : Displayable
    {
        public class DTO
        {
            public int lastFightIndex = -1;
            public int players;
            public int locations;
            public EPiece[,] board;
            public int newPiecePlayer;
            public int newPieceLocation;

            public bool IsDueling => lastFightIndex >= 0;
        }

        [SerializeField] private DisplayPanel[] _panels;
        [SerializeField] private DisplaySlot[] _slotsPlayer1;
        [SerializeField] private DisplaySlot[] _slotsPlayer2;

        private DTO _dto;


        public void Set(DTO dto)
        {
            _dto = dto;

            Refresh();
        }

        public void Refresh()
        {
            if (!_dto.IsDueling)
            {
                ResetAnimations();
            }
            else
            {
                PlayAnimation(0, _dto.lastFightIndex);
                PlayAnimation(1, _dto.lastFightIndex);
                return;
            }

            for (int i = 0; i < _dto.locations; i++)
            {
                SetPiece(0, i, _dto.board[0, i]);
                SetPiece(1, i, _dto.board[1, i]);
            }
        }

        public void PlayAnimation(int player, int location)
        {
            var slot = GetSlot(player, location);
            slot.PlayAnimation();
        }

        private void ResetAnimations()
        {
            for (int i = 0; i < _dto.locations; i++)
            {
                _slotsPlayer1[i].ResetAnimation();
                _slotsPlayer2[i].ResetAnimation();
            }
        }


        private void SetPiece(int player, int location, EPiece piece)
        {
            bool newPiece = player == _dto.newPiecePlayer && location == _dto.newPieceLocation;
            bool unsettedPiece = piece == EPiece.None;

            GetSlot(player, location).Set(piece);
        }

        private DisplaySlot GetSlot(int player, int location)
        {
            if (player == 0)
                return _slotsPlayer1[location];
            else
                return _slotsPlayer2[location];
        }
    }
}
