using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ace_game
{
    class Block : Tile
    {
        public Block(int x, int y, Texture2D spr)
        {
            frame = new Rectangle(x, y, 32, 32);
            sprite = spr;
        }

        public override bool Collider(string direction)
        {
            return true;
        }
    }
}
