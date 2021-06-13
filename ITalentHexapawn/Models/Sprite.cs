using System;
using System.Collections.Generic;
using System.Linq;
using Hexapawn.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn.Models {
    class Sprite : Component {
        public Sprite(Texture2D texture) {
            _texture = texture;
            ScaleX = 1f;
            ScaleY = 1f;
        }

        public Sprite(Texture2D texture, Vector2 position) {
            _texture = texture;
            Position = position;
            ScaleX = 1f;
            ScaleY = 1f;
        }

        #region Fields

        private readonly Texture2D _texture;

        #endregion

        #region Properies

        public float ScaleX;
        public float ScaleY;
        public float Opacity = 1f;

        public Color Colour = Color.White;

        public Vector2 Position { get; set; }

        public override bool IsSelected { get; set; }
        public override bool IsActivated { get; set; }

        #endregion

        #region Methods

        public Rectangle Rectangle => new Rectangle((int) Position.X, (int) Position.Y, (int) (_texture.Width * ScaleX),
            (int) (_texture.Height * ScaleY));

        public Vector2 Center => new Vector2(Position.X + _texture.Width / 2f, Position.Y + _texture.Height / 2f);

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            spriteBatch.Draw(_texture, Rectangle, Colour * Opacity);
        }

        public override void Update(GameTime gameTime) { }

        #endregion
    }
}