﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Models
{
    public class InformationMH
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public InformationMH(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
