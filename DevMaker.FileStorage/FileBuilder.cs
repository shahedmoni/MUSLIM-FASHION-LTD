﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace DevMaker.FileStorage
{

    public static class FileBuilder
    {
        public static string FileName(string title, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameForStorage = $"{title}-{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";

            return fileNameForStorage;
        }


        public static string BasePath()
        {
            var path=   Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "product");
            return path;
        }
    }
}