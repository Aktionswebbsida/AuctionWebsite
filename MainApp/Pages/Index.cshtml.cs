using Data.DTOs;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainApp.Pages
{
    public class IndexModel : PageModel
    {
        public readonly IHttpClientFactory HttpClientFactory;

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public List<AdViewModel> Ads { get; set; } = new List<AdViewModel>();
        public async Task OnGetAsync()
        {
            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.GetAsync("/api/Ad");
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
        }
    }
}
