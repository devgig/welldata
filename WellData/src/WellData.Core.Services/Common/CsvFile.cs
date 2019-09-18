using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WellData.Core.Services.Data
{
    public interface ICsvFile
    {
        string[] Columns { get; }
        IEnumerable<IDictionary<string, string>> LazyRead();
    }

    [ExcludeFromScan]
    public class CsvFile : ICsvFile
    {
        private readonly string _filePath;
        private readonly Stream _stream;
        private  string[] _columns;

        public CsvFile(string filePath)
        {
            _filePath = filePath;
            _columns = ReadColumnStructure(filePath);
        }

        public CsvFile(Stream stream)
        {
            _stream = stream;
            _columns = ReadColumnStructure(_stream);
        }

        public string[] Columns { get { return _columns; } }

        public IEnumerable<IDictionary<string, string>> LazyRead()
        {
            _columns = new string[] { };

            using (var parser = _filePath != null ? CreateTextFieldParser(_filePath) : CreateTextFieldParser(_stream))
            {
                bool first = true;
                while (!parser.EndOfData)
                {
                    if (first)
                    {
                        var readFields = parser.ReadFields() ?? new string[] { };
                        _columns = readFields.ToArray();
                        first = false;
                    }
                    else
                    {
                        var values = parser.ReadFields() ?? new string[] { };
                        var dictionary = new Dictionary<string, string>();
                        var count = Math.Min(_columns.Length, values.Length);

                        for (int i = 0; i < count; i++)
                            dictionary[_columns[i]] = values[i];

                        yield return dictionary;
                    }
                }
            }
        }

        private static string[] ReadColumnStructure(string filePath)
        {
            using (var parser = CreateTextFieldParser(filePath))
            {
                if (!parser.EndOfData)
                    return (parser.ReadFields() ?? new string[] { }).Select(x => x).ToArray();
            }

            return new string[] { };
        }

        private static string[] ReadColumnStructure(Stream stream)
        {
            using (var parser = CreateTextFieldParser(stream))
            {
                if (!parser.EndOfData)
                    return (parser.ReadFields() ?? new string[] { }).Select(x => x).ToArray();
            }

            return new string[] { };
        }

        private static TextFieldParser CreateTextFieldParser(string filePath)
        {
            var parser = new TextFieldParser(filePath);

            parser.SetDelimiters(",");

            return parser;
        }

        private static TextFieldParser CreateTextFieldParser(Stream stream)
        {
            var parser = new TextFieldParser(stream, System.Text.Encoding.UTF8);

            parser.SetDelimiters(",");

            return parser;
        }
    }
}