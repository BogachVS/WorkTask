using System.Xml.Serialization;
namespace WorkTask
{
    internal class FileReading
    {
        public static void SaveData(string filename, List<Pallet> pallets) 
        {
            XmlSerializer serializer = new(typeof(List<Pallet>));
            TextWriter writer = new StreamWriter(filename);
            serializer.Serialize(writer, pallets);
        }
        public static void LoadData(string filename, out List<Pallet>? pallets)
        {
            XmlSerializer serializer = new(typeof(List<Pallet>));
            FileStream writer = new(filename, FileMode.Open);
            pallets = serializer.Deserialize(writer) as List<Pallet>;
        }
    }
}
