using System;
using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    [CreateAssetMenu(fileName = "Settings", menuName = "AutumnYard/Settings")]
    public sealed class Settings : ScriptableObject
    {
        [SerializeField] private PieceSetting _default = PieceSetting.Default;
        [SerializeField] private PieceSetting[] _pieces;

        public PieceSetting GetPieceSetting(EPiece piece)
        {
            for (int i = 0; i < _pieces.Length; i++)
            {
                if (_pieces[i].id == piece)
                    return _pieces[i];
            }
            return _default;
        }

        [Serializable]
        public class PieceSetting
        {
            public EPiece id;
            public Sprite sprite;
            public Color color;
            public string text;

            public static PieceSetting Default =>
                new PieceSetting()
                {
                    id = EPiece.None,
                    sprite = null,
                    color = Color.gray,
                    text = "-"
                };
        }
    }
}
