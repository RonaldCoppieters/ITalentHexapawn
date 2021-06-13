using System;
using Hexapawn.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hexapawn.States {
    class DarwinState : State {
        public DarwinState(HexapawnMain hexapawn, GraphicsDevice graphicsDevice, ContentManager content) :
            base(hexapawn, graphicsDevice, content) {
            _boxes1 = new Sprite(_content.Load<Texture2D>("Textures/Darwin/Boxes01"), new Vector2(250, 25)) {
                ScaleY = .9f
            };
            _boxes2 = new Sprite(_content.Load<Texture2D>("Textures/Darwin/Boxes02"), new Vector2(250, 25)) {
                ScaleY = .9f
            };

            _arrowLeft = new Sprite(_content.Load<Texture2D>("Controls/ArrowLeft"), new Vector2(50, 300));
            _arrowRight = new Sprite(_content.Load<Texture2D>("Controls/ArrowRight"), new Vector2(1080, 300));
        }

        #region Fields

        private readonly Component _boxes1;
        private readonly Component _boxes2;

        private readonly Component _arrowLeft;
        private readonly Component _arrowRight;

        private KeyboardState _previousGamePadState;
        private KeyboardState _currentGamePadState;

        private int _pageIterator;

        #endregion


        #region Methods

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Begin();

            switch (_pageIterator) {
                case 0:
                    _boxes1.Draw(gameTime, spriteBatch);
                    _arrowRight.Draw(gameTime, spriteBatch);
                    break;
                case 1:
                    _boxes2.Draw(gameTime, spriteBatch);
                    _arrowLeft.Draw(gameTime, spriteBatch);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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

            if (_pageIterator > 0 &&
                _currentGamePadState.IsKeyUp(Keys.Left) &&
                _previousGamePadState.IsKeyDown(Keys.Left))
                _pageIterator--;

            if (_pageIterator < 1 &&
                _currentGamePadState.IsKeyUp(Keys.Right) &&
                _previousGamePadState.IsKeyDown(Keys.Right))
                _pageIterator++;


            _boxes1.Update(gameTime);
            _boxes2.Update(gameTime);

            _arrowRight.Update(gameTime);
            _arrowLeft.Update(gameTime);
        }

        #endregion
    }
}