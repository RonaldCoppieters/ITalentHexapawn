using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn.Controls {
    class MenuButton : Component {
        public MenuButton(Texture2D texture, SpriteFont font, Color fontColor) {
            Texture = texture;

            _spriteFont = font;

            FontColor = fontColor;

            ScaleX = 1f;
            ScaleY = 1f;
        }

        #region Fields

        private readonly SpriteFont _spriteFont;

        public float ScaleX;
        public float ScaleY;

        #endregion

        #region Properties

        public override bool IsSelected { get; set; }
        public override bool IsActivated { get; set; }

        public float Opacity = 1f;
        public bool IsActive { get; set; }

        public string Text { get; set; }

        public Color FontColor { get; set; }

        public Vector2 Position { get; set; }

        public Texture2D Texture { get; }

        public Rectangle Rectangle
            => new Rectangle((int) Position.X, (int) Position.Y, (int) (Texture.Width * ScaleX),
                (int) (Texture.Height * ScaleY));

        #endregion

        #region Methods

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            var color = IsSelected ? Color.DarkGray : Color.White;

            spriteBatch.Draw(Texture, Rectangle, color * Opacity);

            if (string.IsNullOrEmpty(Text)) return;
            var x = (Rectangle.X + (Rectangle.Width / 2)) - (_spriteFont.MeasureString(Text).X / 2);
            var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_spriteFont.MeasureString(Text).Y / 2);

            spriteBatch.DrawString(_spriteFont, Text, new Vector2(x, y), FontColor * Opacity);
        }

        public override void Update(GameTime gameTime) { }

        #endregion
    }
}