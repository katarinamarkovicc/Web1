using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Projekat.Models
{
    public class Fajl
    {
        public static List<FitnesCentar> fitnesCentri;

        public static List<FitnesCentar> ReadFitnesCentar()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/fitnesCentri.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(file);

                return fitnesCentri;
            }
        }

        public static FitnesCentar WriteFitnesCentar(FitnesCentar fs)
        {
            string fajl;
            List<FitnesCentar> fitnesCentri;

            string path = HostingEnvironment.MapPath("~/App_Data/fitnesCentri.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                fitnesCentri = JsonConvert.DeserializeObject<List<FitnesCentar>>(file);
                fitnesCentri.Add(fs);
                fajl = JsonConvert.SerializeObject(fitnesCentri);
            }

            File.WriteAllText(path, fajl);
            return fs;
        }

        public static void WriteFitnesCentre(List<FitnesCentar> fs)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/fitnesCentri.json");
            string file;
            file = JsonConvert.SerializeObject(fs);
            File.WriteAllText(path, "");
            File.WriteAllText(path, file);
        }

        /////////////////////////////////////////////////////////////////////////

        public static List<Korisnik> korisnici;

        public static List<Korisnik> ReadKorisnik()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(file);

                return korisnici;
            }
        }

        public static Korisnik WriteKorisnik(Korisnik k)
        {
            string fajl;
            List<Korisnik> korisnici;

            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                korisnici = JsonConvert.DeserializeObject<List<Korisnik>>(file);
                korisnici.Add(k);
                fajl = JsonConvert.SerializeObject(korisnici);
            }

            File.WriteAllText(path, fajl);
            return k;
        }

        public static void WriteKorisnike(List<Korisnik> korisnici)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.json");
            string file;
            file = JsonConvert.SerializeObject(korisnici);
            File.WriteAllText(path, "");
            File.WriteAllText(path, file);
        }

        /////////////////////////////////////////////////////////////////////////

        public static List<GrupniTrening> grupniTreninzi;

        public static List<GrupniTrening> ReadGrupniTrening()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/grupniTreninzi.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(file);

                return grupniTreninzi;
            }
        }

        public static GrupniTrening WriteGrupniTrening(GrupniTrening gt)
        {
            string fajl;
            List<GrupniTrening> grupniTreninzi;

            string path = HostingEnvironment.MapPath("~/App_Data/grupniTreninzi.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                grupniTreninzi = JsonConvert.DeserializeObject<List<GrupniTrening>>(file);
                grupniTreninzi.Add(gt);
                fajl = JsonConvert.SerializeObject(grupniTreninzi);
            }

            File.WriteAllText(path, fajl);
            return gt;
        }

        public static void WriteGrupneTreninge(List<GrupniTrening> gt)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/grupniTreninzi.json");
            string file;
            file = JsonConvert.SerializeObject(gt);
            File.WriteAllText(path, "");
            File.WriteAllText(path, file);
        }

        /////////////////////////////////////////////////////////////////////////

        public static List<Komentar> komentari;

        public static List<Komentar> ReadKomentar()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/komentari.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(file);

                return komentari;
            }
        }

        public static Komentar WriteKomentar(Komentar k)
        {
            string fajl;
            List<Komentar> komentari;

            string path = HostingEnvironment.MapPath("~/App_Data/komentari.json");

            using (StreamReader r = new StreamReader(path))
            {
                string file = r.ReadToEnd();
                komentari = JsonConvert.DeserializeObject<List<Komentar>>(file);
                komentari.Add(k);
                fajl = JsonConvert.SerializeObject(komentari);
            }

            File.WriteAllText(path, fajl);
            return k;
        }

    }
}