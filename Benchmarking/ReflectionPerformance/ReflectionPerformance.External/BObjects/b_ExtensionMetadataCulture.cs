﻿namespace ReflectionPerformance.External.BObjects
{
    public class b_ExtensionMetadataCulture : IETL
    {
        public void LoadForETL()
        {
            System.Console.WriteLine($"Load ETL from {GetType()}");
        }
    }
}