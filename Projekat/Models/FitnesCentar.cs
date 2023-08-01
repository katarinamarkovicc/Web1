using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class FitnesCentar
    {
        public string Naziv { get; set; }
        public string Adresa { get; set; }
        /*public string ImeUlice { get; set; }
        public string Broj { get; set; }
        public string Mesto { get; set; }
        public string PostanskiBroj { get; set; }*/
        public int GodinaOtvaranja { get; set; }
        public string Vlasnik { get; set; } //korisnisko ime Vlasnika
        public int CenaMesecneClanarine { get; set; }
        public int CenaGodisnjeClanarine { get; set; }
        public int CenaJednogTreninga { get; set; }
        public int CenaGrupnogTreninga { get; set; }
        public int CenaPersonalnogTreninga { get; set; }
        public bool Brisanje { get; set; }

        //dodatno
        public int IDFitnesCentra { get; set; }

        public FitnesCentar()
        {
            Brisanje = false;
        }

        public FitnesCentar(string naziv, string adresa, int godinaOtvaranja, string vlasnik, int cenaMesecneClanarine, int cenaGodisnjeClanarine, int cenaJednogTreninga, int cenaGrupnogTreninga, int cenaPersonalnogTreninga)
        {
            Naziv = naziv;
            Adresa = adresa;
            GodinaOtvaranja = godinaOtvaranja;
            Vlasnik = vlasnik;
            CenaMesecneClanarine = cenaMesecneClanarine;
            CenaGodisnjeClanarine = cenaGodisnjeClanarine;
            CenaJednogTreninga = cenaJednogTreninga;
            CenaGrupnogTreninga = cenaGrupnogTreninga;
            CenaPersonalnogTreninga = cenaPersonalnogTreninga;
        }
    }
}