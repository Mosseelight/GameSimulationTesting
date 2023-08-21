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
            int num = new Random().Next(0,2); // choose random size to pick to favor instead of always left
            if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(0,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0 && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 1;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0 && num == 1)
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
            
        }
    }

    /// <summary>
    /// Reacts to gravity and solids
    /// </summary>
    public class WaterPE : Element
    {
        public WaterPE()
        {
            canMove = true;
            color = Color.MediumBlue;
        }

        public override void Update(ref List<Element> elements, ref int[] positionCheck, ref int[] idCheck, ref PixelDrawer drawer)
        {
            int num = new Random().Next(0,4); // choose random size to pick to favor instead of always left

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
            if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y + 1))] == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(0,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0 && num == 0)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y + 1))] == 0 && positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0 && num == 1)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(1,1);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X - 1, position.Y))] == 0 && num == 2)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(-1,0);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
            else if(positionCheck[drawer.GetIndexOnPos(new Vector2(position.X + 1, position.Y))] == 0  && num == 3)
            {
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 0;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = -1;
                position += new Vector2(1,0);
                positionCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = 2;
                idCheck[drawer.GetIndexOnPos(new Vector2(position.X, position.Y))] = id;
            }
        }

    }
}