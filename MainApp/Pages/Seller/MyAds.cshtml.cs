using Data.DTOs;
using Data.Entities;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainApp.Pages.Seller
{
    public class MyAdsModel : PageModel
    {
        public readonly IHttpClientFactory HttpClientFactory;
        public readonly UserManager<User> _userManager;

        public MyAdsModel(IHttpClientFactory httpClientFactory, UserManager<User> userManager)
        {
            HttpClientFactory = httpClientFactory;
            _userManager = userManager;
        }

        [BindProperty]
        public List<AdViewModel> Ads { get; set; } = new List<AdViewModel>();

        public string FullName { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
                return Page();
            }
            if(user != null)
            {
                FullName = $"{user.FirstName} {user.LastName}";
            }
            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.GetAsync($"/api/Ad/seller/{user.Id}");
            if (response.IsSuccessStatusCode)
            {
                var ads = await response.Content.ReadFromJsonAsync<List<AdDto>>();

                if (ads != null)
                {
                    Ads = ads.Select(x => new AdViewModel
                    {
                        AdID = x.AdID,
                        Title = x.Title,
                        Description = x.Description,
                        Place = x.Place,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        SellerName = x.SellerName,
                        StartingPrice = x.StartingPrice,
                        Images = x.Images.Select(i => new ImagesViewModel
                        {
                            Url = i.Url,
                            Description = i.Description
                        }).ToList()

                    }).ToList();
                }
                else
                {
                    // Handle the case when ads is null
                    // You might want to log this or set Ads to an empty list
                    Ads = new List<AdViewModel>();
                }
            }
            else
            {
                // Handle the case when the API call fails
                // You might want to log this or set Ads to an empty list
                Ads = new List<AdViewModel>();
            }
            return Page();
        }
    }
}
