﻿using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Netezos.Utils.Json;

namespace Netezos.Forging.Models
{
    [JsonConverter(typeof(FitnessContentConverter))]
    public class FitnessContent
    {
        public string Major { get; set; }
        public string Minor { get; set; }

        public List<string> ToList()
        {
            return new List<string>() { Major, Minor };
        }

        public FitnessContent BumpFitness()
        {
            var major = long.Parse(Major ?? "0", System.Globalization.NumberStyles.HexNumber);
            var minor = long.Parse(Minor ?? "0", System.Globalization.NumberStyles.HexNumber) + 1;
            return new FitnessContent()
            {
                Major = major.ToString("X2"),
                Minor = minor == 1 ? "0000000000000001" : minor == 2 ? "0000000000000002" : "0000000000000003"
            };
        }
    }
}