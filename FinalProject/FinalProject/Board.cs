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
        List<Entity> entityBoard;
        string[,] boardSpaces;
        List<List<Enemy>> enemyWaveList;
        int levelNum;
        int waveNum;
        int[] pathStartCords;
        int[] pathEndCords;

        int levelWidth = 15;
        int levelHeight = 15;
        int tileSize = 40;
        
        StreamReader input;

        //Read the correct level given to create the board object
        public Board(int level) {
            levelNum = level;
            waveNum = 0;
            entityBoard = new List<Entity>();
            boardSpaces = new string[levelHeight, levelWidth];

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
            for(int y = 0; y < levelHeight; y++) {
                for(int x = 0; x < levelWidth; x++) {
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
            for(int y = 0; y < levelHeight; y++) {
                for(int x = 0; x < levelWidth; x++) {
                    tileBoundingBox = new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize);
                    if(boardSpaces[y, x].Equals("p") || boardSpaces[y, x].Equals("s") || boardSpaces[y, x].Equals("e")) {
                        sb.Draw(pathTexture, tileBoundingBox, Color.White);
                    }
                    if(boardSpaces[y, x].Equals("x")) {
                        sb.Draw(closedSpaceTexture, tileBoundingBox, Color.White);
                    }
                }
            }
        }
        
        public void GetLevelFromFile(int level) {
            try {
                input = new StreamReader("..\\..\\..\\LevelBoards.txt");
                string line = "";
                string[] splitLine;

                //Go to the correct level in the file
                for(int i = 0; i < levelNum; i++) {
                    while(line != "~~~") {
                        line = input.ReadLine();
                    }
                }

                //Get each line of the level, and split it into individual tiles
                for(int y = 0; y < levelHeight; y++) {
                    line = input.ReadLine();
                    splitLine = line.Split(' ');
                    for(int x = 0; x < levelWidth; x++) {
                        //Store each tiles starting value in the array
                        boardSpaces[y, x] = splitLine[x];
                        if(boardSpaces[y, x].Equals("s")) {
                            pathStartCords = new int[] {x, y};
                        }

                        if(boardSpaces[y, x].Equals("e")) {
                            pathEndCords = new int[] {x, y};
                        }
                    }
                }

                input.Close();
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
