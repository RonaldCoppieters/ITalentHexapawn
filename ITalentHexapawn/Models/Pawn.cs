using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn.Models {
    class Pawn : Sprite {
        public Pawn(Texture2D texture) : base(texture) { }
        public Pawn(Texture2D texture, Vector2 position) : base(texture, position) { }

        #region Fields

        private bool _pulseIsIncreasing = true;

        #endregion

        #region Properties

        public bool IsPlayer;

        public float PulseMinScale = 1f;
        public float PulseMaxScale = 1.1f;

        #endregion

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            if (IsActivated) {
                if (ScaleX > PulseMaxScale) _pulseIsIncreasing = false;
                if (ScaleX < PulseMinScale) _pulseIsIncreasing = true;

                ScaleX += _pulseIsIncreasing ? .005f : -.005f;
                ScaleY += _pulseIsIncreasing ? .005f : -.005f;
            } else {
                ScaleX = 1f;
                ScaleY = 1f;
            }
        }
    }
}