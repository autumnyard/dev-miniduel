using AutumnYard.Unity.Core;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayBoard : Displayable,
        DisplaySlot.IEventsListener
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

        public interface IEventsListener
        {
            void OnFinishedFightAnimations();
        }

        [SerializeField] private DisplayPanel[] _panels;
        [SerializeField] private DisplaySlot[] _slotsPlayer1;
        [SerializeField] private DisplaySlot[] _slotsPlayer2;

        private DTO _dto;
        private IEventsListener _listener;

        public void Initialize()
        {

        }

        public void Set(DTO dto)
        {
            _dto = dto;

            RefreshEventListening();
            Refresh();
        }

        #region Event Listening

        public void SetListener(IEventsListener listener)
        {
            _listener = listener;
        }

        public void UnsetListener()
        {
            _listener = null;
        }

        private void RefreshEventListening()
        {
            if (_dto.IsDueling)
            {
                for (int i = 0; i < _slotsPlayer1.Length; i++)
                {
                    _slotsPlayer1[i].UnsetListener();
                }
                _slotsPlayer1[_dto.lastFightIndex].SetListener(this);
            }
        }

        public void OnFinishedFightAnimations()
        {
            _listener?.OnFinishedFightAnimations();
        }

        #endregion // Event Listening


        public void Refresh()
        {
            if (!_dto.IsDueling)
            {
                RefreshNotDuelingPhase();
            }
            else
            {
                RefreshDuelingPhase();
            }
        }


        private void RefreshDuelingPhase()
        {
            PlayAnimation(0, _dto.lastFightIndex);
            PlayAnimation(1, _dto.lastFightIndex);
        }

        private void RefreshNotDuelingPhase()
        {
            ResetAnimations();

            for (int i = 0; i < _dto.locations; i++)
            {
                SetPiece(0, i, _dto.board[0, i]);
                SetPiece(1, i, _dto.board[1, i]);
            }
        }

        private void PlayAnimation(int player, int location)
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
