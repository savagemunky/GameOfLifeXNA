using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLife
{
    public class Life:LifeCommon
    {
        // *************************
        // * Fields and Properties *
        // *************************

        // Set up the board
        // A integer variable to store the board width (in cells, NOT pixels)
        private int boardWidth;

        public int BoardWidth
        {
            get { return boardWidth; }
            set { boardWidth = value; }
        }

        // A integer variable to store the board height (in cells, NOT pixels)
        private int boardHeight;

        public int BoardHeight
        {
            get { return boardHeight; }
            set { boardHeight = value; }
        }

        // This is the main array for storing cell values
        // This holds the current generation of cells and is static until
        // updated with the values calculated in the tempArray
        // Uses boolean values. True = Alive, False = Dead
        private bool[,] boardArray;

        public bool[,] BoardArray
        {
            get { return boardArray; }
            set { boardArray = value; }
        }

        // This is the temporary array where the next generation of cell values
        // is calculated and the copied to the boardArray on completion
        // Uses boolean values. True = Alive, False = Dead
        private bool[,] tempArray;

        public bool[,] TempArray
        {
            get { return tempArray; }
            set { tempArray = value; }
        }

        // This is a string array, which is used to hold the strings "SpriteAliveCell"
        // and "SpriteDeadCell" in place of boolean True or False respectively
        // Boolean values are translated from the boardArray
        private Texture2D[,] displayArray;

        public Texture2D[,] DisplayArray
        {
            get { return displayArray; }
            set { displayArray = value; }
        }

        // The generation counter variable
        // This should not be modified from outside of the Life class
        private int generations;

        public int Generations
        {
            get { return generations; }
        }

        // Scaling variable for use with textures
        private float scale;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        // The ruleSetSelect field sets the ruleset the game will run
        private int ruleSetSelect;

        public int RuleSetSelect
        {
            get { return ruleSetSelect; }
            set { ruleSetSelect = value; }
        }

        // The timeInterval variable controls the speed of the game
        // Values between 0.01 and 2 seem reasonable
        private float timeInterval;

        public float TimeInterval
        {
            get { return timeInterval; }
            set { timeInterval = value; }
        }
        
        // The timeCheck field holds the value of the time elapsed
        // since the arrays and generation counter were last updated
        private float timeCheck = 0.0f;
        
        // The totalTimeElapsed field holds the total time that has
        // elapsed in the program
        // It is used to update timeCheck after an update has been 
        // completed
        private float totalTimeElapsed = 0.0f;

        // Define places to hold KeyStates
        // For checking the previous keystate and against the current keystate
        // A bit like debouncing
        KeyboardState currentKeyState, prevKeyState;


        
        // ****************
        // * Constructors *
        // ****************

        // Default Constructor
        // Defines Board Size
        public Life()
        {
            // Specify the default dimensions of the board in cells
            // This is intended to be a user-specified variable, but
            // for now, it is fixed - these are default values
            // The second constructor has the capability to take user-
            // specified boardWidth and boardHeight variables, but there
            // is currently no way for the user to specify these values.
            boardWidth = 10;
            boardHeight = 10;

            // Make a board (array) using the number of cells wide
            // by the number of cells high
            boardArray = new bool[boardWidth, boardHeight];
            tempArray = new bool[boardWidth, boardHeight];
            displayArray = new Texture2D[boardWidth, boardHeight];

            LoadContent();

            ruleSetSelect = 1;

            // Clear (and thereby initalise) the arrays
            ClearArrays();

            // Zero the generations counter
            generations = 0;

            // Scale the texures to half of their normal size
            // Currently, all textures are 100x100 pixels in size
            // This could potentially be changed on the fly, depending on
            // the boardsize or screen resolution, but for now, it is fixed
            scale = 0.1f;

            // Sets the time that must pass before the game can be updated
            // Or, in other words, timeInterval controls the speed of the
            // game
            timeInterval = 1.0f;

            // For starting / stopping the simulation
            go = false;

            // Specify whether the board should wrap around at the edges
            //bool wrapAround = false;
        }

        // Second constructor that takes values for the board width and
        // height from outside the class
        public Life(int boardWidth, int boardHeight)
        {
            // Set the specified boardWidth and boardHeight values
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;

            // Make a board (array) using the number of cells wide
            // by the number of cells high
            boardArray = new bool[boardWidth, boardHeight];
            tempArray = new bool[boardWidth, boardHeight];
            displayArray = new Texture2D[boardWidth, boardHeight];

            LoadContent();

            ruleSetSelect = 1;

            // Clear (and thereby initalise) the arrays
            ClearArrays();

            // Zero the generations counter
            generations = 0;

            // Scale the texures to half of their normal size
            // Currently, all textures are 100x100 pixels in size
            // This could potentially be changed on the fly, depending on
            // the boardsize or screen resolution, but for now, it is fixed
            scale = 0.1f;

            // Sets the time that must pass before the game can be updated
            // Or, in other words, timeInterval controls the speed of the
            // game
            timeInterval = 1.0f;

            // For starting / stopping the simulation
            go = false;

            // Specify whether the board should wrap around at the edges
            //bool wrapAround = false;
        }

        // Third constructor that takes values for the board width,
        // board height, scale, and time interval from outside the class
        public Life(int boardWidth, int boardHeight, float scale, float timeInterval, bool go)
        {
            // Set the specified boardWidth and boardHeight values
            this.boardWidth = boardWidth;
            this.boardHeight = boardHeight;

            // Make a board (array) using the number of cells wide
            // by the number of cells high
            boardArray = new bool[boardWidth, boardHeight];
            tempArray = new bool[boardWidth, boardHeight];
            displayArray = new Texture2D[boardWidth, boardHeight];

            LoadContent();

            ruleSetSelect = 1;

            // Clear (and thereby initalise) the arrays
            ClearArrays();
            
            // Zero the generations counter
            generations = 0;

            // Scale the texures to half of their normal size
            // Currently, all textures are 100x100 pixels in size
            // This could potentially be changed on the fly, depending on
            // the boardsize or screen resolution, but for now, it is fixed
            this.scale = scale;

            // Sets the time that must pass before the game can be updated
            // Or, in other words, timeInterval controls the speed of the
            // game
            this.timeInterval = timeInterval;

            // For starting / stopping the simulation
            this.go = go;

            // Specify whether the board should wrap around at the edges
            //bool wrapAround = false;
        }


        // ***********
        // * Methods *
        // ***********
        
        // Load Content
        public override void LoadContent()
        {
            Position = new Vector2(0, 0);
            SpriteDeadCell = Game1.Instance.Content.Load<Texture2D>("black_redline_face");
            SpriteAliveCell = Game1.Instance.Content.Load<Texture2D>("green_redline_face");
        }

        // Update game state
        public override void Update(GameTime gameTime)
        {
            // Starting point for enabling detction of Keyboard 
            // key presses
            prevKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            // Use the Spacebar to start and stop the simulation
            // Weirdly, this is not working very well
            // If the spacebar is pressed...
            if (currentKeyState.IsKeyUp(Keys.Space) && prevKeyState.IsKeyDown(Keys.Space))
            {
                // ... and the simulations is stopped...
                if (go == false)
                {
                    // Start the simulation
                    go = true;
                }
                // ... and the simulation is started...
                else if (go == true)
                {
                    // Stop the simulation
                    go = false;
                    
                }
            }

            // Use the plus key to speed up the simulation
            if (currentKeyState.IsKeyUp(Keys.OemPlus) && prevKeyState.IsKeyDown(Keys.OemPlus))
            {
                // Speed up the simulation
                if (timeInterval > 0.05f)
                {
                    timeInterval = timeInterval - 0.05f;
                    if (timeInterval < 0.05f)
                    {
                        timeInterval = 0.05f;
                    }
                }
            }
 
            // Use the minus key to slow down the simulation
            if (currentKeyState.IsKeyUp(Keys.OemMinus) && prevKeyState.IsKeyDown(Keys.OemMinus))
            {
                // Slow down the simulation
                if (timeInterval < 2.00f)
                {
                    timeInterval = timeInterval + 0.05f;
                    if (timeInterval > 2.00f)
                    {
                        timeInterval = 2.00f;
                    }
                }
            }

            // Use the number keys 1-4 to select the rules
            // If the current key state is the 1 key in an un-pressed state
            // and the previous key state is the 1 key in a pressed state
            // then select ruleset 1
            if (currentKeyState.IsKeyUp(Keys.D1) && prevKeyState.IsKeyDown(Keys.D1))
            {
                // Select Conway Rules
                ruleSetSelect = 1;
            }
            if (currentKeyState.IsKeyUp(Keys.D2) && prevKeyState.IsKeyDown(Keys.D2))
            {
                // Select Day And Night Rules
                ruleSetSelect = 2;
            }
            if (currentKeyState.IsKeyUp(Keys.D3) && prevKeyState.IsKeyDown(Keys.D3))
            {
                // Select Walled Cities
                ruleSetSelect = 3;
            }
            if (currentKeyState.IsKeyUp(Keys.D4) && prevKeyState.IsKeyDown(Keys.D4))
            {
                // Select Coral Growth
                ruleSetSelect = 4;
            }


            // Starting point for enabling dection of the mouse 
            // position and button presses
            MouseState mseState = Mouse.GetState();

            // Make sure the mouse cursor is within the game window
            if ((mseState.X > 0) && (mseState.X < (boardWidth * (scale * 100))) 
                && (mseState.Y > 0) && (mseState.Y < (boardHeight * (scale * 100)))) 
            {
                // Use the left mouse button make a cell alive
                if (mseState.LeftButton == ButtonState.Pressed)
                {
                    // It took me a while to figure this out, but after a lot of testing
                    // this seems to identify the relevant cell under the cursor
                    int lmbx = (int)(((float)mseState.X / (scale * 100)));
                    int lmby = (int)(((float)mseState.Y / (scale * 100)));

                    tempArray[lmbx, lmby] = true;

                }
                // Use the right mouse button to make a cell dead
                if (mseState.RightButton == ButtonState.Pressed)
                {

                    int rmbx = (int)(((float)mseState.X / (scale * 100)));
                    int rmby = (int)(((float)mseState.Y / (scale * 100)));
                    
                    tempArray[rmbx, rmby] = false;

                }
                // Update the arrays immediately after any mouse interaction
                UpdateArrays();
                
            }

            if (go == true)
            {
                totalTimeElapsed = totalTimeElapsed + (float)gameTime.ElapsedGameTime.TotalSeconds;

                if ((totalTimeElapsed - timeCheck) >= timeInterval)
                {
                    // Use Conway Rules
                    if (ruleSetSelect == 1)
                    {
                        Conway();
                    }
                    // Use Day And Night Rules
                    else if (ruleSetSelect == 2)
                    {
                        DayAndNight();
                    }
                    // Use Walled Cities Rules
                    else if (ruleSetSelect == 3)
                    {
                        WalledCities();
                    }
                    // Use Coral Growth Rules
                    else if (ruleSetSelect == 4)
                    {
                        CoralGrowth();
                    }
                    // And if all else fails, use Conway Rules
                    else
                    {
                        Conway();
                    }

                    // Update the contents of the board and display array's with the updated
                    // data from the temp array
                    UpdateArrays();

                    // Increase the generation counter
                    generations++;

                    // Update the timeCheck variable for the next update cycle
                    timeCheck = totalTimeElapsed;
                }
            }
        }

        // Draw Life class elements
        public override void Draw(GameTime gameTime)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    // Using the current x and y location,
                    // multiply the with of the reference texture by
                    // the required scaling to get the size (width or height) of
                    // the texture in pixels
                    // Then multiply the size of the texture in pixels, by the
                    // current x and y values to get its position in the game window
                    // Put these position coordinates into the Position vector
                    Position.X = x * (SpriteDeadCell.Width * scale);
                    Position.Y = y * (SpriteDeadCell.Height * scale);
                    
                    // Draw the appropriate texture at the current coordinates (in pixels)
                    Game1.Instance.spriteBatch.Draw
                        (
                        displayArray[x, y],
                        Position,
                        null,
                        Color.White,
                        0f,
                        Vector2.Zero, 
                        scale,
                        SpriteEffects.None,
                        0f
                        );
                }
            }
        }

        // Default Ruleset - Conway B3/S23
        public void Conway()
        {
            int x;
            int y;
            int neighbourCount = 0;

            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    neighbourCount = CalcNeighbourCount(x, y);
                    // Birth Conditions
                    // i.e. The conditions under which a cell will
                    // become alive if it is currently dead
                    if (boardArray[x, y] == false)
                    {

                        if (neighbourCount == 3)
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }

                    }
                    // Survival Conditions
                    // i.e. The conditions under which a cell will
                    // live on if it is already alive
                    else if (boardArray[x, y] == true)
                    {
                        if ((neighbourCount == 2) || (neighbourCount == 3))
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }
                    }
                    // If neither birth or survival conditions are met,
                    // make sure the cell is dead (reduced chance of ambiguity)
                    else
                    {
                        tempArray[x, y] = false;
                    }
                }
            }
        }

        // Day and Night B3678/S34678
        public void DayAndNight()
        {
            int x;
            int y;
            int neighbourCount = 0;

            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    neighbourCount = CalcNeighbourCount(x, y);
                    // Birth Conditions
                    // i.e. The conditions under which a cell will
                    // become alive if it is currently dead
                    if (boardArray[x, y] == false)
                    {

                        if ((neighbourCount == 3) || (neighbourCount == 6) || (neighbourCount == 7) || (neighbourCount == 8))
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }

                    }
                    // Survival Conditions
                    // i.e. The conditions under which a cell will
                    // live on if it is already alive
                    else if (boardArray[x, y] == true)
                    {
                        if ((neighbourCount == 3) || (neighbourCount == 4) || (neighbourCount == 6) || (neighbourCount == 7) || (neighbourCount == 8))
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }
                    }
                    // If neither birth or survival conditions are met,
                    // make sure the cell is dead (reduced chance of ambiguity)
                    else
                    {
                        tempArray[x, y] = false;
                    }
                }
            }
        }

        // Walled Cities B45678/S2345
        public void WalledCities()
        {
            int x;
            int y;
            int neighbourCount = 0;

            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    neighbourCount = CalcNeighbourCount(x, y);
                    // Birth Conditions
                    // i.e. The conditions under which a cell will
                    // become alive if it is currently dead
                    if (boardArray[x, y] == false)
                    {

                        if ((neighbourCount == 4) || (neighbourCount == 5) || (neighbourCount == 6) || (neighbourCount == 7) || (neighbourCount == 8))
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }

                    }
                    // Survival Conditions
                    // i.e. The conditions under which a cell will
                    // live on if it is already alive
                    else if (boardArray[x, y] == true)
                    {
                        if ((neighbourCount == 2) || (neighbourCount == 3) || (neighbourCount == 4) || (neighbourCount == 5))
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }
                    }
                    // If neither birth or survival conditions are met,
                    // make sure the cell is dead (reduced chance of ambiguity)
                    else
                    {
                        tempArray[x, y] = false;
                    }
                }
            }
        }

        // Coral Growth B3/S45678
        public void CoralGrowth()
        {
            int x;
            int y;
            int neighbourCount = 0;

            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    neighbourCount = CalcNeighbourCount(x, y);
                    // Birth Conditions
                    // i.e. The conditions under which a cell will
                    // become alive if it is currently dead
                    if (boardArray[x, y] == false)
                    {

                        if (neighbourCount == 3)
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }

                    }
                    // Survival Conditions
                    // i.e. The conditions under which a cell will
                    // live on if it is already alive
                    else if (boardArray[x, y] == true)
                    {
                        if ((neighbourCount == 4) || (neighbourCount == 5) || (neighbourCount == 6) || (neighbourCount == 7) || (neighbourCount == 8))
                        {
                            tempArray[x, y] = true;
                        }
                        else
                        {
                            tempArray[x, y] = false;
                        }
                    }
                    // If neither birth or survival conditions are met,
                    // make sure the cell is dead (reduced chance of ambiguity)
                    else
                    {
                        tempArray[x, y] = false;
                    }
                }
            }
        }

        // A method to calculate the number of neighbours the current cell has
        public int CalcNeighbourCount(int x, int y)
        {
            int numOfAliveNeighbours = 0;


            // Messy, mind-melting business this... very easy to go wrong
            // Visualise the game board and count carefully - and take your
            // time!
            // I'm sure there are better ways to do this, but I don't have
            // time to work them out now - and as convoluted as it is,
            // I'm familiar with this way of working out border cases from
            // the last version of the Game of Life that I made (it was
            // made in Visual Basic 6, and I don't have easy access to the
            // source code)

            // Top left-hand corner cell
            if (((x - 1) < 0) && ((y - 1) < 0))
            {
                // Check if East Cell is alive
                if (boardArray[(x + 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-East Cell is alive
                if (boardArray[(x + 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South Cell is alive
                if (boardArray[x, (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
            }

            // Top right-hand corner cell
            else if (((x + 1) > (boardWidth - 1)) && ((y - 1) < 0))
            {
                // Check if South Cell is alive
                if (boardArray[x, (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-West Cell is alive
                if (boardArray[(x - 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if West Cell is alive
                if (boardArray[(x - 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
            }

            // Bottom left-hand corner cell
            else if (((x - 1) < 0) && ((y + 1) > (boardHeight - 1)))
            {
                // Check if North Cell is alive
                if (boardArray[x, (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-East Cell is alive
                if (boardArray[(x + 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if East Cell is alive
                if (boardArray[(x + 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }

            }

            // Bottom right-hand corner cell
            else if (((x + 1) > (boardWidth - 1)) && ((y + 1) > (boardHeight - 1)))
            {
                // Check if West Cell is alive
                if (boardArray[(x - 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-West Cell is alive
                if (boardArray[(x - 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North Cell is alive
                if (boardArray[x, (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
            }
            // Top-side cells
            else if ((x >= 0) && (x <= (boardWidth - 1)) && ((y - 1) < 0))
            {
                // Check if East Cell is alive
                if (boardArray[(x + 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-East Cell is alive
                if (boardArray[(x + 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South Cell is alive
                if (boardArray[x, (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-West Cell is alive
                if (boardArray[(x - 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if West Cell is alive
                if (boardArray[(x - 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
            }
            // Left-side cells
            else if (((x - 1) < 0) && (y <= (boardHeight - 1)) && (y >= 0))
            {
                // Check if North Cell is alive
                if (boardArray[x, (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-East Cell is alive
                if (boardArray[(x + 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if East Cell is alive
                if (boardArray[(x + 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-East Cell is alive
                if (boardArray[(x + 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South Cell is alive
                if (boardArray[x, (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
            }
            // Right-side cells
            else if (((x + 1) > (boardWidth - 1)) && (y <= (boardHeight - 1)) && (y >= 0))
            {
                // Check if South Cell is alive
                if (boardArray[x, (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-West Cell is alive
                if (boardArray[(x - 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if West Cell is alive
                if (boardArray[(x - 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-West Cell is alive
                if (boardArray[(x - 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North Cell is alive
                if (boardArray[x, (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
            }
            // Bottom-side Cells
            else if ((x >= 0) && (x <= (boardWidth - 1)) && ((y + 1) > (boardHeight - 1)))
            {
                // Check if West Cell is alive
                if (boardArray[(x - 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-West Cell is alive
                if (boardArray[(x - 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North Cell is alive
                if (boardArray[x, (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-East Cell is alive
                if (boardArray[(x + 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if East Cell is alive
                if (boardArray[(x + 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
            }
            // For all other cells (i.e. the ones that have 8 neighbours)
            else
            {
                // Check if North Cell is alive
                if (boardArray[x, (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-East Cell is alive
                if (boardArray[(x + 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if East Cell is alive
                if (boardArray[(x + 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-East Cell is alive
                if (boardArray[(x + 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South Cell is alive
                if (boardArray[x, (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if South-West Cell is alive
                if (boardArray[(x - 1), (y + 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if West Cell is alive
                if (boardArray[(x - 1), y] == true)
                {
                    numOfAliveNeighbours++;
                }
                // Check if North-West Cell is alive
                if (boardArray[(x - 1), (y - 1)] == true)
                {
                    numOfAliveNeighbours++;
                }
            }

            return numOfAliveNeighbours;
        }

        // A method to clear ALL array to false or SpriteDeadCell states.
        public void ClearArrays()
        {
            int x;
            int y;
            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    tempArray[x, y] = false;
                    boardArray[x, y] = false;
                    displayArray[x, y] = SpriteDeadCell;
                }
            }
        }

        // A method to update the board and display arrays with latest generation
        // of cell values
        public void UpdateArrays()
        {
            int x;
            int y;

            for (y = 0; y < boardHeight; y++)
            {
                for (x = 0; x < boardWidth; x++)
                {
                    if (tempArray[x, y] == true)
                    {
                        boardArray[x, y] = true;
                        displayArray[x, y] = SpriteAliveCell;
                    }
                    else if (tempArray[x, y] == false)
                    {
                        boardArray[x, y] = false;
                        displayArray[x, y] = SpriteDeadCell;
                    }
                }
            }
        }

    }
}
