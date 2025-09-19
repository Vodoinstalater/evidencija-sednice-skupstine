using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using KlaseMapiranja;
using System.Data;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly SednicaServisKlasa _sednicaServis;

        public HomeController()
        {
            try
            {
                _sednicaServis = new SednicaServisKlasa("");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IstorijaSednica()
        {
            try
            {
                var rezultat = _sednicaServis.DajSveSednice();
                
                if (rezultat.Uspesno)
                {
                    ViewBag.Poruka = rezultat.Poruka;
                    return View(rezultat.Podaci);
                }
                else
                {
                    ViewBag.Greska = rezultat.Poruka;
                    return View(new List<SednicaDTO>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Greska = $"Greška pri dohvatanju sednica: {ex.Message}";
                return View(new List<SednicaDTO>());
            }
        }

        public IActionResult IstorijaSaziva()
        {
            try
            {
                var rezultat = _sednicaServis.DajSveSazive();
                
                if (rezultat.Uspesno)
                {
                    ViewBag.Poruka = rezultat.Poruka;
                    return View(rezultat.Podaci);
                }
                else
                {
                    ViewBag.Greska = rezultat.Poruka;
                    return View(new List<SazivDTO>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Greska = $"Greška pri dohvatanju saziva: {ex.Message}";
                return View(new List<SazivDTO>());
            }
        }

        public IActionResult PogledajMandate()
        {
            try
            {
                var rezultat = _sednicaServis.DajSveMandate();
                
                if (rezultat.Uspesno)
                {
                    var uniqueMandate = rezultat.Podaci
                        .GroupBy(m => m.Id)
                        .Select(g => g.First())
                        .ToList();
                    
                    ViewBag.Poruka = rezultat.Poruka;
                    return View(uniqueMandate);
                }
                else
                {
                    ViewBag.Greska = rezultat.Poruka;
                    return View(new List<MandatDTO>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Greska = $"Greška pri dohvatanju mandata: {ex.Message}";
                return View(new List<MandatDTO>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RestDemo()
        {
            return View();
        }
    }

    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
