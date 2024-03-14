using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using static NEA3.Game1;
using System.Runtime.Intrinsics.X86;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
 


namespace NEA3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        const int tilesize = 55; // so i remember teh tile size
        //textures for terrain and ui 
        Texture2D squareTexture, grassTexture, treesquaretexture, mountaintexutre, menuTexture, GUIsqauretexture, uparrowtexture, downarrowtexture, leftturntexture, rightturntexture ,selectedtextureHB , selectedtextureMB , selectedtextureLB, selectedtextureHR, selectedtextureMR, selectedtextureLR, endturnbuttexture,rangefindertexture;
        private Texture2D buttonTexture;
        private SpriteFont myfontyfont;
        private Rectangle buttonRectangle ,endturnbutton,rangefinderbutton; // square which teh tecture will be put in
        private Rectangle forwardbutton;
        private Rectangle backbutton;
        private Rectangle leftbutton;
        private Rectangle rightbutton;
        //private Rectangle forwardbutton;
       public enum gamestate
        {
            menue,
            loading,//intemediate state been plain and menue screen 
            playing,
            victory,
        }
        gamestate currentgamestate = gamestate.menue;
        string menuTitle = "War On Perliculum\n             Prime";
        string turncountwords = "Turn " + turn;//for the tun fon abve the black square 
        string Line = "";
        string bluevic = "VICTORY\n     Blue team wins";
        string redvic = "VICTORY\n     Red team wins";
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
        public static int p1tankleft = 0;
        public static int p2tankleft = 0;
        //misc
        bool bvic = false;
        bool rvic = false;
        bool rangecheck = false;
        float initialZoom = 0.8f;//sets inital zoom
        Vector2 initialPosition = new Vector2(0, 0); // sets initial position of camera
        static Random R = new Random();

        public int[,] tilemap =
        {
            //blank slate for teh amp to be generated
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0}, //  first 0 is x = 0 & y = 0 15 tiles across not exact coord x = 15 tiles  y = 11
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},// x = 0 y= 275
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},
              {0, 0, 0, 0, 0,0, 0, 0, 0, 0,0,0,0,0,0},// last 0 is x = 825 & y= 550
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
            camera = new Camera(GraphicsDevice.Viewport, initialZoom, initialPosition);
            if (currentgamestate == gamestate.menue)
            {
                bool done = false;
                int genplacemnetx = 0;// for randoml picin x oord
                int genplacemnety = 0;//dito for y cords
                int treetilecount = 0;//counts how many tree tiles
                const int treetilemax = 10;//max amoutn of trees
                int moutaintilecount = 0;
                const int mountaintilemax = 5;
                int chanceplace = 0;//te percenae chance tha a square will actully enerate in a tile
                string[,] placedts =
                {
                    { "-","-","-"},
                    { "-","c","-"},
                    { "-","-","-"}

                };
                string[,] placedms =
                {
                    { "-","-","-"},
                    { "-","c","-"},
                    { "-","-","-"}

                };

                //map randomization
                while (done == false)
                {
                    genplacemnetx = R.Next(1, 14);//picks random x coordinate in tilemap 
                    genplacemnety = R.Next(1, 10);//picks radnom y coordinate in tlemap this is to place first forest or moutian tile
                    if (treetilecount <= treetilemax)// cheks to see if the gneratio of tree tiels has already been completed
                    {
                        tilemap[genplacemnety, genplacemnetx] = 1;//sets this as starter tile
                        int fcentrex = genplacemnetx;
                        int fcentrey = genplacemnety;
                        treetilecount++;
                        for (int i = 0; i <= 8; i++)
                        {
                            chanceplace = R.Next(0, 100);
                            if (i == 0)
                            {
                                if (chanceplace <= 20 && (fcentrex - 1 == 0 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))//checks to see if the tree tile is trying to generate were it shouldnt on the left of the array from 0,0 - 0,10
                                {
                                    placedts[0, 2] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey - 1, fcentrex - 1] = 1;// adds to square in into array diagonally down left of the first generated square 
                                    placedts[0, 2] = "O";
                                }

                            }
                            if (i == 1)
                            {
                                if (chanceplace <= 20 && (fcentrex - 1 == 0 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))
                                {
                                    placedts[0, 1] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey - 1, fcentrex] = 1; //left of the mian square
                                    placedts[0, 1] = "O";
                                }
                            }
                            if (i == 2)
                            {
                                if (chanceplace <= 40 && (fcentrex - 1 == 0 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))
                                {
                                    placedts[0, 0] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey - 1, fcentrex + 1] = 1;
                                    placedts[0, 0] = "O";
                                }
                            }
                            if (i == 3)
                            {
                                treetilecount++;
                                tilemap[fcentrey, fcentrex + 1] = 1;
                                placedts[1, 0] = "O";

                            }
                            if (i == 4)
                            {
                                if (chanceplace <= 20 && (fcentrex + 1 == 14 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))
                                {
                                    placedts[2, 0] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey + 1, fcentrex + 1] = 1;
                                    placedts[2, 0] = "O";
                                }
                            }
                            if (i == 5)
                            {
                                if (chanceplace <= 20 && (fcentrex + 1 == 14 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))
                                {
                                    placedts[2, 1] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey + 1, fcentrex] = 1;
                                    placedts[2, 1] = "O";
                                }
                            }
                            if (i == 6)
                            {
                                if (chanceplace <= 20 && (fcentrex + 1 == 14 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))
                                {
                                    placedts[2, 2] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey + 1, fcentrex + 1] = 1;
                                    placedts[2, 2] = "O";
                                }
                            }
                            if (i == 7)
                            {
                                if (chanceplace <= 20 && (fcentrex + 1 == 14 && fcentrey - 1 >= 0 && fcentrey - 1 <= 10))
                                {
                                    placedts[1, 2] = "x";
                                }
                                else
                                {
                                    treetilecount++;
                                    tilemap[fcentrey, fcentrex - 1] = 1;
                                    placedts[1, 2] = "O";
                                }
                            }


                        }
                        while (treetilecount <= treetilemax)
                        {
                            const int n = 3; //n = the highet adn width of placedts array 
                            genplacemnetx = R.Next(-3, 4);// x location on tile map fo were it will be placed relative to first tree tile 
                            genplacemnety = R.Next(-3, 4);// y locationof were on tiel map itll be placed relactive to first tree tiel

                            int xplace = genplacemnetx + fcentrex;
                            int yplace = genplacemnety + fcentrey;
                            if ((xplace >= 1 && xplace <= 13) && (yplace >= 0 && yplace <= 10))
                            {
                                if ((yplace < 11 && xplace < 15) && xplace > 0)
                                {
                                    for (int i = 0; i < n; i++)
                                    {
                                        for (int j = 0; i < n; i++)
                                        {
                                            if (placedts[j, i] != "x" || placedts[j, i] != "-" && ((j != genplacemnetx + 1 || j != genplacemnetx + 2 || j != genplacemnetx - 1 || j != genplacemnetx - 2) && (i != genplacemnety + 1 || i != genplacemnety + 2 || i != genplacemnety - 1 || i != genplacemnety - 2)))
                                            {
                                                if (tilemap[yplace, xplace] != 1)
                                                {
                                                    tilemap[yplace, xplace] = 1;
                                                    treetilecount++;
                                                }
                                                else { }


                                            }
                                            else { }
                                        }
                                    }


                                }
                                else { }


                            }
                            else { }




                        }

                    }
                    else if (mountaintilemax >= moutaintilecount) // cheks to see if moutains have been generated and prevents anymore from bieng generated if max has been reached
                    {
                        if (tilemap[genplacemnety, genplacemnetx] != 1 && tilemap[genplacemnety + 1, genplacemnetx] != 1 && tilemap[genplacemnety - 1, genplacemnetx] != 1 && tilemap[genplacemnety, genplacemnetx + 1] != 1 && tilemap[genplacemnety, genplacemnetx - 1] != 1 && tilemap[genplacemnety - 1, genplacemnetx - 1] != 1 && tilemap[genplacemnety - 1, genplacemnetx + 1] != 1 && tilemap[genplacemnety + 1, genplacemnetx + 1] != 1 && tilemap[genplacemnety + 1, genplacemnetx - 1] != 1)
                        {//checks to see if the tile will be placed on a  tree tile  or near one ^
                            tilemap[genplacemnety, genplacemnetx] = 2;//places moutain tile setting it as centre
                            int mcentrex = genplacemnetx;
                            int mcentrey = genplacemnety;
                            moutaintilecount++;
                            while (moutaintilecount <= mountaintilemax)
                            {
                                const int n = 3; //n = the highet adn width of placedts array 
                                genplacemnetx = R.Next(-3, 4);// x location on tile map fo were it will be placed relative to first tree tile 
                                genplacemnety = R.Next(-3, 4);// y locationof were on tiel map itll be placed relactive to first tree tiel

                                int xplace = genplacemnetx + mcentrex;
                                int yplace = genplacemnety + mcentrey;
                                if ((xplace >= 1 && xplace <= 13) && (yplace >= 0 && yplace <= 10))
                                {
                                    if ((yplace < 11 && xplace < 15) && xplace > 0)
                                    {
                                        for (int i = 0; i < n; i++)
                                        {
                                            for (int j = 0; i < n; i++)
                                            {
                                                if ((xplace != 0 && yplace != 0) || (xplace != 4 && yplace != 4))
                                                {
                                                    if (tilemap[yplace, xplace] != 2 || tilemap[yplace, xplace] != 1)
                                                    {
                                                        tilemap[yplace, xplace] = 2;
                                                        moutaintilecount++;
                                                    }
                                                    else { }


                                                }
                                                else { }
                                            }
                                        }


                                    }
                                    else { }


                                }
                                else { }
                            }

                        }
                        else if (tilemap[genplacemnety, genplacemnetx] == 1 && tilemap[genplacemnety + 1, genplacemnetx] == 1 && tilemap[genplacemnety - 1, genplacemnety] == 1 && tilemap[genplacemnetx, genplacemnety + 1] == 1 && tilemap[genplacemnetx, genplacemnety - 1] == 1 && tilemap[genplacemnetx - 1, genplacemnety - 1] == 1 && tilemap[genplacemnetx - 1, genplacemnety + 1] == 1 && tilemap[genplacemnetx + 1, genplacemnety + 1] == 1 && tilemap[genplacemnetx + 1, genplacemnety - 1] == 1) //checks to see if the tile will be placed on a  tree tile 
                        {
                            while (tilemap[genplacemnetx, genplacemnety] == 1 && tilemap[genplacemnetx + 1, genplacemnety] == 1 && tilemap[genplacemnetx - 1, genplacemnety] == 1 && tilemap[genplacemnetx, genplacemnety + 1] == 1 && tilemap[genplacemnetx, genplacemnety - 1] == 1 && tilemap[genplacemnetx - 1, genplacemnety - 1] == 1 && tilemap[genplacemnetx - 1, genplacemnety + 1] == 1 && tilemap[genplacemnetx + 1, genplacemnety + 1] == 1 && tilemap[genplacemnetx + 1, genplacemnety - 1] == 1)//while loop reradnomisers  x and y start postion till it wont place a moutian tile on a tree tile 
                            {
                                genplacemnetx = R.Next(1, 14);
                                genplacemnety = R.Next(1, 10);

                            }
                        }



                    }
                    if (treetilecount >= treetilemax && moutaintilecount >= mountaintilemax)
                    {
                        done = true;
                        break;
                    }
                    else { }

                }

                //blue tanks
                Bheavy = new tank(0, 268, tank.Direction.right, 75, 80, 1, 75, 5, 2, false, 3, true, 1, false,false,false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
                p1tanks.Add(Bheavy);
                Bmed = new tank(0, 225, tank.Direction.right, 50, 70, 2, 50, 4, 3, false, 2, true, 2, false, false,false);
                p1tanks.Add(Bmed);
                Bmed2 = new tank(0, 315, tank.Direction.right, 50, 70, 2, 50, 4, 3, false, 2, true, 3, false, false,false);
                p1tanks.Add(Bmed2);
                Blight = new tank(0, 360, tank.Direction.right, 25, 60, 3, 25, 2, 5, false, 2, true, 4, false, false,false);
                Blight2 = new tank(0, 360, tank.Direction.right, 25, 60, 3, 25, 2, 5, false, 1, true, 5, false, false,false);
                p1tanks.Add(Blight2);
                ////red tanks
                Rheavy = new tank(610,268 , tank.Direction.left, 75, 80, 1, 75, 5, 2, false, 3, false, 1, false, false, false);// x,y,direction,armour,acc,speed,penpower,range,movepoints,havefired,type,player,id 
                p2tanks.Add(Rheavy);
                Rmed = new tank(620, 225, tank.Direction.left, 50, 70, 2, 50, 4, 3, false, 2, false, 2, false, false, false);
                p2tanks.Add(Rmed);
                Rmed2 = new tank(620, 315, tank.Direction.left, 50, 70, 2, 50, 5, 3, false, 2, false, 3, false, false, false);
                p2tanks.Add(Rmed2);
                Rlight = new tank(620, 360, tank.Direction.left, 25, 60, 3, 25, 2, 5, false, 2, false, 4, false, false, false);
                Rlight2 = new tank(620, 360, tank.Direction.left, 25, 60, 3, 25, 2, 5, false, 1, false, 5, false, false, false);
                p2tanks.Add(Rlight2);
                p1tankleft = p1tanks.Count;
                p2tankleft = p2tanks.Count;
            }
            camera = new Camera(GraphicsDevice.Viewport, initialZoom, initialPosition);
           

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            

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
            endturnbuttexture = Content.Load<Texture2D>("endturnbutton");
            rangefindertexture = Content.Load<Texture2D>("rangefind");
            Vector2 fbposition = new Vector2(710,200);// gives postion for hidden rectangle around buttons
            Vector2 bbposition = new Vector2(710, 250);
            Vector2 lbposition = new Vector2(760, 250);
            Vector2 rbposition = new Vector2(660, 250);
            Vector2 endbposition = new Vector2(710, 400);
            Vector2 rangeposition = new Vector2(630, 400);
            forwardbutton = new Rectangle((int)fbposition.X, (int)fbposition.Y, uparrowtexture.Width, uparrowtexture.Height);//loading positon adn texture for forwad button
            backbutton = new Rectangle((int)bbposition.X, (int)bbposition.Y, downarrowtexture.Width, downarrowtexture.Height);
            leftbutton = new Rectangle((int)lbposition.X, (int)lbposition.Y, leftturntexture.Width, leftturntexture.Height);
            rightbutton = new Rectangle((int)rbposition.X, (int)rbposition.Y, rightturntexture.Width, rightturntexture.Height);
            endturnbutton = new Rectangle((int)endbposition.X, (int)endbposition.Y, endturnbuttexture.Width,endturnbuttexture.Height);
            rangefinderbutton = new Rectangle((int)rangeposition.X, (int)rangeposition.Y,rangefindertexture.Width,rangefindertexture.Height);
            if (currentgamestate == gamestate.menue)
            {
                squareTexture = Content.Load<Texture2D>("menuscreen");
                myfontyfont = Content.Load<SpriteFont>("File");
                buttonTexture = Content.Load<Texture2D>("playbutton");
                // Set the initial position and size of the button
                Vector2 position = new Vector2(Window.ClientBounds.Width / 2 - 100, Window.ClientBounds.Height / 2 + 20);

                buttonRectangle = new Rectangle((int)position.X, (int)position.Y, buttonTexture.Width, buttonTexture.Height);
            }
            if(currentgamestate == gamestate.victory)
            {

            }
            // TODO: use this.Content to load your game content here
        }
        protected override void Update(GameTime gameTime)
        {
            
            MouseState mouseState = Mouse.GetState();
            if (currentgamestate == gamestate.menue)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && buttonRectangle.Contains(mouseState.Position))
                {
                    currentgamestate = gamestate.loading;
                    LoadContent();
                }
            }
            if(currentgamestate == gamestate.playing)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.B))
                {
                    bvic = true;
                    currentgamestate = gamestate.victory;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    rvic = true;
                    currentgamestate = gamestate.victory;
                }
                _spriteBatch.Begin();
                if (mouseState.LeftButton == ButtonState.Pressed && forwardbutton.Contains(mouseState.Position))//up arrow movemnt button
                {
                   if(turn % 2 == 0)// sees whos turjn it is even turns are blue teams turn
                   {
                        if(Bheavy._selected == true)//checks which tnak has been selected 
                        {
                            if ((tilemap[(Bheavy.Y / 55), ((Bheavy.X + 43) / 55)] != 2) || (tilemap[(Bheavy.Y / 55), ((Bheavy.X - 43) / 55)] != 2) || (tilemap[((Bheavy.Y + 43) / 55), (Bheavy.X / 55)] != 2) || (tilemap[((Bheavy.Y - 43) / 55), (Bheavy.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Bheavy.forwadmovement(Bheavy._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Bheavy.Y / 55), (Bheavy.X / 55)] == 1)
                                {
                                    Bheavy.movingintoforest();
                                }
                            }
                        }
                        else if (Bmed._selected == true)
                        {
                            if ((tilemap[(Bmed.Y / 55), ((Bmed.X + 45) / 55)] != 2) || (tilemap[(Bmed.Y / 55), ((Bmed.X - 45) / 55)] != 2) || (tilemap[((Bmed.Y + 45) / 55), (Bmed.X / 55)] != 2) || (tilemap[((Bmed.Y - 45) / 55), (Bmed.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Bmed.forwadmovement(Bmed._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Bmed.Y  / 55), (Bmed.X  / 55)] == 1)
                                {
                                    Bmed.movingintoforest();
                                }
                            }
                        }
                        else if (Bmed2._selected == true)
                        {
                            if ((tilemap[(Bmed2.Y / 55), ((Bmed2.X + 45) / 55)] != 2) || (tilemap[(Bmed2.Y / 55), ((Bmed2.X - 45) / 55)] != 2) || (tilemap[((Bmed2.Y + 45) / 55), (Bmed2.X / 55)] != 2) || (tilemap[((Bmed2.Y - 45) / 55), (Bmed2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Bmed2.forwadmovement(Bmed2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Bmed2.Y  / 55), (Bmed2.X / 55)] == 1)
                                {
                                    Bmed2.movingintoforest();
                                }
                            }
                        }
                        else if (Blight._selected == true)
                        {
                            Blight.forwadmovement(Blight._direction,_spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Blight2._selected == true)
                        {
                            if ((tilemap[(Blight2.Y / 55), ((Blight2.X + 45) / 55)] != 2) || (tilemap[(Blight2.Y / 55), ((Blight2.X - 45) / 55)] != 2) || (tilemap[((Blight2.Y + 45) / 55), ( Blight2.X / 55)] != 2) || (tilemap[((Blight2.Y - 45) / 55), (Blight2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Blight2.forwadmovement(Blight2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Blight2.Y / 55), (Blight2.X/55)] == 1)
                                {
                                    Blight2.movingintoforest();
                                }
                            }
                            

                        }
                   }
                   else
                   {
                        if (Rheavy._selected == true)//checks which tnak has been selected 
                        {
                            if ((tilemap[(Rheavy.Y / 55), ((Rheavy.X + 43) / 55)] != 2) || (tilemap[(Rheavy.Y / 55), ((Rheavy.X - 43) / 55)] != 2) || (tilemap[((Rheavy.Y + 43) / 55), (Rheavy.X / 55)] != 2) || (tilemap[((Rheavy.Y - 43) / 55), (Rheavy.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rheavy.forwadmovement(Rheavy._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rheavy.Y / 55), (Rheavy.X / 55)] == 1)
                                {
                                    Rheavy.movingintoforest();
                                }
                            }
                        }
                        else if (Rmed._selected == true)
                        {
                            if ((tilemap[(Rmed.Y / 55), ((Rmed.X + 45) / 55)] != 2) || (tilemap[(Rmed.Y / 55), ((Rmed.X - 45) / 55)] != 2) || (tilemap[((Rmed.Y + 45) / 55), (Rmed.X / 55)] != 2) || (tilemap[((Rmed.Y - 45) / 55), (Rmed.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rmed.forwadmovement(Rmed._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rmed.Y / 55), (Rmed.X / 55)] == 1)
                                {
                                    Rmed.movingintoforest();
                                }
                            }
                        }
                        else if (Rmed2._selected == true)
                        {
                            if ((tilemap[(Rmed2.Y / 55), ((Rmed2.X + 45) / 55)] != 2) || (tilemap[(Rmed2.Y / 55), ((Rmed2.X - 45) / 55)] != 2) || (tilemap[((Rmed2.Y + 45) / 55), (Rmed2.X / 55)] != 2) || (tilemap[((Rmed2.Y - 45) / 55), (Rmed2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rmed2.forwadmovement(Rmed2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rmed2.Y / 55), (Rmed2.X / 55)] == 1)
                                {
                                    Rmed2.movingintoforest();
                                }
                            }
                        }
                        else if (Rlight._selected == true)
                        {
                            Rlight.forwadmovement(Rlight._direction, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rlight2._selected == true)
                        {
                            if ((tilemap[(Rlight2.Y / 55), ((Rlight2.X + 45) / 55)] != 2) || (tilemap[(Rlight2.Y / 55), ((Rlight2.X - 45) / 55)] != 2) || (tilemap[((Rlight2.Y + 45) / 55), (Rlight2.X / 55)] != 2) || (tilemap[((Rlight2.Y - 45) / 55), (Rlight2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rlight2.forwadmovement(Rlight2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rlight2.Y / 55), (Rlight2.X / 55)] == 1)
                                {
                                    Rlight2.movingintoforest();
                                }
                            }


                        }
                   }
                    
                }
                if (mouseState.LeftButton == ButtonState.Pressed && backbutton.Contains(mouseState.Position))//sees if down button has been clikced does the smae as up button but moves selected tank backwards
                {
                    if (turn % 2 == 0)
                    {
                        if (Bheavy._selected == true)//checks which tnak has been selected 
                        {
                            if ((tilemap[(Bheavy.Y / 55), ((Bheavy.X + 43) / 55)] != 2) || (tilemap[(Bheavy.Y / 55), ((Bheavy.X - 43) / 55)] != 2) || (tilemap[((Bheavy.Y + 43) / 55), (Bheavy.X / 55)] != 2) || (tilemap[((Bheavy.Y - 43) / 55), (Bheavy.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Bheavy.backwardsmovement(Bheavy._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Bheavy.Y / 55), (Bheavy.X / 55)] == 1)
                                {
                                    Bheavy.movingintoforest();
                                }
                            }
                        }
                        else if (Bmed._selected == true)
                        {
                            if ((tilemap[(Bmed.Y / 55), ((Bmed.X + 45) / 55)] != 2) || (tilemap[(Bmed.Y / 55), ((Bmed.X - 45) / 55)] != 2) || (tilemap[((Bmed.Y + 45) / 55), (Bmed.X / 55)] != 2) || (tilemap[((Bmed.Y - 45) / 55), (Bmed.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Bmed.backwardsmovement(Bmed._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Bmed.Y / 55), (Bmed.X / 55)] == 1)
                                {
                                    Bmed.movingintoforest();
                                }
                            }
                        }
                        else if (Bmed2._selected == true)
                        {
                            if ((tilemap[(Bmed2.Y / 55), ((Bmed2.X + 45) / 55)] != 2) || (tilemap[(Bmed2.Y / 55), ((Bmed2.X - 45) / 55)] != 2) || (tilemap[((Bmed2.Y + 45) / 55), (Bmed2.X / 55)] != 2) || (tilemap[((Bmed2.Y - 45) / 55), (Bmed2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Bmed2.backwardsmovement(Bmed2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Bmed2.Y / 55), (Bmed2.X / 55)] == 1)
                                {
                                    Bmed2.movingintoforest();
                                }
                            }
                        }
                        else if (Blight._selected == true)
                        {
                            Blight.backwardsmovement(Blight._direction, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Blight2._selected == true)
                        {
                            if ((tilemap[(Blight2.Y / 55), ((Blight2.X + 45) / 55)] != 2) || (tilemap[(Blight2.Y / 55), ((Blight2.X - 45) / 55)] != 2) || (tilemap[((Blight2.Y + 45) / 55), (Blight2.X / 55)] != 2) || (tilemap[((Blight2.Y - 45) / 55), (Blight2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Blight2.backwardsmovement(Blight2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Blight2.Y / 55), (Blight2.X / 55)] == 1)
                                {
                                    Blight2.movingintoforest();
                                }
                            }


                        }
                    }
                    else
                    {
                        if (Rheavy._selected == true)//checks which tnak has been selected 
                        {
                            if ((tilemap[(Rheavy.Y / 55), ((Rheavy.X + 43) / 55)] != 2) || (tilemap[(Rheavy.Y / 55), ((Rheavy.X - 43) / 55)] != 2) || (tilemap[((Rheavy.Y + 43) / 55), (Rheavy.X / 55)] != 2) || (tilemap[((Rheavy.Y - 43) / 55), (Rheavy.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rheavy.backwardsmovement(Rheavy._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rheavy.Y / 55), (Rheavy.X / 55)] == 1)
                                {
                                    Rheavy.movingintoforest();
                                }
                            }
                        }
                        else if (Rmed._selected == true)
                        {
                            if ((tilemap[(Rmed.Y / 55), ((Rmed.X + 45) / 55)] != 2) || (tilemap[(Rmed.Y / 55), ((Rmed.X - 45) / 55)] != 2) || (tilemap[((Rmed.Y + 45) / 55), (Rmed.X / 55)] != 2) || (tilemap[((Rmed.Y - 45) / 55), (Rmed.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rmed.backwardsmovement(Rmed._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rmed.Y / 55), (Rmed.X / 55)] == 1)
                                {
                                    Rmed.movingintoforest();
                                }
                            }
                        }
                        else if (Rmed2._selected == true)
                        {
                            if ((tilemap[(Rmed2.Y / 55), ((Rmed2.X + 45) / 55)] != 2) || (tilemap[(Rmed2.Y / 55), ((Rmed2.X - 45) / 55)] != 2) || (tilemap[((Rmed2.Y + 45) / 55), (Rmed2.X / 55)] != 2) || (tilemap[((Rmed2.Y - 45) / 55), (Rmed2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rmed2.backwardsmovement(Rmed2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rmed2.Y / 55), (Rmed2.X / 55)] == 1)
                                {
                                    Rmed2.movingintoforest();
                                }
                            }
                        }
                        else if (Rlight._selected == true)
                        {
                            Rlight.backwardsmovement(Rlight._direction, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rlight2._selected == true)
                        {
                            if ((tilemap[(Rlight2.Y / 55), ((Rlight2.X + 45) / 55)] != 2) || (tilemap[(Rlight2.Y / 55), ((Rlight2.X - 45) / 55)] != 2) || (tilemap[((Rlight2.Y + 45) / 55), (Rlight2.X / 55)] != 2) || (tilemap[((Rlight2.Y - 45) / 55), (Rlight2.X / 55)] != 2))// checks ot see if theres a mountain in front
                            {
                                Rlight2.backwardsmovement(Rlight2._direction, _spriteBatch);//calls thetank movmnet method to move the tnak forwad
                                Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                                if (tilemap[(Rlight2.Y / 55), (Rlight2.X / 55)] == 1)
                                {
                                    Rlight2.movingintoforest();
                                }
                            }


                        }
                    }

                }
                if (mouseState.LeftButton == ButtonState.Pressed && leftbutton.Contains(mouseState.Position))
                {
                    if (turn % 2 == 0)// sees whos turjn it is even turns are blue teams turn
                    {
                        if (Bheavy._selected == true)//checks which tnak has been selected 
                        {
                            Bheavy.turningleft(Bheavy._direction,Content, _spriteBatch);//calls turning left method for tnaks
                            Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                        }
                        else if (Bmed._selected == true)
                        {
                            Bmed.turningleft(Bmed._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Bmed2._selected == true)
                        {
                            Bmed2.turningleft(Bmed2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Blight._selected == true)
                        {
                            Blight.turningleft(Blight._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Blight2._selected == true)
                        {
                            Blight2.turningleft(Blight2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        if (Rheavy._selected == true)//checks which tnak has been selected 
                        {
                            Rheavy.turningleft(Rheavy._direction, Content, _spriteBatch);//calls turning left method for tnaks
                            Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                        }
                        else if (Rmed._selected == true)
                        {
                            Rmed.turningleft(Rmed._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rmed2._selected == true)
                        {
                            Rmed2.turningleft(Rmed2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rlight._selected == true)
                        {
                            Rlight.turningleft(Rlight._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rlight2._selected == true)
                        {
                            Rlight2.turningleft(Rlight2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                    }

                }
                if (mouseState.LeftButton == ButtonState.Pressed && rightbutton.Contains(mouseState.Position))
                {
                    if (turn % 2 == 0)// sees whos turjn it is even turns are blue teams turn
                    {
                        if (Bheavy._selected == true)//checks which tnak has been selected 
                        {
                            Bheavy.turningright(Bheavy._direction, Content, _spriteBatch);//calls turning right method for tnaks
                            Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                        }
                        else if (Bmed._selected == true)
                        {
                            Bmed.turningright(Bmed._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Bmed2._selected == true)
                        {
                            Bmed2.turningright(Bmed2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Blight._selected == true)
                        {
                            Blight.turningright(Blight._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Blight2._selected == true)
                        {
                            Blight2.turningright(Blight2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                    }
                    else
                    {
                        if (Rheavy._selected == true)//checks which tnak has been selected 
                        {
                            Rheavy.turningright(Rheavy._direction, Content, _spriteBatch);//calls turning left method for tnaks
                            Thread.Sleep(200);//sleep the program prevents spam clicking adn unintetionly clicking ten times when wanting to click once
                        }
                        else if (Rmed._selected == true)
                        {
                            Rmed.turningright(Rmed._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rmed2._selected == true)
                        {
                            Rmed2.turningright(Rmed2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rlight._selected == true)
                        {
                            Rlight.turningright(Rlight._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                        else if (Rlight2._selected == true)
                        {
                            Rlight2.turningright(Rlight2._direction, Content, _spriteBatch);
                            Thread.Sleep(200);
                        }
                    }

                }
                if(mouseState.LeftButton == ButtonState.Pressed && endturnbutton.Contains(mouseState.Position))//when the end tunr buton is pessed
                {
                    if((turn+1) % 2 == 0)// if the tun is goign to be even and thus blue teams tun then it resets he move of the blue team 
                    {
                        Bheavy.resetmovpoints();
                        Bmed.resetmovpoints();
                        Bmed2.resetmovpoints();
                        Blight2.resetmovpoints();
                        Blight.resetmovpoints();
                    }
                    else if((turn+1) %2 != 0)
                    {
                        Rheavy.resetmovpoints();
                        Rmed.resetmovpoints();
                        Rmed2.resetmovpoints();
                        Rlight2.resetmovpoints();
                        Rlight.resetmovpoints();
                    }
                    turn++;
                    turncountwords = "Turn " + turn;
                    Thread.Sleep(250);
                }
                if (mouseState.LeftButton == ButtonState.Pressed && rangefinderbutton.Contains(mouseState.Position))//when range find buon is clicke it checks which enermy tanks are witin ange of the selcted tank
                {
                    checkshot();
                }
                if(Keyboard.GetState().IsKeyDown(Keys.F))//hold f adn mouse overthe one you want to shoot
                {
                    shooting();
                    p1tankleft = p1tanks.Count;
                    p2tankleft = p2tanks.Count;
                    whosdead();
                }
                Bheavy.Update(gameTime,_spriteBatch);
                Bmed.Update(gameTime, _spriteBatch);
                Bmed2.Update(gameTime, _spriteBatch);
                Blight.Update(gameTime, _spriteBatch);
                Blight2.Update(gameTime, _spriteBatch);
                Rheavy.Update(gameTime, _spriteBatch);
                Rmed.Update(gameTime, _spriteBatch);
                Rmed2.Update(gameTime, _spriteBatch);
                Rlight.Update(gameTime, _spriteBatch);
                Rlight2.Update(gameTime, _spriteBatch);
               
                _spriteBatch.End();
                //selected fun
               
            }
            if(currentgamestate == gamestate.victory)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && buttonRectangle.Contains(mouseState.Position))
                {
                    currentgamestate = gamestate.menue;
                    LoadContent();
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            base.Update(gameTime);
        }
        public void checkshot()//chesks i an enermy ank is inrane
        {
            Bheavy.inrangereset();// resets he inrange varible so that tanks stat shouldnt be in range don apear to be
            Bmed.inrangereset();
            Bmed2.inrangereset();
            Blight2.inrangereset();
            Rheavy.inrangereset();
            Rmed.inrangereset();
            Rmed2.inrangereset();
            Rlight2.inrangereset();
            int xdifference = 0;// chekcs the ange alon x axis
            int ydifference = 0;//dito bu fo y axis
            if (Game1.turn % 2 == 0)//if even un onl hekcs blue team  the ifception begins here
            { 
               if(Bheavy._selected == true)// strin of if staments checks hich tank is sleted so it knwos which one to compare
               {
                    if(Bheavy._direction == tank.Direction.right || Bheavy._direction == tank.Direction.left)//checks the dietion tank is facing so knows if it needs to compare range o x or y axis
                    {
                        xdifference = Math.Abs(Bheavy.X - Rheavy.X);//fidning diference
                        if(xdifference <= (Bheavy.Range * 55))//difference comparedd to rane
                        {
                            ydifference = Math.Abs(Bheavy.Y - Rheavy.Y);//checks that is witin ie conin so a block either side fo the one the tank is faciin
                            if(ydifference <= 130)
                            {
                                Rheavy._inrange = true;//if the tank is in range sets that tanks inrange to bein true
                            }
                        }
                        xdifference = Math.Abs(Bheavy.X - Rmed.X);
                        if (xdifference <= (Bheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Bheavy.Y - Rmed.Y);
                            if (ydifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bheavy.X - Rmed2.X);
                        if (xdifference <= (Bheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Bheavy.Y - Rmed2.Y);
                            if (ydifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bheavy.X - Rlight2.X);
                        if (xdifference <= (Bheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Bheavy.Y - Rlight2.Y);
                            if (ydifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Bheavy.Y - Rheavy.Y);
                        if (ydifference <= (Bheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Bheavy.X - Rheavy.X);
                            if (xdifference <= 130)
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bheavy.Y - Rmed.Y);
                        if (ydifference <= (Bheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Bheavy.X - Rmed.X);
                            if (xdifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bheavy.Y - Rmed2.Y);
                        if (ydifference <= (Bheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Bheavy.X - Rmed2.X);
                            if (xdifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bheavy.Y - Rlight2.Y);
                        if (ydifference <= (Bheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Bheavy.X - Rlight2.X);
                            if (xdifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
               }
               else if(Bmed._selected == true)
               {
                    if (Bmed._direction == tank.Direction.right || Bmed._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Bmed.X - Rheavy.X);
                        if (xdifference <= (Bmed.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed.Y - Rheavy.Y);
                            if (ydifference <= (Bmed.Range * 55))
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bmed.X - Rmed.X);
                        if (xdifference <= (Bmed.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed.Y - Rmed.Y);
                            if (ydifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bmed.X - Rmed2.X);
                        if (xdifference <= (Bmed.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed.Y - Rmed2.Y);
                            if (ydifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bmed.X - Rlight2.X);
                        if (xdifference <= (Bmed.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed.Y - Rlight2.Y);
                            if (ydifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Bmed.Y - Rheavy.Y);
                        if (ydifference <= (Bmed.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed.X - Rheavy.X);
                            if (xdifference <= 130)
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bmed.Y - Rmed.Y);
                        if (ydifference <= (Bmed.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed.X - Rmed.X);
                            if (xdifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bmed.Y - Rmed2.Y);
                        if (ydifference <= (Bmed.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed.X - Rmed2.X);
                            if (xdifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bmed.Y - Rlight2.Y);
                        if (ydifference <= (Bmed.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed.X - Rlight2.X);
                            if (xdifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
                }
               else if(Bmed2._selected == true)
               {
                    if (Bmed2._direction == tank.Direction.right || Bmed2._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Bmed2.X - Rheavy.X);
                        if (xdifference <= (Bmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed2.Y - Rheavy.Y);
                            if (ydifference <= 130)
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bmed2.X - Rmed.X);
                        if (xdifference <= (Bmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed2.Y - Rmed.Y);
                            if (ydifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bmed2.X - Rmed2.X);
                        if (xdifference <= (Bmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed2.Y - Rmed2.Y);
                            if (ydifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Bmed2.X - Rlight2.X);
                        if (xdifference <= (Bmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed2.Y - Rlight2.Y);
                            if (ydifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Bmed2.Y - Rheavy.Y);
                        if (ydifference <= (Bmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed2.X - Rheavy.X);
                            if (xdifference <= 130)
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bmed2.Y - Rmed.Y);
                        if (ydifference <= (Bmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed2.X - Rmed.X);
                            if (xdifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bmed2.Y - Rmed2.Y);
                        if (ydifference <= (Bmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed2.X - Rmed2.X);
                            if (xdifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Bmed2.Y - Rlight2.Y);
                        if (ydifference <= (Bmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Bmed2.X - Rlight2.X);
                            if (xdifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
                }
               else if (Blight2._selected == true)
               {
                    if (Blight2._direction == tank.Direction.right || Blight2._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Blight2.X - Rheavy.X);
                        if (xdifference <= (Blight2.Range * 55))
                        {
                            ydifference = Math.Abs(Blight2.Y - Rheavy.Y);
                            if (ydifference <= 130)
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Blight2.X - Rmed.X);
                        if (xdifference <= (Blight2.Range * 55))
                        {
                            ydifference = Math.Abs(Blight2.Y - Rmed.Y);
                            if (ydifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Blight2.X - Rmed2.X);
                        if (xdifference <= (Blight2.Range * 55))
                        {
                            ydifference = Math.Abs(Blight2.Y - Rmed2.Y);
                            if (ydifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Blight2.X - Rlight2.X);
                        if (xdifference <= (Blight2.Range * 55))
                        {
                            ydifference = Math.Abs(Blight2.Y - Rlight2.Y);
                            if (ydifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Blight2.Y - Rheavy.Y);
                        if (ydifference <= (Blight2.Range * 55))
                        {
                            xdifference = Math.Abs(Blight2.X - Rheavy.X);
                            if (xdifference <= 130)
                            {
                                Rheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Blight2.Y - Rmed.Y);
                        if (ydifference <= (Blight2.Range * 55))
                        {
                            xdifference = Math.Abs(Blight2.X - Rmed.X);
                            if (xdifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Blight2.Y - Rmed2.Y);
                        if (ydifference <= (Blight2.Range * 55))
                        {
                            xdifference = Math.Abs(Blight2.X - Rmed2.X);
                            if (xdifference <= 130)
                            {
                                Rmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Blight2.Y - Rlight2.Y);
                        if (ydifference <= (Blight2.Range * 55))
                        {
                            xdifference = Math.Abs(Blight2.X - Rlight2.X);
                            if (xdifference <= 130)
                            {
                                Rlight2._inrange = true;
                            }
                        }
                    }
               }


            }
            else//red teams checker
            {
                if (Rheavy._selected == true)
                {
                    if (Rheavy._direction == tank.Direction.right || Rheavy._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Rheavy.X - Bheavy.X);
                        if (xdifference <= (Rheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Rheavy.Y - Bheavy.Y);
                            if (ydifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rheavy.X - Bmed.X);
                        if (xdifference <= (Rheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Rheavy.Y - Bmed.Y);
                            if (ydifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rheavy.X - Bmed2.X);
                        if (xdifference <= (Rheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Rheavy.Y - Bmed2.Y);
                            if (ydifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rheavy.X - Blight2.X);
                        if (xdifference <= (Rheavy.Range * 55))
                        {
                            ydifference = Math.Abs(Rheavy.Y - Blight2.Y);
                            if (ydifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Rheavy.Y - Bheavy.Y);
                        if (ydifference <= (Rheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Rheavy.X - Bheavy.X);
                            if (xdifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rheavy.Y - Bmed.Y);
                        if (ydifference <= (Rheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Rheavy.X - Bmed.X);
                            if (xdifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rheavy.Y - Bmed2.Y);
                        if (ydifference <= (Rheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Rheavy.X - Bmed2.X);
                            if (xdifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rheavy.Y - Blight2.Y);
                        if (ydifference <= (Rheavy.Range * 55))
                        {
                            xdifference = Math.Abs(Rheavy.X - Blight2.X);
                            if (xdifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                }
                else if (Rmed._selected == true)
                {
                    if (Rmed._direction == tank.Direction.right ||Rmed._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Rmed.X - Bheavy.X);
                        if (xdifference <= (Rmed.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed.Y - Bheavy.Y);
                            if (ydifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rmed.X - Bmed.X);
                        if (xdifference <= (Rmed.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed.Y - Bmed.Y);
                            if (ydifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rmed.X - Bmed2.X);
                        if (xdifference <= (Rmed.Range * 55))
                        {
                            ydifference = Math.Abs(Bmed.Y - Bmed2.Y);
                            if (ydifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rmed.X - Blight2.X);
                        if (xdifference <= (Rmed.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed.Y - Blight2.Y);
                            if (ydifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Rmed.Y - Bheavy.Y);
                        if (ydifference <= (Rmed.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed.X - Bheavy.X);
                            if (xdifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rmed.Y - Bmed.Y);
                        if (ydifference <= (Rmed.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed.X - Bmed.X);
                            if (xdifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rmed.Y - Bmed2.Y);
                        if (ydifference <= (Rmed.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed.X - Bmed2.X);
                            if (xdifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rmed.Y - Blight2.Y);
                        if (ydifference <= (Rmed.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed.X - Blight2.X);
                            if (xdifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                }
                else if (Rmed2._selected == true)
                {
                    if (Rmed2._direction == tank.Direction.right || Rmed2._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Rmed2.X - Bheavy.X);
                        if (xdifference <= (Rmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed2.Y - Bheavy.Y);
                            if (ydifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rmed2.X - Bmed.X);
                        if (xdifference <= (Rmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed2.Y - Bmed.Y);
                            if (ydifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rmed2.X - Bmed2.X);
                        if (xdifference <= (Rmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed2.Y - Bmed2.Y);
                            if (ydifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rmed2.X - Blight2.X);
                        if (xdifference <= (Rmed2.Range * 55))
                        {
                            ydifference = Math.Abs(Rmed2.Y - Blight2.Y);
                            if (ydifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Rmed2.Y - Bheavy.Y);
                        if (ydifference <= (Rmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed2.X - Bheavy.X);
                            if (xdifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rmed2.Y - Bmed.Y);
                        if (ydifference <= (Rmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed2.X - Bmed.X);
                            if (xdifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rmed2.Y - Bmed2.Y);
                        if (ydifference <= (Rmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed2.X - Bmed2.X);
                            if (xdifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rmed2.Y - Blight2.Y);
                        if (ydifference <= (Rmed2.Range * 55))
                        {
                            xdifference = Math.Abs(Rmed2.X - Blight2.X);
                            if (xdifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                }
                else if (Rlight2._selected == true)
                {
                    if (Rlight2._direction == tank.Direction.right || Blight2._direction == tank.Direction.left)
                    {
                        xdifference = Math.Abs(Rlight2.X - Bheavy.X);
                        if (xdifference <= (Rlight2.Range * 55))
                        {
                            ydifference = Math.Abs(Rlight2.Y - Bheavy.Y);
                            if (ydifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rlight2.X - Bmed.X);
                        if (xdifference <= (Rlight2.Range * 55))
                        {
                            ydifference = Math.Abs(Rlight2.Y - Bmed.Y);
                            if (ydifference <= 130)
                            {
                                Bmed._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rlight2.X - Bmed2.X);
                        if (xdifference <= (Rlight2.Range * 55))
                        {
                            ydifference = Math.Abs(Rlight2.Y - Bmed2.Y);
                            if (ydifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        xdifference = Math.Abs(Rlight2.X - Blight2.X);
                        if (xdifference <= (Rlight2.Range * 55))
                        {
                            ydifference = Math.Abs(Rlight2.Y - Blight2.Y);
                            if (ydifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                    else
                    {
                        ydifference = Math.Abs(Rlight2.Y - Bheavy.Y);
                        if (ydifference <= (Rlight2.Range * 55))
                        {
                            xdifference = Math.Abs(Rlight2.X - Bheavy.X);
                            if (xdifference <= 130)
                            {
                                Bheavy._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rlight2.Y - Bmed.Y);
                        if (ydifference <= (Rlight2.Range * 55))
                        {
                            xdifference = Math.Abs(Rlight2.X - Bmed.X);
                            if (xdifference <= 130)
                            {
                                Rmed._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rlight2.Y - Bmed2.Y);
                        if (ydifference <= (Rlight2.Range * 55))
                        {
                            xdifference = Math.Abs(Rlight2.X - Bmed2.X);
                            if (xdifference <= 130)
                            {
                                Bmed2._inrange = true;
                            }
                        }
                        ydifference = Math.Abs(Rlight2.Y - Blight2.Y);
                        if (ydifference <= (Rlight2.Range * 55))
                        {
                            xdifference = Math.Abs(Rlight2.X - Blight2.X);
                            if (xdifference <= 130)
                            {
                                Blight2._inrange = true;
                            }
                        }
                    }
                }
            }
        }
        public void shooting()//have to do it in here doest work in the class //// be prepared for yet another ifception   //GET THE ARRAYS WORKING EVERYTHING IS OFF BY 1
        {
            int chance = 0;//will eb used to determined if it hits and tehn if it pens the armour and any other percentage calcs
            MouseState mouseState = Mouse.GetState();
            if (turn % 2 == 0)//cheskc if blue tunr
            {
                if (Bheavy._selected == true && Bheavy._havefired == false && Bheavy.canshoot() == true)//checks whicih is selcted and fi its selected
                {
                    if (Rheavy.RRHeavy.Contains(mouseState.Position))
                    {
                        if (Rheavy._inrange == true)
                        {
                            Bheavy._havefired = true;
                            if ((Bheavy._direction == tank.Direction.right && Bheavy._direction == tank.Direction.left) || (Bheavy._direction == tank.Direction.left && Bheavy._direction == tank.Direction.right) || (Bheavy._direction == tank.Direction.up && Bheavy._direction == tank.Direction.down) || (Bheavy._direction == tank.Direction.down && Bheavy._direction == tank.Direction.up))//front
                            {
                                if (Bheavy.Movactpoints < 2)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 50)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 85)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((Bheavy._direction == tank.Direction.left && Bheavy._direction == tank.Direction.left) || (Bheavy._direction == tank.Direction.right && Bheavy._direction == tank.Direction.right) || (Bheavy._direction == tank.Direction.up && Bheavy._direction == tank.Direction.up) || (Bheavy._direction == tank.Direction.down && Bheavy._direction == tank.Direction.down))//back
                            {
                                if (Bheavy.Movactpoints < 2)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 50)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 85)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Rheavy.Movactpoints < 2)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 50)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 30)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 30 && chance <= 60)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                                else if (chance > 60 && chance <= 80)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 80 && chance <= 100)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 85)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 31)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rmed.RRMed.Contains(mouseState.Position))
                    {
                        if (Bheavy.Movactpoints < 3)
                        {
                            chance = R.Next(0, 101);
                            if (chance >= 60)//50% chance to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Rmed.Crewmembers[3] == true)
                                            {
                                                Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Rmed.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Rmed.Crewmembers[2] == true)
                                            {
                                                Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Rmed.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Rmed.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Rmed.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Rmed.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Rmed.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            chance = R.Next(0, 101);
                            if (chance <= 85)//85% to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Rmed.Crewmembers[3] == true)
                                            {
                                                Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Rmed.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Rmed.Crewmembers[2] == true)
                                            {
                                                Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Rmed.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Rmed.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Rmed.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Rmed.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Rmed.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rmed2.RR2Med.Contains(mouseState.Position))
                    {
                        if (Bheavy.Movactpoints < 3)
                        {
                            chance = R.Next(0, 101);
                            if (chance >= 60)//50% chance to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Rmed2.Crewmembers[3] == true)
                                            {
                                                Rmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed2.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Rmed2.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Rmed2.Crewmembers[2] == true)
                                            {
                                                Rmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Rmed2.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Rmed2.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Rmed2.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Rmed2.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Rmed2.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            chance = R.Next(0, 101);
                            if (chance <= 85)//85% to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Rmed2.Crewmembers[3] == true)
                                            {
                                                Rmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed2.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Rmed2.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Rmed2.Crewmembers[2] == true)
                                            {
                                                Rmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Rmed2.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Rmed2.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Rmed2.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Rmed2.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Rmed2.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rlight2.RR2light.Contains(mouseState.Position))//equal cahnce to hit on all the armour no need to know direction
                    {
                        if (Blight2._inrange == true)
                        {
                            Bheavy._havefired = true;
                            if (Bheavy.Movactpoints < 3)
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged  
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 25)//tank so small equal chance of hiting all compionets
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 70)//85% to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 31)
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else if (Bmed._selected == true && Bmed._havefired == false && Bmed.canshoot() == true)
                {
                    if (Rheavy.RRHeavy.Contains(mouseState.Position))
                    {
                        if (Rheavy._inrange == true)
                        {
                            Bmed._havefired = true;
                            if ((Bmed._direction == tank.Direction.right && Rheavy._direction == tank.Direction.left) || (Bmed._direction == tank.Direction.up && Rheavy._direction == tank.Direction.down) || (Bmed._direction == tank.Direction.right && Rheavy._direction == tank.Direction.left) || (Bmed._direction == tank.Direction.down && Rheavy._direction == tank.Direction.up) || (Bmed._direction == tank.Direction.right && Rmed._direction == tank.Direction.up) || (Bmed._direction == tank.Direction.right && Rmed._direction == tank.Direction.down) || (Bmed._direction == tank.Direction.left && Rmed._direction == tank.Direction.up) || (Bmed._direction == tank.Direction.left && Rmed._direction == tank.Direction.down) || (Bmed._direction == tank.Direction.up && Rmed._direction == tank.Direction.left) || (Bmed._direction == tank.Direction.up && Rmed._direction == tank.Direction.right) || (Bmed._direction == tank.Direction.down && Rmed._direction == tank.Direction.left) || (Bmed._direction == tank.Direction.down && Rmed._direction == tank.Direction.right))
                            {
                                if (Bmed.Movactpoints < 5)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//60% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[2] == true)
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 25)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rheavy.Components1[0] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 25)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Bmed.Movactpoints < 5)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[2] == true)
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Rheavy.Components1[0] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[2] == true)
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Rheavy.Components1[0] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rmed.RRMed.Contains(mouseState.Position))
                    {
                        if (Rmed._inrange == true)
                        {
                            Bmed._havefired = true;
                            if ((Bmed._direction == tank.Direction.right && Rmed._direction == tank.Direction.left) || (Bmed._direction == tank.Direction.left && Rmed._direction == tank.Direction.right) || (Bmed._direction == tank.Direction.up && Rmed._direction == tank.Direction.down) || (Bmed._direction == tank.Direction.down && Rmed._direction == tank.Direction.up))//front facing armour
                            {
                                if (Bmed.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[2] == true)
                                                    {
                                                        Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed.Components1[0] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[2] == true)
                                                    {
                                                        Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed.Components1[0] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else//this is  more efficent way fo doeing the case statments this jsut realised not changeing heavy though far too much time 
                            {
                                if (Bmed.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[2] == true)
                                                    {
                                                        Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rmed.Components1[0] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[0] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[2] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[2] == true)
                                                    {
                                                        Rmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 31)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rmed.Components1[0] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rmed2.RR2Med.Contains(mouseState.Position))
                    {
                        if (Rmed2._inrange == true)
                        {
                            Bmed._havefired = true;
                            if ((Bmed._direction == tank.Direction.right && Rmed2._direction == tank.Direction.left) || (Bmed._direction == tank.Direction.left && Rmed2._direction == tank.Direction.right) || (Bmed._direction == tank.Direction.up && Rmed2._direction == tank.Direction.down) || (Bmed._direction == tank.Direction.down && Rmed2._direction == tank.Direction.up))//front facing armour
                            {
                                if (Bmed.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else//this is  more efficent way fo doeing the case statments this jsut realised not changeing heavy though far too much time 
                            {
                                if (Bmed.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 31)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 25 && chance <= 54)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                                else if (chance > 25 && chance <= 77)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 100)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rlight2.RR2light.Contains(mouseState.Position))//equal cahnce to hit on all the armour no need to know direction
                    {
                        if (Rlight2._inrange == true)
                        {
                            Bmed._havefired = true;
                            if (Bmed.Movactpoints < 3)
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged  
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 25)//tank so small equal chance of hiting all compionets
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 70)//85% to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 31)
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }






                        }
                    }
                }
                else if (Bmed2._selected == true && Bmed2._havefired == false && Bmed2.canshoot() == true)
                {
                    if (Rheavy.RRHeavy.Contains(mouseState.Position))
                    {
                        if (Rheavy._inrange == true)
                        {
                            Bmed2._havefired = true;
                            if ((Bmed2._direction == tank.Direction.right && Rheavy._direction == tank.Direction.left) || (Bmed2._direction == tank.Direction.up && Rheavy._direction == tank.Direction.down) || (Bmed2._direction == tank.Direction.right && Rheavy._direction == tank.Direction.left) || (Bmed2._direction == tank.Direction.down && Rheavy._direction == tank.Direction.up) || (Bmed2._direction == tank.Direction.right && Rmed._direction == tank.Direction.up) || (Bmed2._direction == tank.Direction.right && Rmed._direction == tank.Direction.down) || (Bmed2._direction == tank.Direction.left && Rmed._direction == tank.Direction.up) || (Bmed2._direction == tank.Direction.left && Rmed._direction == tank.Direction.down) || (Bmed2._direction == tank.Direction.up && Rmed._direction == tank.Direction.left) || (Bmed2._direction == tank.Direction.up && Rmed._direction == tank.Direction.right) || (Bmed2._direction == tank.Direction.down && Rmed._direction == tank.Direction.left) || (Bmed2._direction == tank.Direction.down && Rmed._direction == tank.Direction.right))
                            {
                                if (Bmed2.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//60% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 25)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 25)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Bmed2.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rheavy.Crewmembers[4] == true)
                                                    {
                                                        Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rheavy.Crewmembers[3] == true)
                                                    {
                                                        Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Rheavy.Components1[1] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Rheavy.Components1[2] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Rheavy.Components1[3] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Rheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rmed.RRMed.Contains(mouseState.Position))
                    {
                        if (Rmed._inrange == true)
                        {
                            Bmed2._havefired = true;
                            if ((Bmed2._direction == tank.Direction.right && Rmed._direction == tank.Direction.left) || (Bmed2._direction == tank.Direction.left && Rmed._direction == tank.Direction.right) || (Bmed2._direction == tank.Direction.up && Rmed._direction == tank.Direction.down) || (Bmed2._direction == tank.Direction.down && Rmed._direction == tank.Direction.up))//front facing armour
                            {
                                if (Bmed2.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[4] == true)
                                                    {
                                                        Rmed.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[4] == true)
                                                    {
                                                        Rmed.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else//this is  more efficent way fo doeing the case statments this jsut realised not changeing heavy though far too much time 
                            {
                                if (Bmed2.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[4] == true)
                                                    {
                                                        Rmed.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rmed.Components1[4] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed.Crewmembers[4] == true)
                                                    {
                                                        Rmed.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed.Crewmembers[3] == true)
                                                    {
                                                        Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 31)
                                                {
                                                    Rmed.Components1[3] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rmed.Components1[1] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rmed.Components1[2] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rmed.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rmed2.RR2Med.Contains(mouseState.Position))
                    {
                        if (Rmed2._inrange == true)
                        {
                            Bmed2._havefired = true;
                            if ((Bmed2._direction == tank.Direction.right && Rmed2._direction == tank.Direction.left) || (Bmed2._direction == tank.Direction.left && Rmed2._direction == tank.Direction.right) || (Bmed2._direction == tank.Direction.up && Rmed2._direction == tank.Direction.down) || (Bmed2._direction == tank.Direction.down && Rmed2._direction == tank.Direction.up))//front facing armour
                            {
                                if (Bmed2.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else //this is  more efficent way fo doeing the case statments this jsut realised not changeing heavy though far too much time 
                            {
                                if (Bmed2.Movactpoints < 3)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 60)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 50)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 75 && chance <= 100)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 70)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Rmed2.Crewmembers[4] == true)
                                                    {
                                                        Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Rmed2.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Rmed2.Crewmembers[3] == true)
                                                    {
                                                        Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Rmed2.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 31)
                                                {
                                                    Rmed2.Components1[3] = false;
                                                }
                                                else if (chance > 25 && chance <= 54)
                                                {
                                                    Rmed2.Components1[1] = false;
                                                }
                                                else if (chance > 25 && chance <= 77)
                                                {
                                                    Rmed2.Components1[2] = false;
                                                }
                                                else if (chance > 25 && chance <= 100)
                                                {
                                                    Rmed2.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Rlight2.RR2light.Contains(mouseState.Position))//equal cahnce to hit on all the armour no need to know direction
                    {
                        if (Rlight2._inrange == true)
                        {
                            Bmed2._havefired = true;
                            if (Bmed2.Movactpoints < 3)
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged  
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 25)//tank so small equal chance of hiting all compionets
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 70)//85% to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 31)
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                else if (Blight2._selected == true && Blight2._havefired == false && Blight2.canshoot() == true)
                {
                    if (Rheavy.RRHeavy.Contains(mouseState.Position))
                    {
                        if (Rheavy._inrange == true)
                        {
                            Blight2._havefired = true;
                            if ((Blight2._direction == tank.Direction.right && Rheavy._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.up && Rheavy._direction == tank.Direction.down) || (Blight2._direction == tank.Direction.right && Rheavy._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.down && Rheavy._direction == tank.Direction.up) || (Blight2._direction == tank.Direction.right && Rmed._direction == tank.Direction.up) || (Blight2._direction == tank.Direction.right && Rmed._direction == tank.Direction.down) || (Blight2._direction == tank.Direction.left && Rmed._direction == tank.Direction.up) || (Blight2._direction == tank.Direction.left && Rmed._direction == tank.Direction.down) || (Blight2._direction == tank.Direction.up && Rmed._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.up && Rmed._direction == tank.Direction.right) || (Blight2._direction == tank.Direction.down && Rmed._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.down && Rmed._direction == tank.Direction.right))
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//60% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 25)//60% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rheavy.Crewmembers[4] == true)
                                                {
                                                    Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rheavy.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rheavy.Crewmembers[3] == true)
                                                {
                                                    Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rheavy.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 25)
                                            {
                                                Rheavy.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rheavy.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rheavy.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rheavy.Components1[4] = false;
                                            }
                                        }
                                    }
                                }


                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 60)//90% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rheavy.Crewmembers[4] == true)
                                                {
                                                    Rheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rheavy.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rheavy.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rheavy.Crewmembers[3] == true)
                                                {
                                                    Rheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rheavy.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rheavy.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 49)
                                            {
                                                Rheavy.Components1[1] = false;
                                            }
                                            else if (chance > 49 && chance <= 66)
                                            {
                                                Rheavy.Components1[2] = false;
                                            }
                                            else if (chance > 66 && chance <= 83)
                                            {
                                                Rheavy.Components1[3] = false;
                                            }
                                            else if (chance > 83 && chance <= 100)
                                            {
                                                Rheavy.Components1[4] = false;
                                            }
                                        }
                                    }
                                }


                            }
                        }
                    }
                    else if (Rmed.RRMed.Contains(mouseState.Position))
                    {
                        if (Rmed._inrange == true)
                        {
                            Blight2._havefired = true;
                            if ((Blight2._direction == tank.Direction.right && Rmed._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.left && Rmed._direction == tank.Direction.right) || (Blight2._direction == tank.Direction.up && Rmed._direction == tank.Direction.down) || (Blight2._direction == tank.Direction.down && Rmed._direction == tank.Direction.up))//front facing armour
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 25)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rmed.Crewmembers[4] == true)
                                                {
                                                    Rmed.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rmed.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rmed.Crewmembers[3] == true)
                                                {
                                                    Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rmed.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 31)
                                            {
                                                Rmed.Components1[3] = false;
                                            }
                                            else if (chance > 31 && chance <= 54)
                                            {
                                                Rmed.Components1[1] = false;
                                            }
                                            else if (chance > 54 && chance <= 77)
                                            {
                                                Rmed.Components1[2] = false;
                                            }
                                            else if (chance > 77 && chance <= 100)
                                            {
                                                Rmed.Components1[4] = false;
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 60)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rmed.Crewmembers[4] == true)
                                                {
                                                    Rmed.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rmed.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rmed.Crewmembers[3] == true)
                                                {
                                                    Rmed.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rmed.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)
                                            {
                                                Rmed.Components1[2] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rmed.Components1[4] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rmed.Components1[3] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rmed.Components1[1] = false;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else if (Rmed2.RR2Med.Contains(mouseState.Position))
                    {
                        if (Rmed2._inrange == true)
                        {
                            Blight2._havefired = true;
                            if ((Blight2._direction == tank.Direction.right && Rmed2._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.left && Rmed2._direction == tank.Direction.right) || (Blight2._direction == tank.Direction.up && Rmed2._direction == tank.Direction.down) || (Blight2._direction == tank.Direction.down && Rmed2._direction == tank.Direction.up))//front facing armour
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 25)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rmed2.Crewmembers[4] == true)
                                                {
                                                    Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rmed2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rmed2.Crewmembers[3] == true)
                                                {
                                                    Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rmed2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 31)
                                            {
                                                Rmed2.Components1[3] = false;
                                            }
                                            else if (chance > 31 && chance <= 54)
                                            {
                                                Rmed2.Components1[1] = false;
                                            }
                                            else if (chance > 54 && chance <= 77)
                                            {
                                                Rmed2.Components1[2] = false;
                                            }
                                            else if (chance > 77 && chance <= 100)
                                            {
                                                Rmed2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 60)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rmed2.Crewmembers[4] == true)
                                                {
                                                    Rmed2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rmed2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rmed2.Crewmembers[3] == true)
                                                {
                                                    Rmed2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rmed2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rmed2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)
                                            {
                                                Rmed2.Components1[2] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rmed2.Components1[4] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rmed2.Components1[3] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rmed2.Components1[1] = false;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else if (Rlight2.RR2light.Contains(mouseState.Position))//equal cahnce to hit on all the armour no need to know direction
                    {

                        if (Rlight2._inrange == true)
                        {
                            Bmed2._havefired = true;
                            if ((Blight2._direction == tank.Direction.right && Rlight2._direction == tank.Direction.right) || (Blight2._direction == tank.Direction.left && Rlight2._direction == tank.Direction.left) || (Blight2._direction == tank.Direction.up && Rlight2._direction == tank.Direction.up) || (Blight2._direction == tank.Direction.down && Rlight2._direction == tank.Direction.down))
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged  
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 49)//tank so small equal chance of hiting all compionets
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 49 && chance <= 66)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 66 && chance <= 83)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 83 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 60)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Rlight2.Crewmembers[4] == true)
                                                {
                                                    Rlight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Rlight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Rlight2.Crewmembers[3] == true)
                                                {
                                                    Rlight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Rlight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Rlight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged  
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 25)//tank so small equal chance of hiting all compionets
                                            {
                                                Rlight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Rlight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Rlight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Rlight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }


                            }
                        }
                    }


                }
            }
            else
            {
                if (Rheavy._selected == true && Rheavy._havefired == false && Rheavy.canshoot() == true)
                {
                    if (Bheavy.RBheavy.Contains(mouseState.Position))
                    {
                        if (Bheavy._inrange == true)
                        {
                            Bheavy._havefired = true;
                            if ((Rheavy._direction == tank.Direction.right && Bheavy._direction == tank.Direction.left) || (Rheavy._direction == tank.Direction.left && Bheavy._direction == tank.Direction.right) || (Rheavy._direction == tank.Direction.up && Bheavy._direction == tank.Direction.down) || (Rheavy._direction == tank.Direction.down && Bheavy._direction == tank.Direction.up))//front
                            {
                                if (Rheavy.Movactpoints < 2)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 50)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Bheavy.Crewmembers[4] == true)
                                                    {
                                                        Bheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Bheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Bheavy.Crewmembers[3] == true)
                                                    {
                                                        Bheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Bheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Bheavy.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Bheavy.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Bheavy.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Bheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 85)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 60)//60% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Bheavy.Crewmembers[4] == true)
                                                    {
                                                        Bheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Bheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Bheavy.Crewmembers[3] == true)
                                                    {
                                                        Bheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Bheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);
                                                if (chance <= 31)
                                                {
                                                    Bheavy.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Bheavy.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Bheavy.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Bheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if ((Rheavy._direction == tank.Direction.left && Bheavy._direction == tank.Direction.left) || (Rheavy._direction == tank.Direction.right && Bheavy._direction == tank.Direction.right) || (Rheavy._direction == tank.Direction.up && Bheavy._direction == tank.Direction.up) || (Rheavy._direction == tank.Direction.down && Bheavy._direction == tank.Direction.down))//back
                            {
                                if (Bheavy.Movactpoints < 2)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 50)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Bheavy.Crewmembers[4] == true)
                                                    {
                                                        Bheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Bheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Bheavy.Crewmembers[3] == true)
                                                    {
                                                        Bheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Bheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Bheavy.Components1[1] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Bheavy.Components1[2] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Bheavy.Components1[3] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Bheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 85)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 90)//90% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Bheavy.Crewmembers[4] == true)
                                                    {
                                                        Bheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Bheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Bheavy.Crewmembers[3] == true)
                                                    {
                                                        Bheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Bheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 49)
                                                {
                                                    Bheavy.Components1[1] = false;
                                                }
                                                else if (chance > 49 && chance <= 66)
                                                {
                                                    Bheavy.Components1[2] = false;
                                                }
                                                else if (chance > 66 && chance <= 83)
                                                {
                                                    Bheavy.Components1[3] = false;
                                                }
                                                else if (chance > 83 && chance <= 100)
                                                {
                                                    Bheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Rheavy.Movactpoints < 2)
                                {
                                    chance = R.Next(0, 101);
                                    if (chance >= 50)//50% chance to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Bheavy.Crewmembers[4] == true)
                                                    {
                                                        Bheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Bheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Bheavy.Crewmembers[3] == true)
                                                    {
                                                        Bheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Bheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 30)
                                                {
                                                    Bheavy.Components1[2] = false;
                                                }
                                                else if (chance > 30 && chance <= 60)
                                                {
                                                    Bheavy.Components1[4] = false;
                                                }
                                                else if (chance > 60 && chance <= 80)
                                                {
                                                    Bheavy.Components1[3] = false;
                                                }
                                                else if (chance > 80 && chance <= 100)
                                                {
                                                    Bheavy.Components1[1] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 85)//85% to hit
                                    {
                                        chance = R.Next(0, 101);
                                        if (chance <= 80)//80% cahnce to pen
                                        {
                                            chance = R.Next(1, 3);
                                            if (chance == 1)//means crew meber will be killed
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 25)//25% to kill driver
                                                {
                                                    if (Bheavy.Crewmembers[4] == true)
                                                    {
                                                        Bheavy.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[1] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                                {
                                                    Bheavy.Crewmembers[3] = false;
                                                }
                                                else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                                {
                                                    if (Bheavy.Crewmembers[3] == true)
                                                    {
                                                        Bheavy.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                    }
                                                    else
                                                    {
                                                        Bheavy.Crewmembers[2] = false;// tank can no longer drive
                                                    }
                                                }
                                                else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                                {
                                                    Bheavy.Crewmembers[4] = false;
                                                }
                                            }
                                            else//means componets will be damged 
                                            {
                                                chance = R.Next(0, 101);//chooseing which one will die
                                                if (chance <= 31)
                                                {
                                                    Bheavy.Components1[3] = false;
                                                }
                                                else if (chance > 31 && chance <= 54)
                                                {
                                                    Bheavy.Components1[1] = false;
                                                }
                                                else if (chance > 54 && chance <= 77)
                                                {
                                                    Bheavy.Components1[2] = false;
                                                }
                                                else if (chance > 77 && chance <= 100)
                                                {
                                                    Bheavy.Components1[4] = false;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Bmed.RBMed.Contains(mouseState.Position))
                    {
                        if (Rheavy.Movactpoints < 3)
                        {
                            chance = R.Next(0, 101);
                            if (chance >= 60)//50% chance to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Bmed.Crewmembers[3] == true)
                                            {
                                                Bmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Bmed.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Bmed.Crewmembers[2] == true)
                                            {
                                                Bmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Bmed.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Bmed.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Bmed.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Bmed.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Bmed.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            chance = R.Next(0, 101);
                            if (chance <= 85)//85% to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Bmed.Crewmembers[3] == true)
                                            {
                                                Bmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Bmed.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Bmed.Crewmembers[2] == true)
                                            {
                                                Bmed.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Bmed.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Bmed.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Bmed.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Bmed.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Bmed.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Bmed2.RB2Med.Contains(mouseState.Position))
                    {
                        if (Rheavy.Movactpoints < 3)
                        {
                            chance = R.Next(0, 101);
                            if (chance >= 60)//50% chance to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Bmed2.Crewmembers[3] == true)
                                            {
                                                Bmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed2.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Bmed2.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Bmed2.Crewmembers[2] == true)
                                            {
                                                Bmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed2.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Bmed2.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Bmed2.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Bmed2.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Bmed2.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Bmed2.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            chance = R.Next(0, 101);
                            if (chance <= 85)//85% to hit
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 80)//80% cahnce to pen
                                {
                                    chance = R.Next(1, 3);
                                    if (chance == 1)//means crew meber will be killed
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)//25% to kill driver
                                        {
                                            if (Bmed2.Crewmembers[3] == true)
                                            {
                                                Bmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed2.Crewmembers[0] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                        {
                                            Bmed2.Crewmembers[2] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                        {
                                            if (Bmed2.Crewmembers[2] == true)
                                            {
                                                Bmed2.Crewmembers[2] = false;// shows commmander swapping wiht driver
                                            }
                                            else
                                            {
                                                Bmed2.Crewmembers[1] = false;// tank can no longer drive
                                            }
                                        }
                                        else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                        {
                                            Bmed2.Crewmembers[3] = false;
                                        }
                                    }
                                    else//means componets will be damged 
                                    {
                                        chance = R.Next(0, 101);//chooseing which one will die
                                        if (chance <= 25)
                                        {
                                            Bmed2.Components1[1] = false;
                                        }
                                        else if (chance > 25 && chance <= 50)
                                        {
                                            Bmed2.Components1[3] = false;
                                        }
                                        else if (chance > 50 && chance <= 75)
                                        {
                                            Bmed2.Components1[2] = false;
                                        }
                                        else if (chance > 75 && chance <= 100)
                                        {
                                            Bmed2.Components1[0] = false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Blight2.RB2light.Contains(mouseState.Position))//equal cahnce to hit on all the armour no need to know direction
                    {
                        if (Blight2._inrange == true)
                        {
                            Rheavy._havefired = true;
                            if (Rheavy.Movactpoints < 3)
                            {
                                chance = R.Next(0, 101);
                                if (chance >= 60)//50% chance to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Blight2.Crewmembers[4] == true)
                                                {
                                                    Blight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Blight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Blight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Blight2.Crewmembers[3] == true)
                                                {
                                                    Blight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Blight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Blight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged  
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 25)//tank so small equal chance of hiting all compionets
                                            {
                                                Blight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Blight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Blight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Blight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                chance = R.Next(0, 101);
                                if (chance <= 70)//85% to hit
                                {
                                    chance = R.Next(0, 101);
                                    if (chance <= 90)//80% cahnce to pen
                                    {
                                        chance = R.Next(1, 3);
                                        if (chance == 1)//means crew meber will be killed
                                        {
                                            chance = R.Next(0, 101);//chooseing which one will die
                                            if (chance <= 25)//25% to kill driver
                                            {
                                                if (Blight2.Crewmembers[4] == true)
                                                {
                                                    Blight2.Crewmembers[4] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Blight2.Crewmembers[1] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 25 && chance <= 50)//25% chance to kill loader
                                            {
                                                Blight2.Crewmembers[3] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)// 25% chance to kill gunner
                                            {
                                                if (Blight2.Crewmembers[3] == true)
                                                {
                                                    Blight2.Crewmembers[3] = false;// shows commmander swapping wiht driver
                                                }
                                                else
                                                {
                                                    Blight2.Crewmembers[2] = false;// tank can no longer drive
                                                }
                                            }
                                            else if (chance > 75 && chance <= 100)// 25% chance to kill commander
                                            {
                                                Blight2.Crewmembers[4] = false;
                                            }
                                        }
                                        else//means componets will be damged 
                                        {
                                            chance = R.Next(0, 101);
                                            if (chance <= 31)
                                            {
                                                Blight2.Components1[3] = false;
                                            }
                                            else if (chance > 25 && chance <= 50)
                                            {
                                                Blight2.Components1[1] = false;
                                            }
                                            else if (chance > 50 && chance <= 75)
                                            {
                                                Blight2.Components1[2] = false;
                                            }
                                            else if (chance > 75 && chance <= 100)
                                            {
                                                Blight2.Components1[4] = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void whosdead()
        {
            if(Bheavy._dead == true)
            {
                p1tanks.Remove(Bheavy);
            }
            if (Bmed._dead == true)
            {
                p1tanks.Remove(Bmed);
            }
            if (Bmed2._dead == true)
            {
                p1tanks.Remove(Bmed2);
            }
            if (Blight2._dead == true)
            {
                p1tanks.Remove(Blight2);
            }
            if (Rheavy._dead == true)
            {
                p1tanks.Remove(Rheavy);
            }
            if (Rmed._dead == true)
            {
                p1tanks.Remove(Rmed);
            }
            if (Rmed2._dead == true)
            {
                p1tanks.Remove(Rmed2);
            }
            if (Rlight2._dead == true)
            {
                p1tanks.Remove(Rlight2);
            }
            p1tankleft = p1tanks.Count;
            p2tankleft = p2tanks.Count;
            if (p1tankleft == 0)
            {
                bvic = true;
                currentgamestate = gamestate.victory;
            }
            else if(p2tankleft == 0)
            {
                rvic = true;
                currentgamestate = gamestate.victory;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            const int selx = 710;// where the imagees of the selted tanks will be drawn
            const int sely = 85;
            int col = 0;
            int row = 0;
            if (currentgamestate == gamestate.menue)// menu screen  // y = 550 x = 825 area = 453750 pixles 
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
            if(currentgamestate == gamestate.loading)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(GUIsqauretexture, new Vector2(670, 50), Color.White);
                _spriteBatch.Draw(uparrowtexture, forwardbutton, Color.White);
                _spriteBatch.Draw(downarrowtexture, backbutton, Color.White);
                _spriteBatch.Draw(leftturntexture, leftbutton, Color.White);
                _spriteBatch.Draw(rightturntexture, rightbutton, Color.White);
                _spriteBatch.Draw(endturnbuttexture, endturnbutton, Color.White);
                _spriteBatch.Draw(rangefindertexture, rangefinderbutton, Color.White);
                Vector2 textMiddlePoint = myfontyfont.MeasureString(turncountwords) / 2;
                Vector2 position = new Vector2(670,10);
                _spriteBatch.DrawString(myfontyfont, turncountwords, position, Color.Black, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 1.0f);
                _spriteBatch.End();
                currentgamestate = gamestate.playing;
            }

            if (currentgamestate == gamestate.playing)
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
                _spriteBatch.Draw(endturnbuttexture, endturnbutton, Color.White);
                _spriteBatch.Draw(rangefindertexture, rangefinderbutton, Color.White);
                Vector2 textMiddlePoint = myfontyfont.MeasureString(turncountwords) / 2;
                Vector2 position = new Vector2(720, 20);
                _spriteBatch.DrawString(myfontyfont, turncountwords, position, Color.Black, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 1.0f);
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
                        _spriteBatch.Draw(selectedtextureLB, new Vector2(selx, sely), Color.White);
                    }
                    else if (Rmed._selected == true || Rmed2._selected == true)
                    {
                        _spriteBatch.Draw(selectedtextureMR, new Vector2(selx, sely), Color.White);
                    }
                    else if (Rlight._selected == true || Rlight2._selected == true)
                    {
                        _spriteBatch.Draw(selectedtextureLR, new Vector2(selx, sely), Color.White);
                    }
                    _spriteBatch.End();
                
            }
            if (currentgamestate == gamestate.victory)
            {
                if (bvic == true)
                {
                    Vector2 textMiddlePoint = myfontyfont.MeasureString(bluevic) / 2;
                    // Places text in center of the screen
                    Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 - 50);
                    _spriteBatch.DrawString(myfontyfont, menuTitle, position, Color.AntiqueWhite, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 1.0f);
                    _spriteBatch.Draw(buttonTexture, buttonRectangle, Color.White);//button rectnagle allows for mouse to click
                }
                else
                {
                    Vector2 textMiddlePoint = myfontyfont.MeasureString(redvic) / 2;
                    // Places text in center of the screen
                    Vector2 position = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2 - 50);
                    _spriteBatch.DrawString(myfontyfont, menuTitle, position, Color.AntiqueWhite, 0, textMiddlePoint, 1.5f, SpriteEffects.None, 1.0f);
                    _spriteBatch.Draw(buttonTexture, buttonRectangle, Color.White);//button rectnagle allows for mouse to click
                }
            }
            base.Draw(gameTime);
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

