using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Wibci.CountryReverseGeocode.Models;

namespace Wibci.CountryReverseGeocode.DataConversion {
    using Models;
    using System.Text;

    class Program {
        const string Indent1 = "    ";
        const string Indent2 = "        ";
        const string Indent3 = "            ";
        const string Indent4 = "                ";
        const string Indent5 = "                    ";
        const string Pre = @"using System.Collections.Generic;
using Wibci.CountryReverseGeocode.Models;

namespace Wibci.CountryReverseGeocode.Data
{
    public class CountryDataProvider : IReverseGeocodeDataProvider
    {
        public static List<AreaData> DATA = new List<AreaData>() {";
            

        const string Post = @"
        };

        public List<AreaData> Data
        {
            get
            {
                return DATA;
            }
        }
    }
}";
        static void Main(string[] args) {
            args.ToList().ForEach(filePath => {
                string fileDirectory = Path.GetDirectoryName(filePath);
                string inputFileName = Path.GetFileName(filePath);
                Console.WriteLine($"# processing '{filePath}'...");
                var outputAreaDataList = new List<AreaData>();
                string fileContents = File.ReadAllText(filePath);
                // var jsonReader = new JsonTextReader(new StringReader(fileContents));
                JObject googleSearch = JObject.Parse(fileContents);

                // get JSON result objects into a list
                IList<JToken> areas = googleSearch["features"].Children().ToList();

                foreach (JToken area in areas) {
                    bool isMultiPolygon = area["geometry"].Value<string>("type") == "MultiPolygon";
                    AreaData areaData;
                    if (isMultiPolygon) {
                        InputMultiPolygonData inputAreaData = area.ToObject<InputMultiPolygonData>();
                        areaData = ConvertMultiPolygonData(inputAreaData);
                    } else {
                        InputPolygonData inputAreaData = area.ToObject<InputPolygonData>();
                        areaData = ConvertPolygonData(inputAreaData);
                    }
                    outputAreaDataList.Add(areaData);
                }

                Console.WriteLine("\tparsed successfully.");

                // write output list
                string outputFileName = inputFileName[0..^5] + "-out.json";
                var outputPath = Path.Combine(fileDirectory, outputFileName);
                File.WriteAllText(outputPath, JsonConvert.SerializeObject(outputAreaDataList));
                Console.WriteLine($"\tSimplified JSON written to '{outputPath}'.");

                // c# output
                outputFileName = inputFileName[0..^5] + "Data.cs";
                outputPath = Path.Combine(fileDirectory, outputFileName);
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(Pre);
                outputAreaDataList.ForEach(ad => AppendAreaData(ad, sb));
                sb.AppendLine(Post);
                File.WriteAllText(outputPath, sb.ToString());
                Console.WriteLine($"\tCS code written to '{outputPath}'.");
            });
        }

        private static void AppendAreaData(AreaData ad, StringBuilder sb)
        {
            sb.Append(Indent3);
            sb.Append("new AreaData(\"");
            sb.Append(ad.id);
            sb.Append("\", \"");
            sb.Append(ad.name);
            sb.AppendLine("\", new List<List<List<double>>>() {");
            ad.coordinates.ForEach(pd => {
                sb.Append(Indent4);
                sb.AppendLine("new List<List<double>>() {");
                pd.ForEach(cd => {
                    sb.Append(Indent5);
                    sb.Append("new List<double>() {");
                    sb.Append(cd[0]);
                    sb.Append(",");
                    sb.Append(cd[1]);
                    sb.AppendLine("},");
                });
                sb.Append(Indent4);
                sb.AppendLine("},");
            });
            sb.Append(Indent3);
            sb.AppendLine("}),");
        }

        private static AreaData ConvertMultiPolygonData(InputMultiPolygonData inputData) {
            var coordinates = inputData.geometry.coordinates.Select(l => l[0]).ToList();
            return new AreaData(inputData.id, inputData.properties.name, coordinates);
        }

        private static AreaData ConvertPolygonData(InputPolygonData inputData) {
            return new AreaData(inputData.id, inputData.properties.name, inputData.geometry.coordinates);
        }
    }
}
