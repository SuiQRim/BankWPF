using System.Windows.Media;

namespace BankObjects.ClientPrefab.Agregates.Reputation
{
    abstract public class ClientReputation
    {
        public ClientReputation(string Reputatuion, int Level, Brush Color) {

            this.Reputatuion = Reputatuion;
            this.Level = Level;
            this.Color = Color;
        }
        public static ClientReputation GetReputationUsingLVL(int statusLVL)
        {
            return statusLVL switch
            {
                1 => new Terribly(),
                2 => new Medium(),
                3 => new Good(),
                4 => new Perfect(),
                _ => new Medium()
            };
        }

        public string Reputatuion { get; set; }
        public int Level { get; set; }
        public Brush Color { get; set; }

        public override string ToString() => $"{Reputatuion} репутация";

    }
}
