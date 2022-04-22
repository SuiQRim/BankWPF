using System.Windows.Media;

namespace BankObjects.ClientPrefab.Agregates.Status
{
    public class ClientStatus : IClientСomponentsData
    {
        public ClientStatus(string Status, int Level, double Commission, Brush Color) 
        {
            this.Type = Status;
            this.Level = Level;
            this.Commission = Commission;
            this.Color = Color;
        }

        public string Type { get; set; }

        public int Level { get; set; }

        public double Commission { get; set; }

        public Brush Color { get; set; }

        public override string ToString() => Type;

    }
}
