using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Umamusume;
using Umamusume.Model;

namespace UmamusumeFriendSearch
{
    class FriendSearch
    {
        private readonly UmamusumeClient _client;

        public FriendSearch(UmamusumeClient client)
        {
            _client = client;
        }

        public async Task<FriendSearchResponse> Search(int viewerId)
        {
            var request = new FriendSearchRequest
            {
                friend_viewer_id = viewerId
            };

            return await Task.Run(() => _client.RetryRequest(request));
        }
    }
}
