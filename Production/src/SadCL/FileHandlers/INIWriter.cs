using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileHandlers
{
    public class PigLatinINIWriter
    {
        private char[] vowels = { 'a', 'e', 'i', 'o', 'u' };

        /*
         * PigLatin() converts a single word to Pig Latin
         */
        private string PigLatin(string word)
        {
            int pos = 0;
            double temp = 0;
            string finalWord = word.ToLower();
            // don't do anything if it is a small word or a number
            if (word.Length > 2 && !double.TryParse(word, out temp))
            {
                while (pos < word.Length-1 && !vowels.Contains(finalWord[pos]))
                {
                    // calculate length of consonant cluster at the beginning
                    pos++;
                }
                if (pos == 0)
                    // begins with vowel sound
                    finalWord = word + "way";
                else
                {
                    // move consonant cluster to the end
                    finalWord = word.Substring(pos, word.Length - pos);
                    finalWord = finalWord + word.Substring(0, pos) + "ay";
                }
            }
            return finalWord;
        }

        /*
         * write() - take a list of nodes (containing a header and every key/value pair)
         * and print them to a file.
         * */
        public void write(List<Node> nodes, string filename)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (var node in nodes)
                {
                    // print pig latin header in brackets
                    file.WriteLine('[' + PigLatin(node.Header) + ']');
                    foreach (var keyvalue in node.Contents)
                    {
                        // print key and value in pig latin
                        file.WriteLine(PigLatin(keyvalue.Key)+'='+PigLatin(keyvalue.Value));
                    }
                    file.WriteLine();
                }
            }
        }
    }
}
