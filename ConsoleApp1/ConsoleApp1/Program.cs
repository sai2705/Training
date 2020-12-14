using System;
using System.Linq;

namespace ConsoleApp1
{
    public class Program
    {
        public string RequiredString(string input,int initialValue, int innerValue, string value)
        {
            string[] initialArray = input.Split(new string[] { "@@" }, StringSplitOptions.None);
            string Value = string.Empty;
            
            if (initialArray.Length > 0)
            {
                for (int i = 0; i < initialArray.Length; i++)
                {
                    string readValue = initialArray[i].ToString();
                    string[] innerArray = readValue.Split(new string[] { "##" }, StringSplitOptions.None);
                if (innerArray[0]==value)
                {
                   


                        Value = innerArray[innerValue].ToString();
                        return Value;

                    }
                }
              
                
            }
            return Value;
        }
        public string Description(string input)
        {

            return RequiredString(input,0, 2,"455");

        }
         

       static public void Main(string[] args)
        {
            string input = "165##CR##Preauthorized ACH Credit@@174##CR##Other Deposit@@399##CR##Miscellaneous Credit@@455##DB##Preauthorized ACH Debit@@475##DB##Check Paid";
            Program program = new Program();
            program.Description(input);
            
        }

       
    }
}
