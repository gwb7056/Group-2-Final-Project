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
        
        StreamReader input;

        public Board(int level) {
            levelNum = level;
            waveNum = 0;
            entityBoard = new List<Entity>();
            boardSpaces = new string[levelHeight, levelWidth];

            try {
                input = new StreamReader("..\\..\\..\\LevelBoards.txt");
                string line = "";
                string[] splitLine;

                for(int i = 0; i < levelNum; i++) {
                    while(line != "~~~") {
                        line = input.ReadLine();
                    }
                }

                for(int i = 0; i < levelHeight; i++) {
                    line = input.ReadLine();
                    splitLine = line.Split(' ');
                    for(int j = 0; j < levelWidth; j++) {
                        boardSpaces[i, j] = splitLine[j];
                    }
                }

                input.Close();
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        public override string ToString() {
            string output = "";
            for(int i = 0; i < levelHeight; i++) {
                for(int j = 0; j < levelWidth; j++) {
                    output += (boardSpaces[i, j] + " ");
                }
                output += "\n";
            }
            return output;
        }
    }
}
