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
    /// <summary>
    /// This enum allows players to win, lose, pause, etc...
    /// </summary>
    enum GameState 
    {
        MainMenu,
        Game,
        Pause,
        Credits,
        GameOver,
        LevelFinished
    }
    /// <summary>
    /// Handles all game functionality
    /// </summary>
    public class Game1 : Game 
    {
        //General Game Properties:
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState activeState;
        private KeyboardState keyBoardState;
        private MouseState mouseState;
        

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
        int totalNumLevels = 2;

        //deck n card stuff
        Deck deck1;
        Stack<Card> deck;
        Stack<Card> discard;
        Card[] hand;
        Card hand0;
        Card hand1;
        Card hand2;
        List<Card> drawableDeck;
        Rectangle card0Position;
        Rectangle card1Position;
        Rectangle card2Position;
        Rectangle deckPosition;
        Rectangle discardPosition;
        Texture2D archerCard;
        Texture2D cannonCard;
        Texture2D mortarCard;
        Texture2D sniperCard;
        Texture2D wizardCard;

        public Game1() 
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize() 
        {
           
            base.Initialize();
          
            //Change the window size
            _graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            _graphics.ApplyChanges();

        }

        /*public void CheckSummons(int )
        {

        }*/

        public void ShuffleDeck()
        {
            Random rng = new Random();
            List<Card> tempDeck = new List<Card>();
            int index2;
            for (int index = 0; index < 15; index++)
            {
                tempDeck.Add(deck.Pop());
            }
            for (int index1 = 0; index1 < 15; index1++)
            {
                index2 = rng.Next(0, tempDeck.Count);
                deck.Push(tempDeck[index2]);
                tempDeck.Remove(tempDeck[index2]);
            }
        }

        protected override void LoadContent() 
        {

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //deck n card stuff
            deck = new Stack<Card>();
            discard = new Stack<Card>();
            hand = new Card[3];
            
            drawableDeck = new List<Card>();
            card0Position = new Rectangle(0, 0, 50, 50);
            card1Position = new Rectangle(60, 0, 50, 50);
            card2Position = new Rectangle(120, 0, 50, 50);
            deckPosition = new Rectangle(200, 0, 50, 50);
            discardPosition = new Rectangle(300, 0, 50, 50);
            archerCard = Content.Load<Texture2D>("archercard");
            cannonCard = Content.Load<Texture2D>("cannoncard");
            mortarCard = Content.Load<Texture2D>("mortarcard");
            sniperCard = Content.Load<Texture2D>("snipercard");
            wizardCard = Content.Load<Texture2D>("wizardcard");
            for (int count = 1; count <= 3; count++)
            {
                deck.Push(new Basic_Archer_Tower());
            }
            for (int count = 1; count <= 3; count++)
            {
                deck.Push(new Cannon_Tower());
            }
            for (int count = 1; count <= 3; count++)
            {
                deck.Push(new Mortar_Tower());
            }
            for (int count = 1; count <= 3; count++)
            {
                deck.Push(new Sniper_Tower());
            }
            for (int count = 1; count <= 3; count++)
            {
                deck.Push(new Wizard_Tower());
            }
            ShuffleDeck();
            hand0 = deck.Pop();
            hand1 = deck.Pop();
            hand2 = deck.Pop();

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
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("ninjaTexture"));
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("shieldmaidenTexture"));
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("swordmanTexture"));

            //Player Textures:
            playerTexture = Content.Load<Texture2D>("among us");
           

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

            keyBoardState = Keyboard.GetState();

            //switch to determine what gamestate we're in
            //  Temporarily making the switches determined by key press rather than button press
            switch (activeState)
            {
                //In the main menu
                case GameState.MainMenu:
                    //
                    if (keyBoardState.IsKeyDown(Keys.Space))
                    {
                        activeState = GameState.Game;
                        gameBoard.GetLevelFromFile(startingLevelNum);
                        player.Health = 100;
                    }
                    //
                    if (keyBoardState.IsKeyDown(Keys.C))
                    {
                        activeState = GameState.Credits;
                    }
                    //No options menu for now
                    break;

                case GameState.Game:
                    //mana system by lance
                    //reminders: make the towers inherit from card class
                    if (deck.Count == 0)
                    {
                        for(int index = 0; index < 12; index++)
                        {
                            deck.Push(discard.Pop());
                        }
                    }
                    if (hand0 == null)
                    {
                        hand0 = deck.Pop();
                        card0Position.X = 0;
                        card0Position.Y = 0;
                    }
                    if (hand1 == null)
                    {
                        hand1 = deck.Pop();
                        card1Position.X = 60;
                        card1Position.Y = 0;
                    }
                    if (hand2 == null)
                    {
                        
                        hand2 = deck.Pop();
                        card2Position.X = 120;
                        card2Position.Y = 0;
                    }

                    mouseState = Mouse.GetState();
                    
                    if (card0Position.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= 1)
                    {
                        card0Position.X = mouseState.Position.X - 20;
                        card0Position.Y = mouseState.Position.Y - 20;
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            towerCount++;
                            for (int width = 0; width < 15; width++)
                            {
                                for (int height = 0; height < 15; height++)
                                {
                                    if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                    { 
                                        if (hand0 is Basic_Archer_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, archerCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand0);
                                                hand0 = null;
                                                
                                            }
                                        }
                                        if (hand0 is Cannon_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, cannonCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand0);
                                                
                                                hand0 = null;
                                                
                                            }
                                        }
                                        if (hand0 is Mortar_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, mortarCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand0);

                                                hand0 = null;
                                               
                                            }

                                        }
                                        if (hand0 is Sniper_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Sniper_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, sniperCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand0);
                                                hand0 = null;
                                                
                                            }
                                        }
                                        if (hand0 is Wizard_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Wizard_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, wizardCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand0);
                                                hand0 = null;
                                               
                                            }
                                        }
                                    }
                                }
                            }
                        }
                      
                    }
                    else if (card0Position.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                    {
                        card0Position.X = 0;
                        card0Position.Y = 0;
                    }
                    if (card1Position.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= 1)
                    {
                        card1Position.X = mouseState.Position.X - 20;
                        card1Position.Y = mouseState.Position.Y - 20;
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            towerCount++;
                            for (int width = 0; width < 15; width++)
                            {
                                for (int height = 0; height < 15; height++)
                                {
                                    if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                    {
                                        if (hand1 is Basic_Archer_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, archerCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand1);
                                                hand1 = null;
                                                
                                            }
                                        }
                                        if (hand1 is Cannon_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, cannonCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand1);
                                                
                                                hand1 = null;
                                               
                                            }
                                        }
                                        if (hand1 is Mortar_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, mortarCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand1);
                                                
                                                hand1 = null;
                                                
                                            }

                                        }
                                        if (hand1 is Sniper_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Sniper_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, sniperCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand1);
                                                hand1 = null;
                                                
                                            }
                                        }
                                        if (hand1 is Wizard_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Wizard_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, wizardCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand1);
                                                hand1 = null;
                                                
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else if (card1Position.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                    {
                        card1Position.X = 60;
                        card1Position.Y = 0;
                    }
                    if (card2Position.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= 1)
                    {
                        card2Position.X = mouseState.Position.X - 20;
                        card2Position.Y = mouseState.Position.Y - 20;
                        if (mouseState.RightButton == ButtonState.Pressed)
                        {
                            towerCount++;
                            for (int width = 0; width < 15; width++)
                            {
                                for (int height = 0; height < 15; height++)
                                {
                                    if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                                    {
                                        if (hand2 is Basic_Archer_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, archerCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand2);
                                                hand2 = null;

                                            }
                                        }
                                        if (hand2 is Cannon_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, cannonCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand2);
                                              
                                                hand2 = null;
                                            }
                                        }
                                        if (hand2 is Mortar_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, mortarCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand2);
                                               
                                                hand2 = null;
                                            }

                                        }
                                        if (hand2 is Sniper_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Sniper_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, sniperCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand2);
                                                hand2 = null;
                                            }
                                        }
                                        if (hand2 is Wizard_Tower)
                                        {
                                            if (gameBoard.AddTowerToBoard(new Wizard_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, wizardCard)))
                                            {
                                                player.Mana--;
                                                discard.Push(hand2);
                                                hand2 = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else if (card2Position.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                    {
                        card2Position.X = 120;
                        card2Position.Y = 0;
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
                    if (keyBoardState.IsKeyDown(Keys.P))
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

                    if (player.Health <= 0)
                    {
                        activeState = GameState.GameOver;
                    }

                    if (gameBoard.LevelFinished) {
                        activeState = GameState.LevelFinished;
                    }

                   

                    base.Update(gameTime);
                    break;

                case GameState.Pause:
                    if (keyBoardState.IsKeyDown(Keys.M))
                    {
                        activeState = GameState.MainMenu;
                    }
                    if (keyBoardState.IsKeyDown(Keys.Space))
                    {
                        activeState = GameState.Game;
                    }
                    break;

                case GameState.Credits:
                    if (keyBoardState.IsKeyDown(Keys.M))
                    {
                        activeState = GameState.MainMenu;
                    }
                    break;

                case GameState.GameOver:
                    if (keyBoardState.IsKeyDown(Keys.M))
                    {
                        activeState = GameState.MainMenu;
                    }
                    break;

                case GameState.LevelFinished:
                    if (keyBoardState.IsKeyDown(Keys.M)) {
                        activeState = GameState.MainMenu;
                        startingLevelNum = 0;
                    }
                    else if (keyBoardState.IsKeyDown(Keys.N) && startingLevelNum + 1 < totalNumLevels) {
                        activeState = GameState.Game;
                        gameBoard.GetLevelFromFile(startingLevelNum += 1);
                        player.Health = 100;
                        player.PlayerPosition = new Rectangle(gameBoard.PathEndCords[0] * gameBoard.TileSize,
                            gameBoard.PathEndCords[1] * gameBoard.TileSize, 40, 40);
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

                case GameState.LevelFinished:
                    _spriteBatch.DrawString(font, "YOU COMPLETED THE LEVEL!", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to return the the main menu", new Vector2(150, 250), Color.Black);
                    if(startingLevelNum + 1 < totalNumLevels) {
                        _spriteBatch.DrawString(font, "Press \"N\" to go to the next level", new Vector2(150, 300), Color.Black);
                    }
                    break;


                case GameState.Game:
                    //Draw the board and entities on the board
                    gameBoard.Draw(_spriteBatch, pathTexture, closedSpaceTexture);

                    //Drawing player
                    player.Draw(_spriteBatch);

                    _spriteBatch.DrawString(font, "Press \"P\" to pause.", new Vector2(50, 50), Color.White);
                    _spriteBatch.DrawString(font, "Health: " + player.Health, new Vector2(50, 100), Color.White);
                    _spriteBatch.DrawString(font, "Mana: " + player.Mana, new Vector2(50, 150), Color.White);

                    if (hand0 != null)
                    {
                        if (hand0 is Basic_Archer_Tower)
                        {
                            _spriteBatch.Draw(archerCard, card0Position, Color.White);
                        }
                        if (hand0 is Cannon_Tower)
                        {
                            _spriteBatch.Draw(cannonCard, card0Position, Color.White);
                        }
                        if (hand0 is Mortar_Tower)
                        {
                            _spriteBatch.Draw(mortarCard, card0Position, Color.White);
                        }
                        if (hand0 is Sniper_Tower)
                        {
                            _spriteBatch.Draw(sniperCard, card0Position, Color.White);
                        }
                        if (hand0 is Wizard_Tower)
                        {
                            _spriteBatch.Draw(wizardCard, card0Position, Color.White);
                        }
                    }
                    if (hand1 != null)
                    {
                        if (hand1 is Basic_Archer_Tower)
                        {
                            _spriteBatch.Draw(archerCard, card1Position, Color.White);
                        }
                        if (hand1 is Cannon_Tower)
                        {
                            _spriteBatch.Draw(cannonCard, card1Position, Color.White);
                        }
                        if (hand1 is Mortar_Tower)
                        {
                            _spriteBatch.Draw(mortarCard, card1Position, Color.White);
                        }
                        if (hand1 is Sniper_Tower)
                        {
                            _spriteBatch.Draw(sniperCard, card1Position, Color.White);
                        }
                        if (hand1 is Wizard_Tower)
                        {
                            _spriteBatch.Draw(wizardCard, card1Position, Color.White);
                        }
                    }
                    if (hand2 != null)
                    {
                        if (hand2 is Basic_Archer_Tower)
                        {
                            _spriteBatch.Draw(archerCard, card2Position, Color.White);
                        }
                        if (hand2 is Cannon_Tower)
                        {
                            _spriteBatch.Draw(cannonCard, card2Position, Color.White);
                        }
                        if (hand2 is Mortar_Tower)
                        {
                            _spriteBatch.Draw(mortarCard, card2Position, Color.White);
                        }
                        if (hand2 is Sniper_Tower)
                        {
                            _spriteBatch.Draw(sniperCard, card2Position, Color.White);
                        }
                        if (hand2 is Wizard_Tower)
                        {
                            _spriteBatch.Draw(wizardCard, card2Position, Color.White);
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
