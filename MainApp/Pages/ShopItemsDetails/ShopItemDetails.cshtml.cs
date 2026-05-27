using Data.DTOs;
using Data.Entities;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainApp.Pages.ShopItemsDetails
{
    public class ShopItemDetailsModel : PageModel
    {
        public readonly IHttpClientFactory _HttpClientFactory;
        public readonly UserManager<User> _userManager;

        public ShopItemDetailsModel(IHttpClientFactory httpClientFactory, UserManager<User> userManager)
        {
            _HttpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        public AdViewModel Ads { get; set; } = new AdViewModel();

        public List<BidViewModel> Bids { get; set; } = new List<BidViewModel>();

        [BindProperty]
        public BidCreateViewModel AddBid { get; set; } = new BidCreateViewModel();

        [BindProperty]
        public WinnerViewModel Winner { get; set; } = new WinnerViewModel();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public bool IsAuctionClosed { get; set; } = false;
        public async Task<IActionResult> OnGetAsync()
        {
          
           
            var client = _HttpClientFactory.CreateClient("APIClient");
            var response = await client.GetAsync($"/api/Ad/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }

           var ads = await response.Content.ReadFromJsonAsync<AdDto>();
            if(ads == null)
            {
                return RedirectToPage("/Index");
            }
            Ads = new AdViewModel
            {
                AdID = ads.AdID,
                Title = ads.Title,
                Description = ads.Description,
                Place = ads.Place,
                StartingPrice = ads.StartingPrice,
                StartDate = ads.StartDate,
                EndDate = ads.EndDate,
                IsClosed = ads.IsClosed,
                WinnerId = ads.WinnerId,
                SellerId = ads.SellerId,
                SellerName = ads.SellerName,
                Images = ads.Images.Select(img => new ImagesViewModel
                {
                    Url = img.Url,
                    Description = img.Description
                }).ToList()







            };
            IsAuctionClosed = Ads.IsClosed || DateTime.Now >= Ads.EndDate;

            var bidsResponse = await client.GetAsync($"/api/Bid/ad/{Id}");
          if(bidsResponse.IsSuccessStatusCode)
          {
              var bids = await bidsResponse.Content.ReadFromJsonAsync<List<BidDto>>();
              if(bids != null)
              {
                    var bidshistory = bids.Where(b => b.AdID == Id).ToList();
                   var highestBid = bidshistory.OrderByDescending(b => b.BidAmount).FirstOrDefault();
                    if (highestBid != null)
                    {
                        Ads.StartingPrice = highestBid.BidAmount;
                    }

                    Bids = bidshistory.OrderByDescending(b => b.BidDate).Select(b => new BidViewModel
                    {
                      BidID = b.BidID,
                      BidAmount = b.BidAmount,
                      BidDate = b.BidDate,
                      UserId = b.UserId,
                      AdID = b.AdID,
                      UserName = b.UserName,
                      
                  }).ToList();
              }
          }
            return Page();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var errormessage = "User is not logged in please log in";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return new BadRequestObjectResult(errormessage);
                ModelState.AddModelError(string.Empty, errormessage);
                return Page();
            }
            AddBid.UserId = user.Id;

            ModelState.Remove("AddBid.UserId");

            AddBid.BidDate = DateTime.Now;

            var client = _HttpClientFactory.CreateClient("APIClient");
            Id = AddBid.AdID;

            await OnGetAsync();
            if(IsAuctionClosed)
            {
                var errormessage = "Auction is closed";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return new BadRequestObjectResult(errormessage);
                ModelState.AddModelError(string.Empty, errormessage);
                return Page();
            }
            if(AddBid.BidAmount > (Ads.StartingPrice * 100))
            {
                var errormessage = "100 times at a time try again";
                if(Request.Headers["X-Requested-With"] == "XMLHttpRequest") return new BadRequestObjectResult(errormessage);
                ModelState.AddModelError(string.Empty, errormessage);
                return Page();
            }

            if(AddBid.BidAmount <= Ads.StartingPrice)
            {
                var errormessage = "bid was to low try again";
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return new BadRequestObjectResult(errormessage);
                ModelState.AddModelError(string.Empty, errormessage);
                return Page();
            }
            var response = await client.PostAsJsonAsync("/bids",AddBid);

            if (response.IsSuccessStatusCode)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Headers["Accept"].ToString().Contains("application/json"))
                {
                    return new OkResult();
                }
                return RedirectToPage(new { Id = AddBid.AdID });
            }
            var apierror = await response.Content.ReadAsStringAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Content(apierror);
            }
            ModelState.AddModelError(string.Empty, apierror);
            return Page();

        }

        
       
    }
}
