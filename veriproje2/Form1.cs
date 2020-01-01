using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Office.Interop.Word;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Diagnostics;



namespace veriproje2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void loading()
        {
            if (progressBar1.Value < 90)
                progressBar1.Value=progressBar1.Value+5;
            else
                progressBar1.Value = 99;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox4.Items.Clear();
            progressBar1.Value = 0;
            

            StreamReader oku;
            if (radioButton1.Checked)
                oku = File.OpenText(@"D:\php test\htdocs\veri\search\text.txt");
            else
                oku = File.OpenText(@"D:\php test\htdocs\veri\search\text.html");

            string yazi;
            string fullyazi ="";

            while ((yazi = oku.ReadLine()) != null)
            {
                fullyazi += yazi+'\n' ;  // fullyazi = fullyazi+ yazi;
                loading();
            }
            
            oku.Close();

            fullyazi = fullyazi.ToLower();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (radioButton3.Checked) { 

                char[] aranan = new char[textBox1.Text.Length];
                
                aranan = (textBox1.Text.ToLower()).ToCharArray(0,textBox1.Text.Length);

                bool bulundu=false;

                char[] bakilan = new char[textBox1.Text.Length];
                
                int satir = 0;
                for (int i = 0; i < (fullyazi.Length- textBox1.Text.Length); i++)
                {
                    bakilan = fullyazi.ToCharArray(i, textBox1.Text.Length); 

                    string saranan = new string(aranan);
                    string sbakilan = new string(bakilan);
                    if(bakilan[0]=='\n')
                    {
                        satir += 1;
                    }
                    if (aranan[0] == bakilan[0])
                    {
                        bulundu = true;
                        for (int k = 1; k < textBox1.Text.Length; k++)
                        {
                            if (aranan[k] == bakilan[k])
                            {

                            }
                            else
                            {
                                bulundu = false;
                            }
                            
                        }
                        if (bulundu)
                        {
                            listBox1.Items.Add(textBox1.Text + " bulunduğu kaçıncı harf : "+i+" ve satir no: "+(satir+1));
                            loading();
                        }
                        
                    }
                    int benzerlikdurumu = LevenshteinDistance(saranan, sbakilan);
                    if (benzerlikdurumu != 0 && benzerlikdurumu < 3)
                    {
                        if (!listBox4.Items.Contains(sbakilan)) {
                            listBox4.Items.Add(sbakilan);
                            loading();
                        }
                        
                    }
                    
                }
            }
            else
            {
                int sayfa = 1;
                string aranan = textBox1.Text;
                Naive_search(fullyazi, aranan, sayfa);
            }
            watch.Stop();

            label9.Text = watch.Elapsed.Seconds.ToString() + " saniye " + watch.Elapsed.Milliseconds.ToString() + " ms";

            label2.Text = listBox1.Items.Count.ToString();
            progressBar1.Value = 100;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            listBox5.Items.Clear();
            progressBar1.Value = 0;

            

            //string testFile = @"D:\php test\htdocs\veri\search\text.docx";
            //
            //Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
            //Document document = application.Documents.Open(testFile);//path here
            //string fullyazi = "";
            //int count = document.Words.Count;
            //for (int i = 1; i <= count; i++)
            //{
            //    fullyazi += document.Words[i].Text;
            //    //fullyazi += document.Paragraphs[i].ToString();
            //
            //}
            //
            //application.Documents.Close();
            //
            //fullyazi = fullyazi.ToLower();

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = @"D:\php test\htdocs\veri\search\text.docx";
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path, ref miss, ref readOnly, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            string totaltext = "";

            object unit = Microsoft.Office.Interop.Word.WdUnits.wdLine;
            object count = docs.Words.Count;
            word.Selection.MoveEnd(ref unit, ref count);
            totaltext = word.Selection.Text;

            docs.Close(ref miss, ref miss, ref miss);
            word.Quit(ref miss, ref miss, ref miss);
            docs = null;
            word = null;


            string fullyazi = totaltext;



            fullyazi = fullyazi.ToLower();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (radioButton3.Checked)
            {


                char[] aranan = new char[textBox2.Text.Length];

                aranan = (textBox2.Text.ToLower()).ToCharArray(0, textBox2.Text.Length);

                bool bulundu = false;

                char[] bakilan = new char[textBox2.Text.Length];

                int satir = 0;
                for (int i = 0; i < (fullyazi.Length - textBox2.Text.Length); i++)
                {
                    bakilan = fullyazi.ToCharArray(i, textBox2.Text.Length);

                    string saranan = new string(aranan);
                    string sbakilan = new string(bakilan);
                    if (bakilan[0] == '\r')
                    {
                        satir += 1;
                    }
                    if (aranan[0] == bakilan[0])
                    {
                        bulundu = true;
                        for (int k = 1; k < textBox2.Text.Length; k++)
                        {
                            if (aranan[k] == bakilan[k])
                            {

                            }
                            else
                            {
                                bulundu = false;
                            }

                        }
                        if (bulundu)
                        {
                            listBox2.Items.Add(textBox2.Text + " bulunduğu kaçıncı harf : " + i + " ve satir no: " + (satir + 1));
                            loading();
                        }

                    }
                    int benzerlikdurumu = LevenshteinDistance(saranan, sbakilan);
                    if (benzerlikdurumu != 0 && benzerlikdurumu < 3)
                    {
                        if (!listBox5.Items.Contains(sbakilan))
                        {
                            listBox5.Items.Add(sbakilan);
                            loading();
                        }

                    }

                }
            }
            else {
                int sayfa = 2;
                string aranan = textBox2.Text;
                Naive_search(fullyazi, aranan, sayfa);

            }
            watch.Stop();

            label10.Text = watch.Elapsed.Seconds.ToString() +" saniye "+watch.Elapsed.Milliseconds.ToString()+" ms";

            label6.Text = listBox2.Items.Count.ToString();
            progressBar1.Value = 100;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
            listBox6.Items.Clear();
            progressBar1.Value = 0;

            

            PdfReader reader = new PdfReader(@"D:\php test\htdocs\veri\search\text.pdf");

            string fullyazi = "";

            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                fullyazi += PdfTextExtractor.GetTextFromPage(reader, page);
                
            }

            reader.Close();

            fullyazi = fullyazi.ToLower();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            if (radioButton3.Checked)
            {

                char[] aranan = new char[textBox3.Text.Length];

                aranan = (textBox3.Text.ToLower()).ToCharArray(0, textBox3.Text.Length);

                bool bulundu = false;

                char[] bakilan = new char[textBox3.Text.Length];
                
                int satir = 0;
                for (int i = 0; i < (fullyazi.Length - textBox3.Text.Length); i++)
                {
                    bakilan = fullyazi.ToCharArray(i, textBox3.Text.Length);

                    string saranan = new string(aranan);
                    string sbakilan = new string(bakilan);

                    if (bakilan[0] == '\n')
                    {
                        satir += 1;
                    }
                    if (aranan[0] == bakilan[0])
                    {
                        bulundu = true;
                        for (int k = 1; k < textBox3.Text.Length; k++)
                        {
                            if (aranan[k] == bakilan[k])
                            {

                            }
                            else
                            {
                                bulundu = false;
                            }

                        }
                        if (bulundu)
                        {
                            listBox3.Items.Add(textBox3.Text + " bulunduğu kaçıncı harf : " + i + " ve satir no: " + (satir + 1));
                            loading();
                        }

                    }
                    int benzerlikdurumu = LevenshteinDistance(saranan, sbakilan);
                    if (benzerlikdurumu != 0 && benzerlikdurumu < 3)
                    {
                        if (!listBox6.Items.Contains(sbakilan))
                        {
                            listBox6.Items.Add(sbakilan);
                            loading();
                        }

                    }

                }
            }
            else {
                int sayfa = 3;
                string aranan = textBox3.Text;
                Naive_search(fullyazi, aranan, sayfa);
            }
            watch.Stop();

            label4.Text = watch.Elapsed.Seconds.ToString() + " saniye " + watch.Elapsed.Milliseconds.ToString() + " ms";

            label8.Text = listBox3.Items.Count.ToString();
            progressBar1.Value = 100;

           

        }

        public void Naive_search(String txt, String pat,int sayfa)
        {
            int M = pat.Length;
            int N = txt.Length;

            string aranan = "";

            for (int i = 0; i <= N - M; i++)
            {
                int j;
                
                for (j = 0; j < M; j++) { 
                    if (txt[i + j] != pat[j]) { 
                        break;
                    }
                }

                if (j == M) { 
                    if(sayfa ==1 )
                        listBox1.Items.Add(pat + " bulunduğu kaçıncı harf : " + i);
                    else if (sayfa == 2)
                        listBox2.Items.Add(pat + " bulunduğu kaçıncı harf : " + i);
                    else if (sayfa == 3)
                        listBox3.Items.Add(pat + " bulunduğu kaçıncı harf : " + i);
                }

                //for (int x = 0; x < M; x++)
                //{
                //    aranan = aranan + txt[i];
                //}

                aranan = txt.Substring(i, M);


                int benzerlikdurumu = LevenshteinDistance(aranan, pat); 
                    if (benzerlikdurumu != 0 && benzerlikdurumu < 3)
                    {
                        if(sayfa ==1) { 
                            if (!listBox4.Items.Contains(aranan)) { 
                                listBox4.Items.Add(aranan);
                            }
                        }
                        else if (sayfa == 2)
                        {
                            if (!listBox5.Items.Contains(aranan))
                            {
                                listBox5.Items.Add(aranan);
                            }
                        }
                        else if (sayfa == 3)
                        {
                            if (!listBox6.Items.Contains(aranan))
                            {
                                listBox6.Items.Add(aranan);
                            }
                        }
                    }
            }
        }
        

        public static int LevenshteinDistance(string first, string second)
        {
            if (first.Length == 0) return second.Length;
            if (second.Length == 0) return first.Length;

            var lenFirst = first.Length;
            var lenSecond = second.Length;

            var d = new int[lenFirst + 1, lenSecond + 1];

            for (var i = 0; i <= lenFirst; i++)
                d[i, 0] = i;

            for (var i = 0; i <= lenSecond; i++)
                d[0, i] = i;

            for (var i = 1; i <= lenFirst; i++)
            {
                for (var j = 1; j <= lenSecond; j++)
                {
                    var match = (first[i - 1] == second[j - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + match);
                }
            }

            return d[lenFirst, lenSecond];
        }

        private void listBox4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox4.SelectedIndex != -1)
            {
                string yeniarama = listBox4.SelectedItem.ToString();
                textBox1.Text = yeniarama;
                button1.PerformClick();
            }
        }

        private void listBox5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox5.SelectedIndex != -1)
            {
                string yeniarama = listBox5.SelectedItem.ToString();
                textBox2.Text = yeniarama;
                button2.PerformClick();
            }
        }

        private void listBox6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox6.SelectedIndex != -1)
            {
                string yeniarama = listBox6.SelectedItem.ToString();
                textBox3.Text = yeniarama;
                button3.PerformClick();
            }
        }
    }
}

