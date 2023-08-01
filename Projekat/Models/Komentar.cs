using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Komentar
    {
        public string Posetilac { get; set; }   //korisnicko ime Posetioca
        public int FitnesCentar { get; set; }
        public string Tekst { get; set; }
        public int Ocena { get; set; }

        public Komentar()
        {

        }

        public Komentar(string posetilac, int fitnesCentar, string tekst, int ocena)
        {
            Posetilac = posetilac;
            FitnesCentar = fitnesCentar;
            Tekst = tekst;
            Ocena = ocena;
        }
    }
}