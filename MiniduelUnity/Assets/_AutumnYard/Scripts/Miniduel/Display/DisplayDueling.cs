using AutumnYard.Unity.Core;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class DisplayDueling : Displayable
    {
        public class DTO
        {
            public int lastFightIndex;
            public bool offenseChange;
            public bool offense;
            public int lastFightPointsPlayer1;
            public int lastFightPointsPlayer2;

            public bool IsDueling => lastFightIndex >= 0;
        }

        [SerializeField] private DisplayOffense[] _offenses;
        [SerializeField] private DisplayPoint[] _pointsPlayer1;
        [SerializeField] private DisplayPoint[] _pointsPlayer2;

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
                Clear();
            }
            else
            {
                // These are the consequences and should be triggered AFTER the animation has finished
                //DisplayFightResults();
            }
        }

        public void RefreshAfterFight()
        {
            DisplayFightResults();
        }


        private void Clear()
        {
            for (int i = 0; i < _pointsPlayer1.Length; i++)
            {
                _pointsPlayer1[i].Unset();
                _pointsPlayer2[i].Unset();
            }
            _offenses[0].Unset();
            _offenses[1].Unset();
        }

        private void DisplayFightResults()
        {
            _offenses[0].Set(_dto.offense == false, _dto.offenseChange);
            _offenses[1].Set(_dto.offense == true, _dto.offenseChange);
            _pointsPlayer1[_dto.lastFightIndex].Set(_dto.lastFightPointsPlayer1);
            _pointsPlayer2[_dto.lastFightIndex].Set(_dto.lastFightPointsPlayer2);
        }
    }
}
