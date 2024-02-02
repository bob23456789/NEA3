using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using static NEA3.Game1;
using System.Runtime.Intrinsics.X86;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;

namespace NEA3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        const int tilesize = 55; // so i rember teh tile size
        //textures for terrain and ui 
        Texture2D squareTexture;
        Texture2D grassTexture;
        Texture2D treesquaretexture;
        Texture2D mountaintexutre;
        Texture2D menuTexture;
        private Texture2D buttonTexture;
        private SpriteFont myfontyfont;
        private Rectangle buttonRectangle; // square which teh tecture will be put in 
        double gamestate = 1;//shows if playign or meue 
        string menuTitle = "War On Perliculum\n             Prime";
        string Line = "";
        // objects
        Camera camera;
        //tank objects
        tank Bheavy;
        tank Bmed;
        tank Bmed2;
        tank Blight;
        tank Blight2;
        tank Rheavy;
        tank Rmed;
        tank Rmed2;
        tank Rlight;
        tank Rlight2;
        //lists
        List<tank> p1tanks = new List<tank>();
        List<tank> p2tanks = new List<tank>();
        //misc
        float initialZoom = 0.7f;//sets inital zoom
        Vector2 initialPosition = new Vector2(-55, 0); // sets inital potion of camera

        public int[,] tilemap =
        {
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0}, //  first 0 is x = 0 & y = 0 15 tiles across
              {0, 0, 0, 1, 1,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 1,1, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 1, 1,1, 1, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 1, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},// x = 0 y= 275
              {0, 0, 0, 0, 0,0, 0, 2, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 2, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 2, 2, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 2, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 2, 0, 0,0,0,0,0,0},// last 0 is x = 825 & y= 550
        };
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //_graphics.IsFullScreen = true;
            //_graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            bool done = false;
            
            camera = new Camera(GraphicsDevice.Viewport, initialZoom, initialPosition);
           //map randomization
            //while(done == false)
            //{
            //    for()
            //}
                //blue tanks
                //Bheavy = new tank(0, 275, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, true, 1, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
                //p1tanks.Add(Bheavy);
                //Bmed = new tank(0, 220, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, true, 2, false);
                //p1tanks.Add(Bmed);
                //Bmed2 = new tank(0, 330, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, true, 3, false);
                //p1tanks.Add(Bmed2);
                //Blight = new tank(0, 170, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 2, true, 4, false);
                //p1tanks.Add(Blight);
                //Blight2 = new tank(0, 280, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 1, true, 5, false);
                //p1tanks.Add(Blight2);
                ////red tanks
                //Rheavy = new tank(825, 275, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, false, 1, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
                //p2tanks.Add(Rheavy);
                //Rmed = new tank(825, 220, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, false, 2, false);
                //p2tanks.Add(Rmed);
                //Rmed2 = new tank(825, 230, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, false, 3, false);
                //p2tanks.Add(Rmed2);
                //Rlight = new tank(825, 170, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 2, false, 4, false);
                //p2tanks.Add(Rlight);
                //Rlight2 = new tank(825, 280, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 1, false, 5, false);
                p2tanks.Add(Rlight2);
                camera = new Camera(GraphicsDevice.Viewport, initialZoom, initialPosition);
           

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            if (gamestate == 1)
            {
                tankinstalise(Bheavy,Bmed,Bmed2,Rmed,Rmed2,Blight,Blight2,Rlight,Rlight2,Rheavy,p1tanks,p2tanks);
                grassTexture = Content.Load<Texture2D>("grass");//loads grass 
                treesquaretexture = Content.Load<Texture2D>("tree");// loads tree tile
                mountaintexutre = Content.Load<Texture2D>("maintain");

            }
            if (gamestate == 0)
            {
                squareTexture = Content.Load<Texture2D>("menuscreen");
                myfontyfont = Content.Load<SpriteFont>("File");
                buttonTexture = Content.Load<Texture2D>("playbutton");
                // Set the initial position and size of the button
                Vector2 position = new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 + 20);

                buttonRectangle = new Rectangle((int)position.X, (int)position.Y, buttonTexture.Width, buttonTexture.Height);
            }
            // TODO: use this.Content to load your game content here
        }
        static void tankinstalise(tank Bheavy ,tank Bmed, tank Bmed2 ,tank Rmed  , tank Rmed2, tank Blight, tank Blight2, tank Rlight, tank Rlight2, tank Rheavy, List<tank> p1tanks, List<tank> p2tanks)
        {
            //blue tanks
            Bheavy = new tank(0, 275, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, true, 1, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
            p1tanks.Add(Bheavy);
            Bmed = new tank(0, 220, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, true, 2, false);
            p1tanks.Add(Bmed);
            Bmed2 = new tank(0, 330, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, true, 3, false);
            p1tanks.Add(Bmed2);
            Blight = new tank(0, 170, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 2, true, 4, false);
            p1tanks.Add(Blight);
            Blight2 = new tank(0, 280, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 1, true, 5, false);
            p1tanks.Add(Blight2);
            //red tanks
            Rheavy = new tank(825, 275, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, false, 1, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
            p2tanks.Add(Rheavy);
            Rmed = new tank(825, 220, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, false, 2, false);
            p2tanks.Add(Rmed);
            Rmed2 = new tank(825, 230, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, false, 3, false);
            p2tanks.Add(Rmed2);
            Rlight = new tank(825, 170, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 2, false, 4, false);
            p2tanks.Add(Rlight);
            Rlight2 = new tank(825, 280, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 1, false, 5, false);
            p2tanks.Add(Rlight2);
            
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && buttonRectangle.Contains(mouseState.Position))
            {
                gamestate = 1;
                LoadContent();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);//set background to black
            int col = 0;
            int row = 0;
            if (gamestate == 0)// menu screen  // y = 550 x = 825 area = 453750 pixles 
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(squareTexture, new Vector2(row, col), Color.White);
                Vector2 textMiddlePoint = myfontyfont.MeasureString(menuTitle) / 2;
                // Places text in center of the screen
                Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 - 50);
                _spriteBatch.DrawString(myfontyfont, menuTitle, position, Color.AntiqueWhite, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 1.0f);
                _spriteBatch.Draw(buttonTexture, buttonRectangle, Color.White);
                _spriteBatch.End();
            }

            if (gamestate == 1)
            {
                Initialize();
                _spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());// begins draws  in the srpites + sets the zoom
                for (int y = 0; y < tilemap.GetLength(0); y++)
                {

                    row = 0;
                    for (int x = 0; x < tilemap.GetLength(1); x++)
                    {
                        if (tilemap[y, x] == 0)
                        {
                            _spriteBatch.Draw(grassTexture, new Vector2(row, col), Color.White);
                        }
                        if (tilemap[y, x] == 1)
                        {
                            _spriteBatch.Draw(treesquaretexture, new Vector2(row, col), Color.White);
                        }
                        if (tilemap[y, x] == 2)
                        {
                            _spriteBatch.Draw(mountaintexutre, new Vector2(row, col), Color.White);
                        }


                        row += 55;
                    }
                    col += 55;
                }
                // drawing the tanks probaly an esasier way 
                Bheavy.Draw(_spriteBatch);
                Rheavy.Draw(_spriteBatch);
                Bmed.Draw(_spriteBatch);
                Rmed.Draw(_spriteBatch);
                Bmed2.Draw(_spriteBatch);
                Rmed2.Draw(_spriteBatch);
                Blight.Draw(_spriteBatch);
                Rlight.Draw(_spriteBatch);
                Blight2.Draw(_spriteBatch);
                Rlight2.Draw(_spriteBatch);
                _spriteBatch.End();


                base.Draw(gameTime);
            }
        }
        public class Camera
        {
            public Matrix Transform { get; private set; }
            private Viewport viewport;
            private float initialZoom;
            private Vector2 initialPosition;

            public Camera(Viewport viewport, float initialZoom, Vector2 initialPosition)
            {
                this.viewport = viewport;
                this.initialZoom = initialZoom;
                this.initialPosition = initialPosition;
                // Initialize the camera transformation with the initial zoom
                Transform = Matrix.CreateScale(initialZoom) * Matrix.CreateTranslation(new Vector3(-initialPosition, 0));
            }

            public Matrix GetViewMatrix()
            {
                return Transform;
            }
        }
    }
}
