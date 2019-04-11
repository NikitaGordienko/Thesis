﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Thesis.Models
{
    public class FileModel
    {
        public string Id { get; set; }

        // название файла
        public string Name { get; set; }

        // полный путь к файлу
        public string Path { get; set; }
    }
}
