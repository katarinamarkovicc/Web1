using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class PosetilacController : Controller
    {
        // GET: Posetilac
        public ActionResult Index()
        {
            return View();
        }

        //PROVERITI !!!
        public ActionResult PrijavaNaTreningPosetilac(int idGrupnogTreninga)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            if (ulogovan.ListaPrijavljenihTreninga.Contains(idGrupnogTreninga))
            {
                ViewBag.Message2 = "Već ste se prijavili na ovaj grupni trening!";
                return View();
            }
            else {

                foreach (GrupniTrening gt in grupniTreninzi)
                {
                    foreach (Korisnik k in korisnici)
                    {
                        if (gt.IDGrupnogTreninga == idGrupnogTreninga && k.KorisnickoIme == ulogovan.KorisnickoIme)
                        {
                            if (gt.MaksimalanBrojPosetilaca == gt.SpisakPosetilaca.Count())
                            {
                                ViewBag.Message3 = "Ne možete se prijaviti na ovaj grupni trening, jer je prijavljen maksimalan broj učesnika!";
                                break;
                            }
                            else
                            {
                                k.ListaPrijavljenihTreninga.Add(idGrupnogTreninga);
                                gt.SpisakPosetilaca.Add(ulogovan.KorisnickoIme);

                                Fajl.WriteKorisnike(korisnici);
                                Fajl.WriteGrupneTreninge(grupniTreninzi);
                                ViewBag.Message = "Uspešno ste se prijavili na grupni trening!";
                                break;

                            }
                        }
                    }
                }
                return View();
            }                 
        }

        public ActionResult RanijiGrupniTreninzi()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();
            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);                 
                }
            }

            ViewBag.NovaLista = novaLista;
            return View();
        }

        //ISPRAVITI
        [HttpPost]
        public ActionResult Pretraga(string naziv, string tipTreninga, string fitnesCentar)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> ranijiTreninzi = new List<GrupniTrening>();

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            List<GrupniTrening> pretrazeni = new List<GrupniTrening>();

            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    ranijiTreninzi.Add(gt);
                }
            }

            if (naziv == "" && tipTreninga == "" && fitnesCentar == "")
            {
                ViewBag.Message = "Niste uneli nijedan parametar forme!";
            }

            foreach (GrupniTrening gt in ranijiTreninzi)
            {
                
                if (naziv != "")
                {
                        if (gt.Naziv.Contains(naziv))
                        {
                            pretrazeni.Add(gt);
                        }
                        else continue;
                }
                if (tipTreninga != "")
                {
                        if (gt.TipTreninga.ToString() == tipTreninga)
                        {
                            if (naziv == "")
                            {
                                pretrazeni.Add(gt);
                            }
                        }
                        else
                        {
                            if (pretrazeni.Contains(gt))
                            {
                                pretrazeni.Remove(gt);
                            }
                        }
                }
                if (fitnesCentar != "") 
                {
                    foreach (FitnesCentar fs in fitnesCentri)
                    {
                        if (gt.FitnesCentar == fs.IDFitnesCentra)
                        {
                            // fitnesCentarIstiNaziv = fs;
                            if (fs.Naziv.Contains(fitnesCentar))
                            {
                                if (naziv == "" && tipTreninga == "")
                                {
                                    pretrazeni.Add(gt);
                                    //break;
                                }
                            }
                            else
                            {

                                if (pretrazeni.Contains(gt))
                                {
                                    pretrazeni.Remove(gt);
                                }
                            }

                        }
                    }
                }
            }

            ViewBag.NovaLista = pretrazeni;

            return View("RanijiGrupniTreninzi");
        }

        public ActionResult SortirajPoNazivuRastuce()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);
                }
            }

            List<GrupniTrening> sortiraniPoNazivu = novaLista.OrderByDescending(o => o.Naziv).ToList();

            ViewBag.NovaLista = sortiraniPoNazivu;
            return View("RanijiGrupniTreninzi");
        }

        public ActionResult SortirajPoNazivuOpadajuce()
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);
                }
            }

            List<GrupniTrening> sortiraniPoNazivu = novaLista.OrderBy(o => o.Naziv).ToList();

            ViewBag.NovaLista = sortiraniPoNazivu;
            return View("RanijiGrupniTreninzi");
        }

        public ActionResult SortirajPoTipuTreningaRastuce()
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);
                }
            }

            List<GrupniTrening> sortiraniPoTipuTreninga = novaLista.OrderBy(o => o.TipTreninga).ToList();
            ViewBag.NovaLista = sortiraniPoTipuTreninga;

            return View("RanijiGrupniTreninzi");
        }

        public ActionResult SortirajPoTipuTreningaOpadajuce()
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);
                }
            }

            List<GrupniTrening> sortiraniPoTipuTreninga = novaLista.OrderByDescending(o => o.TipTreninga).ToList();
            ViewBag.NovaLista = sortiraniPoTipuTreninga;

            return View("RanijiGrupniTreninzi");
        }

        public ActionResult SortirajPoDatumuIVremenuRastuce()
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);
                }
            }

            List<GrupniTrening> sortiraniPoDatumuIVremenu = novaLista.OrderBy(o => o.DatumIVremeTreninga).ToList();
            ViewBag.NovaLista = sortiraniPoDatumuIVremenu;

            return View("RanijiGrupniTreninzi");
        }

        public ActionResult SortirajPoDatumuIVremenuOpadajuce()
        {
            
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];
            List<GrupniTrening> novaLista = new List<GrupniTrening>();

            if (ulogovan.ListaPrijavljenihTreninga == null)
            {
                ulogovan.ListaPrijavljenihTreninga = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaPrijavljenihTreninga.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    novaLista.Add(gt);
                }
            }

            List<GrupniTrening> sortiraniPoDatumuIVremenu = novaLista.OrderByDescending(o => o.DatumIVremeTreninga).ToList();
            ViewBag.NovaLista = sortiraniPoDatumuIVremenu;

            return View("RanijiGrupniTreninzi");
        }

    }
}