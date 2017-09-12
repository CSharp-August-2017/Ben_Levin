using System;
using System.ComponentModel.DataAnnotations;

namespace wall
{
    public class MySqlOptions
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}