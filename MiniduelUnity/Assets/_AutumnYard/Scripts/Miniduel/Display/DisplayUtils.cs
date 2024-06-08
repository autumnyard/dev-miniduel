using UnityEngine;

namespace AutumnYard.Miniduel.Unity.Display
{
    public static class DisplayUtils
    {
        public static string GetPieceColor(EPiece piece)
        {
            Color color;
            switch (piece)
            {
                default:
                case EPiece.None:
                    color = Color.gray;
                    break;

                case EPiece.Attack:
                    color = Color.red;
                    break;

                case EPiece.Defense:
                    color = Color.green;
                    break;

                case EPiece.Parry:
                    color = Color.blue;
                    break;
            }
            return "#" + ColorUtility.ToHtmlStringRGB(color);
        }

        public static string GetPieceWithColor(EPiece piece)
        {
            Color color;
            switch (piece)
            {
                default:
                case EPiece.None:
                    color = Color.gray;
                    break;

                case EPiece.Attack:
                    color = Color.red;
                    break;

                case EPiece.Defense:
                    color = Color.green;
                    break;

                case EPiece.Parry:
                    color = Color.blue;
                    break;
            }
            string hex = DisplayUtils.GetPieceColor(piece);
            return $"<color={hex}>{piece}</color>";
        }
    }
}
