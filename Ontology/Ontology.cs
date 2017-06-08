using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using VDS.RDF.Writing;
using System.IO;

namespace ObjectIdentificationWebApp.Ontology
{
    public static class KnowledgeBase
    {
        //Fields
        public static string hostURI = "http://" + HttpContext.Current.Request.Url.Authority + "/";//+"/ObjectIdentification#";
        //private static string ontologyFile = HttpContext.Current.Server.MapPath("~/Ontology/ObjectIdentification.owl");
        private static string ontologyFile = HttpContext.Current.Server.MapPath("~/Ontology/KB.n3");
        private static Graph graph = new Graph();

        //Constructor
        static KnowledgeBase()
        {
            //Read text file
            string ontology = File.ReadAllText(ontologyFile);
            /*
            //Find original namespace
            int locLessThan = ontology.IndexOf("<")+1;
            int locGreaterThan = ontology.IndexOf(">");
            string origNamespace = ontology.Substring(locLessThan, locGreaterThan - locLessThan);

            //Replace original namespace with host namespace
            ontology = ontology.Replace(origNamespace, hostURI);
            */
            //Load into graph 
            ontology = ontology.Replace("http://system.mentolog.org/", hostURI);
            ontology = ontology.Replace("properties/", "predicates/");
            graph.LoadFromString(ontology);
        }

        #region Lists
        public static List<SparqlResult> getListByClassName(string name)
        {
            //Query graph for owl classes
            SparqlResultSet resultSet = graph.ExecuteQuery(Queries.listByClassName(name)) as SparqlResultSet;

            //Return results
            return resultSet.ToList();
        }
        public static List<SparqlResult> getList_OwlClass()
        {
            //Query graph for owl classes
            SparqlResultSet resultSet = graph.ExecuteQuery(Queries.owlClass) as SparqlResultSet;

            //Return results
            return resultSet.ToList();
        }
        public static List<SparqlResult> getList_OwlPredicate()
        {
            //Query graph for owl classes
            SparqlResultSet resultSet = graph.ExecuteQuery(Queries.owlPredicates) as SparqlResultSet;

            //Return results
            return resultSet.ToList();
        }
        public static List<SparqlResult> getList_OwlResource()
        {
            //Query graph for owl classes
            SparqlResultSet resultSet = graph.ExecuteQuery(Queries.owlResources) as SparqlResultSet;

            //Return results
            return resultSet.ToList();
        }
        public static List<SparqlResult> getList_CustomQuery(string qry)
        {
            //Query graph for owl classes
            SparqlResultSet resultSet = graph.ExecuteQuery(qry) as SparqlResultSet;

            //Return results
            return resultSet.ToList();
        }
        #endregion

        //Details
        public static SparqlResultSet subjectTriplesByName(string name)
        {
            //Search triples
            SparqlResultSet resultSet = graph.ExecuteQuery(Queries.subjectTriplesByName(name)) as SparqlResultSet;
            
            //Return results
            return resultSet;
        }

        //Methods
        public static string GetNodeString(INode node)
        {
            string s = node.ToString();
            switch (node.NodeType)
            {
                case NodeType.Uri:
                    int lio = s.LastIndexOf('#');
                    if (lio == -1)
                    {
                        int lio2 = s.LastIndexOf('/');
                        if (lio2 == -1)
                            return s;
                        else
                            return s.Substring(lio2 + 1);
                    }                        
                    else
                        return s.Substring(lio + 1);                    
                case NodeType.Literal:
                    int delIndex = s.IndexOf("^^");
                    if (delIndex > -1)
                        return s.Substring(0, delIndex);
                    else
                        return string.Format("{0}", s);
                default:
                    return s;
            }
        }

        //Classes
        public static class Queries
        {
            private static string kbNamespace = hostURI;

