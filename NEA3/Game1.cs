using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using static NEA3.Game1;
using System.Runtime.Intrinsics.X86;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System;


namespace NEA3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        const int tilesize = 55; // so i remember teh tile size
        //textures for terrain and ui 
        Texture2D squareTexture, grassTexture, treesquaretexture, mountaintexutre, menuTexture, GUIsqauretexture, uparrowtexture, downarrowtexture, leftturntexture, rightturntexture ,selectedtextureHB , selectedtextureMB , selectedtextureLB, selectedtextureHR, selectedtextureMR, selectedtextureLR;
        private Texture2D buttonTexture;
        private SpriteFont myfontyfont;
        private Rectangle buttonRectangle; // square which teh tecture will be put in
        private Rectangle forwardbutton;
        private Rectangle backbutton;
        private Rectangle leftbutton;
        private Rectangle rightbutton;
        //private Rectangle forwardbutton;
        //private Rectangle forwardbutton;
        double gamestate = 1;//shows if playign or meue 
        string menuTitle = "War On Perliculum\n             Prime";
        string Line = "";
        public static int turn = 0; // even = blue odd = red turn
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
        float initialZoom = 0.8f;//sets inital zoom
        Vector2 initialPosition = new Vector2(0, 0); // sets inital potion of camera

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
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
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
            Bheavy = new tank(0, 285, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, true, 1, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
            p1tanks.Add(Bheavy);
            Bmed = new tank(0, 230, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, true, 2, false);
            p1tanks.Add(Bmed);
            Bmed2 = new tank(0, 340, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, true, 3, false);
            p1tanks.Add(Bmed2);
            Blight = new tank(0, 100, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 2, true, 4, false);
            p1tanks.Add(Blight);
            Blight2 = new tank(0,400, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 1, true, 5, false);
            p1tanks.Add(Blight2);
            ////red tanks
            Rheavy = new tank(770, 285, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, false, 1, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
            p2tanks.Add(Rheavy);
            Rmed = new tank(750, 195, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, false, 2, false);
            p2tanks.Add(Rmed);
            Rmed2 = new tank(750, 305, tank.Direction.right, 50, 70, 2, 50, 5, 3, false, 2, false, 3, false);
            p2tanks.Add(Rmed2);
            Rlight = new tank(750, 170, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 2, false, 4, false);
            p2tanks.Add(Rlight);
            Rlight2 = new tank(785, 400, tank.Direction.right, 25, 60, 3, 25, 3, 5, false, 1, false, 5, false);
            p2tanks.Add(Rlight2);
            camera = new Camera(GraphicsDevice.Viewport, initialZoom, initialPosition);
           

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            if (gamestate == 1)
            {

                //tankinstalise(Bheavy, Bmed, Bmed2, Rmed, Rmed2, Blight, Blight2, Rlight, Rlight2, Rheavy, p1tanks, p2tanks);
                grassTexture = Content.Load<Texture2D>("grass");//loads grass 
                treesquaretexture = Content.Load<Texture2D>("tree");// loads tree tile
                mountaintexutre = Content.Load<Texture2D>("maintain");
                GUIsqauretexture = Content.Load<Texture2D>("blacksquare1");
                uparrowtexture = Content.Load<Texture2D>("uparrow");
                downarrowtexture = Content.Load<Texture2D>("downarrow");
                leftturntexture = Content.Load<Texture2D>("leftturn");
                rightturntexture = Content.Load<Texture2D>("rightturn");
                selectedtextureHB = Content.Load<Texture2D>("blueheavyUF");
                selectedtextureHR = Content.Load<Texture2D>("redheavyUF");
                selectedtextureMB = Content.Load<Texture2D>("bluemediumUF");
                selectedtextureMR = Content.Load<Texture2D>("redmediumUF");
                selectedtextureLB = Content.Load<Texture2D>("LightblueUF");
                selectedtextureLR = Content.Load<Texture2D>("LightredUF");
                Vector2 fbposition = new Vector2(710,200);// gives postion for hidden rectangle around buttons
                Vector2 bbposition = new Vector2(710, 250);
                Vector2 lbposition = new Vector2(760, 250);
                Vector2 rbposition = new Vector2(660, 250);
                forwardbutton = new Rectangle((int)fbposition.X, (int)fbposition.Y, uparrowtexture.Width, uparrowtexture.Height);//foward button rectangle
                backbutton = new Rectangle((int)bbposition.X, (int)bbposition.Y, downarrowtexture.Width, downarrowtexture.Height);
                leftbutton = new Rectangle((int)lbposition.X, (int)lbposition.Y, leftturntexture.Width, leftturntexture.Height);
                rightbutton = new Rectangle((int)rbposition.X, (int)rbposition.Y, rightturntexture.Width, rightturntexture.Height);
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
            MouseState mouseState = Mouse.GetState();
            if (gamestate == 0)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && buttonRectangle.Contains(mouseState.Position))
                {
                    gamestate = 1;
                    LoadContent();
                }
            }
            if(gamestate == 1)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && forwardbutton.Contains(mouseState.Position))
                {
                   
                    
                }
                if (mouseState.LeftButton == ButtonState.Pressed && forwardbutton.Contains(mouseState.Position))
                {


                }
                if (mouseState.LeftButton == ButtonState.Pressed && forwardbutton.Contains(mouseState.Position))
                {


                }
                if (mouseState.LeftButton == ButtonState.Pressed && forwardbutton.Contains(mouseState.Position))
                {


                }
                Bheavy.Update(gameTime);
                Bmed.Update(gameTime);
                Bmed2.Update(gameTime);
                Blight.Update(gameTime);
                Blight2.Update(gameTime);
                Rheavy.Update(gameTime);
                Rmed.Update(gameTime);
                Rmed2.Update(gameTime);
                Rlight.Update(gameTime);
                Rlight2.Update(gameTime);
                //selected fun
               
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            const int selx = 710;// where the imagees of the selted tanks will be drawn
            const int sely = 85;
            int col = 0;
            int row = 0;
            if (gamestate == 0)// menu screen  // y = 550 x = 825 area = 453750 pixles 
            {
                GraphicsDevice.Clear(Color.DarkGray);//sets back ground to dark grey
                _spriteBatch.Begin();
                _spriteBatch.Draw(squareTexture, new Vector2(row, col), Color.White);
                Vector2 textMiddlePoint = myfontyfont.MeasureString(menuTitle) / 2;
                // Places text in center of the screen
                Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 - 50);
                _spriteBatch.DrawString(myfontyfont, menuTitle, position, Color.AntiqueWhite, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 1.0f);
                _spriteBatch.Draw(buttonTexture, buttonRectangle, Color.White);//button rectnagle allows for mouse to click
                _spriteBatch.End();
            }

            if (gamestate == 1)
            {
                GraphicsDevice.Clear(Color.DarkGray);
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
                _spriteBatch.End();
                // drawing the tanks probaly an esasier way 
                _spriteBatch.Begin();
                Bheavy.LoadContent(Content);
                Bheavy.Draw(_spriteBatch);
                Rheavy.LoadContent(Content);
                Rheavy.Draw(_spriteBatch);
                Bmed.LoadContent(Content);
                Bmed.Draw(_spriteBatch);
                Rmed.LoadContent(Content);
                Rmed.Draw(_spriteBatch);
                Bmed2.LoadContent(Content);
                Bmed2.Draw(_spriteBatch);
                Rmed2.LoadContent(Content);
                Rmed2.Draw(_spriteBatch);
                Blight.LoadContent(Content);
                Blight.Draw(_spriteBatch);
                Rlight.LoadContent(Content);
                Rlight.Draw(_spriteBatch);
                Blight2.LoadContent(Content);
                Blight2.Draw(_spriteBatch);
                Rlight2.LoadContent(Content);
                Rlight2.Draw(_spriteBatch);
                _spriteBatch.End();
                //UI
                _spriteBatch.Begin();
                _spriteBatch.Draw(GUIsqauretexture, new Vector2(670, 50), Color.White);
                _spriteBatch.Draw(uparrowtexture, forwardbutton,  Color.White);
                _spriteBatch.Draw(downarrowtexture, backbutton,  Color.White);
                _spriteBatch.Draw(leftturntexture,leftbutton,  Color.White);
                _spriteBatch.Draw(rightturntexture,rightbutton,  Color.White);
                _spriteBatch.End();

                _spriteBatch.Begin();
                //selected fun
                if (Bheavy._selected == true)
                {
                    _spriteBatch.Draw(selectedtextureHB, new Vector2(selx, sely), Color.White);
                }
                else if (Rheavy._selected == true)
                {
                    _spriteBatch.Draw(selectedtextureHR, new Vector2(selx, sely), Color.White);
                }
                else if (Bmed._selected == true || Bmed2._selected == true)
                {
                    _spriteBatch.Draw(selectedtextureMB, new Vector2(selx, sely), Color.White);
                }
                else if (Blight._selected == true || Blight2._selected == true)
                {
                    _spriteBatch.Draw(selectedtextureMR, new Vector2(selx, sely), Color.White);
                }
                else if (Rmed._selected == true || Rmed2._selected == true)
                {
                    _spriteBatch.Draw(selectedtextureLB, new Vector2(selx, sely), Color.White);
                }
                else if (Rlight._selected == true || Rlight2._selected == true)
                {
                    _spriteBatch.Draw(selectedtextureLR, new Vector2(selx, sely), Color.White);
                }
                _spriteBatch.End();
            }
            base.Draw(gameTime);
        }
        public void Selecteddraw(GameTime gameTime)
        {

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
