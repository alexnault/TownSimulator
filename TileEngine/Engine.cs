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


        public static Point ConvertPositionToCell(Vector2 position)
        {
            return new Point(
                (int)(position.X / TileWidth),
                (int)(position.Y / TileHeight)
                );
        }

        public static Rectangle CreateRectForCell(Point cell)
        {
            return new Rectangle(
                cell.X * TileWidth,
                cell.Y * TileHeight,
                TileWidth,
                TileHeight);
        }

        public static Vector2 ConvertCellToPosition(Point cell)
        {
            return new Vector2(
                cell.X * TileWidth + TileWidth,
                cell.Y * TileHeight + TileHeight);

        }
    }
}
