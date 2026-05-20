using Data.DTOs;
using Data.Entities;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainApp.Pages.Seller
{
    public class AdUpdateModel : PageModel
    {
        public readonly IHttpClientFactory HttpClientFactory;

        public readonly UserManager<User> _userManager;


        public AdUpdateModel(IHttpClientFactory httpClientFactory, UserManager<User> userManager)
        {
            HttpClientFactory = httpClientFactory;
            _userManager = userManager;

        }

        [BindProperty]
        public AdUpdateViewModel UpdateAd { get; set; } = new AdUpdateViewModel();

        [BindProperty]
        public List<ImagesUpdateViewModel> UpdateImages { get; set; } = new List<ImagesUpdateViewModel>();
        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.GetAsync($"api/Ad/{id}");

            if (response.IsSuccessStatusCode)
            {
                var AdDTO = await response.Content.ReadFromJsonAsync<AdDto>();
                if (AdDTO != null)
                {
                    
                    UpdateAd = new AdUpdateViewModel
                    {
                        AdID = AdDTO.AdID,
                        Title = AdDTO.Title,
                        Description = AdDTO.Description,
                        StartingPrice = AdDTO.StartingPrice,
                        StartDate = AdDTO.StartDate,
                        EndDate = AdDTO.EndDate,
                        Place = AdDTO.Place
                    };

                    
                    if (AdDTO.Images != null && AdDTO.Images.Any())
                    {
                        UpdateImages = AdDTO.Images.Select(img => new ImagesUpdateViewModel
                        {
                            Url = img.Url,
                            Description = img.Description
                        }).ToList();
                    }
                    else
                    {
                       
                        UpdateImages = new List<ImagesUpdateViewModel> { new ImagesUpdateViewModel() };
                    }

                    return Page();
                }

                ModelState.AddModelError(string.Empty, "Ad data is null.");
                return Page();
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error fetching: {response.StatusCode}");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
                return Page();
            }

            if (UpdateAd.StartDate >= UpdateAd.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Enddate must be after Startdate");
                return Page();
            }

            if (UpdateImages != null && UpdateImages.Any())
            {
                foreach (var image in UpdateImages)
                {
                    if (!string.IsNullOrWhiteSpace(image.Url))
                    {
                        UpdateAd.Images.Add(image);
                    }
                }
            }

            UpdateAd.SellerId = user.Id;

            ModelState.Remove("UpdateAd.SellerId");

            if (!ModelState.IsValid)
            {
                return Page();
            }





            if (UpdateImages != null && UpdateImages.Count > 0)
            {
                var validImages = UpdateImages.Where(img => !string.IsNullOrWhiteSpace(img.Url)).ToList();
                UpdateImages = validImages;
            }




            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.PutAsJsonAsync($"api/Ad/{UpdateAd.AdID}", UpdateAd);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error creating ad: {response.StatusCode} , {errorContent}");
                return Page();
            }
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.DeleteAsync($"api/Ad/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error deleting Ad: {response.StatusCode} , {errorContent}");
                return Page();
            }
        }
    }
}
