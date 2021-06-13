using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn.States {
    public abstract class State {
        public State(HexapawnMain hexapawn, GraphicsDevice graphicsDevice, ContentManager content) {
            _hexapawnMain = hexapawn;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        #region Fields

        protected ContentManager _content;

        protected GraphicsDevice _graphicsDevice;

        protected HexapawnMain _hexapawnMain;

        #endregion

        #region Methods

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        public abstract void PostUpdate(GameTime gameTime);

        public abstract void Update(GameTime gameTime);

        #endregion

    }
}