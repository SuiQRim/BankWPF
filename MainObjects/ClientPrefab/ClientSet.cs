
namespace BankObjects.ClientPrefab
{
    public class ClientSet
    {
        public ClientSet()
        {

        }

        public ClientSet(Client client)
        {
            this.AccountId = client.AccountID;
            this.Name = client.Name;
            this.PhoneNumber = client.PhoneNumber;
            this.Status = client.Status.Level;
            this.Reputation = client.Reputation.Level;
            this.DateToCreate = client.AccountCreateDate;
        }

        public int Id { get; set; }

        public string AccountId { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public int Status { get; set; }

        public int Reputation { get; set; }

        public string DateToCreate { get; set; }

    }
}
