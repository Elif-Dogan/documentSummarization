using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;

namespace DocumentSum
{
    public partial class Document : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {          
        }        
        protected void Button1_Click(object sender, EventArgs e)
        {
           // ListBox2.Visible = true;
            
            //Delimiter for sentences
            char[] delimiterChars = { '.', '?', '\t' };  
            
            //Delimiter for words
            char[] delimiterChars2 = { ' ' };

            //Delimiter for cluster
            char[] delimiterChars3 = { '*' };           

            string[] stopWord = new string[] { " a "," about "," an "," are "," and "," as "," at "," be "," by "," com "
                                              ," for "," from "," how "," in "," is "," it "," of "," on "," or "," that "
                                              ," the "," this "," to ", " was "," what "," when ", " where "," who "," will "
                                              ," with "," the "," www ", " am ", " could "," this "," more ", " such ", " which "
                                              , " that ", " these "};

            string[] suffixes = new string[] { "able ", "ac ","acity ", "ade ","aholic ","al ", "algia ","an ", "ant ", "ar "
                                              , "ard ", "arian ", "arium ", "ary ","ate ", "ation ","ative ", "cide ", "cracy "
                                              , "crat ", "cule ","cy ","cycle ","dom ","dox ","ectomy ","ed ","ee ", "eer "
                                              , "emia ", "en ", "ence ", "ency ", "ent ", "er ", "ern ", "escence ", "ese "
                                              , "esque ", "ess ", "est ", "etic ", "ette ", "ful ", "fy ", "gam ", "gamy "
                                              , "gon ","gonic ", "hood ","ial ", "ian ", "iasis ", "iatric ", "ible "
                                              , "ic ", "ical ", "ile ", "ily ", "ine ", "ing ", "ion ", "ious ", "ish "
                                              , "ism ", "ist ", "iet ", "itis ", "ity ", "ive ", "ization ", "ize ", "less "
                                              , "let ", "like ", "ling ", "loger ", "logist ", "log ", "ly ", "ment ", "ness "
                                              , "oid ", "ology ", "oma ", "onym ", "opia ", "opsy ", "or ", "ory ", "osis "
                                              , "ostomy ","otomy ", "ous ", "path ", "pathy ", "phile ", "phobia ", "phone ","phyte "
                                              , "plegia ", "plegic ", "pnea ", "scopy ", "scope ", "scribe ", "script ", "sect "
                                              , "ship ","sion ", "some ", "sophy ", "sophic ", "th ", "tion ", "tome ", "tomy "
                                              , "trophy ", "tude ", "ty ", "ular ","uous ", "ure ", "ward ", "ware ", "wise "};
         
            StringBuilder input = new StringBuilder(TextBox1.Text);

            string[] sentencesOriginal = input.ToString().Split(delimiterChars);
            int counter = 0;
            string[] sents=new string[400];

            foreach (String match2 in sentencesOriginal)
            {
                counter++;
                sents[counter] = match2;
            }

            ////Data preprocessing            
            foreach (string word in stopWord)
            {
                input.Replace(word, " "); //Remove stopwords
            }
            foreach (string suffix in suffixes)
            {
                input.Replace(suffix, " "); //Removing suffixes
            }
            
            string[] sentences = input.ToString().Split(delimiterChars);
            string[] words = input.ToString().Split(delimiterChars2);
            string text = input.ToString();
            string[] cluster1= new string[400];
            string[] cluster= new string[50];
            int sentenceCount = 0;
            int kValue = 0;
            double[] meanSent=new double[400];           
        
            StringBuilder strBuild = new StringBuilder();
            
            string newString3 = preprocess(text);
            double[] meanClust = new double[50];
            ListBox5.Items.Add("Sentences Means:");

            foreach (String match in sentences)
            {
                sentenceCount++;
                string newString2 = preprocess(match);
                cluster1[sentenceCount] = match;                
                meanSent[sentenceCount] = findMean(newString2);              
                ListBox5.Items.Add(sentenceCount+": "+meanSent[sentenceCount]);
            }
           
            //Determining k value
            int sentcount = sentenceCount / 2;
            sentcount=Convert.ToInt32(Math.Sqrt(sentcount));
            kValue = sentenceCount/sentcount;

         //   Response.Write(kValue);
            
            for (int i = 1; i < sentenceCount; i++)
            {
                if (i % kValue == 0)
                {
                    strBuild.Append(cluster1[i] + "*");
                }
                else
                {
                    strBuild.Append(cluster1[i]);
                }
            }

            string[] clusters = strBuild.ToString().Split(delimiterChars3);
            int clustCount = 0;

            foreach (String clust in clusters)
            {
                clustCount++;
                string newString2 = preprocess(clust);
                meanClust[clustCount] = findMean(newString2);                            
               // ListBox2.Items.Add(clustCount+": "+clust);
            }
            
            ListBox4.Items.Add("Cluster Means:");            

            for (int i = 1; i <= clustCount; i++)
            {
                ListBox4.Items.Add(i+": "+meanClust[i]);
            }

            double mean= findMean(newString3);     
            string[] summary=new string[kValue];
            ListBox6.Visible = true;

            for(int i=0; i<clustCount; i++)
            {
                double min = 999;

                //ListBox6.Items.Add("cluster:"+i+"cluster mean is:"+ meanClust[i]);
                
                for (int j = i*kValue; j < (i*kValue)+kValue; j++)
                {
                    if (Math.Abs(calcDistance(meanClust[i], meanSent[j]))!=0 && min > Math.Abs(calcDistance(meanClust[i], meanSent[j])))
                    {
                        min = Math.Abs(calcDistance(meanClust[i], meanSent[j]));
                        summary[i] = sents[j];                       
                    }
                }
               // ListBox6.Items.Add("Min is: " + findMin(min)+" summary sentence: "+summary[i]);
                ListBox6.Items.Add(summary[i]+". ");
            }
        }
      /*  public double findMin(double min)
        {
            double minimum=999;
            if (minimum > min)
            {
                return min;
            }
            else
                return findMin(minimum);
        }*/

