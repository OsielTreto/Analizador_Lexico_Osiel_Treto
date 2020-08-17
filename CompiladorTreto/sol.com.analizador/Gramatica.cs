using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Ast;
using Irony.Parsing;


namespace CompiladorTreto.sol.com.analizador
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: true)
        {
            #region ER
            IdentifierTerminal id = new IdentifierTerminal("id");
            RegexBasedTerminal numeroentero = new RegexBasedTerminal("[0-9]+");
            RegexBasedTerminal numerodecimal = new RegexBasedTerminal("[0-9]+[.][0-9]");
            StringLiteral STRING = new StringLiteral("STRING", "\"", StringOptions.IsTemplate);

            CommentTerminal comentarioenlinea = new CommentTerminal("comentario linea", "//", "\n", "\r\n");
            CommentTerminal comentariobloque = new CommentTerminal("comentario bloque", "/*", "*/");
            base.NonGrammarTerminals.Add(comentarioenlinea);
            base.NonGrammarTerminals.Add(comentariobloque);
            #endregion

            #region palabras reservadas
            var RESERVADAabstract = "abstract";
            var RESERVADAcontinue = "continue";
            var RESERVADAfor = "for";
            var RESERVADAnew = "new";
            var RESERVADAswitch = "switch";
            var RESERVADAassert = "assert";
            var RESERVADAdefault = "default";
            var RESERVADAgoto = "goto";
            var RESERVADApackage = "package";
            var RESERVADAsynchronized = "synchronized";
            var RESERVADAboolean = "boolean";
            var RESERVADAdo = "do";
            var RESERVADAif = "if";
            var RESERVADAprivate = "private";
            var RESERVADAthis = "this";
            var RESERVADAbreak = "break";
            var RESERVADAdouble = "double";
            var RESERVADAimplements = "implements";
            var RESERVADAprotected = "protected";
            var RESERVADAthrow = "throw";
            var RESERVADAbyte = "byte";
            var RESERVADAelse = "else";
            var RESERVADAimport = "import";
            var RESERVADApublic = "public";
            var RESERVADAthrows = "throws";
            var RESERVADAcase = "case";
            var RESERVADAenum = "enum";
            var RESERVADAinstanceof = "instanceof";
            var RESERVADAreturn = "return";
            var RESERVADAtransient = "transient";
            var RESERVADAcatch = "catch";
            var RESERVADAextends = "extends";
            var RESERVADAint = "int";
            var RESERVADAshort = "short";
            var RESERVADAtry = "try";
            var RESERVADAchar = "char";
            var RESERVADAfinal = "final";
            var RESERVADAinterface = "interface";
            var RESERVADAstatic = "static";
            var RESERVADAvoid = "void";
            var RESERVADAclass = "class";
            var RESERVADAfinally = "finally ";
            var RESERVADAlong = "long";
            var RESERVADAstrictfp = "strictfp";
            var RESERVADAvolatile = "volatile";
            var RESERVADAconst = "const";
            var RESERVADAfloat = "float";
            var RESERVADAnative = "native";
            var RESERVADAsuper = "super";
            var RESERVADAwhile = "while";
            #endregion

            #region Terminales
            NonTerminal SENTENCIAS = new NonTerminal("SENTENCIAS");
            NonTerminal IMPRIMIR = new NonTerminal("IMPRIMIR");
            NonTerminal TIPO = new NonTerminal("TIPO");
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal MAIN = new NonTerminal("MAIN");
            NonTerminal DECLARAR = new NonTerminal("DECLARAR");
            NonTerminal IF = new NonTerminal("IF");
            NonTerminal WHILE = new NonTerminal("WHILE");



            NonTerminal R = new NonTerminal("R");
            NonTerminal E = new NonTerminal("E");

            #endregion

            #region No terminales
            #endregion

            #region Gramatica
            //Gramatica ambigua

            SENTENCIAS.Rule = SENTENCIAS + IMPRIMIR
                | SENTENCIAS + DECLARAR
                | SENTENCIAS + IF
                | SENTENCIAS + WHILE
                | IMPRIMIR
                | DECLARAR
                | IF
                | WHILE;



            INICIO.Rule = ToTerm("public")+ToTerm("class") + id + ToTerm("{") + MAIN + ToTerm("}")
            | ToTerm("public")+ToTerm("class") + id + ToTerm("{") + Empty + ToTerm("}");
            INICIO.ErrorRule = SyntaxError + ToTerm("}");

            MAIN.Rule = ToTerm("public static void main(String []args){") + SENTENCIAS + ToTerm("}");
            //MAIN.Rule = ToTerm("public")+ToTerm("static")+ToTerm("void")+ToTerm("main")+ToTerm("(")+ToTerm("String")+ToTerm("[")+ToTerm("]")+ToTerm("args")+ToTerm(")")+ToTerm("{") + SENTENCIAS + ToTerm("}")
            MAIN.ErrorRule = SyntaxError + ToTerm("}");

            IMPRIMIR.Rule = ToTerm("System")+ToTerm(".")+ToTerm("out")+ToTerm(".")+ToTerm("println") + ToTerm("(")+ STRING+ ToTerm(")")+ToTerm(";");
            IMPRIMIR.ErrorRule = SyntaxError + ToTerm(";");

            TIPO.Rule =
                ToTerm("byte")
                | ToTerm("short")
                | ToTerm("int")
                | ToTerm("long")
                | ToTerm("float")
                | ToTerm("double")
                | ToTerm("char")
                | ToTerm("string")
                | ToTerm("boolean");

            

            R.Rule =
                E +ToTerm("==")+ E
                | E + ToTerm("!=") + E
                | E + ToTerm("<") + E
                | E + ToTerm(">") + E
                | E + ToTerm("<=") + E
                | E + ToTerm(">=") + E
                | E + ToTerm("&") + E
                | E + ToTerm("&&") + E
                | E + ToTerm("||") + E;
            
            E.Rule =
                E + ToTerm("+") + E
                | E + ToTerm("-") + E
                | E + ToTerm("*") + E
                | E + ToTerm("/") + E
                | E + ToTerm("=") + E
                | (E)
                | id
                | numeroentero
                | numerodecimal;



            DECLARAR.Rule =
                 TIPO + id + ToTerm("=") + STRING + ToTerm(";")
                | TIPO + id + ToTerm("=") + numeroentero + ToTerm(";")
                | TIPO + id + ToTerm("=") + numerodecimal + ToTerm(";")
                | TIPO + id + ToTerm(";")
                | Empty;
            DECLARAR.ErrorRule = SyntaxError + ToTerm(";");

            IF.Rule =
                  ToTerm("if") + ToTerm("(") + R + ToTerm(")") + ToTerm("{") + SENTENCIAS + ToTerm("}")
                | ToTerm("if") + ToTerm("(") + R + ToTerm(")") + ToTerm("{") + SENTENCIAS + ToTerm("}")+ToTerm("else")+ToTerm("{")+SENTENCIAS+ToTerm("}");
            IF.ErrorRule = SyntaxError + ToTerm("}");

            WHILE.Rule =
                ToTerm("while")+ToTerm("(") + R + ToTerm(")")+ ToTerm("{")+SENTENCIAS+ToTerm("}");
            WHILE.ErrorRule = SyntaxError + ToTerm("}");




            #endregion

            #region Preferencias
            this.Root = INICIO;
            #endregion
            


            #region Palabras reservadas
            this.MarkReservedWords(RESERVADAabstract,
RESERVADAcontinue,
RESERVADAfor,
RESERVADAnew,
RESERVADAswitch,
RESERVADAassert,
RESERVADAdefault,
RESERVADAgoto,
RESERVADApackage,
RESERVADAsynchronized,
RESERVADAboolean,
RESERVADAdo,
RESERVADAif,
RESERVADAprivate,
RESERVADAthis,
RESERVADAbreak,
RESERVADAdouble,
RESERVADAimplements,
RESERVADAprotected,
RESERVADAthrow,
RESERVADAbyte,
RESERVADAelse,
RESERVADAimport,
RESERVADApublic,
RESERVADAthrows,
RESERVADAcase,
RESERVADAenum,
RESERVADAinstanceof,
RESERVADAreturn,
RESERVADAtransient,
RESERVADAcatch,
RESERVADAextends,
RESERVADAint,
RESERVADAshort,
RESERVADAtry,
RESERVADAchar,
RESERVADAfinal,
RESERVADAinterface,
RESERVADAstatic,
RESERVADAvoid,
RESERVADAclass,
RESERVADAfinally,
RESERVADAlong,
RESERVADAstrictfp,
RESERVADAvolatile,
RESERVADAconst,
RESERVADAfloat,
RESERVADAnative,
RESERVADAsuper,
RESERVADAwhile);
            #endregion

        }
    }
}
