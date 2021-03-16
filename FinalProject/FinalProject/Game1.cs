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
        Board boardTest;
        Texture2D playerTexture;
        Rectangle playerPosition;

        //Tile Textures:
        Texture2D pathTexture;
        Texture2D closedSpaceTexture;
        
        //Tower Textures:

        //Enemy Texture:

        //Game Objects:
        Board gameBoard;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() {
            //zCreating gameBoard
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

            // TODO: use this.Content to load your game content here
            playerTexture = Content.Load<Texture2D>("among us");
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.ForestGreen);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            //Draw the board
            gameBoard.Draw(_spriteBatch, pathTexture, closedSpaceTexture);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
