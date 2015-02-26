using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        // *************************
        // * Fields and Properties *
        // *************************

        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        // NOTE: Be careful with these values, as no sanity checking
        // is done!
        // Max values will depend a lot on your screen resolution
        // Absolute Minimum values:
        // bWidth = 2
        // bHeight = 2
        // tInterval = 0.01f (lower makes no difference though)
        // texScale = 0.02f
        // Board Width variable (see Life class for more)
        private int bWidth = 20;
        // Board Height variable (see Life class for more)
        private int bHeight = 12;
        // Texture Scaling variable (see Life class for more)
        private float texScale = 0.5f;
        // Time Interval variable (see Life class for more)
        private float tInterval = 0.1f;

        // The go boolean field is used to start and stop the simulation
        private bool go = false;

        // Make a new instance of the life class
        // This effectively instantiates the game
        Life life;
        
        // *** Copied from Bryan's Tank Game code
        // Used to make sure only a single instance of Game1 is created
        // A static field called "instance" of type Game1
        private static Game1 instance;

        // *** Copied from Bryan's Tank Game code
        // Used to make sure only a single instance of Game1 is created
        // A static property called "Instance" of Game1 - used to access
        // the "instance" field
        public static Game1 Instance
        {
        	get
	        {
		        return instance;
	        }
        }



        // ****************
        // * Constructors *
        // ****************

        // Default Constructor
        // Mostly Visual Stuio generated code
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            // *** Copied from Bryan's Tank Game code
            // Used to make sure only a single instance of Game1 is created
            instance = this;

            // Set the game screen width and height
            // Ideally this will be calculated by taking into account certain
            // variables, such as the number of cells on the board and the 
            // the scaling applied to textures
            // NOTE: My textures are 100x100 pixels,
            // thus multiplying texScale * 100 gets the scaled texture size in pixels
            // Hard-coding the texture size is not ideal, but works for now.
            graphics.PreferredBackBufferWidth = (int)(bWidth * (texScale * 100));
            graphics.PreferredBackBufferHeight = (int)(bHeight * (texScale * 100));

        }



        // ***********
        // * Methods *
        // ***********

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            // Constructor Testing
            //life = new Life();
            //life = new Life(bWidth, bHeight);
            life = new Life(bWidth, bHeight, texScale, tInterval, go);
            
            // Make the mouse visible in the game
            // It is not possible to interact with the game (using the mouse) if this is not set to true!
            IsMouseVisible = true;

            // Random 50% starting state
            // Its not perfect - there's no activated cell tracking, so the same cell
            // could potentially be activated (set to alive) two (or more) times.
            int x = 0;
            int y = 0;
            Random randomCell = new Random();

            for (int i = 0; i < ( (life.BoardWidth * life.BoardHeight) / 2); i++)
            {
                x = randomCell.Next(0, (bWidth) );
                y = randomCell.Next(0, (bHeight) );
                life.TempArray[x, y] = true;
            }


            // *******************
            // * Begin Test Data *
            // *******************

            /*
            // Checker Board
            for (y = 0; y < life.BoardHeight; y = y + 2)
            {
                for (x = 0; x < life.BoardWidth; x = x + 2)
                {
                    life.TempArray[x, y] = true;
                }
            }
            for (y = 1; y < life.BoardHeight; y = y + 2)
            {
                for (x = 1; x < life.BoardWidth; x = x + 2)
                {
                    life.TempArray[x, y] = true;
                }
            }
            */

            /*
            // Diagonal Line
            for (x = 0; x < life.BoardWidth && y < life.BoardHeight; x++)
            {
                y = x;
                life.TempArray[x, y] = true;
            }
            */

            /*
            // Cross or X pattern
            for (x = 0; x < life.BoardWidth && y < life.BoardHeight; x++)
            {
                y = x;
                life.TempArray[x, y] = true;
            }
            x = 0;
            for (y = (life.BoardHeight - 1); y >= 0; y--)
            {
                life.TempArray[x, y] = true;

                if (x < (life.BoardWidth - 1) )
                {
                    x++;
                }
            }
            */
 
            /*
            // Border Test Square
            for (x = 0; x < life.BoardWidth; x++)
            {
                life.TempArray[x, 0] = true;
            }
            for (y = 0; y < life.BoardHeight; y++)
            {
                life.TempArray[0, y] = true;
            }
            for (x = 0; x < life.BoardWidth; x++)
            {
                life.TempArray[x, (life.BoardHeight - 1)] = true;
            }
            for (y = 0; y < life.BoardHeight; y++)
            {
                life.TempArray[(life.BoardWidth - 1), y] = true;
            }
            */
              
            /*
            // Block
            life.TempArray[1, 1] = true;
            life.TempArray[1, 2] = true;
            life.TempArray[2, 1] = true;
            life.TempArray[2, 2] = true;
            */
            
            /*
            // Blinker (near top-left)
            //life.TempArray[2, 2] = true;
            //life.TempArray[2, 3] = true;
            //life.TempArray[2, 4] = true;
            */

            /*
            // Blinker (near bottom-right)
            life.TempArray[6, 7] = true;
            life.TempArray[7, 7] = true;
            life.TempArray[8, 7] = true;
            */

            /*
            // 3x3 square (top-left)
            for (y = 0; y < 3; y++)
            {
                for (x = 0; x < 3; x++)
                {
                    life.TempArray[x, y] = true;
                }
            }
            */
            // *****************
            // * End Test Data *
            // *****************

            life.UpdateArrays();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            life.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            
            life.Update(gameTime);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            
            spriteBatch.Begin();
            life.Draw(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
