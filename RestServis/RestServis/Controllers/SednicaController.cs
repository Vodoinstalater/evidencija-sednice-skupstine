using Microsoft.AspNetCore.Mvc;
using KlaseMapiranja;

namespace RestServis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SednicaController : ControllerBase
    {
        private readonly SednicaServisKlasa _sednicaServis;

        public SednicaController(SednicaServisKlasa sednicaServis)
        {
            _sednicaServis = sednicaServis;
        }

        [HttpGet("sednice")]
        public IActionResult DajSveSednice()
        {
            try
            {
                // Koristi service layer umesto direktnog poziva
                var rezultat = _sednicaServis.DajSveSednice();
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} sednica", 
                        podaci = rezultat.Podaci ?? new List<SednicaDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju sednica",
                        podaci = new List<SednicaDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<SednicaDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("sednice/{id}")]
        public IActionResult DajSednicuPoId(int id)
        {
            try
            {
                var rezultat = _sednicaServis.DajSveSednice();
                if (rezultat.Uspesno)
                {
                    var sednica = rezultat.Podaci?.FirstOrDefault(s => s.Id == id);
                    if (sednica != null)
                    {
                        return Ok(new { uspesno = true, poruka = "Sednica pronađena", podaci = sednica });
                    }
                    return Ok(new { uspesno = false, poruka = "Sednica nije pronađena" });
                }
                return Ok(new { uspesno = false, poruka = rezultat.Poruka ?? "Greška pri dohvatanju sednice" });
            }
            catch (Exception ex)
            {
                return Ok(new { uspesno = false, poruka = $"Greška: {ex.Message}" });
            }
        }

        [HttpGet("sazivi")]
        public IActionResult DajSveSazive()
        {
            try
            {
                // Koristi service layer umesto direktnog poziva
                var rezultat = _sednicaServis.DajSveSazive();
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} saziva", 
                        podaci = rezultat.Podaci ?? new List<SazivDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju saziva",
                        podaci = new List<SazivDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<SazivDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("mandati")]
        public IActionResult DajSveMandate()
        {
            try
            {
                var rezultat = _sednicaServis.DajSveMandate();
                
                if (rezultat != null && rezultat.Uspesno && rezultat.Podaci != null)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci.Count} mandata", 
                        podaci = rezultat.Podaci,
                        broj = rezultat.Podaci.Count
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat?.Poruka ?? "Nepoznata greška",
                        podaci = new List<MandatDTO>(),
                        broj = 0,
                        debug_info = $"ServiceResult: Uspesno={rezultat?.Uspesno}, Poruka={rezultat?.Poruka}"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<MandatDTO>(),
                    broj = 0,
                    debug_info = $"Exception: {ex.GetType().Name}"
                });
            }
        }

        [HttpGet("test")]
        public IActionResult TestConnection()
        {
            try
            {
                return Ok(new { 
                    uspesno = true, 
                    poruka = "REST API radi!",
                    timestamp = DateTime.Now,
                    service_initialized = _sednicaServis != null
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    timestamp = DateTime.Now
                });
            }
        }

        [HttpGet("zasedanja")]
        public IActionResult DajSvaZasedanja(int? sazivId = null)
        {
            try
            {
                var rezultat = _sednicaServis.DajSvaZasedanja(sazivId);
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} zasedanja", 
                        podaci = rezultat.Podaci ?? new List<ZasedanjeDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju zasedanja",
                        podaci = new List<ZasedanjeDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<ZasedanjeDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("glasanja")]
        public IActionResult DajSvaGlasanja()
        {
            try
            {
                var rezultat = _sednicaServis.DajSvaGlasanja();
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} glasanja", 
                        podaci = rezultat.Podaci ?? new List<GlasanjeDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju glasanja",
                        podaci = new List<GlasanjeDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<GlasanjeDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("lica")]
        public IActionResult DajSvaLica()
        {
            try
            {
                var rezultat = _sednicaServis.DajSvaLica();
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} lica", 
                        podaci = rezultat.Podaci ?? new List<LiceDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju lica",
                        podaci = new List<LiceDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<LiceDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("stranke")]
        public IActionResult DajSveStranke()
        {
            try
            {
                var rezultat = _sednicaServis.DajSveStranke();
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} stranaka", 
                        podaci = rezultat.Podaci ?? new List<StrankaDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju stranaka",
                        podaci = new List<StrankaDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<StrankaDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("pozicije")]
        public IActionResult DajSvePozicije()
        {
            try
            {
                var rezultat = _sednicaServis.DajSvePozicije();
                
                if (rezultat.Uspesno)
                {
                    return Ok(new { 
                        uspesno = true, 
                        poruka = rezultat.Poruka ?? $"Dohvaćeno je {rezultat.Podaci?.Count ?? 0} pozicija", 
                        podaci = rezultat.Podaci ?? new List<PozicijaDTO>(),
                        broj = rezultat.Podaci?.Count ?? 0
                    });
                }
                else
                {
                    return Ok(new { 
                        uspesno = false, 
                        poruka = rezultat.Poruka ?? "Greška pri dohvatanju pozicija",
                        podaci = new List<PozicijaDTO>(),
                        broj = 0
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Greška: {ex.Message}",
                    podaci = new List<PozicijaDTO>(),
                    broj = 0
                });
            }
        }

        [HttpGet("debug")]
        public IActionResult DebugDataFlow()
        {
            try
            {
                // Test service layer call for sazivi
                var rezultat = _sednicaServis.DajSveSazive();
                
                return Ok(new { 
                    uspesno = true, 
                    poruka = "Debug test completed",
                    service_result = rezultat.Uspesno,
                    service_message = rezultat.Poruka,
                    sazivi_count = rezultat.Podaci?.Count ?? 0,
                    sazivi_data = rezultat.Podaci?.Take(3).Select(s => new { 
                        Id = s.Id, 
                        Ime = s.Ime, 
                        Pocetak = s.DatumPocetka, 
                        Kraj = s.DatumZavrsetka 
                    }).ToArray() ?? new object[0]
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    uspesno = false, 
                    poruka = $"Debug greška: {ex.Message}",
                    stack_trace = ex.StackTrace
                });
            }
        }
    }
}
