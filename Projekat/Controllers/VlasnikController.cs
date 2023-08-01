using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class VlasnikController : Controller
    {
        // GET: Vlasnik
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegistracijaTrenera()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrujTrenera(Korisnik kor, string pol)
        {
            //------------------------REGISTRACIJA TRENERA U BAZU---------------------------------------
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik ulogovaniVlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            kor.Uloga = UlogaKorisnika.TRENER;

            DateTime automatski = new DateTime(0001, 1, 1, 12, 0, 0);

            if (pol == "" || kor.DatumRodjenja == automatski || kor.KorisnickoIme == null || kor.Lozinka == null || kor.Email == null || kor.Ime == null || kor.Prezime == null)
            {
                ViewBag.Message = "Niste uneli sva polja!";
                return View("RegistracijaTrenera");
            }

            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == kor.KorisnickoIme)
                {
                    ViewBag.Message = "Trener " + k.KorisnickoIme + " je vec registrovan!";
                    return View("RegistracijaTrenera");
                }
            }

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (ulogovaniVlasnik.FitnesCentriVlasnika == null)
                {
                    ulogovaniVlasnik.FitnesCentriVlasnika = new List<int>();
                }
                if (ulogovaniVlasnik.FitnesCentriVlasnika.Contains(fs.IDFitnesCentra))
                {
                    kor.FitnesCentarTrenera = fs.IDFitnesCentra;
                    break;
                }
            }

            korisnici.Add(kor);
            Fajl.WriteKorisnik(kor);

            ViewBag.Message = "Uspešna registracija trenera!";

            return View("RegistracijaTrenera");
        }

        public ActionResult PregledFitnesCentara()
        {
            List<FitnesCentar> fitnesCentri = Fajl.ReadFitnesCentar();
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> potrebni = new List<FitnesCentar>();


          /*  if (vlasnik.FitnesCentriVlasnika == null)
            {
                vlasnik.FitnesCentriVlasnika = new List<int>();
            }
            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (vlasnik.FitnesCentriVlasnika.Contains(fs.IDFitnesCentra) && fs.Brisanje == false)
                {
                    potrebni.Add(fs);
                }
            }*/

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if(fs.Vlasnik == vlasnik.KorisnickoIme && fs.Brisanje == false)
                {
                    potrebni.Add(fs);    
                }
            }

            ViewBag.ListaFitnesCentara = potrebni;
            return View();
        }

        public ActionResult BrisanjeCentra(int idFitnesCentra)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> potrebni = new List<FitnesCentar>();
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];


            if (vlasnik.FitnesCentriVlasnika == null)
            {
                vlasnik.FitnesCentriVlasnika = new List<int>();
            }

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (vlasnik.FitnesCentriVlasnika.Contains(fs.IDFitnesCentra) && fs.Brisanje == false)
                {
                    potrebni.Add(fs);
                }

            }

            bool neMoze = false;

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.FitnesCentar == idFitnesCentra && gt.DatumIVremeTreninga > DateTime.Now)
                {
                    neMoze = true;
                    break;
                }
            }

            FitnesCentar brisenjeFitnesCentar = new FitnesCentar();
            foreach (FitnesCentar fs in potrebni)
            {
                if (fs.IDFitnesCentra == idFitnesCentra)
                {
                    brisenjeFitnesCentar = fs;
                    break;
                }

            }
            //foreach (FitnesCentar fs in potrebni)
            //{
            //    if (fs.IDFitnesCentra == idFitnesCentra && neMoze == false)
            //    {
            //        fs.Brisanje = true;
            //        Fajl.WriteFitnesCentre(fitnesCentri);
            //        ViewBag.Message = "Uspešno izbrisan fitnes centar!";
            //        break;
            //    }
            //    else
            //    {
            //        ViewBag.Message1 = "Ne možete izbrisati ovaj fitnes centar, jer u sklopu njega postoje treninzi koji tek treba da se održe!";
            //        break;
            //    }

            //}

            if (neMoze)
            {
                ViewBag.Message1 = "Ne možete izbrisati ovaj fitnes centar, jer u sklopu njega postoje treninzi koji tek treba da se održe!";

            }
            else
            {
                brisenjeFitnesCentar.Brisanje = true;
                 Fajl.WriteFitnesCentre(fitnesCentri);
               ViewBag.Message = "Uspešno izbrisan fitnes centar!";
            }

            foreach (Korisnik k in korisnici)
            {
                if (k.FitnesCentarTrenera == idFitnesCentra)
                {
                    k.Blokiran = true;
                    Fajl.WriteKorisnike(korisnici);
                    break;
                }
            }
           
            return View("PorukaIzbrisan");

        }

        public ActionResult ModifikacijaCentra(int idFitnesCentra)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            FitnesCentar kojiTrebaDaSeMenja = new FitnesCentar();

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.IDFitnesCentra == idFitnesCentra)
                {
                    kojiTrebaDaSeMenja = fs;
                    break;
                }
            }

            return View(kojiTrebaDaSeMenja);
        }

        [HttpPost]
        public ActionResult ModifikacijaPosleFormeVlasnik(int idKojiTrebaDaSeMenja, string naziv, string adresa, string godinaOtvaranja, string cenaMesecneClanarine, string cenaGodisnjeClanarine, string cenaJednogTreninga, string cenaGrupnogTreninga, string cenaPersonalnogTreninga)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];

            if (naziv == "" && adresa == "" && godinaOtvaranja == "" && cenaMesecneClanarine == "" && cenaGodisnjeClanarine == "" && cenaJednogTreninga == "" && cenaGrupnogTreninga == "" && cenaPersonalnogTreninga == "")
            {
                ViewBag.Message = "Niste uneli nijedan parametar forme!";
            }

            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (fs.IDFitnesCentra == idKojiTrebaDaSeMenja)
                {
                    if (naziv != "")
                    {
                        fs.Naziv = naziv;
                    }

                    if (adresa != "")
                    {
                        fs.Adresa = adresa;
                    }

                    if (godinaOtvaranja != "")
                    {
                        fs.GodinaOtvaranja = Int32.Parse(godinaOtvaranja);
                    }

                    if (cenaMesecneClanarine != "")
                    {
                        fs.CenaMesecneClanarine = Int32.Parse(cenaMesecneClanarine);
                    }

                    if (cenaGodisnjeClanarine != "")
                    {
                        fs.CenaGodisnjeClanarine = Int32.Parse(cenaGodisnjeClanarine);
                    }

                    if (cenaJednogTreninga != "")
                    {
                        fs.CenaJednogTreninga = Int32.Parse(cenaJednogTreninga);
                    }

                    if (cenaGrupnogTreninga != "")
                    {
                        fs.CenaGrupnogTreninga = Int32.Parse(cenaGrupnogTreninga);
                    }

                    if (cenaPersonalnogTreninga != "")
                    {
                        fs.CenaPersonalnogTreninga = Int32.Parse(cenaPersonalnogTreninga);
                    }

                    Fajl.WriteFitnesCentre(fitnesCentri);
                    break;
                }
            }

            List<FitnesCentar> fitnesCentriNovi = Fajl.ReadFitnesCentar();
            List<FitnesCentar> potrebnaLista = new List<FitnesCentar>();
            Korisnik vlasnik = (Korisnik)Session["korisnik"];


            if (vlasnik.FitnesCentriVlasnika == null)
            {
                vlasnik.FitnesCentriVlasnika = new List<int>();
            }
            foreach (FitnesCentar fs in fitnesCentriNovi)
            {
                if (vlasnik.FitnesCentriVlasnika.Contains(fs.IDFitnesCentra) && fs.Brisanje == false)
                {
                    potrebnaLista.Add(fs);
                }
            }

            ViewBag.ListaFitnesCentara = potrebnaLista;
            return View("PregledFitnesCentara");
        }

        public ActionResult KreiranjeFitnesCentara()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CuvanjeFitnesCentra(string id, string naziv, string adresa, string godinaOtvaranja, string cenaMesecneClanarine, string cenaGodisnjeClanarine, string cenaJednogTreninga, string cenaGrupnogTreninga, string cenaPersonalnogTreninga)
        {
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            Korisnik ulogovani = (Korisnik)Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            if (id == "" || naziv == "" || adresa == "" || godinaOtvaranja == "" || cenaMesecneClanarine == "" || cenaGodisnjeClanarine == "" || cenaJednogTreninga == "" || cenaGrupnogTreninga == "" || cenaPersonalnogTreninga == "")
            {
                ViewBag.Message = "Niste uneli sva polja!";
                return View("KreiranjeFitnesCentara");
            }

            foreach (FitnesCentar fss in fitnesCentri)
            {
                if (fss.IDFitnesCentra == Int32.Parse(id))
                {
                    ViewBag.Message = "Fitnes centar " + fss.IDFitnesCentra + " je vec kreiran!";
                    return View("KreiranjeFitnesCentara");
                }
            }

            FitnesCentar fs = new FitnesCentar();
            fs.IDFitnesCentra = Int32.Parse(id);
            fs.Naziv = naziv;
            fs.Adresa = adresa;
            fs.GodinaOtvaranja = Int32.Parse(godinaOtvaranja);
            fs.CenaGodisnjeClanarine = Int32.Parse(cenaGodisnjeClanarine);
            fs.CenaMesecneClanarine = Int32.Parse(cenaMesecneClanarine);
            fs.CenaJednogTreninga = Int32.Parse(cenaJednogTreninga);
            fs.CenaGrupnogTreninga = Int32.Parse(cenaGrupnogTreninga);
            fs.CenaPersonalnogTreninga = Int32.Parse(cenaPersonalnogTreninga);
            fs.Vlasnik = ulogovani.KorisnickoIme;
            fs.Brisanje = false;

            fitnesCentri.Add(fs);
            //  Fajl.WriteFitnesCentre(fitnesCentri);
            Fajl.WriteFitnesCentar(fs);

           
            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == ulogovani.KorisnickoIme)
                {
                    ulogovani.FitnesCentriVlasnika.Add(fs.IDFitnesCentra);
                    Fajl.WriteKorisnike(korisnici);
                    break;
                }
            }

            List<FitnesCentar> fitnesCentriNovi = Fajl.ReadFitnesCentar();
            List<FitnesCentar> potrebnaLista = new List<FitnesCentar>();

            if (ulogovani.FitnesCentriVlasnika == null)
            {
                ulogovani.FitnesCentriVlasnika = new List<int>();
            }

            foreach (FitnesCentar fss in fitnesCentriNovi)
            {
                if (ulogovani.FitnesCentriVlasnika.Contains(fss.IDFitnesCentra) && fss.Brisanje == false)
                {
                    potrebnaLista.Add(fss);
                }
            }


            ViewBag.ListaFitnesCentara = potrebnaLista;
      
            return View("PregledFitnesCentara");
        }

        public ActionResult PregledSvihTrenera()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> potrebni = new List<FitnesCentar>();
            List<Korisnik> potrebniTreneri = new List<Korisnik>();

            if (vlasnik.FitnesCentriVlasnika == null)
            {
                vlasnik.FitnesCentriVlasnika = new List<int>();
            }
            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (vlasnik.FitnesCentriVlasnika.Contains(fs.IDFitnesCentra))
                {
                    potrebni.Add(fs);
                }
            }

            foreach (Korisnik k in korisnici)
            {
                foreach (FitnesCentar fs in potrebni)
                {
                    if (k.FitnesCentarTrenera == fs.IDFitnesCentra)
                    {
                        potrebniTreneri.Add(k);
                    }
                }
            }

            ViewBag.ListaTrenera = potrebniTreneri;
            return View();
        }

        public ActionResult BlokiranjeTrenera(string korisnickoImeTrenera)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == korisnickoImeTrenera)
                {
                    k.Blokiran = true;
                    Fajl.WriteKorisnike(korisnici);
                    break;
                }
            }

            ViewBag.Message = "Trener " + korisnickoImeTrenera + " je blokiran! Više neće moći da se prijavi u aplikaciju!";

            List<FitnesCentar> fitnesCentri = (List<FitnesCentar>)HttpContext.Application["fitnesCentri"];
            Korisnik vlasnik = (Korisnik)Session["korisnik"];
            List<FitnesCentar> potrebni = new List<FitnesCentar>();
            List<Korisnik> potrebniTreneri = new List<Korisnik>();

            if (vlasnik.FitnesCentriVlasnika == null)
            {
                vlasnik.FitnesCentriVlasnika = new List<int>();
            }
            foreach (FitnesCentar fs in fitnesCentri)
            {
                if (vlasnik.FitnesCentriVlasnika.Contains(fs.IDFitnesCentra))
                {
                    potrebni.Add(fs);
                }
            }


            foreach (Korisnik k in korisnici)
            {
                foreach (FitnesCentar fs in potrebni)
                {
                    if (k.FitnesCentarTrenera == fs.IDFitnesCentra && k.Blokiran == false)
                    {
                        potrebniTreneri.Add(k);
                    }
                }
            }

            ViewBag.ListaTrenera = potrebniTreneri;

            return View("PregledSvihTrenera");
        }
    }
}
 