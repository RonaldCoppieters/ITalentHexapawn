using System;
using System.Collections.Generic;
using Hexapawn.Controls;
using Hexapawn.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hexapawn.States {
    class TutorialState : State {
        public TutorialState(HexapawnMain hexapawn, GraphicsDevice graphicsDevice, ContentManager content) :
            base(hexapawn, graphicsDevice, content) {
            var tutorialImages = new List<Texture2D>() {
                _content.Load<Texture2D>("Textures/Gameboard/board3x3"),
                _content.Load<Texture2D>("Textures/Tutorial/Tutorial01"),
                _content.Load<Texture2D>("Textures/Tutorial/Tutorial02"),
                _content.Load<Texture2D>("Textures/Tutorial/Tutorial03"),
                _content.Load<Texture2D>("Textures/Tutorial/Tutorial05"),
                _content.Load<Texture2D>("Textures/Tutorial/Tutorial06")
            };

            var textField = _content.Load<Texture2D>("Shader/FullShader");
            var spriteFont = _content.Load<SpriteFont>("Fonts/Font");

            var arrowLeftControl = _content.Load<Texture2D>("Controls/ArrowLeft");
            var arrowRightControl = _content.Load<Texture2D>("Controls/ArrowRight");

            _tutorialComponents = new List<Component>();

            foreach (var image in tutorialImages) {
                _tutorialComponents.Add(new Sprite(image, new Vector2(380, 160)) {
                    ScaleX = .8f,
                    ScaleY = .8f
                });
            }

            _leftArrowComponent = new Sprite(arrowLeftControl, new Vector2(100, 300));
            _rightArrowComponent = new Sprite(arrowRightControl, new Vector2(925, 300));

            _tooltipComponent = new MenuButton(textField, spriteFont, Color.Black) {
                Position = new Vector2(600, 100),
                ScaleY = 1f,
                ScaleX = 2f
            };

            _pageComponent = new MenuButton(textField, spriteFont, Color.Black) {
                Position = new Vector2(550, 650),
                ScaleY = 1f,
                ScaleX = 2f
            };
        }


        #region Fields

        private readonly List<Component> _tutorialComponents;

        private readonly MenuButton _tooltipComponent;
        private readonly MenuButton _pageComponent;

        private readonly Component _leftArrowComponent;
        private readonly Component _rightArrowComponent;

        private KeyboardState _previousGamePadState;
        private KeyboardState _currentGamePadState;

        private int _listIterator;

        #endregion


        #region Methods

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            _tutorialComponents[_listIterator].Draw(gameTime, spriteBatch);

            _tooltipComponent.Draw(gameTime, spriteBatch);
            _pageComponent.Draw(gameTime, spriteBatch);

            if (_listIterator > 0) _leftArrowComponent.Draw(gameTime, spriteBatch);
            if (_listIterator < _tutorialComponents.Count - 1) _rightArrowComponent.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime) {
            // Remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime) {
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = Keyboard.GetState();

            if (_currentGamePadState.IsKeyUp(Keys.Back) &&
                _previousGamePadState.IsKeyDown(Keys.Back)) {
                _hexapawnMain.ChangeState(new MenuState(_hexapawnMain, _graphicsDevice, _content));
            }


            if (_listIterator > 0 && _currentGamePadState.IsKeyUp(Keys.Left) &&
                _previousGamePadState.IsKeyDown(Keys.Left)) _listIterator--;

            if (_listIterator < _tutorialComponents.Count - 1 &&
                _currentGamePadState.IsKeyUp(Keys.Right) &&
                _previousGamePadState.IsKeyDown(Keys.Right)) _listIterator++;

            _tooltipComponent.Text = GetTooltip();
            _pageComponent.Text = $"Page {_listIterator + 1} / {_tutorialComponents.Count}";


            foreach (var component in _tutorialComponents) {
                component.Update(gameTime);
            }

            _tooltipComponent.Update(gameTime);
            _pageComponent.Update(gameTime);
            _leftArrowComponent.Update(gameTime);
            _rightArrowComponent.Update(gameTime);
        }

        private string GetTooltip() {
            switch (_listIterator) {
                case 0: return "The game is played on a 3x3 board";
                case 1: return "Each player starts with 3 pawns";
                case 2: return "A single pawn can move one square forward each turn";
                case 3: return "Or a pawn can move diagonally to capture an enemy pawn";
                case 4: return "A player wins when one of their pawns reaches the other side";
                case 5: return "Or when they prevent another player from moving";
                default: throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}