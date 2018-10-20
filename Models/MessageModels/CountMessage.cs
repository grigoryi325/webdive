using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jQuery.Models.MessageModels
{
    public class CountMessage
    {
        public int IdCategory { get; set; }
        public string CategoryMessage { get; set; }
        public int CountMessageInCategory { get; set; }

        //использую конструктор для присвоение переданных в него значений свойствам 
        public CountMessage(int id, string category, int count)
        {
            IdCategory = id;
            CategoryMessage = category;
            CountMessageInCategory = count;
        }
    }
}