        public string preprocess(string pretext)
        {
            string newString = Regex.Replace(pretext, "[.0-9]", ""); //Remove numbers
            newString = newString.Replace(",", " ");       //
            newString = newString.Replace(":", " ");      //
            newString = newString.Replace("?", " ");    ////Remove punctuation characters
            newString = newString.Replace("\"", " ");     //
            newString = newString.Replace("\'", " ");   //
            newString = newString.Replace(".", " ");    //
            newString = newString.ToLower(); // Convert tolower
            newString = newString.Replace("ı", "i");

            return newString;
        }
        public double findMean(string value)
        {
            double mean=0.0;
            List<string> wordList2 = value.Split(' ').ToList();

            // Create a new Dictionary object
            Dictionary<string, int> dictionary2 = new Dictionary<string, int>();

            foreach (string word in wordList2)
            {
                // If the length of the word is at least three letters...
                if (word.Length >= 3)
                {
                    // ...check if the dictionary already has the word.
                    if (dictionary2.ContainsKey(word))
                    {
                        // If we already have the word in the dictionary, increment the count of how many times it appears
                        dictionary2[word]++;
                    }
                    else
                    {
                        // Otherwise, if it's a new word then add it to the dictionary with an initial count of 1
                        dictionary2[word] = 1;
                    }

                } // End of word length check

            }
            var sortedDict = (from entry in dictionary2 orderby entry.Value descending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            // Loop through the sorted dictionary and output the top 10 most frequently occurring words
            double count = 1;
            //ListBox3.Items.Add("---- Term Frequency  ----");
            double freq=0;
            foreach (KeyValuePair<string, int> pair in sortedDict)
            {
                // Output the most frequently occurring words and the associated word counts
               // ListBox3.Items.Add(count + "\t" + pair.Key + "\t" + pair.Value);
                freq+=pair.Value;
                count++;

                // Only display the top 10 words then break out of the loop!
                if (count > value.Length)
                {
                    break;
                }
                
            }
            mean = Convert.ToDouble(freq / count);

            return mean;

        }
        public double calcDistance(double clustMean, double sentenceMean)
        {
            //Euclidean distance
//            double dist=Math.Sqrt((clustMean*clustMean)+(sentenceMean*sentenceMean));
            double dist = clustMean - sentenceMean;
            return dist;
        }
    }
} 