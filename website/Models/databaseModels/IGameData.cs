﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    public interface IGameData
    {
        string Name { get; set; }
        int ID { get; set; }
    }
}