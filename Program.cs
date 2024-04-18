using System.Runtime.Serialization;
using System.Xml;
using static System.Console;

public class Program
{
    static void SaveData<T>(T serializableObject, string filepath) // Сериализуем объекты списка в файл (я сначала сгенерировал 30 паллетов затем записал их в xml файл чтобы потом достать)
    {
        var serializer = new DataContractSerializer(typeof(T));
        var settings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "\t",
        };
        var writer = XmlWriter.Create(filepath, settings);
        serializer.WriteObject(writer, serializableObject);
        writer.Close();
    }
    static T LoadData<T>(string filepath) // Десериализуем объекты из файла т.е. добавляем в наш список
    {
        var fileStream = new FileStream(filepath, FileMode.Open);
        var reader = XmlDictionaryReader.CreateTextReader(fileStream, new XmlDictionaryReaderQuotas());
        var serializer = new DataContractSerializer(typeof(T));
        T serializableObject = (T)serializer.ReadObject(reader, true);
        reader.Close();
        fileStream.Close();
        return serializableObject;
    }
    static void Output(List<Pallet> pallets) // Вывод необходимой информации в консоль
    {
        bool flag = false;
        foreach (var pallet in pallets)
        {
            if (pallet.Boxes==null)
                flag = false;
            else flag = true;
        }
        foreach (var pallet in pallets) // Так как в документе дата была строкой, необходимо преобразование для дальнейших действий
        {
            pallet.maxDate = DateOnly.Parse(pallet.TempMax);
            pallet.ExpirationDate = DateOnly.Parse(pallet.TempExp);
        }
        if (flag)
        {
            WriteLine("\n\t3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема.\n");
            var _3pallet = pallets.OrderByDescending(pallet => pallet.maxDate).Take(3).ToList().OrderBy(pallet => pallet.Capacity);
            foreach (var pallet in _3pallet)
                WriteLine($"|ID:{pallet.ID}|Capacity:{pallet.Capacity}|Max Date:{pallet.maxDate}|\n");
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
        List<Pallet> pallets = LoadData<List<Pallet>>("Pallets.xml");
        Output(pallets);
    }
}
[DataContract] // Контракт данных необходим для привязки данных объекта 
public class Box // Наша коробка
{
    public DateOnly ExpirationDate;
    [DataMember] // Выбираем члены класса которые потом будем сериализовать
    public double Weight;
    [DataMember]
    public double Width;
    [DataMember]
    public double Height;
    [DataMember]
    public double Depth;
    [DataMember]
    public double Capacity;
    public Box(double width, double height, double depth, double weight, DateOnly expDate)
    {
        Width = width; Height = height; Depth = depth; Weight = weight;
        Capacity = Math.Round(Width*Height*Depth, 2, MidpointRounding.ToEven);
        ExpirationDate = expDate;
    }
}
[DataContract]
public class Pallet // Паллета помимо основных свойств содержит еще список коробок которые на ней расположены
{
    public DateOnly ExpirationDate;
    public DateOnly maxDate;
    [DataMember]
    public string TempExp; // Временные переменные нужны так как при загрузке документа дата не считывалась, видимо xml документы не поддерживают тип DateOnly
    [DataMember]
    public string TempMax;
    [DataMember]
    public int ID;
    [DataMember]
    public double Weight;
    [DataMember]
    public double Height;
    [DataMember]
    public double Width;
    [DataMember]
    public double Depth;
    [DataMember]
    public double Capacity;
    [DataMember]
    public List<Box> Boxes = new List<Box>();

    double sumCapacity = 0;
    double sumWeight = 0;
    public Pallet(int id, double height, double width, double depth, string expDate, string max, List<Box> boxes)
    {
        foreach (var box in boxes)
        {
            sumCapacity += box.Capacity;
            sumWeight += box.Weight;
        }
        ID = id; Height = height; Width = width; Depth = depth; Boxes = boxes; TempExp = expDate; TempMax = max;
        Capacity = Math.Round(width*height*depth + sumCapacity, 2);
        Weight = sumWeight + 30;

    }
}
