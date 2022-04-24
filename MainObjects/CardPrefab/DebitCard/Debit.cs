using BankObjects.ClientPrefab.Agregates;
using BankObjects.CardPrefab.Exceptions;
using System.Data.SqlClient;
using BankObjects.DataBase.Messages;
using BankObjects.DataBase;

namespace BankObjects.CardPrefab.DebitCard
{
    public class Debit : Card
    {

        public Debit(string HolderName, string HolderID, BankEvents bankEvents) 
            : base ("8596", 12, HolderName, HolderID, bankEvents) { CashBack = 0; Balance = 0; }

        public Debit(ClientCompetence competence, string HolderName, string HolderID, BankEvents bankEvents ) 
            : base(competence, "8596", 12, HolderName, HolderID, bankEvents) { }

        public Debit(DebitSet debitSet, BankEvents bankEvents) : base ( debitSet ,bankEvents)
        {
            CashBack = debitSet.CashBack;
        }
        protected override void KitSart(ClientCompetence competence)
        {
            Balance = DebitController.DebitCardCash(1000000);
            CashBack = DebitController.DebitCardCashBack(Balance);
        }


        public override void UpData()
        {
            Balance += CashBack;
            CashBack = 0;
            base.UpData();
        }


        private double _CashBack;
        public double CashBack {
            get => _CashBack;
            set => Set(ref _CashBack, value);
        }

        protected override void TransactionCalculate(TransactionMessage transactionMessage)
        {
            CashBack += transactionMessage.CashBack;
            Balance -= transactionMessage.Cash;
        }

        protected override TransactionMessage TransactionSetter(TransactionMessage transactionMessage)
        {

            transactionMessage.Cash -= transactionMessage.CashBack;
            Balance += transactionMessage.Cash;

            return transactionMessage;
        }

        protected override void TransactionValidator(TransactionMessage transactionMessage)
        {
            if (transactionMessage.Cash > Balance)
            {
                throw new NotEnoughFundsException(this, transactionMessage.Cash);
            }

        }
    }
}
