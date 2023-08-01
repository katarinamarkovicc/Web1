using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
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

        public ActionResult DetaljniPrikaz(int idFitnesCentra)
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
                if (maksimalnaGranica != "" && fs.Brisanje == false)
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

            ViewBag.ListaFitnesCentara = pretrazeniFitnesCentri;

            return View("Index");

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
            return View("Index");
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
            return View("Index");
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
            return View("Index");
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
            return View("Index");
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
            return View("Index");
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
            return View("Index");
        }
    }
   
}