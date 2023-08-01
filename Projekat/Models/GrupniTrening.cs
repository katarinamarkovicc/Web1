using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class GrupniTrening
    {
        public string Naziv { get; set; }
        public TipTreninga TipTreninga { get; set; }
        public int FitnesCentar { get; set; }   //ID Fitnes Centra u kom se odrzava trening
        public int TrajanjeTreninga { get; set; }
        public DateTime DatumIVremeTreninga { get; set; }
        public int MaksimalanBrojPosetilaca { get; set; }
        public List<string> SpisakPosetilaca { get; set; } //lista posetilaca, username-ovi posetilaca(korisnika)
        public bool Brisanje { get; set; }

        //dodatno
        public int IDGrupnogTreninga { get; set; }

        public GrupniTrening()
        {
            Brisanje = false;
        }

        public GrupniTrening(string naziv, TipTreninga tipTreninga, int fitnesCentar, int trajanjeTreninga, DateTime datumIVremeTreninga, int maksimalanBrojPosetilaca, List<string> spisakPosetilaca)
        {
            Naziv = naziv;
            TipTreninga = tipTreninga;
            FitnesCentar = fitnesCentar;
            TrajanjeTreninga = trajanjeTreninga;
            DatumIVremeTreninga = datumIVremeTreninga;
            MaksimalanBrojPosetilaca = maksimalanBrojPosetilaca;
            SpisakPosetilaca = spisakPosetilaca;
        }


    }
}