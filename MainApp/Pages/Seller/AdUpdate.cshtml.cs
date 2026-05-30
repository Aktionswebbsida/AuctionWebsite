using Data.DTOs;
using Data.Entities;
using MainApp.Responses;
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
                            ExistingUrls = img.Url,
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

            
            UpdateAd.SellerId = user.Id;

            ModelState.Remove("UpdateAd.SellerId");

            if (!ModelState.IsValid)
            {
                return Page();
            }





          




            var client = HttpClientFactory.CreateClient("APIClient");
            var imageDtos = new List<UpdateImagesDto>();


            if (UpdateImages != null)
            {
                foreach (var item in UpdateImages)
                {
                    if (item.Url != null)
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
                                imageDtos.Add(new UpdateImagesDto
                                {
                                    Description = item.Description,
                                    Url = result.Url
                                });
                            }
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.ExistingUrls))
                    {
                        imageDtos.Add(new UpdateImagesDto
                        {
                            Description = item.Description,
                            Url = item.ExistingUrls
                        });
                    }
                }
            }
            var final = new
            {
               UpdateAd.Title,
                UpdateAd.Description,
                UpdateAd.StartDate,
                UpdateAd.EndDate,
                UpdateAd.Place,
                UpdateAd.StartingPrice,
                UpdateAd.SellerId,
                Images = imageDtos
            };
            var response = await client.PutAsJsonAsync($"api/Ad/{UpdateAd.AdID}", final);
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


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = HttpClientFactory.CreateClient("APIClient");
            var response = await client.DeleteAsync($"api/Ad/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("/Seller/MyAds");
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
