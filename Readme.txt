A bit about the Program

Controls: Cell Activation
Left-Mouse Button: Activate a Cell (bring the cell to life)
Right-Mouse Button: De-activate a Cell (kill the cell)

Controls: Starting and Stopping the simulation
SpaceBar: Start/Stop the simulation

Controls: Simulation Speed
=/+ key: Speed up the simulation (in increments of -0.05 seconds)
-/_ key: Slow down the simulation (in increments of +0.05 seconds)

Controls: Select Life Rules
1 key: Conway (B3/S23)
2 key: Day and Night (B3678/S34678)
3 key: Walled Cities (B45678/S2345)
4 key: Coral Growth (B3/S45678)

Board size and Screen Resolutions
The board size can be changed by tweaking the bWidth and bHeight fields in the Game1 class. Be sure to scale down the texture sizes to match, by using the texScale field, also found in the Game1 class. The 2 main textures used are 100x100 pixels in size. I made them that size because I knew I could scale them down if necessary, and also because it, when using scaling, it’s trivial to calculate the scaled-down size of the textures in pixels.

Be aware that I developed the game using a monitor which has a resolution of 1280x1024. If you run my version of the game of life on a screen with a higher resolution, my values for the width, height and texture scaling may seem overly conservative.

The current values for the Board Width (bWidth), Board Height (bHeight), Texture Scaling (texScale) and Time Interval (tInterval) are:
bWidth = 15;
bHeight = 10;
texScale = 0.5f;
tInterval = 0.1f;

I used these values because they should fit comfortably (including the window border) within the boundaries of an 800x600 screen resolution. However, there is no reason not to use higher values if you have a higher screen resolution (and processing power) to handle the extra cells.
Having said that, the Game of Life is heavily CPU bound, and eventually, if you make a big enough board of cells, the simulation will start to slow down. As well as that, this particular version of the Game of Life is most definitely not optimised for CPU usage, so it is very likely that you will find the CPU at the heart of any performance bottleneck, particularly when larger boards are used, although it will depend on your hardware.

Some suggestions for a 1920x1080 (1080p) or 1920x1200 resolution screen:
bWidth = 18
bHeight = 10
texScale = 1.0f 

bWidth = 38
bHeight = 21
texScale = 0.5f

bWidth = 180
bHeight = 100
texScale = 0.1f



References

Multi-Dimensional Arrays
http://msdn.microsoft.com/en-us/library/2yd9wwz4%28v=vs.100%29.aspx

2D Grid System using XNA
http://www.ehow.com/how_12074109_make-2d-grid-system-xna.html

Scaling a Sprite
http://msdn.microsoft.com/en-us/library/bb194913.aspx

Static Classes and Static Members
http://msdn.microsoft.com/en-us/library/79b3xss3.aspx

Fields
http://msdn.microsoft.com/en-us/library/ms173118.aspx

GraphicsDeviceManager Class (setting the window size)
http://stackoverflow.com/questions/720429/how-do-i-set-the-window-screen-size-in-xna

GraphicsDeviceManager Class
http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.graphicsdevicemanager.aspx

Life Patterns
http://www.bitstorm.org/gameoflife/lexicon/

Life Rules
http://en.wikipedia.org/wiki/Life-like_cellular_automata
http://www.conwaylife.com/wiki/Cellular_automaton

Random
http://msdn.microsoft.com/en-us/library/system.random.aspx

MouseState
http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.mousestate_members.aspx
http://stackoverflow.com/questions/5208499/xna-cannot-see-mouse-in-game

GameStates and Pausing
http://gamedev.stackexchange.com/questions/26294/how-to-pause-and-resume-a-game-in-xna-using-the-same-key

Key Enumeration
http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.input.keys.aspx



- Peter Murtagh, February 2015