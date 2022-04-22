using System.Windows.Media;

namespace BankObjects.ClientPrefab.Agregates
{
    internal interface IClientСomponentsData
    {
        public string Type { get; set; }

        public int Level { get; set; }

        public Brush Color { get; set; }
    }
}
