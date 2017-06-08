using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObjectIdentificationWebApp.Ontology;
using VDS.RDF.Query;
using ObjectIdentificationWebApp.Models;


namespace ObjectIdentificationWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }

        [Route("mentMat/{mentRes}")]
        public ViewResult mentMat(string mentRes)
        {
            List<SparqlResult> kb;
            
            kb = KnowledgeBase.getListByClassName(mentRes);
            ViewBag.Prefix = "Class";
            ViewBag.Title = mentRes;
            return View("mentMat", kb);
        }

        [Route("classes")]
        public ViewResult classes()
        {
            ViewBag.Title = "Classes";
            ViewBag.Prefix = "Class";
            List<SparqlResult> kb = KnowledgeBase.getList_OwlClass();
            return View("List", kb);
        }

        [Route("classes/{className}")]
        public ViewResult classes(string className)
        {
            
            ViewBag.Prefix = "Class";            
            SparqlResultSet triples = KnowledgeBase.subjectTriplesByName("mpc:"+className);
            ViewBag.Title = triples[0]["label"];           
            return View("Triple", triples);
        }

        [Route("predicates")]
        public ViewResult predicates()
        {
            ViewBag.Title = "Predicates";
            ViewBag.Prefix = "Predicate";
            List<SparqlResult> kb = KnowledgeBase.getList_OwlPredicate();
            return View("List", kb);
        }

        [Route("predicates/{predicate}")]
        public ViewResult predicates(string predicate)
        {
            
            ViewBag.Prefix = "Predicate";
            SparqlResultSet triples = KnowledgeBase.subjectTriplesByName("mpp:" + predicate);
            ViewBag.Title = triples[0]["label"];
            return View("Triple", triples);
        }

        [Route("resources")]
        public ViewResult resources()
        {
            ViewBag.Title = "Resources";
            ViewBag.Prefix = "Resource";
            List<SparqlResult> kb = KnowledgeBase.getList_OwlResource();
            return View("List", kb);
        }

        [Route("resources/{resource}")]
        [Route("Res/{resource}")]
        public ViewResult resources(string resource)
        {
           
            ViewBag.Prefix = "Resource";
            SparqlResultSet triples = KnowledgeBase.subjectTriplesByName("mpr:" + resource);
            ViewBag.Title = triples[0]["label"];
            return View("Triple", triples);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("SPARQL")]
        public ViewResult SparqlPost(CustomQry cQry)
        {
            return View("Sparql", cQry);
        }

        [Route("SPARQL")]
        public ViewResult Sparql()
        {
            return View("Sparql");
        }
    }
}