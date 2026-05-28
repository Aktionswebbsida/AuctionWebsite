using Data.DTOs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IBidHubInterface 
    {
        public Task UpdateHighestBid(int AdId, decimal amount);

        public Task YouWon (WinnerDTO Winner );

        public Task BuyNow(BuyNowDTO Winner);
    }
}
