using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnippetManager
{
    class HeaderInfo
    {
        private string title = "";
        internal string Title { get => title; set => title = value; }

        private string description = "";
        internal string Description { get => description; set => description = value; }

        private string author = "";
        internal string Author { get => author; set => author = value; }

        private string snippetType = "";
        internal string SnippetType {
            get => snippetType;
            set {
                if (new string[] { "Expansion", "SurroundsWith" }.Contains(value)) {
                    snippetType = value;
                }
            }
        }


        internal HeaderInfo SetHeaderData(string title, string author, string description, string type) {
            Title = title;
            Author = author;
            Description = description;
            SnippetType = type;

            return this;
        }
    }

    class SnippetInfo {
        private string code = "";
        internal string Code { get => code; set => code = value; }

        private List<Literal> literals = new List<Literal>();
        internal List<Literal> Literals { get => literals; set => literals = value; }


        internal SnippetInfo SetCode(string codeFromRichTextBox) {
            code = codeFromRichTextBox;
            return this;
        }

        internal SnippetInfo SetDeclarations(IEnumerable<Literal> literalData) {
            literals = literalData.ToList();
            return this;
        }
    }

    class Literal {
        public string Id { get; set; }

        public string ToolTip { get; set; }

        public string DefaultText { get; set; }
    }
}
