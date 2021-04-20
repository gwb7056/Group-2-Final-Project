using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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
            _graphics.PreferredBackBufferWidth = 1200;  // set this value to the desired width of your window
            _graphics.PreferredBackBufferHeight = 1200;   // set this value to the desired height of your window
            _graphics.ApplyChanges();
            for (int count = 0; count < 2; count++)
            {
                deck.Push(new Basic_Archer_Tower());
            }
            for (int count = 0; count < 2; count++)
            {
                deck.Push(new Cannon_Tower());
            }
            for (int count = 0; count < 2; count++)
            {
                deck.Push(new Mortar_Tower());
            }
            for (int count = 0; count < 2; count++)
            {
                deck.Push(new Sniper_Tower());
            }
            for (int count = 0; count < 2; count++)
            {
                deck.Push(new Wizard_Tower());
            }
            ShuffleDeck();
            handPositions[0] = new Rectangle(0, 0, 80, 80);
            handPositions[1] = new Rectangle(80, 0, 80, 80);
            handPositions[2] = new Rectangle(160, 0, 80, 80);
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
            gameBoard.PathEndCords[1] * gameBoard.TileSize, gameBoard.TileSize, gameBoard.TileSize);
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

            //gets the keyboard state to register game state changes
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
                        player.PlayerPosition = new Rectangle(gameBoard.PathEndCords[0] * gameBoard.TileSize,
                            gameBoard.PathEndCords[1] * gameBoard.TileSize, gameBoard.TileSize, gameBoard.TileSize);
                        player.Health = 100;
                    }
                    //
                    if (keyBoardState.IsKeyDown(Keys.C))
                    {
                        activeState = GameState.Credits;
                    }
                    //No options menu for now
                    break;

                ///This game state is where the Mana System is located
                ///Mana system was made by Lance Noble
                case GameState.Game:

                    //changing enum states
                    if (keyBoardState.IsKeyDown(Keys.P))
                    {
                        activeState = GameState.Pause;
                    }

                    ///if there's no more cards to draw from the deck on to your hand to play
                    ///shuffle the cards in the discard pile and push them back onto the deck to fill it up
                    ///then the player can draw cards from the deck again
                    if (deck.Count == 0)
                    {
                        ShuffleDiscard();
                        int count = discard.Count;
                        for(int index = 0; index < count; index++)
                        {
                            deck.Push(discard.Pop());
                        }
                    }

                    ///when a player plays a card, it makes sense for that card to disappear and get discarded
                    ///so place in the player's hand that the played card came from becomes empty
                    ///in that situation, the player then must draw from the deck and replace that empty spot in their hand with a new card
                    ///this is what this section of code does
                    if (hand[0] == null)
                    {
                        hand[0] = deck.Pop();
                        handPositions[0].X = 0;
                        handPositions[0].Y = 0;
                    }
                    if (hand[1] == null)
                    {
                        hand[1] = deck.Pop();
                        handPositions[1].X = 80;
                        handPositions[1].Y = 0;
                    }
                    if (hand[2] == null)
                    {
                        hand[2] = deck.Pop();
                        handPositions[2].X = 160;
                        handPositions[2].Y = 0;
                    }

                    //Gets the mouse state to register all functionalities of the mana system
                    mouseState = Mouse.GetState();

                    ///Please refer to the CheckSummons method body for more details
                    ///This method is called once for every card in the player's hand
                    ///The player can have a maximum of three cards in their hand at at time
                    CheckSummons(0);
                    CheckSummons(1);
                    CheckSummons(2);

                    ///this frame counter sets the pace for the player's mana regeneration
                    ///the higher the number after the mod (%) operation is, the longer it takes for the player to regenerate mana
                    frameCounter1 += 1;
                    if (frameCounter1 % 120 == 0)
                    {
                        if (player.Mana < 30)
                        {
                            player.Mana++;
                        }
                    }
                    ///this frame counter sets the pace for how often the towers check for enemies in range
                    frameCounter0 += 1;
                    gameBoard.TowersDamageEnemies(frameCounter0);
                    player.TakeDamage(gameBoard.MoveEnemies());
                    if(frameCounter0 % 60 == 0) 
                    {
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
                            gameBoard.PathEndCords[1] * gameBoard.TileSize, gameBoard.TileSize, gameBoard.TileSize);
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
                    _spriteBatch.DrawString(font, "MAIN MENU", new Vector2(400, 400), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"Spacebar\" to play game.", new Vector2(300, 500), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"C\" for credits.", new Vector2(300, 600), Color.Black);
                    break;

                case GameState.LevelFinished:
                    _spriteBatch.DrawString(font, "YOU COMPLETED THE LEVEL!", new Vector2(400, 400), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to return the the main menu", new Vector2(300, 500), Color.Black);
                    if(startingLevelNum + 1 < totalNumLevels) {
                        _spriteBatch.DrawString(font, "Press \"N\" to go to the next level", new Vector2(300, 600), Color.Black);
                    }
                    break;


                case GameState.Game:
                    //Draw the board and entities on the board
                    gameBoard.Draw(_spriteBatch, pathTexture, closedSpaceTexture);

                    //Drawing player
                    player.Draw(_spriteBatch);

                    _spriteBatch.DrawString(font, "Press \"P\" to pause.", new Vector2(100, 100), Color.White);
                    _spriteBatch.DrawString(font, "Health: " + player.Health, new Vector2(100, 200), Color.White);
                    _spriteBatch.DrawString(font, "Mana: " + player.Mana, new Vector2(100, 300), Color.White);

                    DrawHand(0);
                    DrawHand(1);
                    DrawHand(2);

                    for (int index = 0; index < gameBoard.TowersOnBoard.Count; index++)
                    {
                        if (gameBoard.TowersOnBoard[index].TowerPosition.Contains(mouseState.Position))
                        {
                            if (gameBoard.TowersOnBoard[index].IsFiring)
                            {
                                _spriteBatch.DrawString(font, $"This tower is firing", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y - 15), Color.White);
                            }
                            else if (!gameBoard.TowersOnBoard[index].IsFiring)
                            {
                                _spriteBatch.DrawString(font, $"This tower is not firing", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y - 15), Color.White);
                            }
                            _spriteBatch.DrawString(font, $"Fire Rate: {gameBoard.TowersOnBoard[index].FireRate}", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y), Color.White);
                            _spriteBatch.DrawString(font, $"Damage: {gameBoard.TowersOnBoard[index].Damage}", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y - 30), Color.White);
                            _spriteBatch.DrawString(font, $"Range: {gameBoard.TowersOnBoard[index].Range}", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y - 45), Color.White);
                            _spriteBatch.DrawString(font, $"Current Duration: {gameBoard.TowersOnBoard[index].CurrentDuration}", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y - 60), Color.White);
                            _spriteBatch.DrawString(font, $"Max Duration: {gameBoard.TowersOnBoard[index].MaxDuration}", new Vector2(gameBoard.TowersOnBoard[index].X, gameBoard.TowersOnBoard[index].Y - 75), Color.White);
                        }
                    }

                    break;

                case GameState.Pause:
                    _spriteBatch.DrawString(font, "PAUSE MENU", new Vector2(400, 400), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"Spacebar\" to play game.", new Vector2(300, 500), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to go back to the Menu.", new Vector2(300, 600), Color.Black);
                    break;

                case GameState.Credits:
                    _spriteBatch.DrawString(font, "CREDITS MENU", new Vector2(400, 400), Color.Black);
                    _spriteBatch.DrawString(font, "Griffin Brown, Kylian Hervet, Liam Alexiou, Lance Noble", new Vector2(200, 500), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to go back to the Menu.", new Vector2(300, 600), Color.Black);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(font, "GAME OVER", new Vector2(200, 200), Color.Black);
                    _spriteBatch.DrawString(font, "Press \"M\" to go back to the Menu.", new Vector2(300, 500), Color.Black);
                    break;
            }


            _spriteBatch.End();
            base.Draw(gameTime);

        }

        /// <summary>
        /// This method draws out the cards currently in the player's hand to let them know what they can play
        /// </summary>
        /// <param name="index">the index of one of the three cards in the player's hand to draw</param>
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

        /// <summary>
        /// This method shuffles the deck
        /// This is only called once, and it's called in the Initialize method
        /// This is because after the deck is initially shuffled, the rest of the shuffling will be done in the discard pile
        /// CREDITS TO LIAM ALEXIOU FOR THIS METHOD
        /// </summary>
        public void ShuffleDeck()
        {
            List<Card> tempDeck = new List<Card>();
            int index2;
            int count = deck.Count;
            for (int index = 0; index < count; index++)
            {
                tempDeck.Add(deck.Pop());
            }
            for (int index1 = 0; index1 < count; index1++)
            {
                index2 = rng.Next(0, tempDeck.Count);
                deck.Push(tempDeck[index2]);
                tempDeck.Remove(tempDeck[index2]);
            }
        }

        /// <summary>
        /// This method shuffles the discard pile
        /// This is the one that's called over and over
        /// This is because the deck will continuously be empty, so the discard has to be ready to shuffle and pop the discarded cards back into the deck
        /// CREDITS TO LIAM ALEXIOU FOR THIS METHOD
        /// </summary>
        public void ShuffleDiscard()
        {
            List<Card> tempDeck = new List<Card>();
            int index2;
            int count = discard.Count;
            for (int index = 0; index < count; index++)
            {
                tempDeck.Add(discard.Pop());
            }
            for (int index1 = 0; index1 < count; index1++)
            {
                index2 = rng.Next(0, tempDeck.Count);
                discard.Push(tempDeck[index2]);
                tempDeck.Remove(tempDeck[index2]);
            }
        }

        /// <summary>
        /// This method allows players to interact with the cards (i.e. drag them around) and use them to summon
        /// Here are the mechanics to summon a card:
        ///     hover your mouse cursor over the card you want to summon
        ///     hold left click and drag it to the position at which you want to summon
        ///     WHILE holding left click, press right click to summon it
        ///     There's a neat mechanic where you can hold left click and hover your mouse over multiple cards
        ///     all the cards you hovered over will now be dragable
        ///     and if you keep pressing right click, all the cards under your mouse cursor will be summoned in the same order that your cursor hovered them in
        ///     if you want to cancel the summon, just let go of the left click button
        /// This method checks for the specified card in the player's hand.
        /// If that card is an Archer Tower, for example, 
        /// then call the real constructor of the Archer Tower that properly initializes all its properties (i.e. firerate, duration, range, etc.)
        /// The point of every tower class' default constructors is to simulate the idea that the cards in the deck/hand/discard are not really towers/spells.
        /// The cards are simply a vessel or key or reference point that can activate the summoning of a real tower/spell.
        /// </summary>
        /// <param name="index">the index of one of the three cards in the player's hand to check summons for</param>
        public void CheckSummons(int index)
        {
            if (handPositions[index].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed && player.Mana >= 1)
            {
                handPositions[index].X = mouseState.Position.X - 30;
                handPositions[index].Y = mouseState.Position.Y - 30;
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
                    handPositions[index].X = 80;
                    handPositions[index].Y = 0;
                }
            }
            else if (index == 2)
            {
                if (handPositions[index].Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Released && player.Mana >= 1)
                {
                    handPositions[index].X = 160;
                    handPositions[index].Y = 0;
                }
            }
        }
    }
}
