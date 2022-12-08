using CarStore.TestAuthentication.Commons;
using CarStore.TestAuthentication.Models;
using CarStore.TestAuthentication.Models.ViewModels;
using CarStore.TestAuthentication.Responses;
using CarStore.TestAuthentication.Responses.Objects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace CarStore.TestAuthentication.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HomeController> _logger;        
        private readonly string _apiGatewayUrl;

        public CatalogController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;            
            _apiGatewayUrl = config.GetValue<string>("ApiGatewayUrl"); //Get the config value from the appsettings.json file.
        }


        public async Task<ActionResult> Index()
        {
            var response = new CatalogResponse();
            string errorMessage = string.Empty;
            try
            {
                var client = _httpClientFactory.CreateClient();
                //var accessToken = await HttpContext.GetTokenAsync("access_token");                
                //client.SetBearerToken(accessToken);

                HttpResponseMessage obj = await client.GetAsync(_apiGatewayUrl + "catalogservice/list");
                if (obj != null)
                {
                    if (obj.StatusCode == HttpStatusCode.OK)
                    {
                        Task<string> content = obj.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<CatalogResponse>(content.Result);

                        ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                        return View(response.Catalogs);
                    }
                    errorMessage = obj.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                response.Notification = new NotificationDto
                {
                    NotificationCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }

            TempData["ErrorMessage"] = errorMessage;
            return RedirectToAction("AccessDenied", "Home");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public async Task<ActionResult> DetailsAsync(int id)
        {
            try
            {
                var model = await GetCatalogAsync(id);
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(CatalogViewModel model)
        {
            var response = new CatalogResponse();
            string errorMessage = string.Empty;
            var list = new ListDictionary();

            try
            {
                if (ModelState.IsValid)
                {
                    var client = _httpClientFactory.CreateClient();

                    //var accessToken = await HttpContext.GetTokenAsync("access_token");                
                    //client.SetBearerToken(accessToken);

                    using var request = new HttpRequestMessage(HttpMethod.Post, _apiGatewayUrl + "catalogservice/create");

                    model.CreatedBy = "System";
                    var claim = User?.Claims.FirstOrDefault(k => k.Type == "name");
                    if(claim != null)
                    {
                        model.CreatedBy = claim.Value;
                    }

                    var json = JsonConvert.SerializeObject(model);
                    using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = stringContent;

                    HttpResponseMessage obj = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    if (obj != null)
                    {
                        Task<string> content = obj.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<CatalogResponse>(content.Result);

                        TempData["SuccessMessage"] = response.Notification.InfoMessage;
                        return RedirectToAction(nameof(Index));
                    }
                    errorMessage = obj.StatusCode.ToString();
                }
                else
                {
                    IEnumerable<Error> lst = AllErrors();
                    foreach (var item in lst)
                    {
                        //Check exist keys, if not we will add into list.
                        if (!list.Contains(item.Key))
                        {
                            var errors = lst.Where(p => p.Key == item.Key).Select(p => p.Message).ToList();
                            list.Add(item.Key, errors);
                        }
                    }

                    response.Notification = new NotificationDto
                    {
                        NotificationCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "",
                        DetailErrorMessage = list
                    };
                }


            }
            catch (Exception ex)
            {
                response.Notification = new NotificationDto
                {
                    NotificationCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }

            TempData["ErrorMessage"] = errorMessage;
            return RedirectToAction("AccessDenied", "Home");
        }

        public async Task<ActionResult> EditAsync(int id)
        {
            try
            {
                var catalog = await GetCatalogAsync(id);
                var model = CarStoreDataHelper.MapDataToViewModel(catalog);
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(CatalogViewModel model)
        {
            var response = new CatalogResponse();
            var list = new ListDictionary();
            string errorMessage = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    //Get catalog by Id
                    var catalog = await GetCatalogAsync(model.Id);
                    if (catalog != null)
                    {
                        //Set data for updating
                        catalog.Name = model.Name;
                        catalog.Description = model.Description;
                        catalog.IsActive = model.IsActive;
                        catalog.IsDeleted = model.IsDeleted;

                        model.ModifiedBy = "System";
                        var claim = User?.Claims.FirstOrDefault(k => k.Type == "name");
                        if (claim != null)
                        {
                            model.ModifiedBy = claim.Value;
                        }

                        model = CarStoreDataHelper.MapDataToViewModel(catalog);

                        var client = _httpClientFactory.CreateClient();
                        //var accessToken = await HttpContext.GetTokenAsync("access_token");                
                        //client.SetBearerToken(accessToken);

                        using var request = new HttpRequestMessage(HttpMethod.Post, _apiGatewayUrl + "catalogservice/edit");
                        var json = JsonConvert.SerializeObject(model);
                        using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                        request.Content = stringContent;

                        HttpResponseMessage obj = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                        if (obj != null)
                        {
                            Task<string> content = obj.Content.ReadAsStringAsync();
                            response = JsonConvert.DeserializeObject<CatalogResponse>(content.Result);

                            TempData["SuccessMessage"] = response.Notification.InfoMessage;
                            return RedirectToAction(nameof(Index));
                        }
                        errorMessage = obj.StatusCode.ToString();
                    }
                }
                else
                {
                    IEnumerable<Error> lst = AllErrors();
                    foreach (var item in lst)
                    {
                        //Check exist keys, if not we will add into list.
                        if (!list.Contains(item.Key))
                        {
                            var errors = lst.Where(p => p.Key == item.Key).Select(p => p.Message).ToList();
                            list.Add(item.Key, errors);
                        }
                    }

                    response.Notification = new NotificationDto
                    {
                        NotificationCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "",
                        DetailErrorMessage = list
                    };
                }
            }
            catch (Exception ex)
            {
                response.Notification = new NotificationDto
                {
                    NotificationCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }

            TempData["ErrorMessage"] = errorMessage;
            return RedirectToAction("AccessDenied", "Home");
        }

        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var model = await GetCatalogAsync(id);
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(CatalogModel catalog)
        {
            var response = new CatalogResponse();
            var list = new ListDictionary();
            string errorMessage = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    string modifiedBy = "System";
                    var claim = User?.Claims.FirstOrDefault(k => k.Type == "name");
                    if (claim != null)
                    {
                        modifiedBy = claim.Value;
                    }

                    var model = new CatalogViewModel()
                    {
                        Id = catalog.Id,
                        ModifiedBy = modifiedBy
                    };

                    var client = _httpClientFactory.CreateClient();
                    //var accessToken = await HttpContext.GetTokenAsync("access_token");                
                    //client.SetBearerToken(accessToken);

                    using var request = new HttpRequestMessage(HttpMethod.Post, _apiGatewayUrl + "catalogservice/delete");
                    var json = JsonConvert.SerializeObject(model);
                    using var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = stringContent;

                    HttpResponseMessage obj = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                    if (obj != null)
                    {
                        Task<string> content = obj.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<CatalogResponse>(content.Result);

                        TempData["SuccessMessage"] = response.Notification.InfoMessage;
                        return RedirectToAction(nameof(Index));
                    }
                    errorMessage = obj.StatusCode.ToString();
                }
                else
                {
                    IEnumerable<Error> lst = AllErrors();
                    foreach (var item in lst)
                    {
                        //Check exist keys, if not we will add into list.
                        if (!list.Contains(item.Key))
                        {
                            var errors = lst.Where(p => p.Key == item.Key).Select(p => p.Message).ToList();
                            list.Add(item.Key, errors);
                        }
                    }

                    response.Notification = new NotificationDto
                    {
                        NotificationCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "",
                        DetailErrorMessage = list
                    };
                }
            }
            catch (Exception ex)
            {
                response.Notification = new NotificationDto
                {
                    NotificationCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }

            TempData["ErrorMessage"] = errorMessage;
            return RedirectToAction("AccessDenied", "Home");
        }

        /// <summary>
        /// Get all errors with keys from ModelState.
        /// </summary>
        /// <returns>Enumerable</returns>
        private IEnumerable<Error> AllErrors()
        {
            var result = new List<Error>();
            var errorArr = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors });

            foreach (var error in errorArr)
            {
                var key = error.Key;
                var fieldErrors = error.Errors.Select(error => new Error(key, error.ErrorMessage));
                result.AddRange(fieldErrors);
            }

            return result;
        }

        /// <summary>
        /// Common function gets CatalogModel by Id.
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>CatalogModel</returns>
        private async Task<CatalogModel> GetCatalogAsync(int id)
        {
            var response = new CatalogResponse();
            if (id < 1)
            {
                return response.Catalog;
            }

            try
            {
                var client = _httpClientFactory.CreateClient();
                //var accessToken = await HttpContext.GetTokenAsync("access_token");                
                //client.SetBearerToken(accessToken);

                HttpResponseMessage obj = await client.GetAsync(_apiGatewayUrl + "catalogservice/details/" + id);
                if (obj != null)
                {
                    if (obj.StatusCode == HttpStatusCode.OK)
                    {
                        Task<string> content = obj.Content.ReadAsStringAsync();
                        response = JsonConvert.DeserializeObject<CatalogResponse>(content.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                response.Notification = new NotificationDto
                {
                    NotificationCode = HttpStatusCode.InternalServerError,
                    ErrorMessage = ex.Message
                };
            }

            return response.Catalog;
        }


    }
}
