using Projekat.App_Start;
using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Projekat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           

            List<Korisnik> korisnici = Fajl.ReadKorisnik();
            HttpContext.Current.Application["korisnici"] = korisnici;

            List<FitnesCentar> fitnesCentri = Fajl.ReadFitnesCentar();
            HttpContext.Current.Application["fitnesCentri"] = fitnesCentri;

            List<GrupniTrening> grupniTreninzi = Fajl.ReadGrupniTrening();
            HttpContext.Current.Application["grupniTreninzi"] = grupniTreninzi;

            List<Komentar> komentari = Fajl.ReadKomentar();
            HttpContext.Current.Application["komentari"] = komentari;

        }
    }
}
