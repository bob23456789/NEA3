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

namespace NEA3
{
    internal class tank : Gameobject
    {
        private Rectangle RBheavy; // Rectangle aroudn tanks 
        private Rectangle RRHeavy;
        private Rectangle RBMed;
        private Rectangle RB2Med;
        private Rectangle RRMed;
        private Rectangle RR2Med;
        private Rectangle RBLight;
        private Rectangle RB2light;
        private Rectangle RRLight;
        private Rectangle RR2light;
        protected GraphicsDevice _graphicdevice;
        
        public enum Direction
        {
            down,
            left,
            right,
            up,
        }
        private int _tankID;
        public int TankID { get { return _tankID; } }// unique identifer for each tank  will 1-5 with haevy beign 1 mediums being 2 & 3 light 5 and 5
        public Direction _direction;
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
        private int _movementactionpoints;// how many movement action it can do 
        public int Movactpoints { get { return _movementactionpoints; } }
        public bool _havefired;
        public bool _selected;
        public bool[] Components1 = new bool[4] { true, true, true, true };  // components 1= engine  2= tracks 3=gun 4= ammo if all destroyed tank is destroyed  tracks can be repaired 
        public bool[] Crewmembers = new bool[4] { true, true, true, true }; // crew 1= driver 2= gunner 3= loader 4= commander commander can switch wiht any loader can switch with gunner
        public tank(int x, int y, Direction direction, int armour, int acc, int speed, int penpower, int range, int movepoints, bool havefired, int type, bool player, int tankID, bool selected)
        {
            _tankID = tankID;
            _direction = direction;
            _x = x;
            _y = y;
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
           
        }
        public override void LoadContent(ContentManager Content)
        {
            
            if (Type == 1)
            {
                if (Player == true)
                {
                    Texture = Content.Load<Texture2D>(@"LightblueRF");
                    if(TankID == 4)
                    {
                        Vector2 LBposition = new Vector2(0, 100); //positon of the rectangles for light tank 1
                        RBLight = new Rectangle((int)LBposition.X, (int)LBposition.Y, Texture.Width, Texture.Height); // actual retangle 
                    }
                    else if (TankID == 5)
                    {
                        Vector2 LB2position = new Vector2(0, 400); //positon of the rectangles 
                        RB2light = new Rectangle((int)LB2position.X, (int)LB2position.Y, Texture.Width, Texture.Height);
                    }

                }
                else if (Player == false)
                {
                    Texture = Content.Load<Texture2D>(@"LightredLF");
                    if (TankID == 4)
                    {
                        Vector2 LRposition = new Vector2(635, 100);
                        RRLight = new Rectangle((int)LRposition.X, (int)LRposition.Y, Texture.Width, Texture.Height);
                    }
                    else if (TankID == 5)
                    {
                        Vector2 LR2position = new Vector2(635, 400);
                       RR2light = new Rectangle((int)LR2position.X, (int)LR2position.Y, Texture.Width, Texture.Height);
                    }
                }

            }
            if (Type == 2 )
            {
                if (Player == true)
                {
                    Texture = Content.Load<Texture2D>(@"bluemediumRF");
                    if (TankID == 3)
                    {
                        Vector2 MB2position = new Vector2(320, 340);
                        RBMed = new Rectangle((int)MB2position.X, (int)MB2position.Y, Texture.Width, Texture.Height);
                    }
                    else if (TankID == 2)
                    {
                        Vector2 MBposition = new Vector2(-100, 200);
                        RB2Med = new Rectangle((int)MBposition.X, (int)MBposition.Y, Texture.Width, Texture.Height);
                    }
                }
                else if (Player == false)
                {
                    Texture = Content.Load<Texture2D>(@"redmediumLF");
                    if (TankID == 3)
                    {
                        Vector2 Mr2position = new Vector2(620, 340);
                        RR2Med = new Rectangle((int)Mr2position.X, (int)Mr2position.Y, Texture.Width, Texture.Height);
                    }
                    else if (TankID == 2)
                    {
                        Vector2 MRposition = new Vector2(620, 200);
                        RRMed = new Rectangle((int)MRposition.X, (int)MRposition.Y, Texture.Width, Texture.Height);
                    }
                    
                }

            }
            if (Type == 3 )
            {
                if (Player == true)
                {
                    Texture = Content.Load<Texture2D>(@"blueheavyRF");
                    Vector2 HBposition = new Vector2(0, 285);
                    RBheavy = new Rectangle((int)HBposition.X, (int)HBposition.Y, Texture.Width, Texture.Height);
                }
                else if (Player == false)
                {
                    Texture = Content.Load<Texture2D>(@"redheavyLF");
                    Vector2 HRposition = new Vector2(610, 265);
                    RRHeavy = new Rectangle((int)HRposition.X, (int)HRposition.Y, Texture.Width, Texture.Height);
                }

            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int _X = 0;
            int _Y = 0;
            if (Player == true)
            {
                 _X = 0;
                switch (Type, TankID)
                {
                    case (3, 1):
                        
                        spriteBatch.Draw(Texture, RBheavy, Color.White);
                        break;
                    case (2, 2):
                        
                        
                        spriteBatch.Draw(Texture, RBMed, Color.White);
                        break;
                    case (2, 3):
                       
                        spriteBatch.Draw(Texture, RB2Med, Color.White);
                        break;
                    case (1, 4):
                        _Y = 100;
                        spriteBatch.Draw(Texture, RBLight , Color.White);
                        break;
                    case (1, 5):
                        _Y = 400;
                        spriteBatch.Draw(Texture, RB2light, Color.White);
                        break;

                }
            }
            if (Player == false)
            {
                _X = 620;
                switch (Type, TankID)
                {
                    case (3, 1):
                        
                        spriteBatch.Draw(Texture, RRHeavy, Color.White);
                        break;
                    case (2, 2):
                       
                        spriteBatch.Draw(Texture,RRMed, Color.White);
                        break;
                    case (2, 3):
                       
                        spriteBatch.Draw(Texture,RR2Med, Color.White);
                        break;
                    case (1, 4):
                      
                        spriteBatch.Draw(Texture,RRLight, Color.White);
                        break;
                    case (1, 5):
                       
                        spriteBatch.Draw(Texture,RR2light, Color.White);
                        break;




                }
            }

        }
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Selected();
            base.Update(gameTime);
        }
        public void Selected()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D1))//when 1 is pressed this will select the heavy tank for whihc ever side it is 
            {
                
                if ( Game1.turn % 2 == 0 )//checks whos tunr it is 
                {
                    if(TankID == 1 )// makes sure its the hevay tank on blue team 
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
                    if ((TankID == 3 || TankID == 2 || TankID == 4 || TankID == 5) )
                    {
                        _selected = false;
                    }

                }

            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                if (Game1.turn % 2 == 0 )//checks whos tunr it is 
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
                    if((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 5))
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
                    if ((TankID == 1 || TankID == 2 || TankID == 3 || TankID == 5) )
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
                    if((TankID == 1 || TankID == 2 || TankID == 3 || TankID == 5))
                    {
                        _selected = false;
                    }

                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D5))
            {
                if (Game1.turn % 2 == 0 )//checks whos tunr it is 
                {
                    if (TankID == 5 && Player == true)// makes sure its the hevay tank on blue team 
                    {
                        _selected = true;//sets selcted for that tank to true 

                    }
                    if((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 3) )
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
                    if ((TankID == 1 || TankID == 2 || TankID == 4 || TankID == 3) )
                    {
                        _selected = false;
                    }

                }
            }
            
        }
        public static void movement()
        {

        }
        ////how game going to work

        //armour + pen power
        //if amrmour == pen power pen chance = 60 %
        //if armour<pen power = 80%
        //if armour> pen power = 45%
        //base for heavy = 75 medium = 50 light = 25
        //base pen heavy = 75 medium = 50 light = 25

        //Range
        //1 = 1 tile
        //heavy = 5 medium = 5 light = 3
        //heavy max = opt  medium = 4 light = 2

        //accuracy 
        //base opt range acc heavy = 85 % medium = 70 % light = 60 %
        //if have moeved = true heavy acc = 50 % mediuam = 50 % light not effected 
        //if fireing into foresrt base -10% accuracy  
        //if above opt range  -10% accuracy 
        //if fireing withing 1 tile acc = 90 + -any modifers
        //can not fire through moutians or forests

        //speed
        //1= 1 tile of movment or 55 pixles i think 
        //base speed heavy = 1 medium = 2 light = 3
        //forrest reduce speed by 1 can not go below 1 speed

        //movepoints
        //these count for moveing to other tiels and turing
        //heavy = 2 medium = 3 light = 5

        //damaging tanks
        //when a tank is pened depedning on teh side its hit decide percentages if destroyed
        //crewmate array 
        //if gunner or driver dies then loader and commander can switch with them 
        //if all the crew is dead the tank is destroyed
        //if gunner is dead then have fireing will be impossible(fired will be permantly true)
        //if driver is dead then moving is impossible(movepoints will eb set to zero)

        //components array
        //if tracks destroyed can be repaired movepoints halved
        //repaireing will use all the movepoints that turn
        //if ammo destroyed(not tecnically destroyed just dentoes if hit)80% chance to go boom destroying tank but hard to hit 
        //if engine destroyed tank permantly can not move 10% to go boom 
        //if gun destroyed tank permantly not fire(very difficult)
        //if engine and engine are destroyed tank will be considered destroyed even if all crew is alive
    }
}
