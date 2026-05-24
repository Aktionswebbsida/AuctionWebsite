using Business.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace AuctionApi.Hubs
{
    public class BidHub : Hub<IBidHubInterface>
    {
        public override async Task OnConnectedAsync()
        {
            var clientId = Context.GetHttpContext()?.Request.Query["clientId"].ToString();
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"clientId:{clientId}");
            }
            await base.OnConnectedAsync();
        }
    }
}
