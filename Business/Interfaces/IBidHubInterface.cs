using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface IBidHubInterface 
    {
        public Task UpdateHighestBid(int AdId, decimal amount);
    }
}
