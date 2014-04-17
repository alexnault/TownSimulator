using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TileEngine
{
    //Sert a insérer les variables globales (mostly)
    public static class Engine
    {
        public const int TileWidth = 32;
        public const int TileHeight = 32;

        //TODO: Move eventually in a settings struct/class?
        public const int ScreenWidth = 1280;
        public const int ScreenHeight = 720;

        public static Point ConvertPositionToCell(this Vector2 position)
        {
            return new Point(
                (int)(position.X / TileWidth),
                (int)(position.Y / TileHeight)
                );
        }

        public static Rectangle CreateRectForCell(this Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }

        public static Vector2 ConvertCellToPosition(this Point cell)
        {
            return new Vector2(
                cell.X * TileWidth + TileWidth,
                cell.Y * TileHeight + TileHeight);

        }

        public static bool IsNextTo(this Point p1, Point p2)
        {
            int deltaX = Math.Abs(p1.X - p2.X);
            int deltaY = Math.Abs(p1.Y - p2.Y);

            return (deltaX == 1 && deltaY == 0 || deltaX == 0 && deltaY == 1);

        }

    }
}
