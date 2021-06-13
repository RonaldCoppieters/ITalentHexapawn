using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Hexapawn.Models;

namespace Hexapawn.Manager {
    class DarwinManager {
        public DarwinManager() {
            var darwins = new List<Darwin>();

            for (int i = 0; i < 3; i++) {
                darwins.Add(new Darwin());
            }

            foreach (var darwin in darwins) {
                darwin.InstantiateWithBoxes();
            }

            Darwins = darwins;
        }

        public DarwinManager(List<Darwin> darwins) {
            Darwins = darwins;
        }

        private static string
            _fileName = "darwin.xml"; // Since we don't give a path, this will be saved in the 'bin' folder

        public List<Darwin> Darwins { get; }

        public static DarwinManager Load() {
            // If there isn't a file to load - create a new instance of 'DarwinManager'
            if (!File.Exists(_fileName))
                return new DarwinManager();

            // Otherwise we load the file

            using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open))) {
                var serializer = new XmlSerializer(typeof(List<Darwin>));

                var darwins = (List<Darwin>) serializer.Deserialize(reader);

                return new DarwinManager(darwins);
            }
        }

        public static void Save(DarwinManager darwinManager) {
            // Replaces the file if it exists
            using (var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create))) {
                var serializer = new XmlSerializer(typeof(List<Darwin>));

                serializer.Serialize(writer, darwinManager.Darwins);
            }
        }
    }
}