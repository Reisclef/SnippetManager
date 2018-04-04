using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnippetManager
{
    class HeaderInfo
    {
        private string title;
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
            }
        }

        private string description;
        public string Description {
            get {
                return description;
            }
            set {
                description = value;
            }
        }

        private string author;
        public string Author {
            get {
                return author;
            }
            set {
                author = value;
            }
        }

        private string snippetType;
        public string SnippetType {
            get {
                return snippetType;
            }
            set {
                if (new string[] { "Expansion", "SurroundsWith" }.Contains(value)) {
                    snippetType = value;
                }
            }
        }

        public HeaderInfo() {
            title = "";
            description = "";
            author = "";
            snippetType = "";
        }
    }

    class SnippetInfo {

    }
}
