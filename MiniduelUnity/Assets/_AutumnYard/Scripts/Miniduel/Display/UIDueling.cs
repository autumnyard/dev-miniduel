using AutumnYard.Unity.Core;
using UnityEngine;
using UnityEngine.UI;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UIDueling : Displayable,
        DisplayBoard.IEventsListener
    {
        public class DTO
        {
            public DisplayBoard.DTO board;
            public DisplayDueling.DTO duel;
            public DisplayResults.DTO results;
        }

        [SerializeField] private DisplayBoard _board;
        [SerializeField] private DisplayDueling _duel;
        [SerializeField] private DisplayResults _results;
        [SerializeField] private Image _blockLocation1;
        [SerializeField] private Image _blockLocation2;
        [SerializeField] private Image _blockLocation3;

        private DTO _dto;

        public void Set(DTO dto)
        {
            _board.SetListener(this);

            _dto = dto;
            _board.Set(_dto.board);
            _duel.Set(_dto.duel);
            _results.Set(_dto.results);

            Refresh();
        }


        #region Event Listening

        public void OnFinishedFightAnimations()
        {
            _duel.RefreshAfterFight();
        }

        #endregion // Event Listening


        private void Refresh()
        {
            switch (_dto.results.round)
            {
                default:
                case 0:
                    {
                        _blockLocation1.gameObject.SetActive(true);
                        _blockLocation2.gameObject.SetActive(true);
                        _blockLocation3.gameObject.SetActive(true);
                    }
                    break;

                case 1:
                    {
                        _blockLocation1.gameObject.SetActive(false);
                        _blockLocation2.gameObject.SetActive(true);
                        _blockLocation3.gameObject.SetActive(true);
                    }
                    break;

                case 2:
                    {
                        _blockLocation1.gameObject.SetActive(false);
                        _blockLocation2.gameObject.SetActive(false);
                        _blockLocation3.gameObject.SetActive(true);
                    }
                    break;

                case 3:
                    {
                        _blockLocation1.gameObject.SetActive(false);
                        _blockLocation2.gameObject.SetActive(false);
                        _blockLocation3.gameObject.SetActive(false);
                    }
                    break;
            }

        }
    }
}
