using System;
using System.Collections.Generic;
using Hexapawn.Models;
using Microsoft.Xna.Framework;

namespace Hexapawn.AI {
    class DarwinAI {
        public DarwinAI(Darwin darwin, BoardSquare[,] boardMatrix) {
            _darwin = darwin;
            _boardMatrix = boardMatrix;
        }

        private int _previousBox = -1;
        public int Box { get; private set; }
        public string Token { get; private set; }
        public bool Inverted { get; private set; }

        private readonly Darwin _darwin;

        public Darwin ThisDarwin {
            get => _darwin;
        }

        private readonly BoardSquare[,] _boardMatrix;

        public void DetermineMove() {
            var squaresWithBlackPawns = new List<BoardSquare>();
            var squaresWithWhitePawns = new List<BoardSquare>();

            for (int row = 0; row < 3; row++) {
                for (int col = 0; col < 3; col++) {
                    var boardSquare = _boardMatrix[row, col];
                    if (boardSquare.PawnO != null) {
                        if (boardSquare.PawnO.IsPlayer) squaresWithWhitePawns.Add(boardSquare);
                        else squaresWithBlackPawns.Add(boardSquare);
                    }
                }
            }

            for (int i = 0; i < 2; i++) {
                Inverted = i != 0;

                var zero = Inverted ? 2 : 0;
                var one = 1;
                var two = Inverted ? 0 : 2;

                // 0
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(one, 0),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(one, 2),
                    new Vector2(two, 2)
                })) {
                    Box = 0;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(one, 0, zero, 1);
                    else if (Token == "blue") MovePawn(one, 0, one, 1);
                    else MovePawn(two, 0, two, 1);
                    break;
                }

