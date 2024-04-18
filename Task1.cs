using static System.Console;

class Program 
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
static DateOnly GetRandomDate() 
{
    Random rand = new Random();
    int year = rand.Next(2018,2024); // Случайный год за предыдущие 5 лет
    int month = rand.Next(1,13); // Случайный месяц
    int day;
    if (month is 4 or 6 or 9 or 11) // Случайный день в зависимости от месяца и года
        day = rand.Next(1, 31);
    else if (year == 2020 && month == 2)
        day = rand.Next(1, 30);
    else if (year != 2020 && month == 2)
        day = rand.Next(1, 29);
    else
        day = rand.Next(1,32);
    DateOnly date = new DateOnly(year, month, day);
    return date;
}
// Так как в тз не указано сколько коробок может быть на одном паллете я решил сгенерировать рандомно 10 паллет с 10 коробками на каждой
static List<Pallet> Generate() 
{
    Random rand = new();
    double boxWeight,boxWidth, boxHeight, boxDepth, palWidth, palHeight, palDepth, n = 10;
    int palID;
    DateOnly expDate, palExpDate, palMaxDate;
    List<Pallet>pallets = [];
    while (n>0) 
    {
        List<Box> boxes = [];
        for(int i = 0; i<10; i++) 
        {
            boxWidth = Math.Round((double)rand.Next(1,3)/rand.Next(2,5),1); // Пусть единицей измерения будут метры, поэтому я делю результат чтобы получить не целое число(размер коробки может быть меньше метра)
            boxHeight = Math.Round((double)rand.Next(1, 3)/rand.Next(2, 5),1);
            boxDepth = Math.Round((double)rand.Next(1, 3)/rand.Next(2, 5),1);
            boxWeight = rand.Next(1, 6);
            expDate = GetRandomDate();
            Box box = new(boxWidth,boxHeight,boxDepth,boxWeight,expDate);
            boxes.Add(box);
        }
        palID = rand.Next(1000,9999); // Случайный ID пусть будет 4-х значным числом
        palWidth = Math.Round((double)rand.Next(2,5),2);
        palHeight = Math.Round((double)rand.Next(2, 5), 2);
        palDepth = Math.Round((double)rand.Next(2, 5), 2);
        palExpDate = GetMinDateBox(boxes);
        palMaxDate = GetMaxDateBox(boxes);
        Pallet pallet = new(palID,palHeight,palWidth,palDepth,palExpDate,palMaxDate,boxes);
        pallets.Add(pallet);
        n--;
    }
    return pallets;
}
    static void Main(string[] args) 
    {
        List<Pallet> pallets = Generate();
        WriteLine("\n3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.\n");
        var _3pallet = pallets.OrderByDescending(pallet => pallet.maxDate).Take(3).ToList().OrderBy(pallet => pallet.Capacity);
        foreach (var pallet in _3pallet)
            WriteLine($"|ID:{pallet.ID}|Capacity:{pallet.Capacity}|\n");
        /* Группируем и сортируем группы согласно заданию, однако поскольку
         в этой программе значения генерируются рандомно высока вероятность что 
        сроки годности будут разными в каждой паллете, поэтому в каждой группе будет по одному элементу
        впринципе это исправляется если мы начнем генерировать больше паллетов, тогда вероятность того 
        что у некоторых элементов начнет совпадать дата, возрастает*/
        WriteLine("\nСгруппировать все паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу.");
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
    public readonly DateOnly ExpirationDate;
    public readonly DateOnly maxDate;
    public readonly int ID;
    public readonly double Weight,Height, Width, Depth, Capacity;
    public readonly List<Box> Boxes = new List<Box>();
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