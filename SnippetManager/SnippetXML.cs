using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnippetManager
{
    class SnippetXML
    {
        private XDocument xml;
        public XDocument Xml {
            get {
                return xml;
            }
        }

        private XNamespace MicrosoftNs = "http://schemas.microsoft.com/VisualStudio/2005/CodeSnippet";

        //Default constructor called when no data will populate it
        public SnippetXML(XDocument doc) {
            xml = doc;
        }

        public SnippetXML(HeaderInfo headerInfo, SnippetInfo snippetInfo) {
            xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement codeSnippets = new XElement(MicrosoftNs + "CodeSnippets");
            codeSnippets.Add(BuildLocDefinition());
            codeSnippets.Add(BuildSnippet(headerInfo, snippetInfo));
            xml.Add(codeSnippets);
        }

        private XElement BuildLocDefinition() {
            XNamespace localNs = "urn:locstudio";
            XElement locDefinition = new XElement(localNs + "_locDefinition");
            locDefinition.Add(new XElement(localNs + "_locDefault", new XAttribute("_loc", "locNone")));

            string[] locationTagArray = { "Title", "Description", "Author", "ToolTip" };
            foreach(string tag in locationTagArray){
                XElement locTag = new XElement(localNs + "_locTag", new XAttribute("_loc", "locData")) { Value = tag };
                locDefinition.Add(locTag);
            }

            return locDefinition;
        }

        private XElement BuildSnippet(HeaderInfo headerInfo, SnippetInfo snippetInfo) {
            XElement codeSnippet = new XElement(MicrosoftNs + "CodeSnippet", new XAttribute("Format", "1.0.0"));

            XElement header = new XElement(MicrosoftNs + "Header");
            header.Add(new XElement(MicrosoftNs + "Title") { Value = headerInfo.Title });
            header.Add(new XElement(MicrosoftNs + "Shortcut") { Value = "" });
            header.Add(new XElement(MicrosoftNs + "Description") { Value = headerInfo.Description });
            header.Add(new XElement(MicrosoftNs + "Author") { Value = headerInfo.Author });

            XElement types = new XElement(MicrosoftNs + "SnippetTypes");
            types.Add(new XElement(MicrosoftNs + "SnippetType") { Value = headerInfo.SnippetType });
            header.Add(types);
            codeSnippet.Add(header);

            XElement snippet = new XElement(MicrosoftNs + "Snippet");
            snippet.Add(new XElement(MicrosoftNs + "Declarations") { Value = "" });
            XElement code = new XElement(MicrosoftNs + "Code", new XAttribute("Language", "SQL"));
            code.ReplaceNodes(new XCData(snippetInfo.Code));
            snippet.Add(code);
            codeSnippet.Add(snippet);

            return codeSnippet;
        }

        public HeaderInfo GetHeaderDataFromFile() {
            HeaderInfo headerInfo = new HeaderInfo();

            XElement header = xml.Descendants(MicrosoftNs + "Header").First();

            headerInfo.Title = header.Element(MicrosoftNs + "Title").Value;
            headerInfo.Author = header.Element(MicrosoftNs + "Author").Value;
            headerInfo.Description = header.Element(MicrosoftNs + "Description").Value;
            headerInfo.SnippetType = header.Descendants(MicrosoftNs + "SnippetType").First().Value;

            return headerInfo;
        }

        public SnippetInfo GetSnippetInfoFromFile() {
            SnippetInfo snippetInfo = new SnippetInfo();

            snippetInfo.Code = xml.Descendants(MicrosoftNs + "Code").First().Value;

            XElement declarations = xml.Descendants(MicrosoftNs + "Declarations").First();

            IEnumerable<XElement> literalXML = declarations.Descendants(MicrosoftNs + "Literal");

            foreach (XElement literal in literalXML) {
                snippetInfo.Literals.Add(new Literal() { Id = literal.Element(MicrosoftNs + "ID").Value, ToolTip = literal.Element(MicrosoftNs + "ToolTip").Value, DefaultText = literal.Element(MicrosoftNs + "Default").Value });
            }

            return snippetInfo;
        }
    }
}
