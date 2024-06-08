using AutumnYard.Unity.Core;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UIDueling : Displayable
    {
        public class DTO
        {
            public DisplayBoard.DTO board;
            public DisplayResults.DTO results;
        }

        [SerializeField] private DisplayBoard _board;
        [SerializeField] private DisplayResults _results;

        private DTO _dto;

        public void Set(DTO dto)
        {
            _dto = dto;
            _board.Set(_dto.board);
            _results.Set(_dto.results);
        }
    }
}
