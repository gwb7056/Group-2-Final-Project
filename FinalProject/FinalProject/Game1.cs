using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
        List<Texture2D> towerTextures;

        //Enemy Texture:
        List<Texture2D> enemyTextures;
        Texture2D enemyTestTexture;

        //Player Textures:
        Texture2D playerTexture;
        Player player;
        Texture2D playerHealthTexture;

        //Game Objects and Fields:
        Board gameBoard;
        Rectangle playerPosition;

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //Just for Testing
        //~~~~~~~~~~~~~~~~~~~~~~~~~~~
        int counter = 0;

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;          
        }

        protected override void Initialize() {
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
            towerTextures = new List<Texture2D>();

            //Enemy Textures:
            enemyTextures = new List<Texture2D>();
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("fallguy"));

            //Player Textures:
            playerTexture = Content.Load<Texture2D>("among us");
            playerHealthTexture = Content.Load<Texture2D>("health");

            //Creating gameBoard
            gameBoard = new Board(1, towerTextures, enemyTextures);

            //Creating player
            player = new Player(playerTexture, playerHealthTexture, gameBoard.PathEndCords[0] * gameBoard.TileSize, gameBoard.PathEndCords[1] * gameBoard.TileSize, 40, 40);
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //Just for testing
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            if(counter == 0) {
            gameBoard.MoveEnemies();
                counter = 15;
            }
            else {
                counter -= 1;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.ForestGreen);
            _spriteBatch.Begin();

            //Draw the board
            gameBoard.Draw(_spriteBatch, pathTexture, closedSpaceTexture);

            //Drawing player
            player.Draw(_spriteBatch, Color.White);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
