using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace FinalProject {
    /// <summary>
    /// Griffin Brown:
    /// Create a class that can read the level design from a file given the level number
    /// also holds all of the entities on the board, and keeps track of the coming waves.
    /// Handles all interactions between all entities on the board.
    /// </summary>
    class Board {
        //Fields:
        //Entities on board
        List<Tower> towersOnBoard;
        List<Enemy> enemiesOnBoard;

        //Board data
        string[,] boardSpaces;
        int[] pathStartCords;
        int[] pathEndCords;

        //Coming waves of enemies
        List<List<Enemy>> enemyWaveList;

        //Number variables
        int levelNum;
        int waveNum;
        int enemiesMovingOnBoard;

        //Textures
        List<Texture2D> towerTextures;
        List<Texture2D> enemyTextures;

        //Board size variables
        int levelWidth = 15;
        int levelHeight = 15;
        int tileSize = 40;

        //File IO reader
        StreamReader input;

        //Read the correct level given to create the board object
        public Board(int level, List<Texture2D> _towerTextures, List<Texture2D> _enemyTextures) {
            levelNum = level;

            towersOnBoard = new List<Tower>();
            enemiesOnBoard = new List<Enemy>();

            boardSpaces = new string[levelHeight, levelWidth];

            towerTextures = _towerTextures;
            enemyTextures = _enemyTextures;

            GetLevelFromFile(levelNum);
        }

        /// <summary>
        /// The current Level, Setting this also causes the board to read the new level num from the file
        /// </summary>
        public int LevelNum {
            get {
                return levelNum;
            }
            set {
                levelNum = value;
                GetLevelFromFile(levelNum);
            }
        }

        /// <summary>
        /// Get/Set the current wave number
        /// </summary>
        public int WaveNum {
            get {
                return waveNum;
            }
            set {
                waveNum = value;
            }
        }

        /// <summary>
        /// Indexer for the board array. Can be set
        /// </summary>
        public string this[int x, int y] {
            get {
                return boardSpaces[y, x];
            }
            set {
                boardSpaces[y, x] = value;
            }
        }

        /// <summary>
        /// The cords in tile space of the starting tile
        /// </summary>
        public int[] PathStartCords {
            get {
                return pathStartCords;
            }
        }

        /// <summary>
        /// The cords in tile space of the ending tile
        /// </summary>
        public int[] PathEndCords {
            get {
                return pathEndCords;
            }
        }

        /// <summary>
        /// Get the size of each tile in pixels
        /// </summary>
        public int TileSize {
            get {
                return tileSize;
            }
        }

        /// <summary>
        /// Get the list of enemies on the board
        /// </summary>
        public List<Enemy> EnemiesOnBoard {
            get {
                return enemiesOnBoard;
            }
        }

        /// <summary>
        /// Returns a rectangle object for the tile at the given cords
        /// </summary>
        public Rectangle GetRectangleAtIndex(int x, int y) {
            return new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
        }

        /// <summary>
        /// Draws the board on the screen
        /// </summary>
        public void Draw(SpriteBatch sb, Texture2D pathTexture, Texture2D closedSpaceTexture) {

            Rectangle tileBoundingBox;

            for (int y = 0; y < levelHeight; y++) {

                for (int x = 0; x < levelWidth; x++) {
                    tileBoundingBox = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);

                    if (boardSpaces[y, x].Equals("p") || boardSpaces[y, x].Equals("s") || boardSpaces[y, x].Equals("e")) {
                        sb.Draw(pathTexture, tileBoundingBox, Color.White);
                    }

                    if (boardSpaces[y, x].Equals("x")) {
                        sb.Draw(closedSpaceTexture, tileBoundingBox, Color.White);
                    }
                }
            }

            //Draw all enemies
            foreach (Enemy e in enemiesOnBoard) {
                e.Draw(sb);
            }

            //Draw all towers
            foreach (Tower t in towersOnBoard) {
                t.Draw(sb);
            }

        }

        /// <summary>
        /// Read the level data from the file given the level number
        /// </summary>
        public void GetLevelFromFile(int level) {

            levelNum = level;

            try {
                //Create StreamReader
                input = new StreamReader("..\\..\\..\\LevelBoards.txt");
                string line = "";
                string[] splitLine;

                //Move past notes
                while (line != "^^^^^") {
                    line = input.ReadLine();
                }

                //Go to the correct level in the file
                for (int i = 0; i < levelNum; i++) {
                    while (line != "~~~") {
                        line = input.ReadLine();
                    }
                }

                //Get each line of the level, and split it into individual tiles
                for (int y = 0; y < levelHeight; y++) {
                    line = input.ReadLine();
                    splitLine = line.Split(' ');

                    for (int x = 0; x < levelWidth; x++) {
                        //Store each tiles starting value in the array
                        boardSpaces[y, x] = splitLine[x];

                        //Store the path start and end cords seperately
                        if (boardSpaces[y, x].Equals("s")) {
                            pathStartCords = new int[] { x, y };
                        }

                        if (boardSpaces[y, x].Equals("e")) {
                            pathEndCords = new int[] { x, y };
                        }
                    }
                }

                //Load the waves of the level
                waveNum = 0;
                enemyWaveList = new List<List<Enemy>>();

                //Go until there are no more waves to load
                while (line != "~~~") {
                    line = input.ReadLine();
                    splitLine = line.Split(' ');
                    //Create temp list
                    List<Enemy> waveTempList = new List<Enemy>();
                    //Create enemies based on data in file
                    int.TryParse(splitLine[0], out int enemyNum);
                    int.TryParse(splitLine[1], out int speed);
                    int.TryParse(splitLine[2], out int health);
                    for (int i = 0; i < enemyNum; i++) {
                        waveTempList.Add(new Enemy(new Rectangle((pathStartCords[0] - 1) * tileSize, pathStartCords[1] * tileSize, tileSize, tileSize),
                            enemyTextures[0],
                            health, //Health
                            speed)); //Speed (num pixels moved per frame)
                    }
                    //Put the list for that wave into the wave list
                    enemyWaveList.Add(waveTempList);
                }

                input.Close();

                CreateNextWave();
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Moves each enemy on the board along the path, if there are no enemies left, starts the next wave
        /// </summary>
        public List<Enemy> MoveEnemies() {
            //Check if a new wave needs to be created
            if (enemiesOnBoard.Count == 0) {
                CreateNextWave();
            } 

            //All enemies that have made it to the player/end of path
            List<Enemy> output = new List<Enemy>();

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            //Stand in as currently when we run out of waves the game crashes
            //This just stops this
            if(enemiesOnBoard.Count == 0) {
                return output;
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            //Keep enemies staggered
            int enemyToMoveNum = Math.Min(enemiesMovingOnBoard, enemiesOnBoard.Count);

            //If the first enemy, and by extention all enemies, are on their target tiles, find the next target
            //Also if the wave has just begun, set the very first target
            if((enemiesOnBoard[0].X == enemiesOnBoard[0].TargetX && enemiesOnBoard[0].Y == enemiesOnBoard[0].TargetY) || enemiesMovingOnBoard == 0) {
                enemiesMovingOnBoard += 1;
                enemyToMoveNum = Math.Min(enemiesMovingOnBoard, enemiesOnBoard.Count);

                //For each enemy thats moving, find the target
                for (int s = 0; s < enemyToMoveNum; s++) {
                    //Get the enemy and their current position
                    Enemy e = enemiesOnBoard[s];
                    int enemyX = e.X / tileSize;
                    int enemyY = e.Y / tileSize;

                    //If they have reached the end of the path, remove from list and put into output
                    if(e.X == pathEndCords[0] * tileSize && e.Y == pathEndCords[1] * tileSize) {
                        output.Add(e);
                        enemiesOnBoard.RemoveAt(s);
                        s--;
                        enemyToMoveNum--;
                    }   
                    //Check in all four directions if the next space in that direction is the path and isn't where they last were
                    else if ((boardSpaces[enemyY, enemyX + 1].Equals("p") || boardSpaces[enemyY, enemyX + 1].Equals("s")) && e.LastPos[0] / tileSize != enemyX + 1) {
                        enemyX += 1;
                        e.LastPos = new int[2] { (enemyX - 1) * tileSize, enemyY * tileSize };
                    }
                    else if (boardSpaces[enemyY + 1, enemyX].Equals("p") && e.LastPos[1] / tileSize != enemyY + 1) {
                        enemyY += 1;
                        e.LastPos = new int[2] { enemyX * tileSize, (enemyY - 1) * tileSize };
                    }
                    else if (boardSpaces[enemyY, enemyX - 1].Equals("p") && e.LastPos[0] / tileSize != enemyX - 1) {
                        enemyX -= 1;
                        e.LastPos = new int[2] { (enemyX + 1) * tileSize, enemyY * tileSize };
                    }
                    else if (boardSpaces[enemyY - 1, enemyX].Equals("p") && e.LastPos[1] / tileSize != enemyY - 1) {
                        enemyY -= 1;
                        e.LastPos = new int[2] { enemyX * tileSize, (enemyY + 1) * tileSize };
                    }
                    //Check to see if the next step is the final step
                    else if (boardSpaces[enemyY, enemyX + 1].Equals("e")) {
                        enemyX += 1;
                    }

                    //Set the target to the found tile
                    e.TargetX = enemyX * tileSize;
                    e.TargetY = enemyY * tileSize;
                }
            }

            //For each enemy moving, move them one pixel per speed in that direction
            for(int i = 0; i < enemyToMoveNum; i++) {
                Enemy e = enemiesOnBoard[i];

                for(int s = 0; s < e.Speed; s++) {
                    if(e.Y > e.TargetY) {
                        e.Y -= 1;
                    }
                    else if(e.Y < e.TargetY){
                        e.Y += 1;
                    }

                    if(e.X > e.TargetX) {
                        e.X -= 1;
                    }
                    else if(e.X < e.TargetX){
                        e.X += 1;
                    }
                }
            }

            //return the list of enemies that made it to the player to deal damage
            return output;
        }

        /// <summary>
        /// Get the next wave from the enemyWaveList and puts it on the board
        /// </summary>
        public void CreateNextWave() {
            enemiesMovingOnBoard = 0;
            waveNum += 1;

            if (enemyWaveList.Count == 0) {
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                //Add what happens at the end of the level
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            }
            else {
                for (int i = 0; i < enemyWaveList[0].Count; i++) {
                    enemiesOnBoard.Add(enemyWaveList[0][i]);
                }

                enemyWaveList.RemoveAt(0);
            }
        }

        /// <summary>
        /// Attempt to add a tower to the board. If the space is not a valid space, returns false
        /// </summary>
        public bool AddTowerToBoard(Tower t) {
            if (boardSpaces[t.Y / tileSize, t.X / tileSize].Equals("o")) {
                boardSpaces[t.Y / tileSize, t.X / tileSize] = "t";
                towersOnBoard.Add(t);
                return true;
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// Reduce the tower duration by 1 for each tower on the board
        /// Remove towers that's duration is finished
        /// </summary>
        public void ReduceTowerTimers() {
            foreach (Tower t in towersOnBoard) {
                t.CurrentDuration -= 1;
            }

            RemoveTowers();
        }

        /// <summary>
        /// Check all towers to see if they're duration is done, if so remove from board
        /// </summary>
        public void RemoveTowers() {
            for (int i = 0; i < towersOnBoard.Count; i++) {
                if (towersOnBoard[i].CurrentDuration == 0) {
                    boardSpaces[towersOnBoard[i].Y / tileSize, towersOnBoard[i].X / tileSize] = "o";
                    towersOnBoard.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// For each enemy, if in the right frame for their firerate, damage the enemy in the front
        /// </summary>
        public void TowersDamageEnemies(int frameCounter) 
        {
            
            for(int j = 0; j < towersOnBoard.Count; j++) 
            {
                Tower t = towersOnBoard[j];

                if(frameCounter % (tileSize/t.FireRate) == 0) 
                {
                    t.EnemyInRange(enemiesOnBoard);
                }
            }

            RemoveEnemies();
        }

        /// <summary>
        /// Remove all enemies who's health is less than 0
        /// </summary>
        public void RemoveEnemies() {
            for(int i = 0; i < enemiesOnBoard.Count; i++) {
                if(enemiesOnBoard[i].Health <= 0) {
                    enemiesOnBoard.RemoveAt(i);
                    enemiesMovingOnBoard --;
                    i--;
                }
            }
        }
    }
}
