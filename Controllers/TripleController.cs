using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using System.IO;
using ObjectIdentificationWebApp.Ontology;

namespace ObjectIdentificationWebApp.Controllers
{
    public class TripleController : Controller
    {
        /*
        [Route("Classes/{className}")]
        public ViewResult Class(string className)
        {
            ViewBag.Title = className;
            ViewBag.Prefix = "Class";
            SparqlResultSet triples = KnowledgeBase.subjectTriplesByName(className);
            return View("Triple", triples);
        }
        
        [Route("Predicate/{predicate}")]
        public ViewResult Predicate(string predicate)
        {
            ViewBag.Title = predicate;
            ViewBag.Prefix = "Predicate";
            SparqlResultSet triples = KnowledgeBase.subjectTriplesByName(predicate);
            return View("Triple", triples);
        }
        
        [Route("Resource/{resource}")]
        [Route("Res/{resource}")]
        public ViewResult Resource(string resource)
        {
            ViewBag.Title = resource;
            ViewBag.Prefix = "Resource";
            SparqlResultSet triples = KnowledgeBase.subjectTriplesByName(resource);
            return View("Triple", triples);
        }

        [Route("#{resource}")]
        public ViewResult Resource2(string resource)
        {
            return Resources(resource);
        }
*/
        //Methods
        private static string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                        return s;
                    else
                        return s.Substring(lio + 1);
                case NodeType.Literal:
                    return string.Format("\"{0}\"", s);
                default:
                    return s;
            }
        }
    }
}