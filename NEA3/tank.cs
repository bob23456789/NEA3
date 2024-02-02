using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace NEA3
{
    internal class tank : Gameobject
    {
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
                    Texture = Content.Load<Texture2D>(@"Light_blue_RF");
                }
                else if (Player == false)
                {
                    Texture = Content.Load<Texture2D>(@"Light_red_LF");
                }

            }
            if (Type == 2 && Player == true)
            {
                if (Player == true)
                {
                    Texture = Content.Load<Texture2D>(@"blue_medium_RF");
                }
                else if (Player == false)
                {
                    Texture = Content.Load<Texture2D>(@"red_medium_LF");
                }

            }
            if (Type == 3 && Player == true)
            {
                if (Player == true)
                {
                    Texture = Content.Load<Texture2D>(@"blueheavy_RF");
                }
                else if (Player == false)
                {
                    Texture = Content.Load<Texture2D>(@"red_heavy_LF");
                }

            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            int _X = 0;
            int _Y = 0;
            if (Player == true)
            {
                if (Type == 3 && TankID == 1)
                {
                   
                    _Y = 275;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Color.White);
                }
                else if (Type == 2 && TankID == 2)
                {
                    
                    _Y = 220;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 2 && TankID == 3)
                {
                   
                    _Y = 330;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 1 && TankID == 4)
                {
                   
                    _Y = 170;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 1 && TankID == 5)
                {
                   
                    _Y = 280;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
            }
            if (Player == false)
            {
                _X = 825;
                if (Type == 2 && TankID == 1)
                {
                    
                    _Y = 275;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 2 && TankID == 2)
                {
                    
                    _Y = 220;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 2 && TankID == 3)
                {
                   
                    _Y = 330;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 1 && TankID == 4)
                {
                    
                    _Y = 170;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
                else if (Type == 1 && TankID == 5)
                {
                   
                    _Y = 280;
                    spriteBatch.Draw(Texture, new Vector2(_X, _Y), Microsoft.Xna.Framework.Color.White);
                }
            }

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
