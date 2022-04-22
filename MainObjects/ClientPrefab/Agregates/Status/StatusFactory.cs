namespace BankObjects.ClientPrefab.Agregates.Status
{
    internal static class StatusFactory
    {
        public static ClientStatus GetStatusUsingLVL(int statusLVL)
        {
            return statusLVL switch
            {
                1 => new Individual(),
                2 => new Entity(),
                3 => new V_I_P(),
                _ => new NotExistingStatus(),
            };
        }
    }
}
