using System.Windows.Media;

namespace BankObjects.ClientPrefab.Agregates.Status
{
    internal class NotExistingStatus : ClientStatus
    {
        public NotExistingStatus() : base("Не существующий статус", -1, -1, Brushes.MediumSeaGreen) { }
    }
}
