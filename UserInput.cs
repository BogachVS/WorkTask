using static System.Console;
namespace WorkTask
{
    public class UserInput
    {
        public static DateTime GetMaxDate(List<Box> boxes)
        {
            DateTime MaxDate = DateTime.MinValue;
            if (boxes.Count!=0)
            {
                foreach (var box in boxes)
                    if (MaxDate < box.ExpirationDate)
                        MaxDate = box.ExpirationDate;
            }
            return MaxDate;
        }
        public static DateTime GetMinDate(List<Box> boxes)
        {
            DateTime MinDate = DateTime.MaxValue;
            if (boxes.Count!=0)
            {
                foreach (var box in boxes)
                    if (MinDate > box.ExpirationDate)
                        MinDate = box.ExpirationDate;
            }
            return MinDate;
        }
        public static double GetSumOfCapacity(List<Box> boxes)
        {
            double sum = 0;
            if (boxes.Count!=0)
            {
                foreach (var box in boxes)
                    sum += box.Capacity;
            }
            return sum;
        }
        public static double GetSumOfWeight(List<Box> boxes)
        {
            double sum = 0;
            if (boxes.Count!=0)
            {
                foreach (var box in boxes)
                    sum += box.Weight;
            }
            return sum;
        }
        private static bool PalletHaveBoxes(List<Pallet> pallets)
        {
            foreach (var pallet in pallets)
            {
                if (pallet.Boxes != null)
                    return true;
            }
            return false;
        }
        private static bool CheckInput<T>(string? input, out T? result)
        {
            result = default;
            if (typeof(T) == typeof(int))
            {
                if (int.TryParse(input, out int intValue))
                {
                    if (intValue > 0)
                    {
                        result = (T)(object)intValue;
                        return true;
                    }
                }
                else Clear();
            }
            else if (typeof(T) == typeof(double))
            {
                if (double.TryParse(input, out double doubleValue))
                {
                    if (doubleValue > 0)
                    {
                        result = (T)(object)doubleValue;
                        return true;
                    }
                }
                else Clear();
            } 
            else if(typeof(T) == typeof(DateTime))
            {
                if(DateTime.TryParse(input, out DateTime dateTimeValue))
                {
                    if(dateTimeValue > DateTime.MinValue)
                    {
                        result = (T)(object)dateTimeValue;
                        return true;
                    }
                }
                else Clear();
            }
            return false;
        }
        public static void AddPallet(List<Pallet> pallets)
        {
            Clear();
            List<Box> boxes = [];
            double Width, Height, Depth;
            int ID;
            do { 
                Write("\nВведите ID паллеты : ");
                if (CheckInput(ReadLine(), out ID) && ID.ToString().Length == 4)
                    break;
                } while(true);
            do Write("\nВведите ширину паллеты : " ); while (!CheckInput(ReadLine(), out Width));
            do Write("\nВведите высоту паллеты : " ); while (!CheckInput(ReadLine(), out Height));
            do Write("\nВведите глубину паллеты : "); while (!CheckInput(ReadLine(), out Depth)); 
            Pallet pallet = new(ID, Width, Height, Depth, boxes);
            pallets.Add(pallet);
            Clear();
        }
        public static void AddBox(Pallet pallet) 
        {
            Clear();
            DateTime ExpirationDate, DateOfProduction;
            double BoxWidth, BoxHeight, BoxDepth, BoxWeight;
            Write("\nВведите срок годности или дату производства в формате (дд.мм.гг.) либо (гг.мм.дд)\n");
            do
            {
                Write("\nВвести срок годности - 1 : Ввести дату производства - 2   ");
                if (CheckInput(ReadLine(), out int n) && (n==1 || n==2))
                {
                    if (n==1)
                    {
                        if(CheckInput(ReadLine(),out ExpirationDate))
                        break;
                    }
                    else
                    {
                        if (CheckInput(ReadLine(), out DateOfProduction))
                        {
                            ExpirationDate = DateOfProduction.AddDays(100);
                            break;
                        }
                    }
                }
            } while (true);
            do 
            {
                Write("\nВведите ширину коробки : ");
                if (CheckInput(ReadLine(), out BoxWidth) && (BoxWidth <= pallet.Width))
                    break;
                else
                {
                    Write("\nВведено некорректное значение или ширина коробки превышает ширину паллеты");
                    ReadKey();
                    Clear();
                }
            }while(true);
            do Write("\nВведите высоту коробки : "); while (!CheckInput(ReadLine(), out BoxHeight));
            do
            {
                Write("\nВведите глубину коробки : ");
                if (CheckInput(ReadLine(), out BoxDepth) && (BoxDepth <= pallet.Depth))
                    break;
                else
                {
                    Write("\nВведено некорректное значение или глубина коробки превышает глубину паллеты");
                    ReadKey();
                    Clear();
                }
            } while (true);
            do Write("\nВведите вес коробки : "); while (!CheckInput(ReadLine(), out BoxWeight));
            Box box = new(BoxWidth, BoxHeight, BoxDepth, BoxWeight, ExpirationDate);
            pallet.Boxes.Add(box);
            pallet.ExpirationDate = GetMinDate(pallet.Boxes);
            pallet.MaxDate = GetMaxDate(pallet.Boxes);
            pallet.Capacity += GetSumOfCapacity(pallet.Boxes);
            pallet.Weight = GetSumOfWeight(pallet.Boxes) + 30;
            Clear();
        }
        public static void Output(List<Pallet> pallets)
        {
            if (pallets == null || PalletHaveBoxes(pallets) == false)
            {
                Write("\nНет информации");
                ReadKey();
                Clear();
            }
            else
            {
                Clear();
                var ThreePallet = pallets.OrderByDescending(pallet => pallet.MaxDate).Take(3).OrderBy(pallet => pallet.Capacity).ToList();
                foreach (var pallet in ThreePallet)
                    WriteLine($"|ID:{pallet.ID}|Max Date:{pallet.MaxDate.ToShortDateString()}|Expiration Date:{pallet.ExpirationDate.ToShortDateString()}|Capacity:{pallet.Capacity}");
                var PalletGroups = pallets.OrderBy(pallet => pallet.ExpirationDate).ThenBy(pallet => pallet.Weight).GroupBy(pallet => pallet.ExpirationDate).ToList();
                WriteLine();
                foreach (var group in PalletGroups)
                {
                    WriteLine($"\n|Key Group:{group.Key.ToShortDateString()}|\n");
                    foreach (var pallet in group)
                        WriteLine($"|ID:{pallet.ID}|Expiration Date:{pallet.ExpirationDate.ToShortDateString()}|");
                }
                ReadKey();
                Clear();
            }
        }
        public static void Menu()
        {
            List<Pallet> pallets = [];
            int menu;
            do
            {
                do
                {
                    WriteLine("\n 1 - Добавить паллету");
                    WriteLine("\n 2 - Добавить коробку");
                    WriteLine("\n 3 - Вывести информацию");
                    WriteLine("\n 4 - Выход\n");
                    if (CheckInput(ReadLine(), out menu))
                        break;
                    if (menu<1 || menu>4)
                        Clear();
                }while(true);
                switch(menu) 
                {
                    case 1:
                        {
                            AddPallet(pallets);
                        }break;
                    case 2:
                        {
                            if(pallets.Count==0)
                            {
                                Write("\nНет информации");
                                ReadKey();
                                Clear();
                                break;
                            }
                            int id;
                            bool flag = false;
                            do Write("\nВведите ID паллеты : "); while (!CheckInput(ReadLine(), out id));
                            for(int i = 0; i<pallets.Count; i++)
                            {
                                if (pallets[i].ID==id)
                                {
                                    AddBox(pallets[i]);
                                    flag = true;
                                }
                            }
                            if(!flag)
                            {
                                Write("\nНичего не найдено!");
                                ReadKey();
                                Clear();
                            }

                        }break;
                    case 3: 
                        {
                            Output(pallets);
                        }break;
                }
            }while (menu!=4);
        }
    }
}
