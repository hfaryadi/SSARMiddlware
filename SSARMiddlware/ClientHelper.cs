using System.Collections.Generic;
using System.Linq;

namespace SSARMiddlware
{
    internal static class ClientHelper
    {
        static List<ClientList> Clients = new List<ClientList>()
        {
            new ClientList()
            {
                ClientId = "SSAR-Application",
                ClientSecret = "11E59E9BEAC14681E0631214A8C0FD6E-11E59E9BEAC24681E0631214A8C0FD6E-11E59E9BEAC34681E0631214A8C0FD6E"
            }
        };

        internal static bool HasClient(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                return false;
            }
            return Clients.Any(x => x.ClientId == clientId && x.ClientSecret == clientSecret);
        }
    }

    internal class ClientList
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
