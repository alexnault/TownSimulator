using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TileEngine
{
    public static class InputHelper
    {
        private static KeyboardState newKBState;
        private static KeyboardState oldKBState;
        private static MouseState oldMouseState;
        private static MouseState newMouseState;

        private static Camera _camera;

        public static Vector2 MousePosition
        {
            get { return new Vector2(newMouseState.X, newMouseState.Y); }
        }


        public static void Update(Camera camera)
        {
            oldKBState = newKBState;
            newKBState = Keyboard.GetState();

            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();

            _camera = camera;
        }

        public static bool IsNewKeyPressed(Keys key)
        {
            return (newKBState.IsKeyDown(key) && oldKBState.IsKeyUp(key));
        }

        public static bool IsKeyDown(Keys key)
        {
            return newKBState.IsKeyDown(key);
        }

        public static bool LeftMouseButtonClicked()
        {
            return (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released);
        }

        public static bool LeftMouseButtonDown()
        {
            return (newMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool RightMouseButtonClicked()
        {
            return (newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released);
        }
        public static bool RightMouseButtonDown()
        {
            return (newMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool IsMouseOn(Rectangle rect)
        {
            return rect.Intersects(new Rectangle(newMouseState.X + (int)_camera.Position.X, newMouseState.Y + (int)_camera.Position.Y, 1, 1));
        }

    }
}
