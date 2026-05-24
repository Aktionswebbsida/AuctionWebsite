using Data.DTOs;
using Data.Entities;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        [BindProperty]
       public BidCreateViewModel AddBid {  get; set; } = new BidCreateViewModel();


        [BindProperty(SupportsGet =true)]
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            var client = _HttpClientFactory.CreateClient("APIClient");
            var response = await client.GetAsync($"/api/Ad/{Id}");
            if (response.IsSuccessStatusCode)
            {
                var ads = await response.Content.ReadFromJsonAsync<AdDto>();

                if (ads != null)
                {
                    Ads = new AdViewModel
                    {
                        AdID = ads.AdID,
                        Title = ads.Title,
                        Description = ads.Description,
                        Place = ads.Place,
                        StartDate = ads.StartDate,
                        EndDate = ads.EndDate,
                        SellerName = ads.SellerName,
                        StartingPrice = ads.StartingPrice,
                        Images = ads.Images.Select(i => new ImagesViewModel
                        {
                            Url = i.Url,
                            Description = i.Description
                        }).ToList()

                    };

                    return Page();
                }
            }

            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
                return Page();
            }
            AddBid.UserId = user.Id;

            ModelState.Remove("AddBid.UserId");

            AddBid.BidDate = DateTime.Now;

            var client = _HttpClientFactory.CreateClient("APIClient");
            Id = AddBid.AdID;
            var response = await client.PostAsJsonAsync("/bids",AddBid);

            if(response.IsSuccessStatusCode)
            {
                return RedirectToPage(new { Id = AddBid.AdID });
            }
            await OnGetAsync();
            ModelState.AddModelError(string.Empty, "low bid, try again");
            return Page();
        }
    }
}
