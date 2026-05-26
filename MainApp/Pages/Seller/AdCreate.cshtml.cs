using Data.Entities;
using MainApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace MainApp.Pages.Seller
{
    public class AdCreateModel : PageModel
    {
        public readonly IHttpClientFactory HttpClientFactory;
        public readonly UserManager<User> _userManager;



        public AdCreateModel(IHttpClientFactory httpClientFactory, UserManager<User> userManager)
        {
            HttpClientFactory = httpClientFactory;
            _userManager = userManager;

        }

        [BindProperty]
        public AdCreateViewModel NewAd { get; set; } = new AdCreateViewModel();

        [BindProperty]
        public List<ImagesCreateViewModel> NewImages { get; set; } = new List<ImagesCreateViewModel>();


        public void OnGet()
        {
            if(NewImages == null || NewImages.Count == 0)
            {
                NewImages = new List<ImagesCreateViewModel>
                {
                    new ImagesCreateViewModel()
                };
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "User not found. Please log in again.");
                return Page();
            }

            if (NewAd.StartDate >= NewAd.EndDate)
            {
                ModelState.AddModelError(string.Empty, "Enddate must be after Startdate");
                return Page();
            }

            if(NewImages != null && NewImages.Any()) { 
                foreach(var image in NewImages)
                {
                    if(!string.IsNullOrWhiteSpace(image.Url))
                    {
                        NewAd.Images.Add(image);
                    }
                }
            }

            NewAd.SellerId = user.Id;

            ModelState.Remove("NewAd.SellerId");

            if (!ModelState.IsValid)
            {
                return Page();
            }



          

            if(NewImages != null &&NewImages.Count > 0)
            {
                var validImages = NewImages.Where(img => !string.IsNullOrWhiteSpace(img.Url)).ToList();
                NewImages = validImages;
            }
          



            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.PostAsJsonAsync("api/Ad", NewAd);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Seller/MyAds");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, $"Error creating ad: {response.StatusCode} , {errorContent}");
                return Page();
            }
        }
    }
}
