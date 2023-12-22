using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Turk_Medya_Case.Models.Detail;
using Turk_Medya_Case.Models.MainPage;

namespace Turk_Medya_Case.Controllers
{
    public class DetailController : Controller
    {
        /// <summary>
        /// Belli bir URL üzerinden json verisini alıp arayüze gönderme işlemi yapar.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            // Hata kontrolü
            try
            {
                #region URL Request and Get Data
                // URL tanımı
                string detailUrl = "https://www.turkmedya.com.tr/detay.json";

                HttpClient client = new HttpClient();

                // async olarak request
                var response = await client.GetAsync(detailUrl);

                // response status code 2** dışında bir şey dönerse hata fırlatacak
                response.EnsureSuccessStatusCode();

                // gelen response u json'dan tanımladığım sınıfa dönüştürdüm
                var jsonResponse = response.Content.ReadAsStringAsync().Result;
                var detailJson = JsonConvert.DeserializeObject<DetailModel>(jsonResponse);
                #endregion

                return View(detailJson);
            }
            catch (Exception ex)
            {
                // hataya düştüğünde hatanın mesajını dönecek
                return Content(ex.Message);
            }         
        }
    }
}
