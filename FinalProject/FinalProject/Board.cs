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
    /// Griffin Brown
    /// </summary>
    class Board {
        List<Entity> entityBoard;
        string[,] boardSpaces;
        List<List<Enemy>> enemyWaveList;
        int levelNum;
        int waveNum;

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
                    }
                }

                input.Close();
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
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
        /// Draws the board on the scree
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
                    if(boardSpaces[y, x].Equals("p")) {
                        sb.Draw(pathTexture, tileBoundingBox, Color.White);
                    }
                    if(boardSpaces[y, x].Equals("x")) {
                        sb.Draw(closedSpaceTexture, tileBoundingBox, Color.White);
                    }
                }
            }
        }
    }
}
