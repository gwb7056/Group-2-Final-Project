using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject {
    enum GameState {
        MainMenu,
        Game,
        Pause,
        Credits,
        GameOver
    }
    public class Game1 : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Fonts:
        SpriteFont font;
        

        //Tile Textures:
        Texture2D pathTexture;
        Texture2D closedSpaceTexture;
        
        //Tower Textures:

        //Enemy Texture:

        //Player Textures:
        Texture2D playerTexture;
        Player player;
        //Game Objects and Fields:
        Board gameBoard;
        Rectangle playerPosition;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            //Creating gameBoard
            gameBoard = new Board(1);
            
            base.Initialize();

             //Change the window size
            _graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
        }

        protected override void LoadContent() {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Fonts:
            font = Content.Load<SpriteFont>("Arial12");

            //Tile Textures:
            pathTexture = Content.Load<Texture2D>("pathTexture");
            closedSpaceTexture = Content.Load<Texture2D>("closedSpaceTexture");

            //Tower Textures:

            //Enemy Textures:

            //Player Textures:
            playerTexture = Content.Load<Texture2D>("among us");
            //creating player
            player = new Player(playerTexture, 50, 50, 100, 100);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.ForestGreen);
            _spriteBatch.Begin();
            //drawing player
            player.Draw(_spriteBatch, Color.White);
            //Draw the board
            gameBoard.Draw(_spriteBatch, pathTexture, closedSpaceTexture);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
