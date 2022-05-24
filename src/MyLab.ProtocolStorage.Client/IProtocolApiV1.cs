using System.Threading.Tasks;
using MyLab.ApiClient;
using MyLab.ProtocolStorage.Models;
using Newtonsoft.Json.Linq;

namespace MyLab.ProtocolStorage.Client
{
    /// <summary>
    /// The protocols API contract
    /// </summary>
    [Api("v1/protocols", Key = "protocol-storage:protocols")]
    public interface IProtocolApiV1
    {
        /// <summary>
        /// Pushes event into specified protocol
        /// </summary>
        [Post("{protocolId}/collector")]
        Task PushEventAsync([Path]string protocolId, [JsonContent] PushProtocolEventRequest request);

        /// <summary>
        /// Searches for specified protocol items
        /// </summary>
        [Post("{protocolId}/collector")]
        Task<JObject> SearchAsync([Path] string protocolId, [JsonContent] ClientSearchRequest request, [Header("X-Search-Token")] string searchToken);
    }
}