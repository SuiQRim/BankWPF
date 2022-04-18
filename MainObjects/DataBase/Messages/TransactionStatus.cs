using System.Windows.Media;

namespace BankObjects.EventMessages.Transaction
{
    public abstract class TransactionStatus
    {
        public TransactionStatus(string Status, Brush Color)
        {

            this.Status = Status;
            this.Color = Color;
        }
        public string Status { get; private set; }

        public Brush Color { get; private set; }

        public override string ToString() => $"Запрос {Status}";

    }

    public class TransactionConfirmed : TransactionStatus
    {
        public TransactionConfirmed() : base("выполнен", Brushes.ForestGreen) { }
    }

    public class TransactionRejected : TransactionStatus
    {
        public TransactionRejected() : base("откленен", Brushes.OrangeRed) { }
    }

    public class TransactionInWaiting : TransactionStatus
    {
        public TransactionInWaiting() : base("в ожидании", Brushes.CadetBlue) { }
    }
}
