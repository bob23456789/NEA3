using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static NEA3.tank;


namespace NEA3
{
    internal class tank : Gameobject
    {
        string Aboom = "";//blank like this means the ammo hasnt been hit and the test to see if ti goes boom has been done
        string Eboom = "";//same as last but for enigne
        public Rectangle RBheavy; // Rectangle aroudn tanks 
        public Rectangle RRHeavy;
        public Rectangle RBMed;
        public Rectangle RB2Med;
        public Rectangle RRMed;
        public Rectangle RR2Med;
        public Rectangle RBLight;
        public Rectangle RB2light;
        public Rectangle RRLight;
        public Rectangle RR2light;
         GraphicsDevice _graphicdevice;
        public Color colour;
        
        public enum Direction
        {
            down,
            left,
            right,
            up,
        }
        private int _tankID;
        public int TankID { get { return _tankID; } }// unique identifer for each tank  will 1-5 with haevy beign 1 mediums being 2 & 3 light 5 and 5
        public Direction  _direction;
        private bool _player; // true = p1 false = p2
        public bool Player { get { return _player; } }
        private int _type; // 1 = light 2 = medium 3 = heavy 0 = dead
        public int Type { get { return _type; } }
        private int _armour;// armour value of tank  
        public int Armour { get { return _armour; } }
        private int _acc;// accuracy of tank 
        public int Acc { get { return _acc; } }
        private int _speed;// accuracy of tank 
        public int Speed { get { return _speed; } }
        private int _penpower;
        public int Penpower { get { return _penpower; } }
        private int _range;// hwo far the gun can shoot
        public int Range { get { return _range; } }
        private int _x;
        public int X { get { return _x; } }
        private int _y;
        public int Y { get { return _y; } }
        public bool _inrange;
        private int _movementactionpoints;// how many movement action it can do 
        public int Movactpoints { get { return _movementactionpoints; } }
        public bool _havefired;
        public bool _selected;
        public bool[] Components1 = new bool[4] { true, true, true, true };  // components 1= engine  2= tracks 3=gun 4= ammo if all destroyed tank is destroyed  tracks can be repaired 
        public bool[] Crewmembers = new bool[4] { true, true, true, true }; // crew 1= driver 2= gunner 3= loader 4= commander commander can switch wiht any loader can switch with gunner
        public bool _dead;
        public tank(int X, int Y, Direction direction, int armour, int acc, int speed, int penpower, int range, int movepoints, bool havefired, int type, bool player, int tankID, bool selected, bool inrange,bool dead)
        {
            _tankID = tankID;
            _direction = direction;
            _x = X;
            _y = Y;
            _player = player;
            _type = type;
            Location = new Vector2(0, 0);
            _armour = armour;
            _acc = acc;
            _speed = speed;
            _penpower = penpower;
            _selected = selected;
            _range = range;
            _movementactionpoints = movepoints;
            _havefired = havefired;
            _inrange = inrange;
            _dead = dead;
            colour = Color.White;
        }
        public override void LoadContent(ContentManager Content)
        {
            if (_direction == Direction.right)
            {
                if (Type == 1)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"LightblueRF");
                        if (TankID == 4)
                        {
                            Vector2 LBposition = new Vector2(X, Y); //positon of the rectangles for light tank 1
                            RBLight = new Rectangle((int)LBposition.X, (int)LBposition.Y, Texture.Width, Texture.Height); // actual retangle 
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LB2position = new Vector2(X, Y); //positon of the rectangles 
                            RB2light = new Rectangle((int)LB2position.X, (int)LB2position.Y, Texture.Width, Texture.Height);
                        }

                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"LightredRF");
                        if (TankID == 4)
                        {
                            Vector2 LRposition = new Vector2(X, Y);
                            RRLight = new Rectangle((int)LRposition.X, (int)LRposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LR2position = new Vector2(X, Y);
                            RR2light = new Rectangle((int)LR2position.X, (int)LR2position.Y, Texture.Width, Texture.Height);
                        }
                    }

                }
                if (Type == 2)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"bluemediumRF");
                        if (TankID == 2)
                        {
                            Vector2 MBposition = new Vector2(X, Y);
                            RBMed = new Rectangle((int)MBposition.X, (int)MBposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 3)
                        {
                            Vector2 MB2position = new Vector2(X, Y);
                            RB2Med = new Rectangle((int)MB2position.X, (int)MB2position.Y, Texture.Width, Texture.Height);
                        }
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redmediumRF");
                        if (TankID == 3)
                        {
                            Vector2 Mr2position = new Vector2(X, Y);
                            RR2Med = new Rectangle((int)Mr2position.X, (int)Mr2position.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 2)
                        {
                            Vector2 MRposition = new Vector2(X, Y);
                            RRMed = new Rectangle((int)MRposition.X, (int)MRposition.Y, Texture.Width, Texture.Height);
                        }

                    }

                }
                if (Type == 3)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"blueheavyRF");
                        Vector2 HBposition = new Vector2(X, Y);
                        RBheavy = new Rectangle((int)HBposition.X, (int)HBposition.Y, Texture.Width, Texture.Height);
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redheavyRF");
                        Vector2 HRposition = new Vector2(X, Y);
                        RRHeavy = new Rectangle((int)HRposition.X, (int)HRposition.Y, Texture.Width, Texture.Height);
                    }

                }
            }
            if (_direction == Direction.left)
            {
                if (Type == 1)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"LightblueLF");
                        if (TankID == 4)
                        {
                            Vector2 LBposition = new Vector2(X, Y); //positon of the rectangles for light tank 1
                            RBLight = new Rectangle((int)LBposition.X, (int)LBposition.Y, Texture.Width, Texture.Height); // actual retangle 
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LB2position = new Vector2(X, Y); //positon of the rectangles 
                            RB2light = new Rectangle((int)LB2position.X, (int)LB2position.Y, Texture.Width, Texture.Height);
                        }

                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"LightredLF");
                        if (TankID == 4)
                        {
                            Vector2 LRposition = new Vector2(X, Y);
                            RRLight = new Rectangle((int)LRposition.X, (int)LRposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LR2position = new Vector2(X, Y);
                            RR2light = new Rectangle((int)LR2position.X, (int)LR2position.Y, Texture.Width, Texture.Height);
                        }
                    }

                }
                if (Type == 2)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"bluemediumLF");
                        if (TankID == 2)
                        {
                            Vector2 MBposition = new Vector2(X, Y);
                            RBMed = new Rectangle((int)MBposition.X, (int)MBposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 3)
                        {
                            Vector2 MB2position = new Vector2(X, Y);
                            RB2Med = new Rectangle((int)MB2position.X, (int)MB2position.Y, Texture.Width, Texture.Height);
                        }
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redmediumLF");
                        if (TankID == 3)
                        {
                            Vector2 Mr2position = new Vector2(X, Y);
                            RR2Med = new Rectangle((int)Mr2position.X, (int)Mr2position.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 2)
                        {
                            Vector2 MRposition = new Vector2(X, Y);
                            RRMed = new Rectangle((int)MRposition.X, (int)MRposition.Y, Texture.Width, Texture.Height);
                        }

                    }

                }
                if (Type == 3)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"blueheavyLF");
                        Vector2 HBposition = new Vector2(X, Y);
                        RBheavy = new Rectangle((int)HBposition.X, (int)HBposition.Y, Texture.Width, Texture.Height);
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redheavyLF");
                        Vector2 HRposition = new Vector2(X, Y);
                        RRHeavy = new Rectangle((int)HRposition.X, (int)HRposition.Y, Texture.Width, Texture.Height);
                    }

                }
            }
            if (_direction == Direction.up)
            {
                if (Type == 1)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"LightblueUF");
                        if (TankID == 4)
                        {
                            Vector2 LBposition = new Vector2(X, Y); //positon of the rectangles for light tank 1
                            RBLight = new Rectangle((int)LBposition.X, (int)LBposition.Y, Texture.Width, Texture.Height); // actual retangle 
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LB2position = new Vector2(X, Y); //positon of the rectangles 
                            RB2light = new Rectangle((int)LB2position.X, (int)LB2position.Y, Texture.Width, Texture.Height);
                        }

                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"LightredUF");
                        if (TankID == 4)
                        {
                            Vector2 LRposition = new Vector2(X, Y);
                            RRLight = new Rectangle((int)LRposition.X, (int)LRposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LR2position = new Vector2(X, Y);
                            RR2light = new Rectangle((int)LR2position.X, (int)LR2position.Y, Texture.Width, Texture.Height);
                        }
                    }

                }
                if (Type == 2)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"bluemediumUF");
                        if (TankID == 2)
                        {
                            Vector2 MBposition = new Vector2(X, Y);
                            RBMed = new Rectangle((int)MBposition.X, (int)MBposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 3)
                        {
                            Vector2 MB2position = new Vector2(X, Y);
                            RB2Med = new Rectangle((int)MB2position.X, (int)MB2position.Y, Texture.Width, Texture.Height);
                        }
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redmediumUF");
                        if (TankID == 3)
                        {
                            Vector2 Mr2position = new Vector2(X, Y);
                            RR2Med = new Rectangle((int)Mr2position.X, (int)Mr2position.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 2)
                        {
                            Vector2 MRposition = new Vector2(X, Y);
                            RRMed = new Rectangle((int)MRposition.X, (int)MRposition.Y, Texture.Width, Texture.Height);
                        }

                    }

                }
                if (Type == 3)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"blueheavyUF");
                        Vector2 HBposition = new Vector2(X, Y);
                        RBheavy = new Rectangle((int)HBposition.X, (int)HBposition.Y, Texture.Width, Texture.Height);
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redheavyUF");
                        Vector2 HRposition = new Vector2(X, Y);
                        RRHeavy = new Rectangle((int)HRposition.X, (int)HRposition.Y, Texture.Width, Texture.Height);
                    }

                }
            }
            if (_direction == Direction.down)
            {
                if (Type == 1)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"LightblueDF");
                        if (TankID == 4)
                        {
                            Vector2 LBposition = new Vector2(X, Y); //positon of the rectangles for light tank 1
                            RBLight = new Rectangle((int)LBposition.X, (int)LBposition.Y, Texture.Width, Texture.Height); // actual retangle 
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LB2position = new Vector2(X, Y); //positon of the rectangles 
                            RB2light = new Rectangle((int)LB2position.X, (int)LB2position.Y, Texture.Width, Texture.Height);
                        }

                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"LightredDF");
                        if (TankID == 4)
                        {
                            Vector2 LRposition = new Vector2(X, Y);
                            RRLight = new Rectangle((int)LRposition.X, (int)LRposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 5)
                        {
                            Vector2 LR2position = new Vector2(X, Y);
                            RR2light = new Rectangle((int)LR2position.X, (int)LR2position.Y, Texture.Width, Texture.Height);
                        }
                    }

                }
                if (Type == 2)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"bluemediumDF");
                        if (TankID == 2)
                        {
                            Vector2 MBposition = new Vector2(X, Y);
                            RBMed = new Rectangle((int)MBposition.X, (int)MBposition.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 3)
                        {
                            Vector2 MB2position = new Vector2(X, Y);
                            RB2Med = new Rectangle((int)MB2position.X, (int)MB2position.Y, Texture.Width, Texture.Height);
                        }
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redmediumDF");
                        if (TankID == 3)
                        {
                            Vector2 Mr2position = new Vector2(X, Y);
                            RR2Med = new Rectangle((int)Mr2position.X, (int)Mr2position.Y, Texture.Width, Texture.Height);
                        }
                        else if (TankID == 2)
                        {
                            Vector2 MRposition = new Vector2(X, Y);
                            RRMed = new Rectangle((int)MRposition.X, (int)MRposition.Y, Texture.Width, Texture.Height);
                        }

                    }

                }
                if (Type == 3)
                {
                    if (Player == true)
                    {
                        Texture = Content.Load<Texture2D>(@"blueheavyDF");
                        Vector2 HBposition = new Vector2(X, Y);
                        RBheavy = new Rectangle((int)HBposition.X, (int)HBposition.Y, Texture.Width, Texture.Height);
                    }
                    else if (Player == false)
                    {
                        Texture = Content.Load<Texture2D>(@"redheavyDF");
                        Vector2 HRposition = new Vector2(X, Y);
                        RRHeavy = new Rectangle((int)HRposition.X, (int)HRposition.Y, Texture.Width, Texture.Height);
                    }

                }
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
           
            if (Player == true)
            {
              
                switch (Type, TankID)
                {
                    case (3, 1):

                        spriteBatch.Draw(Texture, RBheavy, colour);
                        break;
                    case (2, 2):


                        spriteBatch.Draw(Texture, RBMed, colour);
                        break;
                    case (2, 3):

                        spriteBatch.Draw(Texture, RB2Med, colour);
                        break;
                    case (1, 4):
                      
                        spriteBatch.Draw(Texture, RBLight,colour );
                        break;
                    case (1, 5):
                        
                        spriteBatch.Draw(Texture, RB2light, colour);
                        break;

                }
            }
            if (Player == false)
            {
                
                switch (Type, TankID)
                {
                    case (3, 1):

                        spriteBatch.Draw(Texture, RRHeavy,colour );
                        break;
                    case (2, 2):

                        spriteBatch.Draw(Texture, RRMed,colour );
                        break;
                    case (2, 3):

                        spriteBatch.Draw(Texture, RR2Med,colour );
                        break;
                    case (1, 4):

                        spriteBatch.Draw(Texture, RRLight,colour );
                        break;
                    case (1, 5):

                        spriteBatch.Draw(Texture, RR2light,colour);
                        break;




                }
            }

        }
        public override void Update(GameTime gameTime , SpriteBatch spriteBatch)
        {
            MouseState mouseState = Mouse.GetState();
            Selected();
            boomornoboom();
            amidead();
            if((_inrange == true && _dead == true)|| (_inrange == false && _dead == true))
            {
                changecolour(Color.Black);
                Draw(spriteBatch);
            }
            if (_inrange == true)
            {
                changecolour(Color.Red);
                Draw(spriteBatch);
            }
            if (_inrange == false)
            {
                changecolour(Color.White);
                Draw(spriteBatch);
            }
            base.Update(gameTime,spriteBatch);
        }
        public void Selected()
        {
            
            if (Keyboard.GetState().IsKeyDown(Keys.D1))//when 1 is pressed this will select the heavy tank for whihc ever side it is 
            {

                if (Game1.turn % 2 == 0)//checks whos tunr it is 
                {
                    if (TankID == 1)// makes sure its the hevay tank on blue team 
                    {
                        _selected = true;//sets selcted for that tank to true 

                    }
                    if ((TankID == 3 || TankID == 2 || TankID == 4 || TankID == 5))
                    {
                        _selected = false;//sets selected for other tanks to false 
                    }
                }
                else
                {
                    if (TankID == 1 && Player == false)
                    {
                        _selected = true;

                    }
                    if ((TankID == 3 || TankID == 2 || TankID == 4 || TankID == 5))
                    {
                        _selected = false;
                    }

                }

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                if (Game1.turn % 2 == 0)//checks whos tunr it is 
                {
                    if (TankID == 2 && Player == true)// makes sure its the hevay tank on blue team 
                    {
                        _selected = true;//sets selcted for that tank to true 

                    }
                    if ((TankID == 1 || TankID == 3 || TankID == 4 || TankID == 5))
                    {
                        _selected = false;//sets selected for other tanks to false 
                    }
                }
                else
                {
                    if (TankID == 2 && Player == false)
                    {
                        _selected = true;

                    }
                    if ((TankID == 1 || TankID == 3 || TankID == 4 || TankID == 5))
                    {
                        _selected = false;
                    }

                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                if (Game1.turn % 2 == 0) //checks whos tunr it is 
                {
                    if (TankID == 3 && Player == true)// makes sure its the hevay tank on blue team 
                    {
                        _selected = true;//sets selcted for that tank to true 

                    }
                    if ((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 5))
                    {
                        _selected = false;//sets selected for other tanks to false 
                    }
                }
                else
                {
                    if (TankID == 3 && Player == false)
                    {
                        _selected = true;

                    }
                    if ((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 5))
                    {
                        _selected = false;
                    }

                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                if (Game1.turn % 2 == 0 || Game1.turn == 0)//checks whos tunr it is 
                {
                    if (TankID == 4 && Player == true)// makes sure its the hevay tank on blue team 
                    {
                        _selected = true;//sets selcted for that tank to true 

                    }
                    if ((TankID == 1 || TankID == 2 || TankID == 3 || TankID == 5))
                    {
                        _selected = false;//sets selected for other tanks to false 
                    }
                }
                else
                {
                    if (TankID == 4 && Player == false)
                    {
                        _selected = true;

                    }
                    if ((TankID == 1 || TankID == 2 || TankID == 3 || TankID == 5))
                    {
                        _selected = false;
                    }

                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                if (Game1.turn % 2 == 0)//checks whos tunr it is 
                {
                    if (TankID == 5 && Player == true)// makes sure its the hevay tank on blue team 
                    {
                        _selected = true;//sets selcted for that tank to true 

                    }
                    if ((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 3))
                    {
                        _selected = false;//sets selected for other tanks to false 
                    }
                }
                else
                {
                    if (TankID == 5 && Player == false)
                    {
                        _selected = true;

                    }
                    if ((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 3))
                    {
                        _selected = false;
                    }

                }
            }

        }
        public void forwadmovement(Direction direction, SpriteBatch spriteBatch) // method to move tanks forward
        {
            if(((_x - 55 >= 0 || _x + 55 < 825) && (_y - 55 >= 0 && _y + 55 <= 550)) && _movementactionpoints > 0)//makes sure they ahve enough movement points
            { 
                if (direction == Direction.right)// chekcs direction of the tank as to know which coord to change 
                {
                    if(TankID == 1 || TankID == 5)//due to size of heavy tank has to have different change in x
                    {
                        _x += 43;//changes x coord
                        _movementactionpoints--;// removes movement point
                    }
                    else
                    {
                        _x += 45;
                        _movementactionpoints--;
                    }
                
                }
                else if (direction == Direction.left)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _x += -43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _x += -45;
                        _movementactionpoints--;
                    }
                }
                else if (direction == Direction.up)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _y += -43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _y += -45;
                        _movementactionpoints--;
                    }
                }
                else if (direction == Direction.down)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _y += 43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _y += 45;
                        _movementactionpoints--;
                    }
                }
            }
            
            Draw(spriteBatch);
        }
        public void backwardsmovement(Direction direction, SpriteBatch spriteBatch)//same as forward movement method but moves tanks bakcwards instead
        {
            if (((_x > 0 || _x + 55 < 825) && (_y - 55 >= 0 || _y + 55 <= 550)) && _movementactionpoints > 0)
            {
                if (direction == Direction.right)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _x += -43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _x += -45;
                        _movementactionpoints--;
                    }

                }
                else if (direction == Direction.left)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _x += 43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _x += 45;
                        _movementactionpoints--;
                    }
                }
                else if (direction == Direction.up)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _y += 43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _y += 45;
                        _movementactionpoints--;
                    }
                }
                else if (direction == Direction.down)
                {
                    if (TankID == 1 || TankID == 5)
                    {
                        _y += -43;
                        _movementactionpoints--;
                    }
                    else
                    {
                        _y += -45;
                        _movementactionpoints--;
                    }
                }
            }
            Draw(spriteBatch);
        }

        public void turningleft(Direction direction, ContentManager content, SpriteBatch spriteBatch)//cahnges the direction of the tank  moving it 90 degrees anti clock wise
        {
            if(direction == Direction.right)//finds oringal direction
            {
                _direction = Direction.up;//set direction depending on oringal direction
                _movementactionpoints--;
            }
            else if (direction == Direction.up)
            {
                _direction = Direction.left;
                _movementactionpoints--;
            }
            else if (direction == Direction.left)
            {
                _direction = Direction.down;
                _movementactionpoints--;
            }
            else if (direction == Direction.down)
            {
                _direction = Direction.right;
                _movementactionpoints--;
            }
            _movementactionpoints--;
            LoadContent(content);
            Draw(spriteBatch);
        }
        public void turningright(Direction direction,ContentManager content , SpriteBatch spriteBatch)//same as last for turing 90 degrees clock wise
        {
            if (direction == Direction.right)
            {
                _direction = Direction.down;
                _movementactionpoints--;
            }
            else if (direction == Direction.up)
            {
                _direction = Direction.right;
                _movementactionpoints--;
            }
            else if (direction == Direction.left)
            {
                _direction = Direction.up;
                _movementactionpoints--;
            }
            else if (direction == Direction.down)
            {
                _direction = Direction.left;
                _movementactionpoints--;
            }
            _movementactionpoints--;
            LoadContent(content);
            Draw(spriteBatch);
        }
        public void movingintoforest()//removes all move points
        {
            _movementactionpoints = 0;
        }
        public void resetmovpoints()
        {
            if (Crewmembers[1] != false && Components1[1] != false && Components1[2] != false && _dead!= true)
            {

                if (Type == 1)
                {
                    _movementactionpoints = 5;
                    _havefired = false;
                }
                else if (Type == 2)
                {
                    _movementactionpoints = 3;
                    _havefired = false;
                }
                else if (Type == 3)
                {
                    _movementactionpoints = 2;
                    _havefired = false;
                }
            }
            else
            {
                _movementactionpoints = 0;//carnt move if no dirver or engine
                Components1[2] = true;//tracks being repaired
                _havefired = true;
            }
        }//esets all points back to ma
        public void inrangereset()//reseats in rane varible to false
        {
            _inrange = false;
            
        }
       
        public void changecolour(Color newColor)
        {
            colour = newColor;
        }
        public bool canshoot()
        {
            bool canshoot = true;
            if (Components1[3] == false || Crewmembers[2] == false)
            {
                return canshoot;
            }
            else
            {
                return canshoot;
            }
        }//checks it has componets and crewmembers to shoot
        public void boomornoboom()
        {
            int chanceboom = 0;
            Random R = new Random();
            if (Aboom == "" && Components1[3] == false)
            {
                chanceboom = R.Next(0, 101);
                if(chanceboom <= 70)//70% cahnce to blow up the tank
                {
                    Aboom = "y";
                    _dead = true;
                    Components1[0] = false;
                    Components1[1] = false;
                    Components1[2] = false;
                    Components1[3] = false;
                    Crewmembers[0] = false;
                    Crewmembers[1] = false;
                    Crewmembers[2] = false;
                    Crewmembers[3] = false;
                }
                else
                {
                    Aboom = "N";
                }
            }
            if(Eboom == "" && Components1[0] == false)
            {
                chanceboom = R.Next(0, 101);
                if (chanceboom <= 20)//20% cahnce to blow up the tank
                {
                    Eboom = "y";
                    _dead = true;
                    Components1[0] = false;
                    Components1[1] = false;
                    Components1[2] = false;
                    Components1[3] = false;
                    Crewmembers[0] = false;
                    Crewmembers[1] = false;
                    Crewmembers[2] = false;
                    Crewmembers[3] = false;
                }
                else
                {
                    Eboom = "N";
                }
            }
        }
        
        public void amidead()
        {
            if((Game1.p1tankleft == 1 && Components1[3] == false)|| (Game1.p1tankleft == 1 && Crewmembers[2] == false) || (Game1.p2tankleft == 1 && Components1[3] == false) || (Game1.p2tankleft == 1 && Crewmembers[2] == false))
            {
                _dead = true;
            }
            if ((Components1[1] == false && Components1[3] == false) || (Components1[1] == false && Crewmembers[2] == false) || (Crewmembers[1] == false && Components1[3] == false) || (Crewmembers[1] == false && Crewmembers[2] == false))
            {
                _dead = true;
            }
            if(_dead == true)
            {
                _movementactionpoints = 0;
                _havefired = true;
                changecolour(Color.Black);
            }
            else
            {

            }
        }
       
    }
}
