﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todoapp.Repositories
{
    public abstract class BaseRepository
    {
        protected string? connectionString;
    }
}
