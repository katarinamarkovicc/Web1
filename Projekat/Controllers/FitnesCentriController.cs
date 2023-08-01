using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class FitnesCentriController : Controller
    {
        // GET: FitnesCentri
        public ActionResult Index()
        {
            return View();
        }

        /////////////////////////////POSETILAC//////////////////////////////////////////
      
        public ActionResult FitnesCentriPosetilac()
        {
          
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();



            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderBy(o => o.Naziv).ToList();

             ViewBag.ListaFitnesCentara = sortiraniPoNazivu;

             return View();
        }

        public ActionResult DetaljniPrikazPosetilac(int idFitnesCentra)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];

            List<FitnesCentar> novaLista = new List<FitnesCentar>();
            List<GrupniTrening> novaLista2 = new List<GrupniTrening>();
            List<Komentar> novaLista3 = new List<Komentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.IDFitnesCentra == idFitnesCentra)
                {
                    novaLista.Add(fs);
                }
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.FitnesCentar == idFitnesCentra && gt.DatumIVremeTreninga > DateTime.Now && gt.Brisanje == false)
                {
                    novaLista2.Add(gt);
                }
            }

            foreach (Komentar k in komentari)
            {
                if (k.FitnesCentar == idFitnesCentra)
                {
                    novaLista3.Add(k);
                }
            }

            ViewBag.NovaLista = novaLista;
            ViewBag.NovaLista2 = novaLista2;
            ViewBag.NovaLista3 = novaLista3;
            return View();
        }

        [HttpPost]
        public ActionResult PretragaFitnesCentara(string naziv, string adresa, string minimalnaGranica, string maksimalnaGranica)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pretrazeniFitnesCentri = new List<FitnesCentar>();

            //ako nije uneo nijedan parametar
            if (naziv == "" && adresa == "" && minimalnaGranica == "" && maksimalnaGranica == "")    // 0 za int?
            {
                ViewBag.Message = "Niste uneli nijedan parametar forme!";
            }

            //ako unese bar jedan parametar
            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (naziv != "")
                {
                    if (fs.Naziv.Contains(naziv) && fs.Brisanje == false)
                    {
                        pretrazeniFitnesCentri.Add(fs);
                    }
                    else continue;
                }
                if (adresa != "")
                {
                    if (fs.Adresa.Contains(adresa) && fs.Brisanje == false)
                    {
                        if (naziv == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
                if (minimalnaGranica != "")
                {
                    if (fs.GodinaOtvaranja >= Int32.Parse(minimalnaGranica) && fs.Brisanje == false)
                    {
                        if (naziv == "" && adresa == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
                if (maksimalnaGranica != "")
                {
                    if (fs.GodinaOtvaranja <= Int32.Parse(maksimalnaGranica) && fs.Brisanje == false)
                    {
                        if (naziv == "" && adresa == "" && minimalnaGranica == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
            }

            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in pretrazeniFitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            ViewBag.ListaFitnesCentara = fitnesCentriVazeci;

            return View("FitnesCentriPosetilac");

        }

        public ActionResult SortirajPoNazivuRastuce()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];


            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();



            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderBy(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;
            return View("FitnesCentriPosetilac");
        }

        public ActionResult SortirajPoNazivuOpadajuce()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];



            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();



            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderByDescending(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;
            return View("FitnesCentriPosetilac");
        }

        public ActionResult SortirajPoAdresiRastuce()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];



            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();



            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoAdresi = fitnesCentriVazeci.OrderBy(o => o.Adresa).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoAdresi;
            return View("FitnesCentriPosetilac");
        }

        public ActionResult SortirajPoAdresiOpadajuce()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];



            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();



            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoAdresi = fitnesCentriVazeci.OrderByDescending(o => o.Adresa).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoAdresi;
            return View("FitnesCentriPosetilac");
        }

        public ActionResult SortirajPoGodiniRastuce()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoGodini = fitnesCentriVazeci.OrderBy(o => o.GodinaOtvaranja).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoGodini;
            return View("FitnesCentriPosetilac");
        }

        public ActionResult SortirajPoGodiniOpadajuce()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoGodini = fitnesCentriVazeci.OrderByDescending(o => o.GodinaOtvaranja).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoGodini;

            return View("FitnesCentriPosetilac");
        }

        //////////////////////////////TRENER/////////////////////////////////////////////////

        public ActionResult FitnesCentriTrener()
        {

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderBy(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;

            return View();
        }

        public ActionResult DetaljniPrikazTrener(int idFitnesCentra)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];

            List<FitnesCentar> novaLista = new List<FitnesCentar>();
            List<GrupniTrening> novaLista2 = new List<GrupniTrening>();
            List<Komentar> novaLista3 = new List<Komentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.IDFitnesCentra == idFitnesCentra)
                {
                    novaLista.Add(fs);
                }
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.FitnesCentar == idFitnesCentra && gt.DatumIVremeTreninga > DateTime.Now && gt.Brisanje == false)
                {
                    novaLista2.Add(gt);
                }
            }

            foreach (Komentar k in komentari)
            {
                if (k.FitnesCentar == idFitnesCentra)
                {
                    novaLista3.Add(k);
                }
            }

            ViewBag.NovaLista = novaLista;
            ViewBag.NovaLista2 = novaLista2;
            ViewBag.NovaLista3 = novaLista3;
            return View();
        }

        [HttpPost]
        public ActionResult PretragaFitnesCentaraTrener(string naziv, string adresa, string minimalnaGranica, string maksimalnaGranica)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pretrazeniFitnesCentri = new List<FitnesCentar>();

            //ako nije uneo nijedan parametar
            if (naziv == "" && adresa == "" && minimalnaGranica == "" && maksimalnaGranica == "")    // 0 za int?
            {
                ViewBag.Message = "Niste uneli nijedan parametar forme!";
            }

            //ako unese bar jedan parametar
            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (naziv != "")
                {
                    if (fs.Naziv.Contains(naziv))
                    {
                        pretrazeniFitnesCentri.Add(fs);
                    }
                    else continue;
                }
                if (adresa != "")
                {
                    if (fs.Adresa.Contains(adresa))
                    {
                        if (naziv == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
                if (minimalnaGranica != "")
                {
                    if (fs.GodinaOtvaranja >= Int32.Parse(minimalnaGranica))
                    {
                        if (naziv == "" && adresa == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
                if (maksimalnaGranica != "")
                {
                    if (fs.GodinaOtvaranja <= Int32.Parse(maksimalnaGranica))
                    {
                        if (naziv == "" && adresa == "" && minimalnaGranica == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
            }

            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in pretrazeniFitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }
            ViewBag.ListaFitnesCentara = fitnesCentriVazeci;

            return View("FitnesCentriTrener");

        }

        public ActionResult SortirajPoNazivuRastuceTrener()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderBy(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;
            return View("FitnesCentriTrener");
        }

        public ActionResult SortirajPoNazivuOpadajuceTrener()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderByDescending(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;
            return View("FitnesCentriTrener");
        }

        public ActionResult SortirajPoAdresiRastuceTrener()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoAdresi = fitnesCentriVazeci.OrderBy(o => o.Adresa).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoAdresi;
            return View("FitnesCentriTrener");
        }

        public ActionResult SortirajPoAdresiOpadajuceTrener()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoAdresi = fitnesCentriVazeci.OrderByDescending(o => o.Adresa).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoAdresi;
            return View("FitnesCentriTrener");
        }

        public ActionResult SortirajPoGodiniRastuceTrener()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoGodini = fitnesCentriVazeci.OrderBy(o => o.GodinaOtvaranja).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoGodini;
            return View("FitnesCentriTrener");
        }

        public ActionResult SortirajPoGodiniOpadajuceTrener()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoGodini = fitnesCentriVazeci.OrderByDescending(o => o.GodinaOtvaranja).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoGodini;

            return View("FitnesCentriTrener");
        }

        ///////////////////////////////VLASNIK//////////////////////////////////////////////

        public ActionResult FitnesCentriVlasnik()
        {

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderBy(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;

            return View();
        }

        public ActionResult DetaljniPrikazVlasnik(int idFitnesCentra)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentari"];

            List<FitnesCentar> novaLista = new List<FitnesCentar>();
            List<GrupniTrening> novaLista2 = new List<GrupniTrening>();
            List<Komentar> novaLista3 = new List<Komentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.IDFitnesCentra == idFitnesCentra)
                {
                    novaLista.Add(fs);
                }
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.FitnesCentar == idFitnesCentra && gt.DatumIVremeTreninga > DateTime.Now && gt.Brisanje == false)
                {
                    novaLista2.Add(gt);
                }
            }

            foreach (Komentar k in komentari)
            {
                if (k.FitnesCentar == idFitnesCentra)
                {
                    novaLista3.Add(k);
                }
            }

            ViewBag.NovaLista = novaLista;
            ViewBag.NovaLista2 = novaLista2;
            ViewBag.NovaLista3 = novaLista3;
            return View();
        }

        [HttpPost]
        public ActionResult PretragaFitnesCentaraVlasnik(string naziv, string adresa, string minimalnaGranica, string maksimalnaGranica)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> pretrazeniFitnesCentri = new List<FitnesCentar>();

            //ako nije uneo nijedan parametar
            if (naziv == "" && adresa == "" && minimalnaGranica == "" && maksimalnaGranica == "")    // 0 za int?
            {
                ViewBag.Message = "Niste uneli nijedan parametar forme!";
            }

            //ako unese bar jedan parametar
            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (naziv != "")
                {
                    if (fs.Naziv.Contains(naziv))
                    {
                        pretrazeniFitnesCentri.Add(fs);
                    }
                    else continue;
                }
                if (adresa != "")
                {
                    if (fs.Adresa.Contains(adresa))
                    {
                        if (naziv == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
                if (minimalnaGranica != "")
                {
                    if (fs.GodinaOtvaranja >= Int32.Parse(minimalnaGranica))
                    {
                        if (naziv == "" && adresa == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
                if (maksimalnaGranica != "")
                {
                    if (fs.GodinaOtvaranja <= Int32.Parse(maksimalnaGranica))
                    {
                        if (naziv == "" && adresa == "" && minimalnaGranica == "")
                        {
                            pretrazeniFitnesCentri.Add(fs);
                        }
                    }
                    else
                    {
                        if (pretrazeniFitnesCentri.Contains(fs))
                        {
                            pretrazeniFitnesCentri.Remove(fs);
                        }
                    }
                }
            }
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in pretrazeniFitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }
            ViewBag.ListaFitnesCentara = fitnesCentriVazeci;

            return View("FitnesCentriVlasnik");

        }

        public ActionResult SortirajPoNazivuRastuceVlasnik()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderBy(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;
            return View("FitnesCentriVlasnik");
        }

        public ActionResult SortirajPoNazivuOpadajuceVlasnik()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoNazivu = fitnesCentriVazeci.OrderByDescending(o => o.Naziv).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoNazivu;
            return View("FitnesCentriVlasnik");
        }

        public ActionResult SortirajPoAdresiRastuceVlasnik()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoAdresi = fitnesCentriVazeci.OrderBy(o => o.Adresa).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoAdresi;
            return View("FitnesCentriVlasnik");
        }

        public ActionResult SortirajPoAdresiOpadajuceVlasnik()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoAdresi = fitnesCentriVazeci.OrderByDescending(o => o.Adresa).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoAdresi;
            return View("FitnesCentriVlasnik");
        }

        public ActionResult SortirajPoGodiniRastuceVlasnik()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoGodini = fitnesCentriVazeci.OrderBy(o => o.GodinaOtvaranja).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoGodini;
            return View("FitnesCentriVlasnik");
        }

        public ActionResult SortirajPoGodiniOpadajuceVlasnik()
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<FitnesCentar> fitnesCentriVazeci = new List<FitnesCentar>();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.Brisanje == false)
                {
                    fitnesCentriVazeci.Add(fs);
                }
            }

            List<FitnesCentar> sortiraniPoGodini = fitnesCentriVazeci.OrderByDescending(o => o.GodinaOtvaranja).ToList();

            ViewBag.ListaFitnesCentara = sortiraniPoGodini;

            return View("FitnesCentriVlasnik");
        }

        /////////////////////////////PRIKAZ PROFILA///////////////////////////////////////////////

        public ActionResult PregledProfilaPosetilac()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            Korisnik onajStoMiTreba = new Korisnik();

            foreach (Korisnik kr in korisnici)
            {
                if (kr.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    onajStoMiTreba = kr;
                    break;
                }
                /*else
                {
                    ViewBag.Message = "Doslo je do greske, nije pronadjen korisnik u bazi!";
                }*/
            }
            
            return View(onajStoMiTreba);
        }

        public ActionResult PregledProfilaTrener()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            Korisnik onajStoMiTreba = new Korisnik();

            foreach (Korisnik kr in korisnici)
            {
                if (kr.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    onajStoMiTreba = kr;
                    break;
                }
                /*else
                {
                    ViewBag.Message = "Doslo je do greske, nije pronadjen korisnik u bazi!";
                }*/
            }

            return View(onajStoMiTreba);
        }

        public ActionResult PregledProfilaVlasnik()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik korisnik = (Korisnik)Session["korisnik"];

            Korisnik onajStoMiTreba = new Korisnik();

            foreach (Korisnik kr in korisnici)
            {
                if (kr.KorisnickoIme == korisnik.KorisnickoIme)
                {
                    onajStoMiTreba = kr;
                    break;
                }
                /*else
                {
                    ViewBag.Message = "Doslo je do greske, nije pronadjen korisnik u bazi!";
                }*/
            }

            return View(onajStoMiTreba);
        }

        /////////////////////////////IZMENA PROFILA///////////////////////////////////////////////

        public ActionResult IzmenaProfila()
        {
            Korisnik ulogovani = (Korisnik)Session["korisnik"];

            if (ulogovani.Uloga == UlogaKorisnika.POSETILAC)
            {
                return View("IzmenaProfilaPosetilac");
            }
            else if (ulogovani.Uloga == UlogaKorisnika.TRENER)
            {
                return View("IzmenaProfilaTrener");
            }
            else 
            {
                return View("IzmenaProfilaVlasnik");
            }

        }

        [HttpPost]
        public ActionResult IzmeniProfil(Korisnik kor)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            DateTime automatski = new DateTime(0001, 1, 1, 12, 0, 0);
            Korisnik izmenjeni = new Korisnik();
            Korisnik ulogovani = (Korisnik)Session["korisnik"];

            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == ulogovani.KorisnickoIme)
                {
                    if (kor.KorisnickoIme != null)
                    {
                        k.KorisnickoIme = kor.KorisnickoIme;
                      
                    }
                    

                    if (kor.Ime != null)
                    {
                        k.Ime = kor.Ime;
                       
                    }
                   
                    if (kor.Prezime != null)
                    {
                        k.Prezime = kor.Prezime;
                       
                    }
                    else
                    

                    if (kor.Lozinka != null)
                    {
                        k.Lozinka = kor.Lozinka;
                    }
                    

                    if (kor.DatumRodjenja != automatski)
                    {
                        k.DatumRodjenja = kor.DatumRodjenja;
                    }
                   

                    if (kor.Email != null)
                    {
                        k.Email = kor.Email;
                    }
                   

                    izmenjeni = k;

                    Fajl.WriteKorisnike(korisnici);

                    break;
                }
            }


            if (ulogovani.Uloga == UlogaKorisnika.POSETILAC)
            {
                return View("PregledProfilaPosetilac", izmenjeni);
            }
            else if (ulogovani.Uloga == UlogaKorisnika.TRENER)
            {
                return View("PregledProfilaTrener", izmenjeni);
            }
            else
            {
                return View("PregledProfilaVlasnik", izmenjeni);
            }


        }

        
    }
}