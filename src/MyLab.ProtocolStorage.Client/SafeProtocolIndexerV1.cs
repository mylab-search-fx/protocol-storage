using System;
using System.Threading.Tasks;
using MyLab.Log.Dsl;
using MyLab.ProtocolStorage.Client.Models;

namespace MyLab.ProtocolStorage.Client
{
    /// <summary>
    /// Posts the protocol events without exceptions
    /// </summary>
    public class SafeProtocolIndexerV1
    {
        private readonly IProtocolApiV1 _initialApi;
        private readonly IDslLogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="SafeProtocolIndexerV1"/>
        /// </summary>
        public SafeProtocolIndexerV1(IProtocolApiV1 initialApi)
        {
            _initialApi = initialApi ?? throw new ArgumentNullException(nameof(initialApi));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SafeProtocolIndexerV1"/>
        /// </summary>
        public SafeProtocolIndexerV1(IProtocolApiV1 initialApi, IDslLogger logger)
        {
            _initialApi = initialApi ?? throw new ArgumentNullException(nameof(initialApi));
            _logger = logger;
        }

        /// <summary>
        /// Pushes event into specified protocol
        /// </summary>
        public async Task PostEventAsync(string protocolId, ProtocolEvent eventObj)
        {
            try
            {
                await _initialApi.PostEventAsync(protocolId, eventObj);
            }
            catch (Exception e)
            {
                _logger?.Error("Protocol writing error", e)
                    .AndFactIs("protocol-id", protocolId)
                    .AndFactIs("event-obj", eventObj)
                    .Write();
            }
        }
    }
}