            public static string listByClassName(string name)
            {
                string qry = "PREFIX mpc: <" + kbNamespace + "classes/>" + Environment.NewLine +
                             "PREFIX mpp: <" + kbNamespace + "predicates/>" + Environment.NewLine +
                             "PREFIX mpr: <" + kbNamespace + "resources/>" + Environment.NewLine +
                    @"
                    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                    PREFIX owl: <http://www.w3.org/2002/07/owl#>
                    PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                    PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>

                    SELECT DISTINCT ?subject ?label
                    WHERE
                    {
                        ?subject rdf:type mpc:{name}.
                        ?subject rdfs:label ?label.                        
                    } ORDER BY ?label
                    ";

                qry = qry.Replace("{name}", name);

                return qry;
            }

            public static string subjectTriplesByName(string name)
            {
                string qry = "PREFIX mpc: <" + kbNamespace + "classes/>" + Environment.NewLine +
                             "PREFIX mpp: <" + kbNamespace + "predicates/>" + Environment.NewLine +
                             "PREFIX mpr: <" + kbNamespace + "resources/>" + Environment.NewLine +
                    @"
                    PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                    PREFIX owl: <http://www.w3.org/2002/07/owl#>
                    PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                    PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>

                    SELECT DISTINCT ?predicate ?object ?predLabel ?objLabel ?label
                    WHERE
                    {
                        {name} ?predicate ?object.
                        {name} rdfs:label ?label.
                        ?predicate rdfs:label ?predLabel
                        OPTIONAL {?object rdfs:label ?objLabel}
                    } ORDER BY ?predLabel
                    ";

                qry = qry.Replace("{name}", name);

                return qry;
            }            

            public static string owlClass 
            {
                get
                {
                    string qry = "PREFIX mpc: <" + kbNamespace + "classes/>" + Environment.NewLine +
                             "PREFIX mpp: <" + kbNamespace + "predicates/>" + Environment.NewLine +
                             "PREFIX mpr: <" + kbNamespace + "resources/>" + Environment.NewLine +
                        @"
PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
PREFIX owl: <http://www.w3.org/2002/07/owl#>
PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>

SELECT DISTINCT ?subject ?label

WHERE
{
    ?subject rdf:type owl:Class.
    ?subject rdfs:label ?label.
}

ORDER BY ?subject
                        ";
                    return qry;
                }
            }
            public static string owlPredicates
            {
                get
                {
                    string qry = "PREFIX mpc: <" + kbNamespace + "classes/>" + Environment.NewLine +
                             "PREFIX mpp: <" + kbNamespace + "predicates/>" + Environment.NewLine +
                             "PREFIX mpr: <" + kbNamespace + "resources/>" + Environment.NewLine +
                        @"
                        PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                        PREFIX owl: <http://www.w3.org/2002/07/owl#>
                        PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                        PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>

                        SELECT DISTINCT ?subject ?label

                        WHERE
                        {
                            ?subject rdf:type ?o.
                            ?subject rdfs:label ?label.
                            FILTER (?o IN (owl:ObjectProperty, owl:DatatypeProperty) )
                        }

                        ORDER BY ?subject
                        ";
                    return qry;
                }
            }
            public static string owlResources
            {
                get
                {
                    string qry = "PREFIX mpc: <" + kbNamespace + "classes/>" + Environment.NewLine +
                             "PREFIX mpp: <" + kbNamespace + "predicates/>" + Environment.NewLine +
                             "PREFIX mpr: <" + kbNamespace + "resources/>" + Environment.NewLine +
                        @"
                        PREFIX rdf: <http://www.w3.org/1999/02/22-rdf-syntax-ns#>
                        PREFIX owl: <http://www.w3.org/2002/07/owl#>
                        PREFIX rdfs: <http://www.w3.org/2000/01/rdf-schema#>
                        PREFIX xsd: <http://www.w3.org/2001/XMLSchema#>

                        SELECT DISTINCT ?subject ?label

                        WHERE
                        {
                            ?subject rdf:type owl:NamedIndividual.
                            ?subject rdfs:label ?label
                        }

                        ORDER BY ?subject
                        ";
                    return qry;
                }
            }
        }
    }
}