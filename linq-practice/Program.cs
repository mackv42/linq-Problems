using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_practice
{
    class Program
    {

        public static IEnumerable<string> wordsWithTh(List<string> search)
        {
            IEnumerable<string> ret = search.Where((s) => {
                for (int i = 0; i < s.Count() - 1; i++)
                {
                    if (String.CompareOrdinal(Char.ToString(s[i]) + Char.ToString(s[i + 1]), "th") == 0)
                    {
                        return true;
                    }
                }

                return false;
            });

            return ret;
        }

        public class saveState
        {
            public int skip;
            public saveState()
            {
                skip = -1;
            }
        };
        public static IEnumerable<string> noCopy(List<string> search)
        {
            IEnumerable<string> ret = new List<string>();
            ret = search;

            for (int i = 0; i < search.Count(); i++)
            {
                string tmp = search[i];

                for(int j = i+1; j<ret.Count(); j++)
                {
                    if(search[i] == search[j])
                    {
                        search[j] = "";
                    }
                }
            }

            return ret.Where((s) => s!="");

            //or just
            //return ret.Distinct();
        }

        public static double getAverage(List<string> grades)
        {
            Func<string, IEnumerable<double>> convertToNumbers = (string g) =>
            {
                List<double> ret = new List<double>();

                //Use tryParse here
                for(int i=0; i<g.Count(); i++)
                {
                    try
                    {
                        ret.Add(double.Parse(Char.ToString(g[i]) + Char.ToString(g[i + 1]) + Char.ToString(g[i + 2])));
                        i+=2;
                    } catch(Exception E)
                    {
                        try
                        {
                            ret.Add(double.Parse(Char.ToString(g[i]) + Char.ToString(g[i + 1])));
                            i++;
                        } catch(Exception F)
                        {
                            continue;
                        }
                    }
                    
                }
                return ret;
            };

            Func<IEnumerable<double>, double> averaged = (IEnumerable<double> g) =>
            {
                double total = 0;
                foreach (var n in g)
                {
                    total += n;
                }

                return total / g.Count();
            };

            
            List<double> averages = new List<double>();
           
            foreach (var Class in grades){
                var classNumbers = convertToNumbers(Class);
                IEnumerable<double> woMin = from c in classNumbers where c > classNumbers.Min() select c;
                
                averages.Add(averaged(
                    woMin
                )); 
            }

            return averaged(averages);
        }

        public static string compressAndOrder( string str ) 
        {
            IEnumerable<char> ret = str;
            IEnumerable<char> ordered = str.OrderBy((x) => { return -x; });

            ret = ordered.Distinct();

            List<int> LetterFrequency = new List<int>();
            foreach(var c in ret)
            {
                LetterFrequency.Add(ordered.Where(x => x == c).Count());
            }

            string final = "";
            int i = 0;

            foreach (var c in ret)
            {
                final += LetterFrequency[i++].ToString() + Char.ToString(c);
            }

            return final;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("----Words With Th------------");
            List<string> words = new List<string>() { "the", "bike", "this", "it", "tenth", "mathematics" };
            IEnumerable<string> strings = wordsWithTh(words);

            foreach(var s in strings)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("------------no copy--------------");
            List<string> names = new List<string>() { "Mike", "Brad", "Nevin", "Ian", "Mike" };
            IEnumerable<string> removeCopies = noCopy(names);

            foreach (var s in removeCopies)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine("---------Classes grade average----------");
            List<string> classGrades = new List<string>()
            {
            "80,100,92,89,65",
            "93,81,78,84,69",
            "73,88,83,99,64",
            "98,100,66,74,55"
            };
            Console.WriteLine(getAverage(classGrades));

            Console.WriteLine("--------Compress and Order---------");
            Console.WriteLine(compressAndOrder("wwwwombaatt"));
        }
    }
}
