using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace NEA3
{
    internal class Gameobject
    {
        public Vector2 Location; //public anyone can see it!
        protected Texture2D Texture;
        private Rectangle tankRectangle;
        public virtual void LoadContent(ContentManager Content)
        {
            tankRectangle = new Rectangle((int)position.X, (int)position.Y, Texture.Width, Texture.Height);
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(Texture,Location,tankRectangle, Color.White);
        }
    }
}
