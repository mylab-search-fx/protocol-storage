using System.Threading.Tasks;
using MyLab.ApiClient;

namespace MyLab.ProtocolStorage.Client
{
    /// <summary>
    /// The token API contract
    /// </summary>
    [Api("v1/search-token", Key = "protocol-storage")]
    public interface ITokenApiV1
    {
        /// <summary>
        /// Creates token to search for protocol items without restrictions
        /// </summary>
        [Post("total")]
        Task<string> CreateTotalTokenAsync();

        /// <summary>
        /// Creates token to search for protocol items which owned by specified subject
        /// </summary>
        [Post("for-subject/{subjectId}")]
        Task<string> CreateTokenForSubjectAsync([Path]string subjectId);
    }
}
