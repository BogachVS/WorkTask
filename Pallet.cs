using System;
namespace WorkTask
{
    public class Pallet : Object
    {
        public DateTime MaxDate;
        public List<Box> Boxes;
        public int ID;
        public Pallet() { }
        public Pallet(int id, double width, double height, double depth, List<Box> boxes) 
        {
            ID = id; Width = width; Height = height; Depth = depth; Boxes = boxes;
            Capacity = Math.Round(width * height * depth, 2);
        }
    }
}
