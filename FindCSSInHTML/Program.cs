using HapCss;
using HtmlAgilityPack;
using System.Collections.Generic;
using Fizzler.Systems.HtmlAgilityPack;
using System.Threading;
using System.IO;

namespace FindCSSInHTML {
    class Program {
        static string CACHEDSITEROOT = "C:\\Sandbox\\Grangetown\\FashionAdvice\\";
        static string SITENAME = "method4.co.uk";
        static string ROOT = "http://www." + SITENAME;
        static string PAGE = "/";
        static string SELECTOR = ".serviceDetail div.name";
        static int RATELIMIT = 500;

        static void Main(string[] args) {
            Program p = new Program();
            p.Go();
        }

        private void Go() {
            var web = new HtmlWeb();

            System.Console.WriteLine("Fashion Advice - helping you find your styles.");

            List<HtmlNode> siteTree = BuildSiteTree(web);

            System.Console.WriteLine("");
            System.Console.WriteLine("Done.");
            System.Console.ReadLine();
        }

        /// <summary>
        /// Using a standard "List<T>.Contains" doesnt seem to work very well
        /// with HtmlNodes. So we're making our own!
        /// </summary>
        /// <param name="list">List to check</param>
        /// <param name="node">Node to find</param>
        /// <returns></returns>
        private bool ListContains(List<HtmlNode> list, HtmlNode node) {
            foreach (HtmlNode listNode in list) {
                if (listNode.Attributes["href"] != null && node.Attributes["href"] != null) {
                    if (listNode.Attributes["href"].Value == node.Attributes["href"].Value) {
                        return true;
                    }
                }
            }
            return false;
        }


        private List<HtmlNode> BuildSiteTree(HtmlWeb web) {
            List<HtmlNode> siteTree = new List<HtmlNode>();

            var doc = web.Load(ROOT + PAGE);
            var node = doc.DocumentNode;
            siteTree.Add(node);

            GoDeeper(web, node, siteTree, 1);

            return siteTree;
        }

        /// <summary>
        /// Recursively find a selector in a site.
        /// </summary>
        /// <param name="web"></param>
        /// <param name="node"></param>
        /// <param name="previousNodes"></param>
        private void GoDeeper(HtmlWeb web, HtmlNode node, List<HtmlNode> previousNodes, int level) {
            Thread.Sleep(RATELIMIT);
            var linksOnNode = node.QuerySelectorAll("a");

            //We have a list of links for the page.
            foreach (var linkOnNode in linksOnNode) {

                //If we've already parsed that page, don't continue
                if (linkOnNode.Attributes["href"] != null) {
                    if (linkOnNode.Attributes["href"].Value != "/" &&
                        !ListContains(previousNodes, linkOnNode) &&
                        !linkOnNode.Attributes["href"].Value.Contains("?") &&
                        !linkOnNode.Attributes["href"].Value.Contains(".")
                        ) {

                        //Ensure the page is inside our intended site
                        if (linkOnNode.Attributes["href"].Value.StartsWith("/")) {

                            previousNodes.Add(linkOnNode);
                            FindSelectorInNode(web, linkOnNode);

                            var linkPage = web.Load(ROOT + linkOnNode.Attributes["href"].Value);
                            GoDeeper(web, linkPage.DocumentNode, previousNodes, ++level);

                        }
                    } else {
                        //System.Console.WriteLine("Link has already been checked. Link: " + linkOnNode.Attributes["href"].Value);
                    }
                }
            }
        }

        private void FindSelectorInNode(HtmlWeb web, HtmlNode node) {
            if (node.Attributes["href"] != null) {
                var doc = web.Load(ROOT + node.Attributes["href"].Value);

                CachePage(node, doc);

                var selectors = doc.DocumentNode.QuerySelectorAll(SELECTOR);
                bool hasSelector = false;
                foreach (var selector in selectors) {
                    hasSelector = true;
                    break;
                }
                if (hasSelector) {
                    System.Console.WriteLine("");
                    System.Console.Write("Found selector: " + SELECTOR + " on page " + node.Attributes["href"].Value);
                } else {
                    System.Console.Write(".");
                }
            }   
        }

        private void CachePage(HtmlNode node, HtmlDocument doc) {
            string dir = CACHEDSITEROOT + SITENAME + node.Attributes["href"].Value + "page.html";

            FileInfo file = new FileInfo(dir);
            file.Directory.Create();

            File.WriteAllText(file.FullName, doc.DocumentNode.OuterHtml);
        }
    }
}
