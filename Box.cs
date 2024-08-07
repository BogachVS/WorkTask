namespace WorkTask
{
    public class Box : Object
    {
        public Box() { }
        public Box(double width, double height, double depth, double weight, DateTime expirationdate) 
        {
            Width = width; Height = height; Depth = depth; Weight = weight; ExpirationDate = expirationdate;
            Capacity = Math.Round(width * height * depth, 2);
        }
    }
}
