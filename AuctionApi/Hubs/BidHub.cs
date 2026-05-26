using Business.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AuctionApi.Hubs
{
    public class BidHub : Hub<IBidHubInterface>
    {
        public override async Task OnConnectedAsync()
        {
            var adId = Context.GetHttpContext()?.Request.Query["adId"].ToString();
            if (!string.IsNullOrWhiteSpace(adId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"adId:{adId}");
            }
            await base.OnConnectedAsync();
        }
    }
}
