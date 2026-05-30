using Data.DTOs;
using Data.Entities;
using MainApp.Responses;
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

            
            NewAd.SellerId = user.Id;

            ModelState.Remove("NewAd.SellerId");

            if (!ModelState.IsValid)
            {
                return Page();
            }



          

           
          



            var client = HttpClientFactory.CreateClient("APIClient");
            var imageDtos = new List<ImagesCreateDto>();

            if (NewImages != null)
            {
                foreach (var item in NewImages.Where(i => i.Url != null))
                {
                    using var content = new MultipartFormDataContent();
                    using var streamContent = new StreamContent(item.Url!.OpenReadStream());
                    content.Add(streamContent, "file", item.Url.FileName);

                    var imgResponse = await client.PostAsync("api/Image/ImgUpload", content);

                    if (imgResponse.IsSuccessStatusCode)
                    {
                        var result = await imgResponse.Content.ReadFromJsonAsync<ImgUploadResponse>();
                        if (result?.Url != null)
                        {
                         
                            imageDtos.Add(new ImagesCreateDto
                            {
                                Description = item.Description,
                                Url = result.Url
                            });
                        }
                    }
                }
            }

            var final = new 
            {
                NewAd.Title,
                NewAd.Description,
                NewAd.StartDate,
                NewAd.EndDate,
                NewAd.Place,
                NewAd.StartingPrice,
                NewAd.SellerId,
                Images = imageDtos
            };
            var response = await client.PostAsJsonAsync("api/Ad", final);
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
