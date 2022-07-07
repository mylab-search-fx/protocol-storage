using System.Threading.Tasks;
using MyLab.ApiClient;
using MyLab.ProtocolStorage.Client.Models;

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
        Task PostEventAsync([Path]string protocolId, [JsonContent] ProtocolEvent eventObj);

        /// <summary>
        /// Searches for specified protocol items
        /// </summary>
        [Post("{protocolId}/searcher")]
        Task<SearchResult> SearchAsync([Path] string protocolId, [JsonContent] ClientSearchRequest request, [Header("X-Search-Token")] string searchToken);
    }
}