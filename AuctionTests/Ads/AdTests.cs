using Business.Interfaces;
using Business.Services;
using Data.DTOs;
using Data.Entities;
using Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuctionTests.Ads
{
    public class AdTests
    {
        public readonly Mock<IHubContext> _mockHub;
        public readonly Mock<IAdInterface> _mockad;
        public readonly Mock<IAdRepository> _adrepo;
        public readonly Mock<IUserRepository> _userrepo;
        public readonly Mock<IHubClients> _mockClients;
        public readonly Mock<IClientProxy> _mockClientProxy;

        public AdTests()
        {
            _mockad = new Mock<IAdInterface>();
            _adrepo = new Mock<IAdRepository>();
            _userrepo = new Mock<IUserRepository>();
            _mockHub = new Mock<IHubContext>();
            _mockClients = new Mock<IHubClients>();
            _mockClientProxy = new Mock<IClientProxy>();
        }


        [Fact]
        public async Task Test_CreateAd()
        {
            var adDto = new AdCreateDto
            {
              
               
               
                Title = "Test",
                StartingPrice = 1,

                
            };

            _adrepo.Setup(repo => repo.AddAdAsync(It.IsAny<Ad>())).ReturnsAsync(new Ad { Title =adDto.Title});
            _adrepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);
            
            var service = new AdService(_adrepo.Object, _mockHub.Object);
            var result = await service.CreateAdAsync(adDto);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);

            _adrepo.Verify(x => x.AddAdAsync(It.Is<Ad>(x => x.Title == "Test")), Times.Once());

        }
        [Fact]
        public async Task Test_Buy_Now()
        {
            var user1 = new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                UserName = "Test",

            };
            var ad = new Ad
            {
                AdID = 1,
                WinnerId = 1,
                IsSold = true,
                Title = "Test",
                StartingPrice = 1,

                IsClosed = true
            };
            _userrepo.Setup(repo => repo.CreateAsync(It.IsAny<User>())).ReturnsAsync(user1);
            _userrepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);
            _adrepo.Setup(repo => repo.AddAdAsync(It.IsAny<Ad>())).ReturnsAsync(ad);
            _adrepo.Setup(repo => repo.BuyNow(1, 1)).ReturnsAsync(ad);
            _adrepo.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            
            _mockClients.Setup(c => c.Group("adId:1")).Returns(_mockClientProxy.Object);
            _mockHub.Setup(x => x.Clients).Returns(_mockClients.Object);




            var service = new AdService(_adrepo.Object, _mockHub.Object);

            await service.BuyNow(1, 1);


          

            _mockClientProxy.Verify(p => p.SendCoreAsync("BuyNow", It.IsAny<object[]>(), default),
                Times.Once());


        }

    }
}
