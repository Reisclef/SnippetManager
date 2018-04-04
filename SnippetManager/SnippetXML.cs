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
        public SnippetXML() : this(null) {
        }

        public SnippetXML(HeaderInfo headerInfo) {
            xml = new XDocument(new XDeclaration("1.0", "utf-8", null));
            XElement codeSnippets = new XElement(MicrosoftNs + "CodeSnippets");
            codeSnippets.Add(BuildLocDefinition());
            codeSnippets.Add(BuildSnippet(headerInfo));
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

        private XElement BuildSnippet(HeaderInfo headerInfo) {
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
            snippet.Add(new XElement(MicrosoftNs + "Code", new XAttribute("Language", "SQL")){ Value = "" });
            codeSnippet.Add(snippet);

            return codeSnippet;
        }
    }
}
