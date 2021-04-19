using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Game Title: Magic The Towering
/// Genre: Tower Defense
/// Objective: Defend the base from enemies trying to attack it
/// Tools: You can summon different types of towers and (soon) spells to stop enemies from making contact with the base
/// Mechanics: The game is organized into a 15x15 tile board (think of it as a chess board)
///     There is a designated path (marked by color) that trails from the left side of the board to the right side of the board
///     That path is the path that the enemies move along to get to the base
///     The base is located at the very end of the path
///     Some of the tiles outside the path (marked by the color green) are where you will place your defenses
///     This chess board layout ensures that all entities are placed in a very organized and cohesive manner.
///     The player has a hand which is a set of cards he is restricted to playing
///     If the player needs more cards in his hand after playing some, the Mana System will draw cards from the deck to place in their hand
///     After the player plays a card, that card goes in a discard pile
///     Eventually, the deck will run out of cards to draw from
///     This is where the discard pile comes in
///     The discard pile is shuffled and every card that has been discarded will now be pushed onto the deck again
///     The deck fills up and the player can continue drawing cards onto their hand to play
/// </summary>
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
        //General Game Properties: How the player will operate the game
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameState activeState;
        private KeyboardState keyBoardState;
        private MouseState mouseState;
        
        //Graphics: What can be seen in the game
        private SpriteFont font;
        private Texture2D pathTexture;
        private Texture2D closedSpaceTexture;
        private Texture2D enemyTestTexture;
        private Texture2D playerTexture;
        private Texture2D archerCard;
        private Texture2D cannonCard;
        private Texture2D mortarCard;
        private Texture2D sniperCard;
        private Texture2D wizardCard;
        private List<Texture2D> towerTextures;
        private List<Texture2D> enemyTextures;

        /// <summary>
        /// The Board:
        /// This is basically a manager class.
        /// It manages NEARLY everything about the game.
        /// This includes tower placement, tower actions, enemy movement, player damage, etc.
        /// However, it does not manage the mana/deck/card system. They are done in Game1.
        /// </summary>
        private Board gameBoard;
        private Player player;
        private int frameCounter0;
        private int frameCounter1;
        private int towerCount;
        private int startingLevelNum;
        private int totalNumLevels;

        /// <summary>
        /// The Mana System:
        /// This is just what we'll call handling cards, the deck, mana costs, and summoning
        /// </summary>
        private Stack<Card> deck;
        private Stack<Card> discard;
        private Card[] hand;
        private Rectangle[] handPositions;
        private Random rng;

        /// <summary>
        /// Nearly all the fields in Game1 that need to be initialized.
        /// The fields that can't be initialized here are the textures, gameBoard, and player.
        /// This is because they require textures to be loaded, and they are not loaded yet here.
        /// Other value types and reference types are initialized here. 
        /// </summary>
        public Game1() 
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            enemyTextures = new List<Texture2D>();
            towerTextures = new List<Texture2D>();
            frameCounter0 = 0;
            frameCounter1 = 0;
            towerCount = 0;
            startingLevelNum = 0;
            totalNumLevels = 2;
            deck = new Stack<Card>();
            discard = new Stack<Card>();
            hand = new Card[3];
            rng = new Random();
            handPositions = new Rectangle[3];
        }

        /// <summary>
        /// We used this to change the window from the default to 600 x 600
        /// We did this because our tile sizes are 40 x 40 pixels, which is a divisor of 600 so everything fits nicely.
        /// This means that the entire gameboard is 15 x 15 tiles.
        /// We also used this to start putting some cards in our newly instantiated deck.
        /// So when the game starts, there's already a deck full of cards that's fully shuffled.
        /// And the player already has a pre-drawn hand from that deck from which they can start playing cards.
        /// </summary>
        protected override void Initialize() 
        {
            base.Initialize();
            //Change the window size
            _graphics.PreferredBackBufferWidth = 600;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 600;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
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
            handPositions[0] = new Rectangle(0, 0, 50, 50);
            handPositions[1] = new Rectangle(60, 0, 50, 50);
            handPositions[2] = new Rectangle(120, 0, 50, 50);
            hand[0] = deck.Pop();
            hand[1] = deck.Pop();
            hand[2] = deck.Pop();
        }


        /// <summary>
        /// This is where all the Texture2D fields are loaded along with the Board field and Player field
        /// </summary>
        protected override void LoadContent() 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            /*For now, these textures serve as both card textures and tower textures.
             Ideally, there would be two sets of textures.
            One set is the set of card textures that actually look like cards with idk a symbol signifying which card is which
            You'll see that set of card textures in your hand.
            Another set is the set of the actual tower textures (and soon to come) spells that you'll see when you summon a tower using the cards*/
            archerCard = Content.Load<Texture2D>("archercard");
            cannonCard = Content.Load<Texture2D>("cannoncard");
            mortarCard = Content.Load<Texture2D>("mortarcard");
            sniperCard = Content.Load<Texture2D>("snipercard");
            wizardCard = Content.Load<Texture2D>("wizardcard");
            
            ///This is a plain basic font 
            ///We'll use this to show how much health the base has and how much mana the player has left
            ///Also used to show the state of the game (aka whether you won, lost, pause, etc.)
            ///Using text to visually represent all this stuff is fine
            ///However, ideally, it would be nice to make visuals.
            font = Content.Load<SpriteFont>("Arial12");

            ///These are the options for textures that each tile can have
            ///the path textures are marked by (coffee?) colored squares and signify where the enemies will be moving
            ///The closed space textures are marked by a black X and mean that you cannot place defenses there
            ///The spaces eligible for placing defenses are the green ones with no X on it
            pathTexture = Content.Load<Texture2D>("pathTexture");
            closedSpaceTexture = Content.Load<Texture2D>("closedSpaceTexture");

            ///These are the different textures that the enemies can have
            ///Each different type of enemy has their own unique texture to signify which enemy is which
            ///Enemy textures are added in a list.
            ///This is because the Board object (gameBoard) that will be instantiated in a couple of lines take in this list
            ///gameBoard will use this list to assign the proper texture to every enemy that it will spawn
            ///Please refer to Board.cs class for more details
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("fallguy"));
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("ninjaTexture"));
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("shieldmaidenTexture"));
            enemyTextures.Add(enemyTestTexture = Content.Load<Texture2D>("swordmanTexture"));

            ///This is the player texture that will signify what the base looks like and where it is
            ///It's an among us character, but now that we're solidifying the algorithms,
            ///We can look towards working on assets
            playerTexture = Content.Load<Texture2D>("among us");
           
            ///Creating the game board
            ///Please refer to Board.cs class for more details
            gameBoard = new Board(startingLevelNum, towerTextures, enemyTextures);

            ///Creating the player (base)
            ///Please refer to Player.cs class for more details
            player = new Player(playerTexture, gameBoard.PathEndCords[0] * gameBoard.TileSize,
            gameBoard.PathEndCords[1] * gameBoard.TileSize, 40, 40);
        }

        /// <summary>
        /// This is the meat of the game.
        /// It handles game state changes, summoning defenses, drawing from deck, discarding, reshuffling, player damage, enemy movement, etc.
        /// </summary>
        /// <param name="gameTime">the game time to update</param>
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

                ///This entire game state is the Mana System
                ///Author: Lance Noble
                case GameState.Game:
                    
                    ///
                    if (deck.Count == 0)
                    {
                        ShuffleDiscard();
                        for(int index = 0; index < 12; index++)
                        {
                            deck.Push(discard.Pop());
                        }
                    }
                    if (hand[0] == null)
                    {
                        hand[0] = deck.Pop();
                        handPositions[0].X = 0;
                        handPositions[0].Y = 0;
                    }
                    if (hand[1] == null)
                    {
                        hand[1] = deck.Pop();
                        handPositions[1].X = 60;
                        handPositions[1].Y = 0;
                    }
                    if (hand[2] == null)
                    {
                        hand[2] = deck.Pop();
                        handPositions[2].X = 120;
                        handPositions[2].Y = 0;
                    }

                    mouseState = Mouse.GetState();
                    CheckSummons(0);
                    CheckSummons(1);
                    CheckSummons(2);

                    frameCounter1 += 1;
                    if (frameCounter1 % 120 == 0)
                    {
                        if (player.Mana < 30)
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
                    frameCounter0 += 1;
                    gameBoard.TowersDamageEnemies(frameCounter0);
                    player.TakeDamage(gameBoard.MoveEnemies());

                    if(frameCounter0 % 60 == 0) {
                        gameBoard.ReduceTowerTimers();
                    }

                    if (player.Health <= 0)
                    {
                        activeState = GameState.GameOver;
                    }

                    if (gameBoard.LevelFinished) 
                    {
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

                    DrawHand(0);
                    DrawHand(1);
                    DrawHand(2);

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

        public void DrawHand(int index)
        {
            if (hand[index] != null)
            {
                if (hand[index] is Basic_Archer_Tower)
                {
                    _spriteBatch.Draw(archerCard, handPositions[index], Color.White);
                }
                if (hand[index] is Cannon_Tower)
                {
                    _spriteBatch.Draw(cannonCard, handPositions[index], Color.White);
                }
                if (hand[index] is Mortar_Tower)
                {
                    _spriteBatch.Draw(mortarCard, handPositions[index], Color.White);
                }
                if (hand[index] is Sniper_Tower)
                {
                    _spriteBatch.Draw(sniperCard, handPositions[index], Color.White);
                }
                if (hand[index] is Wizard_Tower)
                {
                    _spriteBatch.Draw(wizardCard, handPositions[index], Color.White);
                }
            }
        }

        public void ShuffleDeck()
        {
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

        public void ShuffleDiscard()
        {
            List<Card> tempDeck = new List<Card>();
            int index2;
            for (int index = 0; index < 12; index++)
            {
                tempDeck.Add(discard.Pop());
            }
            for (int index1 = 0; index1 < 12; index1++)
            {
                index2 = rng.Next(0, tempDeck.Count);
                discard.Push(tempDeck[index2]);
                tempDeck.Remove(tempDeck[index2]);
            }
        }

        public void CheckSummons(int index)
        {
            if (handPositions[index].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= 1)
            {
                handPositions[index].X = mouseState.Position.X - 20;
                handPositions[index].Y = mouseState.Position.Y - 20;
                if (mouseState.RightButton == ButtonState.Pressed)
                {
                    towerCount++;
                    for (int width = 0; width < 15; width++)
                    {
                        for (int height = 0; height < 15; height++)
                        {
                            if (gameBoard.GetRectangleAtIndex(width, height).Contains(mouseState.Position))
                            {
                                if (hand[index] is Basic_Archer_Tower)
                                {
                                    if (gameBoard.AddTowerToBoard(new Basic_Archer_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, archerCard)))
                                    {
                                        player.Mana--;
                                        discard.Push(hand[index]);
                                        hand[index] = null;

                                    }
                                }
                                if (hand[index] is Cannon_Tower)
                                {
                                    if (gameBoard.AddTowerToBoard(new Cannon_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, cannonCard)))
                                    {
                                        player.Mana--;
                                        discard.Push(hand[index]);

                                        hand[index] = null;

                                    }
                                }
                                if (hand[index] is Mortar_Tower)
                                {
                                    if (gameBoard.AddTowerToBoard(new Mortar_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, mortarCard)))
                                    {
                                        player.Mana--;
                                        discard.Push(hand[index]);

                                        hand[index] = null;

                                    }

                                }
                                if (hand[index] is Sniper_Tower)
                                {
                                    if (gameBoard.AddTowerToBoard(new Sniper_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, sniperCard)))
                                    {
                                        player.Mana--;
                                        discard.Push(hand[index]);
                                        hand[index] = null;

                                    }
                                }
                                if (hand[index] is Wizard_Tower)
                                {
                                    if (gameBoard.AddTowerToBoard(new Wizard_Tower(width * gameBoard.TileSize, height * gameBoard.TileSize, wizardCard)))
                                    {
                                        player.Mana--;
                                        discard.Push(hand[index]);
                                        hand[index] = null;

                                    }
                                }
                            }
                        }
                    }
                }

            }
            if (index == 0)
            {
                if (handPositions[index].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                {
                    handPositions[index].X = 0;
                    handPositions[index].Y = 0;
                }
            }
            else if (index == 1)
            {
                if (handPositions[index].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                {
                    handPositions[index].X = 60;
                    handPositions[index].Y = 0;
                }
            }
            else if (index == 2)
            {
                if (handPositions[index].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                {
                    handPositions[index].X = 120;
                    handPositions[index].Y = 0;
                }
            }
        }
    }
}
