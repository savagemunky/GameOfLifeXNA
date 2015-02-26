using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    public abstract class LifeCommon
    {
        // *************************
        // * Fields and Properties *
        // *************************

        // The go boolean field is used to start and stop the simulation
        public bool go;

        // Position is a 2-dimensional vector, used here for
        // storing the x, y coordinates of a texture
        public Vector2 Position;
        
        // The wrapAround boolean is not currently used - it
        // was intended to be used to let the game know if the
        // board "wrapped around" from one edge to its opposite
        // edge (i.e. left->right, top->bottom, etc) to give 
        // the appearance of an endless board
        // It takes significant extra code dealing with the
        // boundary cells to implement this, and I don't have
        // time unfortunately
        //public bool wrapAround;
        
        // spriteAliveCell is used to store the "alive" texture
        private Texture2D spriteAliveCell;

        // SpriteAliveCell properties - get and set
        public Texture2D SpriteAliveCell
        {
            get { return spriteAliveCell; }
            set { spriteAliveCell = value; }
        }

        // spriteDeadCell is used to store the "dead" texture
        private Texture2D spriteDeadCell;

        public Texture2D SpriteDeadCell
        {
            get { return spriteDeadCell; }
            set { spriteDeadCell = value; }
        }



        // ***********
        // * Methods *
        // ***********

        // *** Copied from Bryan's Tank Game code
        // Abstract classes
        // These need to be implemented in any child classes
        // that inherit from LifeCommon
        
        // LoadContent: Loads game resources (images, sounds, etc)
        public abstract void LoadContent();
        
        // Update: Updates the game state (game objects)
        public abstract void Update(GameTime gameTime);
        
        // Draw: Draws or redraws the elements within the function
        // on the screen
        public abstract void Draw(GameTime gameTime);
    }
}
