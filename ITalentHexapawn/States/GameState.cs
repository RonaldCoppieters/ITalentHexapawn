using System;
using System.Collections.Generic;
using Hexapawn.AI;
using Hexapawn.Controls;
using Hexapawn.Manager;
using Hexapawn.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hexapawn.States {
    class GameState : State {
        public GameState(HexapawnMain hexapawn, GraphicsDevice graphicsDevice, ContentManager content) : base(hexapawn,
            graphicsDevice, content) {
            var gameBoardImg = _content.Load<Texture2D>("Textures/Gameboard/board3x3");
            var textFieldImg = _content.Load<Texture2D>("Controls/TextField");
            var spriteFont = _content.Load<SpriteFont>("Fonts/Font");

            _components = new List<Component>() {
                new Sprite(gameBoardImg, new Vector2(350, 100)),
                new MenuButton(textFieldImg, spriteFont, Color.White) {
                    Text = "Round",
                    Position = new Vector2(50, 100),
                    ScaleY = .5f,
                    ScaleX = .5f
                }
            };

            _roundSprites = new List<Sprite>() {
                new Sprite(_content.Load<Texture2D>("Fonts/1")),
                new Sprite(_content.Load<Texture2D>("Fonts/2")),
                new Sprite(_content.Load<Texture2D>("Fonts/3")),
                new Sprite(_content.Load<Texture2D>("Fonts/4")),
                new Sprite(_content.Load<Texture2D>("Fonts/5")),
                new Sprite(_content.Load<Texture2D>("Fonts/6")),
                new Sprite(_content.Load<Texture2D>("Fonts/7")),
                new Sprite(_content.Load<Texture2D>("Fonts/8"))
            };

            _boxSprites = new List<Sprite>() {
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/0")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/1")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/2")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/3")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/4")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/5")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/6")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/7")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/8")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/9")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/10")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/11")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/12")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/13")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/14")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/15")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/16")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/17")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/18")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/19")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/20")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/21")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/22")),
                new Sprite(_content.Load<Texture2D>("Textures/Darwin/23"))
            };

            _boneButtons = new List<MenuButton>() {
                new MenuButton(_content.Load<Texture2D>("Textures/Darwin/BoneGreen"), spriteFont,
                    Color.White) {
                    Position = new Vector2(910, 100),
                    ScaleX = .4f,
                    ScaleY = .4f
                },
                new MenuButton(_content.Load<Texture2D>("Textures/Darwin/BoneBlue"), spriteFont,
                    Color.White) {
                    Position = new Vector2(910, 200),
                    ScaleX = .4f,
                    ScaleY = .4f
                },
                new MenuButton(_content.Load<Texture2D>("Textures/Darwin/BonePurple"), spriteFont,
                    Color.White) {
                    Position = new Vector2(1040, 100),
                    ScaleX = .4f,
                    ScaleY = .4f
                },
                new MenuButton(_content.Load<Texture2D>("Textures/Darwin/BoneOrange"), spriteFont,
                    Color.White) {
                    Position = new Vector2(1040, 200),
                    ScaleX = .4f,
                    ScaleY = .4f
                }
            };

            foreach (var sprite in _roundSprites) {
                sprite.Position = new Vector2(100, 220);
                sprite.Position = new Vector2(100, 220);
            }

            _boardMatrix = new BoardSquare[3, 3];
            _darwinAI = new DarwinAI(_hexapawnMain.Darwins[_hexapawnMain.LoadedFile], _boardMatrix);


            _winButton = new MenuButton(
                _content.Load<Texture2D>("Controls/WinButton"), spriteFont, Color.Black) {
                Position = new Vector2(400, 150),
                Text = "You win!"
            };

            _loseButton = new MenuButton(
                _content.Load<Texture2D>("Controls/LoseButton"), spriteFont, Color.Black) {
                Position = new Vector2(400, 150),
                Text = "You lose!"
            };

            _gameOver = false;
            _playerVictory = true;
            _boxMatch = false;

            _helloFoxAnimation = new Animation(_content.Load<Texture2D>("Textures/Darwin/FoxHello"),
                new Vector2(800, 470), 77, 7);

            _darwinText = new MenuButton(_content.Load<Texture2D>("Textures/Darwin/SpeechBubble"), spriteFont,
                Color.Black) {
                Position = new Vector2(500, 370)
            };

            InitializeBoard();
        }

        #region Fields

        private KeyboardState _previousGamePadState;
        private KeyboardState _currentGamePadState;

        private readonly List<Component> _components;
        private readonly List<MenuButton> _boneButtons;
        private readonly List<Sprite> _roundSprites;
        private readonly List<Sprite> _boxSprites;

        private readonly Component _loseButton;
        private readonly Component _winButton;

        private Pawn _randomBone;

        private int _boardIteratorX = 1;
        private int _boardIteratorY = 1;
        private int _round;
        private int _lastFullBox;
        private int _boxDisplayPhase;

        private string _lastFullBoXToken;

        private bool _moveResolved;
        private bool _gameOver;
        private bool _playerVictory;
        private bool _boxMatch;

        private BoardSquare[,] _previousBoardMatrix;
        private readonly BoardSquare[,] _boardMatrix;

        private BoardSquare _selectedSquare;

        private readonly DarwinAI _darwinAI;

        private readonly Animation _helloFoxAnimation;
        private readonly MenuButton _darwinText;

        #endregion

        private List<Pawn> NewPawnList(bool isPlayer) {
            List<Pawn> pawnList = new List<Pawn>();

            if (isPlayer) {
                var whitePawnImg = _content.Load<Texture2D>("Textures/Gameboard/WhitePawn");

                pawnList.Add(new Pawn(whitePawnImg) {IsPlayer = true});
                pawnList.Add(new Pawn(whitePawnImg) {IsPlayer = true});
                pawnList.Add(new Pawn(whitePawnImg) {IsPlayer = true});
            }

            if (!isPlayer) {
                var blackPawnImg = _content.Load<Texture2D>("Textures/Gameboard/BlackPawn");

                pawnList.Add(new Pawn(blackPawnImg) {IsPlayer = false});
                pawnList.Add(new Pawn(blackPawnImg) {IsPlayer = false});
                pawnList.Add(new Pawn(blackPawnImg) {IsPlayer = false});
            }

            return pawnList;
        }

        private void InitializeBoard() {
            var squareImg = _content.Load<Texture2D>("Textures/Gameboard/Square");

            var pawnListWhite = NewPawnList(true);
            var pawnListBlack = NewPawnList(false);

            _boardMatrix[0, 0] = new BoardSquare(squareImg, new Vector2(455, 205), 0, 0) {
                PawnO = pawnListBlack[0]
            };
            _boardMatrix[0, 1] = new BoardSquare(squareImg, new Vector2(566, 205), 0, 1) {
                PawnO = pawnListBlack[1]
            };
            _boardMatrix[0, 2] = new BoardSquare(squareImg, new Vector2(678, 205), 0, 2) {
                PawnO = pawnListBlack[2]
            };
            _boardMatrix[1, 0] = new BoardSquare(squareImg, new Vector2(455, 315), 1, 0);
            _boardMatrix[1, 1] = new BoardSquare(squareImg, new Vector2(566, 315), 1, 1);
            _boardMatrix[1, 2] = new BoardSquare(squareImg, new Vector2(678, 315), 1, 2);
            _boardMatrix[2, 0] = new BoardSquare(squareImg, new Vector2(455, 427), 2, 0) {
                PawnO = pawnListWhite[0]
            };
            _boardMatrix[2, 1] = new BoardSquare(squareImg, new Vector2(566, 427), 2, 1) {
                PawnO = pawnListWhite[1]
            };
            _boardMatrix[2, 2] = new BoardSquare(squareImg, new Vector2(678, 427), 2, 2) {
                PawnO = pawnListWhite[2]
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            foreach (var component in _components) {
                component.Draw(gameTime, spriteBatch);
            }

            _roundSprites[_round].Draw(gameTime, spriteBatch);

            if (!_boxMatch) {
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        _boardMatrix[i, j].IsSelected = _boardIteratorX == j &&
                                                        _boardIteratorY == i;

                        _boardMatrix[i, j].Draw(gameTime, spriteBatch);
                    }
                }
            } else {
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        _previousBoardMatrix[i, j].Draw(gameTime, spriteBatch);
                    }
                }

                _helloFoxAnimation.Draw(gameTime, spriteBatch);
                if (_helloFoxAnimation.IsActivated) _darwinText.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            if (_boxDisplayPhase > 0) {
                spriteBatch.Begin();

                _boxSprites[_darwinAI.Box].Draw(gameTime, spriteBatch);
                foreach (var bone in _boneButtons) {
                    bone.Draw(gameTime, spriteBatch);
                }

                _randomBone.Draw(gameTime, spriteBatch);

                spriteBatch.End();
            }

            if (!_gameOver) return;
            if (_boxDisplayPhase < 2) return;
            if (_boxMatch) return;

            spriteBatch.Begin();

            if (_playerVictory) _winButton.Draw(gameTime, spriteBatch);
            else _loseButton.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime) { }

        public override void Update(GameTime gameTime) {
            _helloFoxAnimation.Update(gameTime);

            if (_boxMatch) {
                GetBoxToDisplay();
                return;
            }

            if (_gameOver || _round > 8) {
                GameOverActions();

                return;
            }

            if (_round % 2 == 0) PlayerActions();
            else DarwinActions();

            if (_selectedSquare != null) {
                var selectedBoardSquare = _boardMatrix[_boardIteratorY, _boardIteratorX].Position;
                _selectedSquare.PawnO.Position = new Vector2(selectedBoardSquare.X + 30f, selectedBoardSquare.Y + 10f);
            }

            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    _boardMatrix[i, j].Update(gameTime);
                }
            }
        }

        private void SetPreviousBoard() {
            var oldBoardMatrix = new BoardSquare[3, 3];
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    oldBoardMatrix[i, j] = _boardMatrix[i, j].Clone(_content);
                }
            }

            _previousBoardMatrix = oldBoardMatrix;
        }

        private void GetBoxToDisplay() {
            var currentToken = _darwinAI.Token;
            var currentBoxNumber = _darwinAI.Box;
            var currentBox = _darwinAI.ThisDarwin.LogicBoxes[currentBoxNumber];

            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = Keyboard.GetState();


            if (_currentGamePadState.IsKeyUp(Keys.Space) &&
                _previousGamePadState.IsKeyDown(Keys.Space) &&
                _boxDisplayPhase == 2) {
                _boxDisplayPhase = _gameOver ? 3 : 0;
                _boxMatch = false;
            }

            if (_currentGamePadState.IsKeyUp(Keys.Space) &&
                _previousGamePadState.IsKeyDown(Keys.Space) &&
                _boxDisplayPhase == 1) {
                _boxSprites[currentBoxNumber].Opacity = 1f;
                foreach (var bone in _boneButtons) {
                    bone.Opacity = 1f;
                }

                _boxDisplayPhase++;
            }


            if (_boxDisplayPhase == 2) {
                _randomBone.Opacity += .005f;
            }

            if (_boxDisplayPhase == 1) {
                _boxSprites[currentBoxNumber].Opacity += .005f;
                foreach (var bone in _boneButtons) {
                    bone.Opacity += .005f;
                }
            }

            // Initialization of Displayphase
            if (_boxDisplayPhase != 0 || !_boxMatch) return;

            _boneButtons[0].Text = currentBox.Green.ToString();
            _boneButtons[1].Text = currentBox.Blue.ToString();
            _boneButtons[2].Text = currentBox.Purple.ToString();
            _boneButtons[3].Text = currentBox.Orange.ToString();

            foreach (var bone in _boneButtons) {
                bone.Opacity = 0f;
            }

            _boxSprites[currentBoxNumber].Opacity = 0f;
            _boxSprites[currentBoxNumber].Position = new Vector2(950, 300);

            switch (currentToken) {
                case "green":
                    _randomBone = new Pawn(_content.Load<Texture2D>("Textures/Darwin/BoneGreen"),
                        new Vector2(970, 550));
                    break;
                case "blue":
                    _randomBone = new Pawn(_content.Load<Texture2D>("Textures/Darwin/BoneBlue"), new Vector2(970, 550));
                    break;
                case "purple":
                    _randomBone = new Pawn(_content.Load<Texture2D>("Textures/Darwin/BonePurple"),
                        new Vector2(970, 550));
                    break;
                default:
                    _randomBone = new Pawn(_content.Load<Texture2D>("Textures/Darwin/BoneOrange"),
                        new Vector2(970, 550));
                    break;
            }

            _randomBone.Opacity = 0f;
            _randomBone.IsSelected = true;
            _randomBone.ScaleY = .5f;
            _randomBone.ScaleX = .5f;

            _boxDisplayPhase++;

            _helloFoxAnimation.IsActivated = true;

            Random random = new Random();

            var randNum = random.Next(0, 11);

            switch (randNum) {
                case 0:
                    _darwinText.Text = "Is this it?";
                    break;
                case 1:
                    _darwinText.Text = "This looks right!";
                    break;
                case 2:
                    _darwinText.Text = "Found it!!";
                    break;
                case 3:
                    _darwinText.Text = "Let's see...";
                    break;
                case 4:
                    _darwinText.Text = "Here you go.";
                    break;
                case 5:
                    _darwinText.Text = "You're cute!";
                    break;
                case 6:
                    _darwinText.Text = "These are heavy.";
                    break;
                case 7:
                    _darwinText.Text = "Play with me!";
                    break;
                case 8:
                    _darwinText.Text = "I love this game!";
                    break;
                case 9:
                    _darwinText.Text = "Let me think...";
                    break;
                case 10:
                    _darwinText.Text = "Do i get a treat?";
                    break;
            }
        }

        public void DarwinActions() {
            SetPreviousBoard();
            _darwinAI.DetermineMove();

            var currentToken = _darwinAI.Token;
            var currentBoxNumber = _darwinAI.Box;
            var currentBox = _darwinAI.ThisDarwin.LogicBoxes[currentBoxNumber];

            if (!(currentBox.Green == 0 &&
                  currentBox.Blue == 0 &&
                  currentBox.Purple == 0 &&
                  currentBox.Orange == 0)) {
                _lastFullBox = currentBoxNumber;
                _lastFullBoXToken = currentToken;
            }

            _boxMatch = true;
            _gameOver = _darwinAI.CheckGameOver();

            if (_gameOver) return;

            _round++;
            _gameOver = CheckPlayerGameOver();
        }

        private void PlayerActions() {
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = Keyboard.GetState();

            if (_currentGamePadState.IsKeyUp(Keys.Left) &&
                _previousGamePadState.IsKeyDown(Keys.Left) &&
                _boardIteratorX > 0) _boardIteratorX--;

            if (_currentGamePadState.IsKeyUp(Keys.Right) &&
                _previousGamePadState.IsKeyDown(Keys.Right) &&
                _boardIteratorX < 2) _boardIteratorX++;

            if (_currentGamePadState.IsKeyUp(Keys.Up) &&
                _previousGamePadState.IsKeyDown(Keys.Up) &&
                _boardIteratorY > 0) _boardIteratorY--;

            if (_currentGamePadState.IsKeyUp(Keys.Down) &&
                _previousGamePadState.IsKeyDown(Keys.Down) &&
                _boardIteratorY < 2) _boardIteratorY++;

            if (_currentGamePadState.IsKeyUp(Keys.Space) &&
                _previousGamePadState.IsKeyDown(Keys.Space)) {
                var selectedSquare = _boardMatrix[_boardIteratorY, _boardIteratorX];
                if (_selectedSquare != null) VerifyMove();

                if (selectedSquare.PawnO != null &&
                    selectedSquare.PawnO.IsPlayer &&
                    _selectedSquare == null &&
                    _moveResolved == false) {
                    selectedSquare.PawnO.IsActivated = true;
                    _selectedSquare = selectedSquare;
                }

                _moveResolved = false;
            }

            if (_currentGamePadState.IsKeyUp(Keys.Back) &&
                _previousGamePadState.IsKeyDown(Keys.Back)) {
                if (_selectedSquare != null) {
                    _selectedSquare.PawnO.IsActivated = false;
                    _selectedSquare = null;
                }
            }
        }

        private void VerifyMove() {
            var newSquare = _boardMatrix[_boardIteratorY, _boardIteratorX];

            // Pawn just gets dropped on the same square
            // Pawn can be placed one square forwards
            if (_selectedSquare == newSquare) {
                _selectedSquare.PawnO.IsActivated = false;
                _selectedSquare = null;
                _moveResolved = true;
            } else if (newSquare.LocationX == _selectedSquare.LocationX &&
                       newSquare.LocationY == _selectedSquare.LocationY - 1 &&
                       newSquare.PawnO == null) {
                _selectedSquare.PawnO.IsActivated = false;
                newSquare.PawnO = _selectedSquare.PawnO;
                _selectedSquare.PawnO = null;
                _selectedSquare = null;
                _round++;
                _moveResolved = true;
            } else if ((newSquare.LocationX == _selectedSquare.LocationX - 1 ||
                        newSquare.LocationX == _selectedSquare.LocationX + 1) &&
                       newSquare.LocationY == _selectedSquare.LocationY - 1 &&
                       newSquare.PawnO != null &&
                       !newSquare.PawnO.IsPlayer) {
                _selectedSquare.PawnO.IsActivated = false;
                newSquare.PawnO = _selectedSquare.PawnO;
                _selectedSquare.PawnO = null;
                _selectedSquare = null;
                _round++;
                _moveResolved = true;
            }
        }

        private bool CheckPlayerGameOver() {
            // Player loses when black reaches backrow
            var backRow = new List<BoardSquare>() {
                _boardMatrix[2, 0],
                _boardMatrix[2, 1],
                _boardMatrix[2, 2]
            };

            foreach (var square in backRow) {
                if (square.PawnO != null && !square.PawnO.IsPlayer) {
                    _playerVictory = false;
                    return true;
                }
            }

            // Or when the player can't move
            var whitePawnSquareList = new List<BoardSquare>();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    var square = _boardMatrix[i, j];
                    if (square.PawnO == null) continue;
                    if (square.PawnO.IsPlayer) whitePawnSquareList.Add(square);
                }
            }

            foreach (var square in whitePawnSquareList) {
                // has a pawn that can move forward
                var squareForwardY = square.LocationY - 1;
                if (squareForwardY > 3) continue;
                if (_boardMatrix[squareForwardY, square.LocationX].PawnO == null) {
                    return false;
                }

                // has a pawn that can capture diagonally
                var diagonalLeft = new Vector2(square.LocationX - 1, square.LocationY - 1);
                var diagonalRight = new Vector2(square.LocationX + 1, square.LocationY + 1);

                if (diagonalLeft.X > 0 && diagonalLeft.Y < 3) {
                    if (_boardMatrix[(int) diagonalLeft.Y, (int) diagonalLeft.X].PawnO != null &&
                        !_boardMatrix[(int) diagonalLeft.Y, (int) diagonalLeft.X].PawnO.IsPlayer) return false;
                }

                if (diagonalRight.X < 2 && diagonalRight.Y < 3) {
                    if (_boardMatrix[(int) diagonalRight.Y, (int) diagonalRight.X].PawnO != null &&
                        !_boardMatrix[(int) diagonalRight.Y, (int) diagonalRight.X].PawnO.IsPlayer) return false;
                }
            }

            _playerVictory = false;
            return true;
        }

        private void GameOverActions() {
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = Keyboard.GetState();

            if (_currentGamePadState.IsKeyUp(Keys.Space) &&
                _previousGamePadState.IsKeyDown(Keys.Space)) {
                var darwin = _darwinAI.ThisDarwin;
                var boxNumber = _darwinAI.Box;
                var logicBox = darwin.LogicBoxes[boxNumber];
                var token = _darwinAI.Token;

                if (logicBox.Green == 0 &&
                    logicBox.Blue == 0 &&
                    logicBox.Purple == 0 &&
                    logicBox.Orange == 0) {
                    boxNumber = _lastFullBox;
                    logicBox = darwin.LogicBoxes[boxNumber];
                    token = _lastFullBoXToken;
                }


                var count = 0;

                switch (token) {
                    case "green":
                        logicBox.Green += _playerVictory ? -1 : 1;
                        count = logicBox.Green;
                        break;
                    case "blue":
                        logicBox.Blue += _playerVictory ? -1 : 1;
                        count = logicBox.Blue;
                        break;
                    case "purple":
                        logicBox.Purple += _playerVictory ? -1 : 1;
                        count = logicBox.Purple;
                        break;
                    case "orange":
                        logicBox.Orange += _playerVictory ? -1 : 1;
                        count = logicBox.Orange;
                        break;
                }

                Console.WriteLine($"BOX {boxNumber}, color={token}, count={count}");

                darwin.level++;

                DarwinManager.Save(new DarwinManager(_hexapawnMain.Darwins));
                _hexapawnMain.ChangeState(new MenuState(_hexapawnMain, _graphicsDevice, _content));
            }
        }
    }
}