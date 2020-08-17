using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using System.Diagnostics;
using CompiladorTreto.sol.com.analizador;

namespace CompiladorTreto
{
    public partial class Form_Main : Form
    {
        Boolean bandera1 = false;
        RegexLexer csLexer = new RegexLexer();
        bool load;
        public List<string> palabrasReservadas;

        public Form_Main()
        {
            InitializeComponent();
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnMax_Click(object sender, EventArgs e)
        {
            this.Opacity = 100;
            btnMax.Visible = false;
            btnRest.Visible = true;
            this.WindowState = FormWindowState.Maximized;
            bandera1=true;
        }

        private void btnCer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea cerrar el compilador?", "Cerrar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();

            }
        }

        private void btnRest_Click(object sender, EventArgs e)
        {
            btnRest.Visible = false;
            btnMax.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Opacity = 10;
            bandera1=false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void BarraSuperior_Paint(object sender, PaintEventArgs e)
        {
           
        }
        int posY = 0;
        int posX = 0;
        private void BarraSuperior_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button != MouseButtons.Left)
            {
                posX = e.X;
                posY = e.Y;
            }
            else
            {
                Left = Left + (e.X - posX);
                Top = Top + (e.Y - posY);
            }
        }

        private void BarraSuperior_DoubleClick(object sender, EventArgs e)
        {
            if (bandera1 == false)
            {
                this.Opacity = 100;
                btnMax.Visible = false;
                btnRest.Visible = true;
                this.WindowState = FormWindowState.Maximized;
                bandera1 = true;
            }else{

                btnRest.Visible = false;
                btnMax.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Opacity = 10;
                bandera1 = false;
            }
        }

        private void bOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                string ruta = op.FileName; // ruta del archivo seleccionado
                string leer = File.ReadAllText(ruta);
                tbCodigo.Text = leer;
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (sd.ShowDialog() == DialogResult.OK)
            {
                string rutag = sd.FileName; // ruta del archivo seleccionado
                File.WriteAllText(rutag, tbCodigo.Text);
            }
        }

        private void Form_Principal_Load(object sender, EventArgs e)
        {

            timer1.Interval = 10;
            timer1.Start();
            using (StreamReader sr = new StreamReader(@"..\..\RegexLexer.cs"))
            {
                //tbxCode.Text = sr.ReadToEnd();

                csLexer.AddTokenRule(@"\s+", "ESPACIO", true);
                csLexer.AddTokenRule(@"\b[_a-zA-Z][\w]*\b", "IDENTIFICADOR");
                csLexer.AddTokenRule("\".*?\"", "CADENA");
                csLexer.AddTokenRule(@"'\\.'|'[^\\]'", "CARACTER");
                csLexer.AddTokenRule("//[^\r\n]*", "COMENTARIO1");
                csLexer.AddTokenRule("/[*].*?[*]/", "COMENTARIO2");
                csLexer.AddTokenRule(@"\d*\.?\d+", "NUMERO");
                csLexer.AddTokenRule(@"[\(\)\{\}\[\];,]", "DELIMITADOR");
                //csLexer.AddTokenRule(@"\|\||&&", "OPERADOR LOGICO");
                csLexer.AddTokenRule(@"[\.=\+\-/*%]", "OPERADOR");
                csLexer.AddTokenRule(@">|<|==|>=|<=|!", "COMPARADOR");



                palabrasReservadas = new List<string>() {"abstract",
        "continue",
"for",
"new",
"switch",
"assert",
"default",
"goto",
"package",
"synchronized",
"boolean",
"do",
"if",
"private",
"this",
"break",
"double",
"implements",
"protected",
"throw",
"byte",
"else",
"import",
"public",
"throws",
"case",
"enum",
"instanceof",
"return",
"transient",
"catch",
"extends",
"int",
"short",
"try",
"char",
"final",
"interface",
"static",
"void",
"class",
"finally ",
"long",
"strictfp",
"volatile",
"const",
"float",
"native",
"super",
"while"};

                csLexer.Compile(RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

                load = true;
                AnalizeCode();
                tbCodigo.Focus();
            }
        }

        private void AnalizeCode()
        {
            tabla.ClearSelection();  

            lvToken.Items.Clear();
            tabla.Rows.Clear();

            int n = 0, e = 0;

            foreach (var tk in csLexer.GetTokens(tbCodigo.Text))
            {
                if (tk.Name == "ERROR") e++;

                if (tk.Name == "IDENTIFICADOR")
                    if (palabrasReservadas.Contains(tk.Lexema))
                        tk.Name = "RESERVADO";
                tabla.Rows.Add(tk.Name, tk.Lexema, tk.Linea.ToString(), tk.Columna.ToString(), tk.Index.ToString());
                
                /*ListViewItem lista = new ListViewItem(tk.Name);
                lista.SubItems.Add(tk.Lexema);
                lista.SubItems.Add(tk.Linea.ToString());
                lista.SubItems.Add(tk.Columna.ToString());
                lista.SubItems.Add(tk.Index.ToString());
                lvToken.Items.Add( lista);
                */
                n++;
            }

            lMensaje.Text = string.Format("Analizador Lexico - {0} tokens {1} errores", n, e);
        }

        private void tbCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            AnalizeCode();
        }

        private void tbCodigo_Enter(object sender, EventArgs e)
        {
            AnalizeCode();

        }

        private void tbCodigo_Leave(object sender, EventArgs e)
        {
            AnalizeCode();

        }

        private void tbCodigo_TextChanged(object sender, EventArgs e)
        {
            AnalizeCode();
        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void tbCodigo_TextChanged_1(object sender, EventArgs e)
        {
            AnalizeCode();

        }

        private void tbCodigo_TextChanged_2(object sender, EventArgs e)
        {
            AnalizeCode();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        int CARACTER;
        

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Color Verdes = Color.FromArgb(141, 240, 1);
            SolidBrush myBrushs = new SolidBrush(Verdes); 
            CARACTER = 0;
            int ALTURA = tbCodigo.GetPositionFromCharIndex(0).Y-1;

            if (tbCodigo.Lines.Length > 0)
            {
                for (int i=0; i<tbCodigo.Lines.Length;i++)
                {
                    e.Graphics.DrawString(Convert.ToString(i + 1), tbCodigo.Font, myBrushs, pictureBox1.Width - (e.Graphics.MeasureString(Convert.ToString(i + 1), tbCodigo.Font).Width + 10), ALTURA);
                    CARACTER =CARACTER + tbCodigo.Lines[i].Length + 1;
                    ALTURA = tbCodigo.GetPositionFromCharIndex(CARACTER).Y;
                }
            }
            else
            {
                e.Graphics.DrawString("1", tbCodigo.Font, myBrushs, pictureBox1.Width - (e.Graphics.MeasureString("1", tbCodigo.Font).Width + 10), ALTURA);

            }
        
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            AnalizeCode();

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvToken_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            AnalizeCode();

            bool resultado = Sintactico.analizar(tbCodigo.Text);
            if (resultado)
            {
                /*label6.ForeColor = Color.FromArgb(141, 240, 1);
                label6.Text = "Sintaxis correcta";
                label6.Visible = true; 
    */
                tbCodigo.ForeColor = Color.Black;
    }
            else
            {
                /*
                                label6.ForeColor = Color.Red;
                                label6.Text = "Error de sintaxis";
                                label6.Visible = true;
                 */
                tbCodigo.ForeColor = Color.Red;
    }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbCodigo.Clear();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            bool resultado = Sintactico.analizar(tbCodigo.Text);
            if (resultado)
            {
                label6.ForeColor = Color.FromArgb(141, 240, 1); 
                label6.Text = "Sintaxis correcta";
                label6.Visible = true; ;
            }
            else
            {
                label6.ForeColor = Color.Red;
                label6.Text = "Error de sintaxis";
                label6.Visible = true;
            }
        }
    }
}
