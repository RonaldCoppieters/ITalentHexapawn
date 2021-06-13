using System;
using System.Collections.Generic;
using Hexapawn.Controls;
using Hexapawn.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hexapawn.States {
    class MenuState : State {
        public MenuState(HexapawnMain hexapawn, GraphicsDevice graphicsDevice, ContentManager content) :
            base(hexapawn, graphicsDevice, content) {

            var menuButton = _content.Load<Texture2D>("Controls/MenuButton");
            var spriteFont = _content.Load<SpriteFont>("Fonts/Font");

            // Options at the start screen
            _components = new List<Component>() {
                new MenuButton(menuButton, spriteFont, Color.White) {
                    Position = new Vector2(400, 200),
                    Text = "Start",
                    ScaleY = .5f
                },
                new MenuButton(menuButton, spriteFont, Color.White) {
                    Position = new Vector2(400, 300),
                    Text = "Tutorial",
                    ScaleY = .5f
                },
                new MenuButton(menuButton, spriteFont, Color.White) {
                    Position = new Vector2(400, 400),
                    Text = "Darwin",
                    ScaleY = .5f
                },
                new MenuButton(menuButton, spriteFont, Color.White) {
                    Position = new Vector2(400, 500),
                    Text = "Quit",
                    ScaleY = .5f
                }
            };

            _helloFoxAnimation = new Animation(_content.Load<Texture2D>("Textures/Darwin/FoxHello"),
                new Vector2(800, 470), 77, 7);

            _darwinText = new MenuButton(_content.Load<Texture2D>("Textures/Darwin/SpeechBubble"), spriteFont, Color.Black) {
                Text = "Hello Switch2IT!! :)",
                Position = new Vector2(500, 370)
            };
        }

        #region Fields

        private readonly List<Component> _components;

        private KeyboardState _previousGamePadState;
        private KeyboardState _currentGamePadState;

        private int _menuIterator;

        private readonly Animation _helloFoxAnimation;
        private readonly MenuButton _darwinText;

        #endregion


        #region Methods

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

            spriteBatch.Begin();

            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }

            _helloFoxAnimation.Draw(gameTime, spriteBatch);

            if (_helloFoxAnimation.IsActivated) _darwinText.Draw(gameTime, spriteBatch);

            spriteBatch.End();

        }

        public override void PostUpdate(GameTime gameTime) {
            // Remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime) {
            _helloFoxAnimation.Update(gameTime);

            var capabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (capabilities.IsConnected)
            {
                GamePad.GetState(PlayerIndex.One);

                _previousGamePadState = _currentGamePadState;
                _currentGamePadState = Keyboard.GetState();

                if (_currentGamePadState.IsKeyUp(Keys.G) &&
                    _previousGamePadState.IsKeyDown(Keys.G)) {
                    _helloFoxAnimation.IsActivated = true;
                }

                if (_menuIterator > 0)
                {
                    if (_currentGamePadState.IsKeyUp(Keys.Up) &&
                        _previousGamePadState.IsKeyDown(Keys.Up))
                        _menuIterator--;
                }

                if (_menuIterator < _components.Count - 1)
                {
                    if (_currentGamePadState.IsKeyUp(Keys.Down) &&
                        _previousGamePadState.IsKeyDown(Keys.Down))
                        _menuIterator++;
                }

                for (var i = 0; i < _components.Count; i++)
                {
                    _components[i].IsSelected = _menuIterator == i;

                    _components[i].IsActivated =
                        _components[i].IsSelected &&
                        _currentGamePadState.IsKeyUp(Keys.Space) &&
                        _previousGamePadState.IsKeyDown(Keys.Space);

                    _components[i].Update(gameTime);
                }
            } else
            {
                Console.WriteLine("No gamepad found...");
            }

            for (var i = 0; i < _components.Count; i++) {
                if (!_components[i].IsActivated) continue;
                switch (i) {
                    case 0:
                        _hexapawnMain.ChangeState(new LoadState(_hexapawnMain, _graphicsDevice, _content));
                        break;
                    case 1:
                        _hexapawnMain.ChangeState(new TutorialState(_hexapawnMain, _graphicsDevice, _content));
                        break;
                    case 2:
                        _hexapawnMain.ChangeState(new DarwinState(_hexapawnMain, _graphicsDevice, _content));
                        break;
                    case 3:
                        _hexapawnMain.Exit();
                        break;
                    default: throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        #endregion
    }
}