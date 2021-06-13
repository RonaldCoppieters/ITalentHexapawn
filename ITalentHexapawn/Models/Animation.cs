using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn.Models {
    class Animation : Component {
        public Animation(Texture2D texture, Vector2 position, int frameCount, int framesPerRow) {
            _texture = texture;
            Position = position;

            _frameCount = frameCount;
            _framesPerRow = framesPerRow;

            _frameWidth = texture.Width / framesPerRow;
            _frameHeight = texture.Height / (frameCount / framesPerRow);

            FrameSpeed = .1f;
        }

        private readonly Texture2D _texture;

        private readonly int _frameCount;

        private readonly int _framesPerRow;

        private readonly int _frameWidth;

        private readonly int _frameHeight;

        private int _currentFrame;

        private Rectangle ThisRectangle =>
            new Rectangle(
                (_currentFrame % _framesPerRow) * _frameWidth,
                (int) (_currentFrame / _framesPerRow) * _frameHeight,
                _frameWidth, _frameHeight);

        public float FrameSpeed;

        private float _timer;

        public Vector2 Position { get; set; }

        public bool IsLooping { get; set; }

        public override bool IsSelected { get; set; }
        public override bool IsActivated { get; set; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            if (!IsActivated) return;

            spriteBatch.Draw(_texture, Position, ThisRectangle, Color.White);
        }

        public override void Update(GameTime gameTime) {

            if (!IsActivated) return;

            _timer += (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer < FrameSpeed) return;

            _timer = 0;
            _currentFrame++;

            if (_currentFrame < _frameCount) return;

            if (!IsLooping) IsActivated = false;
            _currentFrame = 0;
        }
    }
}