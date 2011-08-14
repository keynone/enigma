using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPG
{
    public class Hero
    {
        public Vector2 Location = Vector2.Zero;
        public bool IsMoving = false;
        public byte StepsLeft = 0;
    }
}
