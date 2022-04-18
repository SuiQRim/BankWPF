using BankObjects.ClientPrefab.Agregates.Reputation;
using BankObjects.ClientPrefab.Agregates.Status;
using BankObjects.CardPrefab.DebitCard;
using BankObjects.CardPrefab.CreditCard;
using BankObjects.CardPrefab.Invest;

namespace BankObjects.ClientPrefab.Agregates
{

    public class ClientCompetence
    {
        public ClientCompetence(ClientStatus status, ClientReputation reputation )
        {
            MaxDebitCard = DebitController.MaxCountDebit(status, reputation);
            MaxCreditCard = CreditController.MaxCountCredit(status, reputation);

            CreditPercent = CreditController.CreditPrecent(reputation, status);
            InvestPrecent = InvestController.InvestmenPrecent(reputation, status);
        }
        
        //Максимальное кол-во дебетовых карт
        public int MaxDebitCard;

        //Максимальное кол-во кредитных карт
        public int MaxCreditCard;

        //Процент выручки с кредитной карты
        public double CreditPercent { get; set; }

        //Процент выручки с кредитной карты
        public double InvestPrecent { get; set; }
    }
}
