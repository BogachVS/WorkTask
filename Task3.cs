using static System.Console;

public class Program 
{
static DateOnly GetMaxDateBox(List<Box> boxes) // Аналогично только теперь ищем коробку с максимальным сроком годности
{
    Box max = boxes[0];
    foreach (var box in boxes)
        if(max.ExpirationDate < box.ExpirationDate)
            max = box;
    return max.ExpirationDate;
}
static DateOnly GetMinDateBox(List<Box> boxes) // Метод для поиска минимальной даты из списка коробок, которые лежат на паллете
{
    Box min = boxes[0];
    foreach (var box in boxes)
        if(min.ExpirationDate > box.ExpirationDate)
            min = box;
    return min.ExpirationDate;
}
 void GetDate(out DateOnly date) // Метод для ввода даты с консоли 
{
    int year, month, day;
        bool flag;
    Write("Введите год : ");
        do
        {
            flag = int.TryParse(ReadLine(),out year);
            if(!flag)
                Clear();
        } while ((year < 2000 || year > 2024) && !flag);
    Write("\nВведите месяц : ");
        do
        {
            flag = int.TryParse(ReadLine(), out month);
            if(!flag)
                Clear();
        } while ((month < 1 || month > 12) && !flag);
    Write("\nВведите день : ");
        if (month is 4 or 6 or 9 or 11)
        {
            do
            {
                flag = int.TryParse(ReadLine(), out day);
                if(!flag)
                    Clear();
            } while ((day < 1 || day > 30) && !flag);
        }
        else if(year is 2004 or 2008 or 2012 or 2016 or 2020 or 2024 && month is 2)
        {
            do
            {
                flag = int.TryParse(ReadLine(), out day);
                if(!flag)
                    Clear();
            } while ((day < 1 || day > 29) && !flag);
        }
        else if(month is 2)
        {
            do
            {
                flag = int.TryParse(ReadLine(), out day);
                if(!flag)
                    Clear();
            } while ((day < 1 || day > 28) && !flag);
        }
        else
        {
            do
            {
                flag = int.TryParse(ReadLine(), out day);
                if(!flag)
                    Clear();
            } while ((day < 1 || day > 31) && !flag);
        }
        WriteLine();
        date = new(year, month, day);
}
void AddPallet(ref List<Pallet> pallets) // Добавляем паллет в список
{
    List<Box> boxes = [];
    Random random = new();
    DateOnly expDate, maxDate;
    double width, height, depth;
    bool flag;
    int id;
        {
        Clear();
        
            do
            {
                Write("Введите ID паллеты (должно быть 4-х значное значение): ");
                flag = int.TryParse(ReadLine(), out id);
                if(!flag)
                    Clear();
            }
            while ((id<1000 || id>9999)&&!flag);
        
            do
            {
                Write("\nВведите ширину паллеты в метрах : ");
                flag = double.TryParse(ReadLine(), out width);
                if(!flag)
                    Clear();
            }
            while (!flag);
        
            do
            {
                Write("\nВведите высоту паллеты в метрах : ");
                flag = double.TryParse(ReadLine(), out height);
                if(!flag)
                    Clear();
            }
            while (!flag);
        
            do
            {
                Write("\nВведите глубину паллеты в метрах : ");
                flag = double.TryParse(ReadLine(), out depth);
                if(!flag)
                    Clear();
            }
            while (!flag);
        WriteLine();
        expDate = new();
        maxDate = new();
        Pallet pallet = new(id,height,width,depth,expDate,maxDate,boxes);
        pallets.Add(pallet);
        Clear();
    }
}
void AddBox(Pallet pallet) // Добавляем коробку на паллет
{
    DateOnly expDate, dateOfProd;
    double width, height, depth, weight;
    int n;
    bool flag;
        Clear();
        WriteLine("Введите срок годности или дату производства коробки \n");
        do
        {
            Write("1-Ввести срок годности : 2-Ввести дату производства коробки ");
            flag = int.TryParse(ReadLine(),out n);
            if(!flag)
                Clear();
        } while ((n<1 || n>2) && !flag);
        if(n==1)
            GetDate(out expDate);
        else
        {
            GetDate(out dateOfProd);
            expDate = dateOfProd.AddDays(100);
        }
        do
        {
            Write("\nВведите ширину коробки в сантиметрах от 10 до 100 : ");
            flag = double.TryParse(ReadLine(), out width);
            if(!flag)
                Clear();
        }while ((width < 10 || width > 100) && !flag); 
        
        do
        {
            Write("\nВведите высоту коробки в сантиметрах от 10 до 100 : ");
            flag = double.TryParse(ReadLine(), out height);
            if(!flag)
                Clear();
        }while ((height < 10 || height > 100) && !flag);
        
        do
        {
            Write("\nВведите глубину коробки в сантиметрах от 10 до 100 : ");
            flag = double.TryParse(ReadLine(), out depth);
            if(!flag)
                Clear();
        }while ((depth < 10 || depth > 100) && !flag); 
        
        do
        {
            Write("\nВведите вес коробки в киллограммах : ");
            flag = double.TryParse(ReadLine(), out weight);
            if(!flag)
                Clear();
        }while (!flag);
        WriteLine();
        width = Math.Round(width/100, 2);
        height = Math.Round(height/100, 2);
        depth = Math.Round(depth/100, 2);
        Box box = new(width,height,depth,weight,expDate);
        pallet.Boxes.Add(box);
        pallet.ExpirationDate = GetMinDateBox(pallet.Boxes);
        pallet.maxDate = GetMaxDateBox(pallet.Boxes);
        Clear();
}
void Output(List<Pallet> pallets) // Вывод необходимой информации в консоль
{
        Clear();
        bool flag = false;
        foreach (var pallet in pallets)
        {
            if (pallet.Boxes==null)
                flag = false;
            else flag = true;
        }
        if (flag)
        {
            WriteLine("\n\t3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.\n");
            var _3pallet = pallets.OrderByDescending(pallet => pallet.maxDate).Take(3).ToList().OrderBy(pallet => pallet.Capacity);
            foreach (var pallet in _3pallet)
                WriteLine($"|ID:{pallet.ID}|Capacity:{pallet.Capacity}|\n");
            // Группируем и сортируем группы согласно заданию
            WriteLine("\n\tСгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.");
            var groups = pallets.OrderBy(pallet => pallet.ExpirationDate).ThenBy(pallet => pallet.Weight).GroupBy(pallet => pallet.ExpirationDate).ToList();
            foreach (var group in groups)
            {
                WriteLine($"\n|\t\tKey:{group.Key}\t");
                foreach (var pallet in group)
                {
                    WriteLine($"|\tID:{pallet.ID}\t|\tExpiration Date:{pallet.ExpirationDate}\t|\tWeight:{pallet.Weight}\t|");
                }
            }
        }
        else Write("\tНет информации\n");
        ReadKey();
        Clear();
}
public static void Main(string[] args) 
    {
        List<Pallet> pallets = [];
        int point,id;
        bool flag,flag1;
        Program pr = new();
        Clear();
        do
        {
            WriteLine("\n1 - Добавить новую паллету в список");
            WriteLine("2 - Добавить новую коробку на паллету");
            WriteLine("3 - Вывод необходимой информации");
            WriteLine("0 - Выход\n");
            do
            {
                flag = int.TryParse(ReadLine(), out point);
                if (!flag)
                    Clear();
            }while (!flag);
            switch(point) 
            {
                case 1 : 
                    {
                        pr.AddPallet(ref pallets);
                    }break;
                case 2 :
                    {
                        Write("Введите ID паллеты : ");
                        do
                        {
                            flag1 = int.TryParse(ReadLine(), out id);
                            if (!flag1)
                                Clear();
                        }while (!flag1);
                        foreach (var pallet in pallets)
                        {
                            if (pallet.ID == id)
                                pr.AddBox(pallet);
                            else Write("\nНе найдено\n");
                        }
                    }break;
                case 3 :
                    {
                        pr.Output(pallets);
                    }break;
            }
        } while (point!=0);
    }
}
class Box // Наша коробка
{
    public readonly DateOnly ExpirationDate;
    public readonly double Weight, Width, Height, Depth ,Capacity;
    public Box(double width, double height, double depth, double weight, DateOnly expDate) 
    {
        Width = width; Height = height; Depth = depth; Weight = weight;
        Capacity = Width*Height*Depth;
        ExpirationDate = expDate;
    }
}
class Pallet // Паллета помимо основных свойств содержит еще список коробок которые на ней расположены
{
    public DateOnly ExpirationDate, maxDate;
    public readonly int ID;
    public readonly double Weight,Height, Width, Depth, Capacity;
    public List<Box> Boxes = new List<Box>();
    double sumCapacity = 0;
    double sumWeight = 0;
    public Pallet(int id, double height, double width, double depth, DateOnly expDate, DateOnly max, List<Box> boxes) 
    {
        foreach (var box in boxes) 
        {
            sumCapacity += box.Capacity;
            sumWeight += box.Weight;
        }
        ID = id; Height = height; Width = width; Depth = depth; Boxes = boxes; ExpirationDate = expDate; maxDate = max;
        Capacity = Math.Round(width*height*depth + sumCapacity,1);
        Weight = sumWeight + 30;
    }
}