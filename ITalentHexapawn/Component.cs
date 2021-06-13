using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn {
    abstract class Component {
        public abstract bool IsSelected { get; set; }

        public abstract bool IsActivated { get; set; }

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
    }
}