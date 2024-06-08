using AutumnYard.Unity.Core;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UIPreparing : Displayable
    {
        public class DTO
        {
            public DisplayBoard.DTO board;
        }

        [SerializeField] private DisplayBoard _board;

        private DTO _dto;

        public void Set(DTO dto)
        {
            _dto = dto;
            _board.Set(_dto.board);
        }
    }
}
