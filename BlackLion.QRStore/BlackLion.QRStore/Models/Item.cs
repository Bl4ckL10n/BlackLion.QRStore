using SQLite;

namespace BlackLion.QRStore.Models
{
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
    }
}