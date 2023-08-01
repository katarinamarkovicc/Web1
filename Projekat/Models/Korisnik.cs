using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Korisnik
    {
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public PolKorisnika Pol { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public UlogaKorisnika Uloga { get; set; }
        public List<int> ListaPrijavljenihTreninga { get; set; }    //ako je korisnik posetilac
        public List<int> ListaTreningaTrenera { get; set; } //ako je korisnik trener
        public int FitnesCentarTrenera { get; set; }    //ako je korisnik trener
        public List<int> FitnesCentriVlasnika { get; set; } //ako je korisnik vlasnik
        public bool Blokiran { get; set; }

        public Korisnik()
        {
            Blokiran = false;
        }

        public Korisnik(string korisnickoIme, string lozinka, string ime, string prezime, PolKorisnika pol, string email, DateTime datumRodjenja, UlogaKorisnika uloga, List<int> listaPrijavljenihTreninga, List<int> listaTreningaTrenera, int fitnesCentarTrenera, List<int> fitnesCentriVlasnika)
        {
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            ListaPrijavljenihTreninga = listaPrijavljenihTreninga;
            ListaTreningaTrenera = listaTreningaTrenera;
            FitnesCentarTrenera = fitnesCentarTrenera;
            FitnesCentriVlasnika = fitnesCentriVlasnika;
        }
    }
}