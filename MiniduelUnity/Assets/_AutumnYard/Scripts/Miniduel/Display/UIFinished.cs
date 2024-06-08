using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UIFinished : MonoBehaviour
    {
        public class DTO
        {

        }

        private DTO _dto;

        public void Set(DTO dto)
        {
            _dto = dto;
        }
    }
}
