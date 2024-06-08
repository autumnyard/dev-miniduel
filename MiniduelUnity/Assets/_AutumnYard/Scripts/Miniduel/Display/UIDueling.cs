using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public class UIDueling : MonoBehaviour
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
