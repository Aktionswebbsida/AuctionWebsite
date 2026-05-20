using Data.DTOs;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainApp.Pages.ShopItemsDetails
{
    public class ShopItemDetailsModel : PageModel
    {
        public readonly IHttpClientFactory _HttpClientFactory;

        public ShopItemDetailsModel(IHttpClientFactory httpClientFactory)
        {
            _HttpClientFactory = httpClientFactory;
        }

        public AdViewModel Ads { get; set; } = new AdViewModel();


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
    }
}
