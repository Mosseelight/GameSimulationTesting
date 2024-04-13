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
    

    //movement defines

    public abstract class Unmoveable : Element
    {
        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {
            positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 100;
            idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
        }
    }

    /// <summary>
    /// Swaps places with gas and liquid movable
    /// </summary>
    public abstract class Solid : Element
    {
        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {

            int num = new Random().Next(0,2); // choose random size to pick to favor instead of always left
            bool displaceLiq = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 2;
            if(displaceLiq)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                int aboveid = idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))];
                elements[aboveid].position = new Vector2(position.X, position.Y);
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = aboveid;
                
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] = id;
                elements[id].position = new Vector2(position.X, position.Y + 1);
                return;
            }
            bool displaceLiqLU = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 2;
            if(displaceLiqLU && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                int aboveid = idCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))];
                elements[aboveid].position = new Vector2(position.X, position.Y);
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = aboveid;

                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] = id;
                elements[id].position = new Vector2(position.X - 1, position.Y + 1);
                return;
            }
            bool displaceLiqRU = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 2;
            if(displaceLiqRU && num == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                int aboveid = idCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))];
                elements[aboveid].position = new Vector2(position.X, position.Y);
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = aboveid;

                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] = id;
                elements[id].position = new Vector2(position.X + 1, position.Y + 1);
                return;
            }
            bool grounded = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 0;
            if(grounded)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;

                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] = id;
                elements[id].position = new Vector2(position.X, position.Y + 1);
                return;
            }
            bool LUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 0;
            if(LUnder && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;

                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] = id;
                elements[id].position = new Vector2(position.X - 1, position.Y + 1);
                return;
            }
            bool RUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 0;
            if(RUnder && num == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;

                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] = id;
                elements[id].position = new Vector2(position.X + 1, position.Y + 1);
                return;
            }
        }
    }

    /// <summary>
    /// Swaps places with solid and gas
    /// </summary>
    public abstract class Liquid : Element
    {

        public int disp {get; set;} // viscosity
        public int level {get; set;} // bouyency

        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {
            int num = new Random().Next(0,2); // choose random size to pick to favor instead of always left

            oldpos = position;
            //displacement

            //gravity stuff
            bool ground = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 0;
            if(ground)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(0,1f);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
                return;
            }
            bool LUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0;
            if(!ground && LUnder && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
                return;
            }
            bool RUnder = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0;
            if(!ground && RUnder && num == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
                return;
            }
            bool Left = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0;
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
                return;
            }
            bool Right = positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0;
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
                return;
            }
        }
    }

    public abstract class Gas : Element
    {

    }

    //----Elements----
    //Can only run things using stuff from element
    //----------------
    

    //more specific implements

    /// <summary>
    /// Reacts to gravity
    /// </summary>
    public class SandPE : Solid
    {
        public SandPE()
        {
            color = Color.LightGoldenrodYellow;
            canMove = true;
        }
    }

    public class StonePE : Solid
    {
        public StonePE()
        {
            color = Color.LightGray;
            canMove = true;
        }
    }

    /// <summary>
    /// Unmoveable pixel
    /// </summary>
    public class WallPE : Unmoveable
    {
        public WallPE()
        {
            canMove = false;
            color = Color.Black;
        }
    }

    /// <summary>
    /// Reacts to gravity and solids
    /// </summary>
    public class WaterPE : Liquid
    {

        public WaterPE()
        {
            disp = 5;
            canMove = true;
            color = Color.MediumBlue;
        }

    }
}