using System;
using System.Collections.Generic;
using Hexapawn.Controls;
using Hexapawn.Manager;
using Hexapawn.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hexapawn.States {
    class LoadState : State {
        public LoadState(HexapawnMain hexapawn, GraphicsDevice graphicsDevice, ContentManager content) : base(hexapawn,
            graphicsDevice, content) {
            var spriteFont = _content.Load<SpriteFont>("Fonts/Font");

            var loadButton1 = _content.Load<Texture2D>("Controls/LoadButton01");
            var loadButton2 = _content.Load<Texture2D>("Controls/LoadButton02");
            var loadButton3 = _content.Load<Texture2D>("Controls/LoadButton03");

            _loadComponents = new List<MenuButton>() {
                new MenuButton(loadButton1, spriteFont, Color.White) {
                    ScaleY = .6f,
                    ScaleX = .8f,
                    FontColor = Color.Black,
                    Position = new Vector2((graphicsDevice.Viewport.Width - loadButton1.Width) / 2f,
                        (graphicsDevice.Viewport.Height - loadButton1.Height) / 2f - 175)
                },
                new MenuButton(loadButton2, spriteFont, Color.White) {
                    ScaleY = .6f,
                    ScaleX = .8f,
                    FontColor = Color.Black,
                    Position = new Vector2((graphicsDevice.Viewport.Width - loadButton2.Width) / 2f,
                        (graphicsDevice.Viewport.Height - loadButton2.Height) / 2f + 50)
                },
                new MenuButton(loadButton3, spriteFont, Color.White) {
                    ScaleY = .6f,
                    ScaleX = .8f,
                    FontColor = Color.Black,
                    Position = new Vector2((graphicsDevice.Viewport.Width - loadButton3.Width) / 2f,
                        (graphicsDevice.Viewport.Height - loadButton3.Height) / 2f + 275)
                },
            };

            _darwins = DarwinManager.Load().Darwins;
        }

        #region Fields

        private readonly List<MenuButton> _loadComponents;

        private KeyboardState _previousGamePadState;
        private KeyboardState _currentGamePadState;

        private List<Darwin> _darwins;

        private int _menuIterator;

        #endregion

        #region Properties

        #endregion

        #region Methods

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            foreach (var component in _loadComponents) {
                component.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime) {
            // Remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime) {
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = Keyboard.GetState();


            if (_currentGamePadState.IsKeyUp(Keys.Back) &&
                _previousGamePadState.IsKeyDown(Keys.Back))
                _hexapawnMain.ChangeState(new MenuState(_hexapawnMain, _graphicsDevice, _content));

            if (_currentGamePadState.IsKeyUp(Keys.Up) &&
                _previousGamePadState.IsKeyDown(Keys.Up) &&
                _menuIterator > 0)
                _menuIterator--;

            if (_currentGamePadState.IsKeyUp(Keys.Down) &&
                _previousGamePadState.IsKeyDown(Keys.Down) &&
                _menuIterator < _loadComponents.Count - 1)
                _menuIterator++;

            if (_currentGamePadState.IsKeyUp(Keys.Space) &&
                _previousGamePadState.IsKeyDown(Keys.Space)) {
                _hexapawnMain.Darwins = _darwins;
                _hexapawnMain.LoadedFile = _menuIterator;
                _hexapawnMain.ChangeState(new GameState(_hexapawnMain, _graphicsDevice, _content));
            }


            for (var i = 0; i < _loadComponents.Count; i++) {
                _loadComponents[i].IsSelected = _menuIterator == i;
                _loadComponents[i].Text = $"Darwin level {_darwins[i].level}";
            }

            foreach (var component in _loadComponents) {
                component.Update(gameTime);
            }
        }

        #endregion
    }
}