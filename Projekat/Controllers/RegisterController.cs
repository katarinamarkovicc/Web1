using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistracijaKorisnika(Korisnik kor, string pol)
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

            kor.Uloga = UlogaKorisnika.POSETILAC;

            DateTime automatski = new DateTime(0001, 1, 1, 12, 0, 0);

            if (pol == "" || kor.DatumRodjenja == automatski || kor.KorisnickoIme == null || kor.Lozinka == null || kor.Email == null || kor.Ime == null || kor.Prezime == null)
            {
                ViewBag.Message = "Niste uneli sva polja!";
                return View("Index");
            }

            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == kor.KorisnickoIme)
                {
                    ViewBag.Message = "Korisnik " + k.KorisnickoIme + " je vec registrovan!";
                    return View("Index");
                }
            }

            korisnici.Add(kor);
            Fajl.WriteKorisnik(kor);

            Session["korisnik"] = kor;

            ViewBag.Message = "Uspesna registracija!";

            return RedirectToAction("FitnesCentriPosetilac", "FitnesCentri");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogovanjeKorisnika(string korisnickoIme, string lozinka)
        {         
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Application["korisnici"];

           

            foreach (Korisnik k in korisnici)
            {
                if (k.KorisnickoIme == korisnickoIme && k.Lozinka == lozinka)
                {
                    if (k.Blokiran == true)
                    {
                        ViewBag.Message1 = "Ne možete se prijaviti, jer ste blokirani!";
                        return View("Login");
                    }
                    Session["korisnik"] = k;

                    if (k.Uloga == UlogaKorisnika.POSETILAC)
                    {
                        return RedirectToAction("FitnesCentriPosetilac", "FitnesCentri");
                    }
                    if (k.Uloga == UlogaKorisnika.TRENER)
                    {
                        return RedirectToAction("FitnesCentriTrener", "FitnesCentri");
                    }
                    if (k.Uloga == UlogaKorisnika.VLASNIK)
                    {
                        return RedirectToAction("FitnesCentriVlasnik", "FitnesCentri");
                    }
                }
            }

            ViewBag.Message = "Nije pronadjen korisnik sa datim podacima!";


            return View("Login");
        }

        public ActionResult LogOut()
        {
            Session["korisnik"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}