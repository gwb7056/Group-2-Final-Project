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
        List<Tower> towersOnBoard;
        List<Enemy> enemiesOnBoard;

        string[,] boardSpaces;
        List<int[]> path;
        int[] pathStartCords;
        int[] pathEndCords;

        List<List<Enemy>> enemyWaveList;

        int levelNum;
        int waveNum;
        int waveStepsTaken;

        List<Texture2D> towerTextures;
        List<Texture2D> enemyTextures;

        int levelWidth = 15;
        int levelHeight = 15;
        int tileSize = 40;

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
        /// <param name="x">
        /// X cord of space
        /// </param>
        /// <param name="y">
        /// Y cord of space
        /// </param>
        /// <returns>
        /// Returns the value in the array at the given cords
        /// </returns>
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
        /// Returns a rectangle object for the tile at the given cords
        /// </summary>
        public Rectangle GetRectangleAtIndex(int x, int y) {
            return new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
        }

        /// <summary>
        /// Board Class ToString()
        /// </summary>
        /// <returns>
        /// Returns the board in an X by Y grid in one string
        /// </returns>
        public override string ToString() {
            string output = "";

            for (int y = 0; y < levelHeight; y++) {
                for (int x = 0; x < levelWidth; x++) {
                    output += (boardSpaces[y, x] + " ");
                }

                output += "\n";
            }

            return output;
        }

        /// <summary>
        /// Draws the board on the screen
        /// </summary>
        /// <param name="sb">
        /// The SpriteBatch object
        /// </param>
        /// <param name="pathTexture">
        /// Texture2D for the path tiles
        /// </param>
        /// <param name="closedSpaceTexture">
        /// Texture2D for the closed space tiles
        /// </param>
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

            try {

                //Create StreamReader
                input = new StreamReader("..\\..\\..\\LevelBoards.txt");
                string line = "";
                string[] splitLine;

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
                    //Create temp list
                    List<Enemy> waveTempList = new List<Enemy>();
                    //Create enemies based on data in file
                    int.TryParse(line, out int enemyNum);
                    for (int i = 0; i < enemyNum; i++) {

                        waveTempList.Add(new Enemy(new Rectangle((pathStartCords[0] - 1) * tileSize, pathStartCords[1] * tileSize, tileSize, tileSize),
                            enemyTextures[0],
                            10, //Health
                            1)); //Speed (num tiles per time unit)
                    }
                    //Put the list for that wave into the wave list
                    enemyWaveList.Add(waveTempList);

                }

                CreateNextWave();

                input.Close();

            }
            catch (Exception e) {

                Debug.WriteLine(e.Message);

            }

        }

        /// <summary>
        /// Moves each enemy on the board along the path, if there are no enemies left, starts the next wave
        /// </summary>
        public List<Enemy> MoveEnemies() {

            if (enemiesOnBoard.Count == 0) {

                CreateNextWave();

            }

            List<Enemy> output = new List<Enemy>();

            waveStepsTaken += 1;
            int enemyToMoveNum = Math.Min(waveStepsTaken, enemiesOnBoard.Count);

            for (int s = 0; s < enemyToMoveNum; s++) {

                Enemy e = enemiesOnBoard[s];
                int enemyX = e.X / tileSize;
                int enemyY = e.Y / tileSize;
                bool spaceFound = false;

                for (int i = 0; i < e.Speed; i++) {

                    if ((boardSpaces[enemyY, enemyX + 1].Equals("p") || boardSpaces[enemyY, enemyX + 1].Equals("s")) && e.LastPos[0] / tileSize != enemyX + 1) {

                        enemyX += 1;
                        e.LastPos = new int[2] { (enemyX - 1) * tileSize, enemyY * tileSize };
                        spaceFound = true;

                    }
                    else if (boardSpaces[enemyY + 1, enemyX].Equals("p") && e.LastPos[1] / tileSize != enemyY + 1) {

                        enemyY += 1;
                        e.LastPos = new int[2] { enemyX * tileSize, (enemyY - 1) * tileSize };
                        spaceFound = true;

                    }
                    else if (boardSpaces[enemyY, enemyX - 1].Equals("p") && e.LastPos[0] / tileSize != enemyX - 1) {

                        enemyX -= 1;
                        e.LastPos = new int[2] { (enemyX + 1) * tileSize, enemyY * tileSize };
                        spaceFound = true;

                    }
                    else if (boardSpaces[enemyY - 1, enemyX].Equals("p") && e.LastPos[1] / tileSize != enemyY - 1) {

                        enemyY -= 1;
                        e.LastPos = new int[2] { enemyX * tileSize, (enemyY + 1) * tileSize };
                        spaceFound = true;

                    }

                    //Check to see if the next step is the final step
                    if (!spaceFound) {
                        if (boardSpaces[enemyY, enemyX + 1].Equals("e")) {
                            output.Add(e);
                            i = e.Speed;
                            enemiesOnBoard.RemoveAt(s);
                            s--;
                            enemyToMoveNum--;

                        }

                    }

                }

                e.X = enemyX * tileSize;
                e.Y = enemyY * tileSize;

            }
            return output;

        }

        /// <summary>
        /// Get the next wave from the enemyWaveList and puts it on the board
        /// </summary>
        public void CreateNextWave() {

            waveStepsTaken = 0;
            waveNum += 1;

            if (enemyWaveList.Count == 0) {

                //Add what happens at the end of the level

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
        /// <param name="t">
        /// Tower object to attempt to add
        /// </param>
        /// <returns>
        /// Returns true if the addition was successful, falso otherwise
        /// </returns>
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

        public void ReduceTowerTimers() {
            foreach (Tower t in towersOnBoard) {
                t.CurrentDuration -= 1;
            }

            RemoveTowers();
        }

        public void RemoveTowers() {
            for (int i = 0; i < towersOnBoard.Count; i++) {
                if (towersOnBoard[i].CurrentDuration == 0) {
                    boardSpaces[towersOnBoard[i].Y / tileSize, towersOnBoard[i].X / tileSize] = "o";
                    towersOnBoard.RemoveAt(i);
                    i--;
                }
            }
        }

        public void TowersDamageEnemies() {
            for(int j = 0; j < towersOnBoard.Count; j++) {
                Tower t = towersOnBoard[j];
                for(int i = 0; i < enemiesOnBoard.Count; i++) {
                    if (t.EnemyInRange(enemiesOnBoard[i])) {
                        i = enemiesOnBoard.Count;
                    }
                }
            }

            RemoveEnemies();
        }

        public void RemoveEnemies() {
            for(int i = 0; i < enemiesOnBoard.Count; i++) {
                if(enemiesOnBoard[i].Health <= 0) {
                    enemiesOnBoard.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
