using System.Data.Entity;
using System.Data.SqlClient;
using System.Collections.Generic;
using BankObjects.ClientPrefab;
using BankObjects.CardPrefab.DebitCard;
using BankObjects.CardPrefab.CreditCard;
using BankObjects.CardPrefab.Invest;

namespace BankObjects.EventMessages
{
    internal class BankDBContext : DbContext
    {
        public BankDBContext(string sqlConnection): base (sqlConnection) { }

        public DbSet<ClientSet> Client { get; set; } 

        public DbSet<DebitSet> DebitCard { get; set; }

        public DbSet<CreditSet> CreditCard { get; set; }

        public DbSet<InvestmentSet> Investment { get; set; }
    }
}
