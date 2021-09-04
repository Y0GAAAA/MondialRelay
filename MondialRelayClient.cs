using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

using HtmlAgilityPack;
using MondialRelay.Json;
using MondialRelay.Exceptions;

namespace MondialRelay
{

    public class MondialRelayClient
    {

        private const string SEARCH_REQUEST_ENDPOINT = "http://mondialrelay.fr/_mvc/fr-FR/SuiviExpedition/RechercherJsonResponsive";

        private readonly HttpClient client = new HttpClient();
        private readonly HtmlDocument document = new HtmlDocument();

        private byte[] PayloadBytes { get; set; }
        private HttpContent PayloadContent { 
            get {
                var content = new ByteArrayContent(PayloadBytes);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
                return content;
            }
        }

        public MondialRelayClient(ulong trackingNumber, int postalCode)
        {
            this.client.DefaultRequestHeaders.Host = "www.mondialrelay.fr";
            this.PayloadBytes = Encoding.Default.GetBytes($"IdChampDeRetour=suivie_mon_colis&NumeroExpedition={trackingNumber}&CodePostal={postalCode}");
        }

        /// <summary>
        /// Returns the last delivery step that has been validated.
        /// </summary>
        /// <returns></returns>
        public async Task<PackageDeliveryProgress> GetLastCompletedTask()
        {

            var response = await client.PostAsync(SEARCH_REQUEST_ENDPOINT, PayloadContent);
            var json = await response.Content.ReadAsStringAsync();

            var objectifiedResponse = MondialRelayQueryResponse.FromJsonString(json);

            if (!objectifiedResponse.Success)
                throw new PackageNotFoundException();

            document.LoadHtml(objectifiedResponse.Message);

            var steps = document.DocumentNode.SelectNodes("div/div/div[1]/div/ul/li");

            // not that we care but TakeWhile + Count should be more efficient than Count()
            int stepIndex = steps.TakeWhile(stepElement => stepElement.GetClasses().Contains("validate")).Count();

            return (PackageDeliveryProgress) stepIndex;
        }

    }

}
