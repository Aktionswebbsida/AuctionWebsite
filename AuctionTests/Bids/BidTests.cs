using AuctionApi.Hubs;
using Business.Interfaces;
using Business.Services;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionTests.Bids
{
    public class BidTests
    {
        public readonly Mock<IHubContext> _mockHub;
        public readonly Mock<IBidInterface> _mockbid;
        public readonly Mock<IBidRepository> _bidrepo;
          public readonly Mock<IHubClients> _mockClients;
        public readonly Mock<IClientProxy> _mockClientProxy;

        public BidTests()
        {
          _mockbid = new Mock<IBidInterface>();
            _bidrepo = new Mock<IBidRepository>();
            _mockHub = new Mock<IHubContext>();
        _mockClients = new Mock<IHubClients>();
            _mockClientProxy = new Mock<IClientProxy>();
        }


        [Fact]

        public async Task Test_IF_Two_People_Can_bid()
        {
            var fakebids1 = new Bid
            {
                BidID = 1,
                BidAmount = 500,
                UserId = 1,
                BidDate = DateTime.UtcNow,

            };
              var fakebids2 = new Bid
              {
                  BidID = 2,
                  BidAmount = 500,
                  UserId = 2,
                  BidDate = DateTime.UtcNow,

              };
            _bidrepo.Setup(repo => repo.CreateBidAsync(It.IsAny<Bid>())).ReturnsAsync(fakebids1);
            _bidrepo.Setup(repo => repo.CreateBidAsync(It.IsAny<Bid>())).ReturnsAsync(fakebids2);
            _bidrepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var dto = new BidCreateDto
            {
                AdID = 1,
                BidAmount = 1,
                BidDate = DateTime.Now,
                UserId = 1,
                UserName = "Test",
            };

            var dto1 = new BidCreateDto
            {
                AdID = 2,
                BidAmount = 2,
                BidDate = DateTime.Now,
                UserId = 2,
                UserName = "Test2",
            };
            _mockClients.Setup(c => c.Group("adId:1")).Returns(_mockClientProxy.Object);
            _mockClients.Setup(c => c.Group("adId:2")).Returns(_mockClientProxy.Object);
            _mockHub.Setup(x => x.Clients).Returns(_mockClients.Object);

           
          

            var service = new  BidService( _bidrepo.Object, _mockHub.Object);

            await service.CreateBidAsync(dto);


            await service.CreateBidAsync(dto1);

            _mockClientProxy.Verify(p => p.SendCoreAsync("UpdateHighestBid", It.IsAny<object[]>(), default),
                Times.Exactly(2));


        }
    }
}
