using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace verificadorTAGS
{
    class Program
    {
        static void Main(string[] args)
        {
            int contador = 0;
            string linea;
            TextReader leer = new StreamReader("tag.txt");
            while ((linea = leer.ReadLine()) != "#") contador++;
            leer.Close();
            string[] tag = new string[contador];
            string tags;
            string patronEtiquetas = "[C]||[B]||[CENTER]||[T]||[X]||[/S]";
            
            int i = 0,j=0,k=0, tamA=0, tamC=0;
            TextReader leerArch = new StreamReader("tag.txt");
            while ((linea = leerArch.ReadLine()) != "#")
            {
                string[] tagApertura = new string[6];
                string[] tagCierre = new string[6];
                
                char caracter=' ';
                tag = linea.Split('#');
                tags = linea;
                do
                {
                    caracter = tags[i];
                    if (caracter == '<')
                    {
                        string caracTag = "";
                        do
                        {
                            caracTag += caracter;
                            i++;
                            caracter = tags[i];
                            if (caracter == '>')
                            {
                                caracTag += caracter;
                                Regex miRegex = new Regex("/");
                                MatchCollection elMatch = miRegex.Matches(caracTag);
                                if (elMatch.Count > 0)
                                {
                                    tagCierre[k] = caracTag;
                                    caracTag = "";
                                    k++;
                                }
                                else
                                {
                                    caracTag.ToCharArray();
                                    if (elMatch.Count == 0)                                    
                                    {     
                                        Regex miRegexAp = new Regex(patronEtiquetas);
                                        MatchCollection elMatch2 = miRegexAp.Matches(caracTag);
                                        if (elMatch2.Count > 3 && elMatch2.Count < 5)
                                        {
                                            tagApertura[j] = caracTag;
                                            caracTag = "";
                                            j++;
                                        }      
                                    }
                                }
                            }
                        } while (caracter != '>');
                    }
                    i++;                    
                } while (caracter != '#');
                tamA = j; tamC = k;
                int x=0;
                bool band = false;
                do
                {
                    Regex miRegex3 = new Regex($"[{tagApertura[tamA - 1]}]");
                    MatchCollection elMatch3 = miRegex3.Matches(tagCierre[x]);
                    if (elMatch3.Count > 2) 
                    {
                        if (x == j - 1 && x == k - 1) Console.WriteLine("Correctly tagged paragraph");
                        else
                        {
                            if (j < k && x == (j - 1)) Console.WriteLine($"Expected # found {tagCierre[tamC-1]}");
                            else
                            {
                                if (j > k && x == (k - 1))
                                {
                                    string expected = "";
                                    char [] expect = tagApertura[0].ToCharArray();
                                    for (int y=0;y<expect.Length;y++)
                                    {
                                        expected += expect[y];
                                        if (y == 0) expected += "/";
                                    }
                                    Console.WriteLine($"Expected {expected} found #");
                                }
                            }
                        }
                    }
                    else
                    {
                        if(j == k && x == (j - 1)) Console.WriteLine($"Expected {tagCierre[tamC]} found {tagCierre[0]}");                        
                    }
                    tamA--; tamC--; x++;
                    if (tamA== 0 || tamC==0) band = true;
                } while (!band);
                i = 0; j = 0; k = 0; x = 0;
            }
            leerArch.Close();
        }
    }
}
