using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Hexapawn.Models {
    class BoardSquare : Sprite {
        public BoardSquare(Texture2D texture) : base(texture) { }
        public BoardSquare(Texture2D texture, Vector2 position) : base(texture, position) { }

        public BoardSquare(Texture2D texture, Vector2 position, int y, int x) : this(texture, position) {
            LocationX = x;
            LocationY = y;
        }

        public Pawn PawnO;

        public int LocationX { get; }
        public int LocationY { get; }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) {
            Opacity = IsSelected ? .3f : 0f;
            Colour = Color.CornflowerBlue;

            base.Draw(gameTime, spriteBatch);

            if (PawnO == null) return;

            if (!PawnO.IsActivated) PawnO.Position = new Vector2(Position.X + 30f, Position.Y + 10f);

            PawnO.Draw(gameTime, spriteBatch);
        }

        public override void Update(GameTime gameTime) {
            base.Update(gameTime);

            PawnO?.Update(gameTime);
        }

        public BoardSquare Clone(ContentManager content) {
            Texture2D pawnTexture;
            Texture2D squareTexture = content.Load<Texture2D>("Textures/Gameboard/Square");
            Pawn clonedPawn;

            var clonedSquare = new BoardSquare(squareTexture, this.Position, this.LocationY, this.LocationX);

            if (this.PawnO == null) return clonedSquare;

            if (!this.PawnO.IsPlayer) pawnTexture = content.Load<Texture2D>("Textures/Gameboard/BlackPawn");
            else pawnTexture = content.Load<Texture2D>("Textures/Gameboard/WhitePawn");
            clonedPawn = new Pawn(pawnTexture) {
                IsPlayer = this.PawnO.IsPlayer,
                Position = this.PawnO.Position
            };
            clonedSquare.PawnO = clonedPawn;

            return clonedSquare;
        }
    }
}