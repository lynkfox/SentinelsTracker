using Google;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using website.Controllers.BusinessLogic.GoogleReader;
using website.Models.databaseModels;

namespace TrackerUnitTests
{
    [TestClass]
    public class GameEntryReaderUnitTests
    {
        static readonly string SpreadsheetId = "185wr2Ws6D_8IAIsxIzxniN7tDwEm0IXsOZEm5lR9rMI";

        [TestMethod]
        public void HeroesAreReadInAsATeamFromEntryLists()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var row = values[2];

            var heroTeam = reader.RetrieveHeroTeam(row.Skip(17).Take(10));

            Assert.AreEqual("K.N.Y.F.E.", heroTeam.First().Hero.Name);
            Assert.IsTrue(heroTeam.First().Incapped);
            Assert.AreEqual(4, heroTeam.Count);
        }

        [TestMethod]
        public void VillainsReadInAsSolo()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var row = values[2];

            var villain = reader.RetrieveVillains(row.Skip(1).Take(15));

            Assert.AreEqual("Plague Rat", villain.First().Villain.Name);
            Assert.AreEqual(1, villain.Count);
            Assert.IsTrue(villain.First().Flipped);

        }
    }
}


