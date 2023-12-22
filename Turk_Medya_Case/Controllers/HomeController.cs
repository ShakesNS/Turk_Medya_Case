using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging.Signing;
using System.Diagnostics;
using System.Text.Json.Serialization;
using Turk_Medya_Case.Models;
using Turk_Medya_Case.Models.MainPage;

namespace Turk_Medya_Case.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Belirli bir URL üzerinden json verisini filtreleyip arayüze dönme işlemi yapar.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="category"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(int? page, string? category, string? searchString)
        {
            // Hata kontrolü
            try
            {
                #region URL Request and Get Data
                // URL tanımlama
                string mainPageUrl = "https://www.turkmedya.com.tr/anasayfa.json";

                HttpClient client = new HttpClient();

                // async olarak request
                var response = await client.GetAsync(mainPageUrl);

                // response status code 2** dışında bir şey dönerse hata fırlatacak
                response.EnsureSuccessStatusCode();

                // gelen response u json'dan tanımladığım sınıfa dönüştürdüm
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                var mainPageJson = JsonConvert.DeserializeObject<MainPageModel>(jsonResponse);

                // İki farklı itemList olduğundan ikisini birleştirdim ve elimde bir liste var
                var newsList = mainPageJson.data.ToList().SelectMany(x => x.itemList).ToList();
                #endregion


                #region Pagination and Filtering
                // Kullanılan bütün kategorilerin listesi elimde olmadığından gelen veriden ayrıştırma yaparak kategorilerin bir listesini çıkardım.
                var categories = newsList.DistinctBy(item => item.category.slug).Select(x => x.category.slug).Where(i => !string.IsNullOrEmpty(i) && i != "\"").ToList();
                if (category == "")
                    category = null;

                // Kullanacağım listelerin tanımı
                var filteredList = new List<ItemList>();

                // Search parametresi boş değil ise
                if (searchString != null)
                {
                    // kategoriye bak
                    if (string.IsNullOrEmpty(category))
                    {
                        // kategori boş ise filteredList değişkenin içine searchString ile filtrelenmiş veriyi ata
                        filteredList = newsList.Where(x => x.title.Contains(searchString)).ToList();
                    }
                    else
                    {
                        // kategori boş değil ise searchString ve kategori ile filtreleme yap
                        filteredList= newsList.Where(item => item.category.slug == category && item.title.Contains(searchString)).ToList();
                    }
                }
                else
                {
                    //searchString boş ise kategoriyi kontrol et
                    if (string.IsNullOrEmpty(category))
                    {
                        // kategori boş ise ham veriyi değişkenin içine ata.
                        filteredList = newsList;
                    }
                    else
                    {
                        // kategori boş değil ise kategori ile filtreleme yap
                        filteredList = newsList.Where(item => item.category.slug == category).ToList();
                    }
                }

                // Sayfalama için kullanılacak değişken tanımları
                int pageSize = 5;
                int pageNumber = page ?? 1;
                var paginatedWords = filteredList.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                #endregion

                // Arayüz tarafında sayfalama ve anlık verileri görebileceğim değişkenlerin tanımı
                ViewBag.CurrentPage = pageNumber;
                ViewBag.TotalPages = (int)Math.Ceiling(filteredList.Count / (double)pageSize);
                ViewBag.Categories = categories;
                ViewBag.CurrentCategory = category;
                ViewBag.CurrentSearch = searchString;

                // en sonunda elimde olan listeyi arayüze return ediyorum.
                return View(filteredList);
            }
            catch (Exception ex)
            {
                // hataya düştüğünde hatanın mesajını dönecek
                return Content(ex.Message);
            }
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}