                // 1
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(one, 0),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 2),
                    new Vector2(one, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 1;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(zero, 0, zero, 1);
                    else MovePawn(zero, 0, one, 1);
                    break;
                }

                // 2
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(zero, 1),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(one, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 2;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(zero, 0, one, 1);
                    else if (Token == "blue") MovePawn(two, 0, one, 1);
                    else if (Token == "purple") MovePawn(two, 0, two, 1);
                    else MovePawn(zero, 1, zero, 2);
                    break;
                }

                // 3
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(two, 0),
                    new Vector2(one, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 3;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 0, two, 1);
                    else if (Token == "blue") MovePawn(one, 1, one, 2);
                    else if (Token == "purple") MovePawn(one, 0, zero, 1);
                    else MovePawn(one, 1, two, 2);
                    break;
                }

                // 4
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(one, 1),
                    new Vector2(one, 2)
                })) {
                    Box = 4;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 0, one, 1);
                    else if (Token == "blue") MovePawn(zero, 0, one, 1);
                    else MovePawn(two, 0, two, 1);
                    break;
                }

                // 5
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(one, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(two, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 5;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(one, 0, one, 1);
                    else if (Token == "blue") MovePawn(one, 0, two, 1);
                    else MovePawn(one, 0, zero, 1);
                    break;
                }

                // 6
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(two, 0),
                    new Vector2(one, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 2),
                    new Vector2(two, 1)
                })) {
                    Box = 6;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(one, 0, two, 1);
                    else if (Token == "blue") MovePawn(one, 1, one, 2);
                    else MovePawn(one, 1, one, 2);
                    break;
                }

                // 7
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(two, 0),
                    new Vector2(zero, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 2),
                    new Vector2(one, 1),
                    new Vector2(two, 1)
                })) {
                    Box = 7;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 0, one, 1);
                    else MovePawn(one, 0, two, 1);
                    break;
                }

                // 8
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(two, 0),
                    new Vector2(zero, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(two, 1),
                    new Vector2(one, 2)
                })) {
                    Box = 8;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(zero, 1, zero, 2);
                    else MovePawn(zero, 1, one, 2);
                    break;
                }

                // 9
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(one, 0),
                    new Vector2(two, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(one, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 9;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(one, 0, zero, 1);
                    else MovePawn(zero, 0, one, 1);
                    break;
                }

                // 10
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(one, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 10;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 0, two, 1);
                    else MovePawn(two, 0, one, 1);
                    break;
                }

                // 11
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 2),
                    new Vector2(one, 1)
                })) {
                    Box = 11;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "purple") MovePawn(two, 0, two, 1);
                    else MovePawn(two, 0, one, 1);
                    break;
                }

                // 12
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(two, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(two, 2)
                })) {
                    Box = 12;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    MovePawn(two, 0, two, 1);
                    break;
                }

                // 13
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(two, 0),
                    new Vector2(zero, 1),
                    new Vector2(one, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(two, 1)
                })) {
                    Box = 13;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "purple") MovePawn(zero, 1, zero, 2);
                    else MovePawn(one, 1, one, 2);
                    break;
                }

                // 14
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(one, 1),
                    new Vector2(two, 1)
                })) {
                    Box = 14;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    MovePawn(zero, 0, one, 1);
                    break;
                }

                // 15
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(zero, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(one, 1),
                    new Vector2(two, 1)
                })) {
                    Box = 15;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "purple") MovePawn(zero, 1, zero, 2);
                    else MovePawn(one, 0, two, 1);
                    break;
                }

                // 16
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(two, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1),
                    new Vector2(one, 1)
                })) {
                    Box = 16;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "purple") MovePawn(two, 1, two, 2);
                    else MovePawn(one, 0, zero, 1);
                    break;
                }

                // 17
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(zero, 1),
                    new Vector2(one, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(two, 1)
                })) {
                    Box = 17;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "blue") MovePawn(zero, 1, zero, 2);
                    else MovePawn(one, 1, one, 2);
                    break;
                }

                // 18
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(two, 0),
                    new Vector2(one, 1),
                    new Vector2(two, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1)
                })) {
                    Box = 18;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 1, two, 2);
                    else MovePawn(one, 1, one, 2);
                    break;
                }

                // 19
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(two, 0),
                    new Vector2(zero, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(one, 1)
                })) {
                    Box = 19;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 0, two, 1);
                    if (Token == "purple") MovePawn(zero, 1, zero, 2);
                    else MovePawn(two, 0, one, 1);
                    break;
                }

                // 20
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(one, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(zero, 1)
                })) {
                    Box = 20;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(one, 0, zero, 1);
                    else MovePawn(one, 1, one, 2);
                    break;
                }

                // 21
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(one, 0),
                    new Vector2(one, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(two, 1)
                })) {
                    Box = 21;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "blue") MovePawn(one, 1, one, 2);
                    else MovePawn(one, 0, two, 1);
                    break;
                }

                // 22
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(zero, 0),
                    new Vector2(zero, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(one, 1)
                })) {
                    Box = 22;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "purple") MovePawn(zero, 0, one, 1);
                    else MovePawn(zero, 1, zero, 2);
                    break;
                }

                // 23
                if (CheckMatch(squaresWithBlackPawns, new List<Vector2>() {
                    new Vector2(two, 0),
                    new Vector2(two, 1)
                }) && CheckMatch(squaresWithWhitePawns, new List<Vector2>() {
                    new Vector2(one, 1)
                })) {
                    Box = 23;
                    Token = GetToken(_darwin.LogicBoxes[Box]);
                    if (Token == "green") MovePawn(two, 0, one, 1);
                    else MovePawn(two, 1, two, 2);
                    break;
                }
            }
        }

        private void MovePawn(int oldX, int oldY, int newX, int newY) {
            var oldSquare = _boardMatrix[oldY, oldX];
            var newSquare = _boardMatrix[newY, newX];

            newSquare.PawnO = oldSquare.PawnO;
            oldSquare.PawnO = null;
        }

        private bool CheckMatch(List<BoardSquare> squareList, List<Vector2> locations) {
            if (squareList.Count != locations.Count) return false;

            foreach (var square in squareList) {
                var match = false;

                foreach (var location in locations) {
                    if (square.LocationX == (int) location.X && square.LocationY == (int) location.Y) match = true;
                }

                if (!match) return false;
            }

            return true;
        }

        public bool CheckGameOver() {
            if (Box == _previousBox) return true;
            _previousBox = Box;
            return false;
        }

        private string GetToken(LogicBox logicBox) {
            int tokenTotal = logicBox.Green + logicBox.Blue + logicBox.Purple + logicBox.Orange;

            var random = new Random();

            var randomFloat = random.NextDouble() * tokenTotal;

            var fraction = logicBox.Green;

            if (logicBox.Green != 0 && fraction > randomFloat) return "green";

            fraction += logicBox.Blue;

            if (logicBox.Blue != 0 && fraction > randomFloat) return "blue";

            fraction += logicBox.Purple;

            if (logicBox.Purple != 0 && fraction > randomFloat) return "purple";

            return "orange";
        }
    }
}