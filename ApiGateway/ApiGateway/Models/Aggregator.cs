using BrotliSharpLib;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ocelot.Configuration;
using Ocelot.Middleware;
using Ocelot.Multiplexer;
using System.Net;
using System.Net.Http.Headers;

namespace ApiGateway.Models
{
    public class Aggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
        {
            var users = new UserModel();
            var authentications = new AuthenticationModel();

            foreach (var response in responses)
            {
                string downStreamRouteKey = ((DownstreamRoute)response.Items["DownstreamRoute"]).Key;
                DownstreamResponse downstreamResponse = (DownstreamResponse)response.Items["DownstreamResponse"];
                byte[] downstreamResponseContent = await downstreamResponse.Content.ReadAsByteArrayAsync();

                if (downStreamRouteKey == "user")
                {
                    users = JsonConvert.DeserializeObject<UserModel>(DeCompressBrotli(downstreamResponseContent));
                }

                if (downStreamRouteKey == "authentication")
                {
                    authentications = JsonConvert.DeserializeObject<AuthenticationModel>(DeCompressBrotli(downstreamResponseContent));
                }
            }

            return GetUserAuth(authentications, users);
        }
        public DownstreamResponse GetUserAuth(AuthenticationModel authentication, UserModel user)
        {

            var userAuth = new UserAuthModel
            {
                MobileNumber = authentication.MobileNumber,
                Email = authentication.Email,
                Address = user.Address,
                Education = user.Education,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var aggregatedAnswer = JsonConvert.SerializeObject(userAuth);

            var stringContent = new StringContent(aggregatedAnswer)
            {
                Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
            };

            return new DownstreamResponse(stringContent, HttpStatusCode.OK, new List<KeyValuePair<string, IEnumerable<string>>>(), "OK");
        }
        private string DeCompressBrotli(byte[] responseContent)
        {
            return System.Text.Encoding.UTF8.GetString(responseContent);
        }
    }
}
