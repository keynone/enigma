using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPG
{
    static class Camera
    {
        public static int X;
        public static int Y;

        public static void FollowCharacter(MapCharacter mapCharacter, Vector2 Offset)
        {
            X = (mapCharacter.X - (int)Offset.X + 8);
            Y = (mapCharacter.Y - (int)Offset.Y + 8);
        }
    }
}
