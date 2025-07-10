using System.IO;
using System.Reflection;
using MenuFilesGen.Models;
using MenuFilesGen.Repositories;
using NUnit.Framework;

namespace MenuGenUnitTests
{
    public class ReadCsvTests
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _csvFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "drzTools_BlockFix.txt");
            _columnNumbers = new ColumnNumbers();
        }

        [Test]
        public void GetAllData()
        {
            CommandRepository rep = new CommandRepository(_columnNumbers);
            rep.ReadFromCsv(_csvFile);
            // Добавить проверку количества команд, панелей, элементов ленты
            Assert.That(rep.PanelDefinitions.Count, Is.EqualTo(15));
            Assert.That(rep.RibbonPaletteDefinitions.Count, Is.EqualTo(3));
            Assert.That(rep.CommandDefinitions.Count, Is.EqualTo(144));
        }

        private string _csvFile;
        private ColumnNumbers _columnNumbers;
    }
}
