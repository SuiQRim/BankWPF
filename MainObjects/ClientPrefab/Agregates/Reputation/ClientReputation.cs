using System.Windows.Media;

namespace BankObjects.ClientPrefab.Agregates.Reputation
{
    abstract public class ClientReputation : IClientСomponentsData
    {
        public ClientReputation(string Reputatuion, int Level, Brush Color) {

            this.Type = Reputatuion;
            this.Level = Level;
            this.Color = Color;
        }

        public string Type { get; set; }
        public int Level { get; set; }
        public Brush Color { get; set; }

        public override string ToString() => $"{Type} репутация";

    }
}
