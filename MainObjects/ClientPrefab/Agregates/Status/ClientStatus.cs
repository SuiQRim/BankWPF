using System.Windows.Media;

namespace BankObjects.ClientPrefab.Agregates.Status
{
    public class ClientStatus
    {
        public ClientStatus(string Status, int Level, double Commission, Brush Color) 
        {
            this.Status = Status;
            this.Level = Level;
            this.Commission = Commission;
            this.Color = Color;
        }

        public static ClientStatus GetStatusUsingLVL(int statusLVL) 
        {
            return statusLVL switch
            {
                1 => new Individual(),
                2 => new Entity(),
                3 => new V_I_P(),
                _ => new Individual(),
            };
        }

        public string Status { get; set; }

        public int Level { get; set; }

        public double Commission { get; set; }

        public Brush Color { get; set; }

        public override string ToString() => Status;

    }
}
