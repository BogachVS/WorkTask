namespace WorkTask
{
    public class AppGeneration
    {
        static void GetRandomDate(out DateTime date) 
        {
            int year, month, day;
            Random rand = new ();
            year = rand.Next(2000,2025);
            month = rand.Next(1, 13);
            if(DateTime.DaysInMonth(year, month) == 28) 
                day = rand.Next(1,29);
            else if (DateTime.DaysInMonth(year,month) == 29)
                day = rand.Next(1,30);
            else if(DateTime.DaysInMonth(year, month) == 30)
                day = rand.Next(1,31);
            else 
                day = rand.Next(1,32);
            date = new (year, month, day);
        }
        static List<Box> GetBoxes(double width, double depth)
        {
            DateTime ExpirationDate;
            Random rand = new ();
            List<Box> boxes = [];
            double BoxWidth, BoxHeight, BoxDepth, BoxWeight;
            for(int i = 0; i<16; i++) 
            {
                do BoxWidth = rand.Next(1,6); while(BoxWidth > width);
                BoxHeight = rand.Next(1,6);
                do BoxDepth = rand.Next(1,6); while (BoxDepth > depth);
                BoxWeight = rand.Next(11);
                if (rand.Next(1, 3) == 1)
                    GetRandomDate(out ExpirationDate);
                else
                {
                    GetRandomDate(out DateTime DateOfProduction);
                    ExpirationDate = DateOfProduction.AddDays(100);
                }
                Box box = new (BoxWidth, BoxHeight, BoxDepth, BoxWeight, ExpirationDate);
                boxes.Add(box);
            }
            return boxes;
        }
        static List<Pallet> GeneratePallets()
        {
            List<Pallet> pallets = [];
            List<Box> boxes = [];
            Random rand = new ();
            double Width,Height,Depth;
            int ID;
            for(int i = 0; i<30; i++) 
            {
                Width = rand.Next(1, 6);
                Height = rand.Next(1, 6);
                Depth = rand.Next(1, 6);
                ID = rand.Next(1000, 10000);
                boxes = GetBoxes(Width,Depth);
                Pallet pallet = new(ID, Width, Height, Depth, boxes)
                {
                    ExpirationDate = UserInput.GetMinDate(boxes),
                    MaxDate = UserInput.GetMaxDate(boxes)
                };
                pallet.Capacity += UserInput.GetSumOfCapacity(boxes);
                pallet.Weight = UserInput.GetSumOfWeight(boxes) + 30;
                pallets.Add(pallet);
            }
            return pallets;
        }

        public static void Main()
        {
            List<Pallet> pallets = [];
            //pallets = GeneratePallets(); // Генерация прямо в приложении
            //FileReading.SaveData("Pallets.xml", pallets);
            //FileReading.LoadData("Pallets.xml", out pallets); // Чтение из файла
            //UserInput.Output(pallets);
            UserInput.Menu(); // Пользовательский ввод
        }
    }
    
}