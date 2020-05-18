using System;
using System.Collections.Generic;
using System.Text;

namespace Producer.Model
{
    public class RandomModel
    {
        public int RandomNumber { get; set; }
        public string Key { get; set; }
        public string Message { get; set; }
        public DateTime DateTimeCreated => DateTime.Now;
    }
}
