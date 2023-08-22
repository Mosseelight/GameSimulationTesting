using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameTesting;
using Microsoft.Xna.Framework;

namespace PixelSimElement
{

    public abstract class Element
    {
        public Vector2 position {get; set;}
        public Color color {get; set;}
        public bool canMove {get; set;}
        public int id {get; set;}
        public Vector2 oldpos {get; set;}

        /// <summary>
        /// Position check must be updated when pixel pos changed
        /// </summary>
        public abstract void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer);
        public bool BoundsCheck(int minX, int minY, int maxX, int maxY)
        {
            if(position.X <= minX || position.X >= maxX || position.Y <= minY || position.Y >= maxY)
                return true;
            return false;
        }
    }

    //----Elements----
    //Can only run things using stuff from element
    //----------------
    
    /// <summary>
    /// Reacts to gravity
    /// </summary>
    public class SandPE : Element
    {
        public SandPE()
        {
            color = Color.Yellow;
            canMove = true;
        }

        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {
            bool grounded = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 0;
            bool LUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0;
            bool RUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0;

            int num = new Random().Next(0,2); // choose random size to pick to favor instead of always left
            if(grounded)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(0,1f);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(LUnder && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(RUnder && num == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
        }
    }

    /// <summary>
    /// Unmoveable pixel
    /// </summary>
    public class WallPE : Element
    {
        public WallPE()
        {
            canMove = false;
            color = Color.Black;
        }

        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {
            positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 100;
            idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
        }
    }

    /// <summary>
    /// Reacts to gravity and solids
    /// </summary>
    public class WaterPE : Element
    {
        int disp = 5;
        public WaterPE()
        {
            canMove = true;
            color = Color.MediumBlue;
        }

        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {
            int num = new Random().Next(0,2); // choose random size to pick to favor instead of always left
            bool ground = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 0;
            bool LUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0;
            bool RUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0;
            bool Left = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0;
            bool Right = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0;

            oldpos = position;
            //displacement
            if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y - 1))] == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                int aboveid = idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y - 1))];
                elements[aboveid].position = new Vector2(position.X, position.Y);
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = aboveid;
                position += new Vector2(0,-1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }

            //gravity stuff
            if(ground)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(0,1f);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            if(!ground && LUnder && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            if(!ground && RUnder && num == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            if(!ground && Left && num == 0)
            {
                int count = 0;
                for (int i = 0; i < disp; i++)
                {
                    if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - (i + 1), position.Y))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - (i + 1), position.Y + 1))] != 0)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-count,0);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            if(!ground && Right && num == 1)
            {
                int count = 0;
                for (int i = 0; i < disp; i++)
                {
                    if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + (i + 1), position.Y))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + (i + 1), position.Y + 1))] != 0)
                    {
                        count++;
                    }
                    else
                    {
                        break;
                    }
                }
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(count,0);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
        }

    }
}