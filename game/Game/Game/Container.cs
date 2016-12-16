using System;
using System.Collections.Generic;

namespace Game
{
    public class Container
    {
        public List<Item> Contents = new List<Item>();
        public int Size { get; set; }
        public int Weight { get; set; }

        public Container()
        {
        }        

        public bool AddItem(Item i)
        {
            Contents.Add(i);
            Size = Contents.Count;
            return true;
        }

        public bool RemoveItem(out Item i, int position, string iD)
        {
            i = null;
            if (Contents.Exists(item => item.ID == iD))
            {
                i = Contents[position];
                Contents.RemoveAt(position);
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ViewItem(int i)
        {
            return Contents[i].Name.ToString();
        }
    }
}