using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class TrenerController : Controller
    {
        // GET: Trener
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PregledGrupnihTreninga()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }

            ViewBag.ListaTreninga = potrebnaLista;

            return View();
        }

        public ActionResult BrisanjeTreninga(int idTreninga)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }
            
            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.IDGrupnogTreninga == idTreninga)
                {
                    if (gt.SpisakPosetilaca == null)
                    {
                        gt.SpisakPosetilaca = new List<string>();
                    }

                    if (gt.SpisakPosetilaca.Count == 0 && gt.DatumIVremeTreninga > DateTime.Now)
                    {
                        gt.Brisanje = true;
                        
                        Fajl.WriteGrupneTreninge(grupniTreninzi);
                        break;
                    }
                    else
                    {
                        ViewBag.Message = "Postoje posetioci ili posetilac, koji se prijavio da učestvuje u ovom treningu! Ili je trening prošao!";

                        ViewBag.ListaTreninga = potrebnaLista;
                        return View("PregledGrupnihTreninga");
                    }
                }
               
            }

            List<GrupniTrening> grupniTreninziNovi = Fajl.ReadGrupniTrening();
            List<GrupniTrening> potrebnaListaNova = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninziNovi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.Brisanje == false)
                {                   
                    potrebnaListaNova.Add(gt);
                }
            }

            ViewBag.ListaTreninga = potrebnaListaNova;

            return View("PregledGrupnihTreninga");
        }

        public ActionResult ModifikacijaTreninga(int idTreninga)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            GrupniTrening kojiTrebaDaSeMenja = new GrupniTrening();

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.IDGrupnogTreninga == idTreninga)
                {
                    kojiTrebaDaSeMenja = gt;
                    break;
                }
            }
            
            return View(kojiTrebaDaSeMenja);
        }

        [HttpPost]
        public ActionResult ModifikacijaPosleForme(string naziv, string tipTreninga, string trajanjeTreninga, string datumIVremeTreninga, string maksimalanBrojPosetilaca, int idKojiTrebaDaSeMenja)   //probati sa celim modelom ako ovo ne bude radilo
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];


            if (naziv == "" && tipTreninga == "" && trajanjeTreninga == "" && datumIVremeTreninga == "" && maksimalanBrojPosetilaca == "")
            {
                ViewBag.Message = "Niste uneli nijedan parametar forme!";
            }


            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (gt.IDGrupnogTreninga == idKojiTrebaDaSeMenja)
                {
                    if (gt.DatumIVremeTreninga > DateTime.Now)
                    {
                        if (naziv != "")  //probati sa praznim stringom, ako ovo ne bude radilo
                        {
                            gt.Naziv = naziv;
                        }

                        if (tipTreninga != "")
                        {
                            if (tipTreninga == "YOGA")
                            {
                                gt.TipTreninga = TipTreninga.YOGA;
                            }
                            else if (tipTreninga == "LESS_MILLS_TONE")
                            {
                                gt.TipTreninga = TipTreninga.LESS_MILLS_TONE;
                            }
                            else if (tipTreninga == "BODY_PUMP")
                            {
                                gt.TipTreninga = TipTreninga.BODY_PUMP;
                            }
                        }

                        if (trajanjeTreninga != "")
                        {
                            gt.TrajanjeTreninga = Int32.Parse(trajanjeTreninga);
                        }

                        if (datumIVremeTreninga != "")
                        {
                            gt.DatumIVremeTreninga = DateTime.Parse(datumIVremeTreninga);
                        }

                        if (maksimalanBrojPosetilaca != "")
                        {
                            gt.MaksimalanBrojPosetilaca = Int32.Parse(maksimalanBrojPosetilaca);
                        }

                        Fajl.WriteGrupneTreninge(grupniTreninzi);
                    }
                    else
                    {
                        ViewBag.Message2 = "Ne možete menjati trening koji se još uvek nije održao!";
                    }
                    break;
                }
            }

            List<GrupniTrening> grupniTreninziNovi = Fajl.ReadGrupniTrening();
            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninziNovi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }

            ViewBag.ListaTreninga = potrebnaLista;

            return View("PregledGrupnihTreninga");
        }

        public ActionResult PregledProslihGrupnihTreninga()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }
            
            ViewBag.ListaTreninga = potrebnaLista;

            return View();
        }

        public ActionResult VidiPosetioce(int idTreninga)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            List<Korisnik> potrebniKorisnici = new List<Korisnik>();
            

            foreach (Korisnik k in korisnici)
            {
                if (k.ListaPrijavljenihTreninga == null)
                {
                    k.ListaPrijavljenihTreninga = new List<int>();
                }
                if (k.Uloga == UlogaKorisnika.POSETILAC && k.ListaPrijavljenihTreninga.Contains(idTreninga))
                {
                    potrebniKorisnici.Add(k);
                }
            }
            ViewBag.ListaKorisnika = potrebniKorisnici;
            return View();
        }

        //-----------------------------PRETRAGE I SORTIRANJA PROSLIH TRENINGA---------------------------

        [HttpPost]
        public ActionResult Pretraga(string naziv, string tipTreninga, string minimalnaGranica, string maksimalnaGranica)
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> pretrazenaLista = new List<GrupniTrening>();

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }


            foreach (GrupniTrening gt in potrebnaLista)
            {
                //ako nije uneo nijedan parametar
                if (naziv == "" && tipTreninga == "" && minimalnaGranica == "" && maksimalnaGranica == "")    // 0 za int?
                {
                    ViewBag.Message = "Niste uneli nijedan parametar forme!";
                }

                if (naziv != "")
                {
                    if (gt.Naziv.Contains(naziv))
                    {
                        pretrazenaLista.Add(gt);
                    }
                    else continue;
                }
                if (tipTreninga != "")
                {
                    if (gt.TipTreninga.ToString() == tipTreninga)
                    {
                        if (naziv == "")
                        {
                            pretrazenaLista.Add(gt);
                        }
                    }
                    else
                    {
                        if (pretrazenaLista.Contains(gt))
                        {
                            pretrazenaLista.Remove(gt);
                        }
                    }
                }
                if (minimalnaGranica != "")
                {
                    if (gt.DatumIVremeTreninga >= DateTime.Parse(minimalnaGranica))
                    {
                        if (naziv == "" && tipTreninga == "")
                        {
                            pretrazenaLista.Add(gt);
                        }
                    }
                    else
                    {
                        if (pretrazenaLista.Contains(gt))
                        {
                            pretrazenaLista.Remove(gt);
                        }
                    }
                }
                if (maksimalnaGranica != "")
                {
                    if (gt.DatumIVremeTreninga <= DateTime.Parse(maksimalnaGranica))
                    {
                        if (naziv == "" && tipTreninga == "" && minimalnaGranica == "")
                        {
                            pretrazenaLista.Add(gt);
                        }
                    }
                    else
                    {
                        if (pretrazenaLista.Contains(gt))
                        {
                            pretrazenaLista.Remove(gt);
                        }
                    }
                }
            }

            ViewBag.ListaTreninga = pretrazenaLista;


            return View("PregledProslihGrupnihTreninga");
        }

        public ActionResult SortirajPoNazivuRastuce()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }

        

            List<GrupniTrening> sortiraniPoNazivu = potrebnaLista.OrderBy(o => o.Naziv).ToList();

            ViewBag.ListaTreninga = sortiraniPoNazivu;

            return View("PregledProslihGrupnihTreninga");
        }

        public ActionResult SortirajPoNazivuOpadajuce()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }

           

            List<GrupniTrening> sortiraniPoNazivu = potrebnaLista.OrderByDescending(o => o.Naziv).ToList();

            ViewBag.ListaTreninga = sortiraniPoNazivu;

            return View("PregledProslihGrupnihTreninga");
        }

        public ActionResult SortirajPoTipuTreningaRastuce()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }


            List<GrupniTrening> sortiraniPoTipuTreninga = potrebnaLista.OrderBy(o => o.TipTreninga).ToList();

            ViewBag.ListaTreninga = sortiraniPoTipuTreninga;

            return View("PregledProslihGrupnihTreninga");
        }

        public ActionResult SortirajPoTipuTreningaOpadajuce()
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }


            List<GrupniTrening> sortiraniPoTipuTreninga = potrebnaLista.OrderByDescending(o => o.TipTreninga).ToList();

            ViewBag.ListaTreninga = sortiraniPoTipuTreninga;

            return View("PregledProslihGrupnihTreninga");
        }

        public ActionResult SortirajPoDatumuRastuce()
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }


            List<GrupniTrening> sortiraniPoTipuTreninga = potrebnaLista.OrderBy(o => o.DatumIVremeTreninga).ToList();

            ViewBag.ListaTreninga = sortiraniPoTipuTreninga;

            return View("PregledProslihGrupnihTreninga");
        }

        public ActionResult SortirajPoDatumuOpadajuce()
        {

            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            Korisnik ulogovan = (Korisnik)Session["korisnik"];

            List<GrupniTrening> potrebnaLista = new List<GrupniTrening>();

            if (ulogovan.ListaTreningaTrenera == null)
            {
                ulogovan.ListaTreningaTrenera = new List<int>();
            }

            foreach (GrupniTrening gt in grupniTreninzi)
            {
                if (ulogovan.ListaTreningaTrenera.Contains(gt.IDGrupnogTreninga) && gt.DatumIVremeTreninga < DateTime.Now && gt.Brisanje == false)
                {
                    potrebnaLista.Add(gt);
                }
            }


            List<GrupniTrening> sortiraniPoTipuTreninga = potrebnaLista.OrderByDescending(o => o.DatumIVremeTreninga).ToList();

            ViewBag.ListaTreninga = sortiraniPoTipuTreninga;

            return View("PregledProslihGrupnihTreninga");
        }

        //-------------------------------KREIRANJE NOVOG TRENINGA------------------------------------

        public ActionResult KreiranjeTreninga()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CuvanjeTreninga(string tipTreninga, string naziv, string id, string trajanjeTreninga, string datumIVremeTreninga, string maksimalanBrojPosetilaca)
        {
            List<GrupniTrening> grupniTreninzi = (List<GrupniTrening>)HttpContext.Application["grupniTreninzi"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];
            Korisnik ulogovani = (Korisnik)Session["korisnik"];

           // DateTime automatski = new DateTime(0001, 1, 1, 12, 0, 0);

            if (tipTreninga == "" || naziv == "" || id == "" || trajanjeTreninga == "" || datumIVremeTreninga == "" || maksimalanBrojPosetilaca == "")
            {
                ViewBag.Message = "Niste uneli sva polja!";
                return View("KreiranjeTreninga");
            }

            foreach (GrupniTrening gtt in grupniTreninzi)
            {
                if (gtt.IDGrupnogTreninga == Int32.Parse(id))
                {
                    ViewBag.Message = "Trening " + gtt.IDGrupnogTreninga + " je vec kreiran!";
                    return View("KreiranjeTreninga");
                }
            }

            GrupniTrening gt = new GrupniTrening();
            gt.Naziv = naziv;
            gt.IDGrupnogTreninga = Int32.Parse(id);

            if (tipTreninga == "YOGA")
            {
                gt.TipTreninga = TipTreninga.YOGA;
            }
            else if (tipTreninga == "LESS_MILLS_TONE")
            {
                gt.TipTreninga = TipTreninga.LESS_MILLS_TONE;
            }
            else if (tipTreninga == "BODY_PUMP")
            {
                gt.TipTreninga = TipTreninga.BODY_PUMP;
            }

            gt.MaksimalanBrojPosetilaca = Int32.Parse(maksimalanBrojPosetilaca);
            gt.TrajanjeTreninga = Int32.Parse(trajanjeTreninga);
            //gt.DatumIVremeTreninga = DateTime.Parse(datumIVremeTreninga);

            DateTime prosledjeniDatum = DateTime.Parse(datumIVremeTreninga);

            if (prosledjeniDatum < (DateTime.Now.AddDays(3)))
            {
                ViewBag.Message2 = "Ne možete kreirati trening u prošlosti. Morate ga kreirati 3 dana unapred!";
                return View("KreiranjeTreninga");
            }
            else
            {
                gt.DatumIVremeTreninga = prosledjeniDatum;
            }

            if (gt.SpisakPosetilaca == null)
            {
                gt.SpisakPosetilaca = new List<string>();
            }

            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == ulogovani.KorisnickoIme)
                {
                    ulogovani.ListaTreningaTrenera.Add(Int32.Parse(id));
                    Fajl.WriteKorisnike(korisnici);
                    break;
                }
            }
          

            grupniTreninzi.Add(gt);
            Fajl.WriteGrupniTrening(gt);

            ViewBag.Message3 = "Uspešno kreiranje treninga!";
            return RedirectToAction("PregledGrupnihTreninga");
        }
    }
}