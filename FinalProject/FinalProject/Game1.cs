using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject 
{
    enum GameState 
    {
        MainMenu,
        Game,
        Pause,
        Credits,
        GameOver
    }

    public class Game1 : Game 
    {
        private GameState activeState = GameState.MainMenu;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Fonts:
        SpriteFont font;

        //Tile Textures:
        Texture2D pathTexture;
        Texture2D closedSpaceTexture;

        //deck stuff


        //Tower Textures:
        List<Texture2D> towerTextures;
        Texture2D towerTexture;
        Texture2D towerTexture1;
        List<Rectangle> towerPositions;
        MouseState previousMouseState;
        MouseState mouseState;

        //Enemy Texture:
        List<Texture2D> enemyTextures;
        Texture2D enemyTestTexture;

        //Player Textures:
        Texture2D playerTexture;
        Player player;
        Texture2D playerHealthTexture;

        //Card Stuff:
        Texture2D cardTexture;
        Card card;
        Card card1;
        Card card2;
        
        int number;
        Random rng = new Random();

        //Game Objects and Fields:
        Board gameBoard;
        Rectangle playerPosition;
        int frameCounter = 0;
        int frameCounter1 = 0;
        int towerCount = 0;
        int startingLevelNum = 0;

        public Game1() 
        {

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

        }

        protected override void Initialize() 
        {
            towerPositions = new List<Rectangle>();
            base.Initialize();
          
            //Change the window size
            _graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

        }

        protected override void LoadContent() 
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //card stuff

            //Fonts:
            font = Content.Load<SpriteFont>("Arial12");

            //Tile Textures:
            pathTexture = Content.Load<Texture2D>("pathTexture");
            closedSpaceTexture = Content.Load<Texture2D>("closedSpaceTexture");

            //Tower Textures:
            towerTextures = new List<Texture2D>();
            towerTexture = Content.Load<Texture2D>("tower");
            towerTexture1 = Content.Load<Texture2D>("cannontower");

            //Enemy Textures:
            enemyTextures = new List<Texture2D>();
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("fallguy"));

            //Player Textures:
            playerTexture = Content.Load<Texture2D>("among us");
            playerHealthTexture = Content.Load<Texture2D>("health");

            //Creating gameBoard
            gameBoard = new Board(startingLevelNum, towerTextures, enemyTextures);

            //Creating player
            player = new Player(playerTexture, playerHealthTexture, gameBoard.PathEndCords[0] * gameBoard.TileSize,
            gameBoard.PathEndCords[1] * gameBoard.TileSize, 40, 40);
        }

        protected override void Update(GameTime gameTime) 
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();

            //switch to determine what gamestate we're in
            //  Temporarily making the switches determined by key press rather than button press
            switch (activeState)
            {
                //In the main menu
                case GameState.MainMenu:
                    //
                    if (state.IsKeyDown(Keys.Space))
                    {
                        activeState = GameState.Game;
                        gameBoard.GetLevelFromFile(startingLevelNum);
                    }
                    //
                    if (state.IsKeyDown(Keys.C))
                    {
                        activeState = GameState.Credits;
                    }
                    //No options menu for now
                    break;

                case GameState.Game:
                    //mana system by lance
                    //reminders: make the towers inherit from card class
                    if (card == null)
                    {
                        number = rng.Next(0, 2);
                        if (number == 0)
                        {
                            card = new Cannon_Tower(0, 0, towerTexture1);
                            card.PosX = 0;
                            card.PosY = 0;
                            card.ManaCost = 1;
                           
                        }
                        else if (number == 1)
                        {
                            card = new Mortar_Tower(0, 0, towerTexture);
                            card.PosX = 0;
                            card.PosY = 0;
                            card.ManaCost = 1;
                            
                        }
                    }
                    if (card1 == null)
                    {
                        number = rng.Next(0, 2);
                        if (number == 0)
                        {
                            card1 = new Cannon_Tower(0, 0, towerTexture1);
                            card1.PosX = 60;
                            card1.PosY = 0;
                            card1.ManaCost = 1;
                           
                        }
                        else if (number == 1)
                        {
                            card1 = new Mortar_Tower(0, 0, towerTexture);
                            card1.PosX = 60;
                            card1.PosY = 0;
                            card1.ManaCost = 1;
                           
                        }
                    }
                    if (card2 == null)
                    {
                        number = rng.Next(0, 2);
                        if (number == 0)
                        {
                            card2 = new Cannon_Tower(0, 0, towerTexture1);
                            card2.PosX = 120;
                            card2.PosY = 0;
                            card2.ManaCost = 1;
                            
                   
                        }
                        else if (number == 1)
                        {
                            card2 = new Mortar_Tower(0, 0, towerTexture);
                            card2.PosX = 120;
                            card2.PosY = 0;
                            card2.ManaCost = 1;
                           
                        }
                    }
                    mouseState = Mouse.GetState();
                    if (card.CardPosition.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= card.ManaCost)
                    {
                        card.PosX = mouseState.Position.X - 20;
                        card.PosY = mouseState.Position.Y - 20;
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            towerCount++;
                            for (int width = 0; width < 15; width++)
                            {
                                for (int height = 0; height < 15; height++)
                                {
                                    if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                    { 
                                        /*gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                        player.Mana--;
                                        card = null;*/
                                        if (card is Cannon_Tower)
                                        {
                                            gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture1));
                                            player.Mana--;
                                            card = null;
                                        }
                                        if (card is Mortar_Tower)
                                        {
                                            gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                            player.Mana--;
                                            card = null;
                                        }
                                    }
                                }
                            }
                        }
                      
                    }
                    else if (card.CardPosition.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= card.ManaCost)
                    {
                        card.PosX = 0;
                        card.PosY = 0;
                    }
                    if (card1.CardPosition.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= card1.ManaCost)
                    {
                        
                        card1.PosX = mouseState.Position.X - 20;
                        card1.PosY = mouseState.Position.Y - 20;
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            towerCount++;
                            for (int width = 0; width < 15; width++)
                            {
                                for (int height = 0; height < 15; height++)
                                {
                                    if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                    {
                                        /*gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                        player.Mana--;
                                        card = null;*/
                                        if (card1 is Cannon_Tower)
                                        {
                                            gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture1));
                                            player.Mana--;
                                            card1 = null;
                                        }
                                        if (card1 is Mortar_Tower)
                                        {
                                            gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                            player.Mana--;
                                            card1 = null;
                                        }
                                    }
                                }
                            }
                        }
                      
                    }
                    else if (card1.CardPosition.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= card1.ManaCost)
                    {
                        card1.PosX = 60;
                        card1.PosY = 0;
                    }
                    if (card2.CardPosition.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= card2.ManaCost)
                    {
                        card2.PosX = mouseState.Position.X - 20;
                        card2.PosY = mouseState.Position.Y - 20;
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            towerCount++;
                            for (int width = 0; width < 15; width++)
                            {
                                for (int height = 0; height < 15; height++)
                                {
                                    if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                    {
                                        /*gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                         player.Mana--;
                                         card = null;*/
                                        if (card2 is Cannon_Tower)
                                        {
                                            gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture1));
                                            player.Mana--;
                                            card2 = null;
                                        }
                                        if (card2 is Mortar_Tower)
                                        {
                                            gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                            player.Mana--;
                                            card2 = null;
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                    else if (card2.CardPosition.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= card2.ManaCost)
                    {
                        card2.PosX = 120;
                        card2.PosY = 0;
                    }
                    frameCounter1 += 1;
                    if (frameCounter1 % 120 == 0)
                    {
                        if (player.Mana < 10)
                        {
                            player.Mana++;
                        }
                    }

                    //changing enum states
                    if (state.IsKeyDown(Keys.P))
                    {
                        activeState = GameState.Pause;
                    }
                    //The rest of the code for game
                    frameCounter += 1;
                    gameBoard.TowersDamageEnemies(frameCounter);
                    player.TakeDamage(gameBoard.MoveEnemies());

                    if(frameCounter % 60 == 0) {
                        gameBoard.ReduceTowerTimers();
                    }

                    //Spawning towers

                    /*if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                    {

                        towerCount++;
                        for (int width = 0; width < 15; width++)
                        {

                            for (int height = 0; height < 15; height++)
                            {
                                if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                {
                                    gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, towerTexture));
                                }
                            }
                        }
                    }*/

                    if (player.Health <= 0)
                    {
                        activeState = GameState.GameOver;
                    }

                    previousMouseState = mouseState;
                    base.Update(gameTime);
                    break;

                case GameState.Pause:
                    if (state.IsKeyDown(Keys.M))
                    {
                        activeState = GameState.MainMenu;
                    }
                    if (state.IsKeyDown(Keys.Space))
                    {
                        activeState = GameState.Game;
                    }
                    break;

                case GameState.Credits:
                    if (state.IsKeyDown(Keys.M))
                    {
                        activeState = GameState.MainMenu;
                    }
                    break;

                case GameState.GameOver:
                    if (state.IsKeyDown(Keys.M))
                    {
                        activeState = GameState.MainMenu;
                    }
                    break;
            }
            
        }

        protected override void Draw(GameTime gameTime) 
        {

            GraphicsDevice.Clear(Color.ForestGreen);
            _spriteBatch.Begin();
            switch (activeState)
            {
                case GameState.MainMenu:
                    _spriteBatch.DrawString(font, "MAIN MENU", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"Spacebar\" to play game.", new Vector2(150, 250), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"C\" for credits.", new Vector2(150, 300), Color.Black);
                    break;

                case GameState.Game:
                    //Draw the board and entities on the board
                    gameBoard.Draw(_spriteBatch, pathTexture, closedSpaceTexture);

                    //Drawing player
                    player.Draw(_spriteBatch);

                    _spriteBatch.DrawString(font, "Press \"P\" to pause.", new Vector2(50, 50), Color.White);
                    _spriteBatch.DrawString(font, "Health: " + player.Health, new Vector2(50, 100), Color.White);
                    _spriteBatch.DrawString(font, "Mana: " + player.Mana, new Vector2(50, 150), Color.White);
                    if (card != null)
                    {
                        if (card is Mortar_Tower)
                        {
                            _spriteBatch.Draw(towerTexture, card.CardPosition, Color.White);
                        }
                        else if (card is Cannon_Tower)
                        {
                            _spriteBatch.Draw(towerTexture1, card.CardPosition, Color.White);
                        }
                    }
                    if (card1 != null)
                    {
                        if (card1 is Mortar_Tower)
                        {
                            _spriteBatch.Draw(towerTexture, card1.CardPosition, Color.White);
                        }
                        else if (card1 is Cannon_Tower)
                        {
                            _spriteBatch.Draw(towerTexture1, card1.CardPosition, Color.White);
                        }
                    }
                    if (card2 != null)
                    {
                        if (card2 is Mortar_Tower)
                        {
                            _spriteBatch.Draw(towerTexture, card2.CardPosition, Color.White);
                        }
                        else if (card2 is Cannon_Tower)
                        {
                            _spriteBatch.Draw(towerTexture1, card2.CardPosition, Color.White);
                        }
                    }
                    break;

                case GameState.Pause:
                    _spriteBatch.DrawString(font, "PAUSE MENU", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"Spacebar\" to play game.", new Vector2(150, 250), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to go back to the Menu.", new Vector2(150, 300), Color.Black);
                    break;

                case GameState.Credits:
                    _spriteBatch.DrawString(font, "CREDITS MENU", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(font, "Griffin Brown, Kylian Hervet, Liam Alexiou, Lance Noble", new Vector2(100, 250), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to go back to the Menu.", new Vector2(150, 300), Color.Black);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(font, "GAME OVER", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to go back to the Menu.", new Vector2(150, 250), Color.Black);
                    break;
            }


            _spriteBatch.End();
            base.Draw(gameTime);

        }

    }

}
