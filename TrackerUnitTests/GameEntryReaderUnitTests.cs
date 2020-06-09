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

            var heroTeam = reader.CreateHeroTeams(reader.ExtractHeroTeam(row.Skip(17).Take(10)));

            Assert.AreEqual(45, heroTeam.First().HeroId);
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

            var villain = reader.CreateVillainTeams(reader.ExtractVillainTeam(row.Skip(1).Take(15)));

            Assert.AreEqual(46, villain.First().VillainId);
            Assert.AreEqual(1, villain.Count);
            Assert.IsTrue(villain.First().Flipped);
            Assert.IsFalse(villain.First().VillainTeamGame);

        }

       [TestMethod]
       public void TeamVillainReadIn()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var row = values[5];

            var villain = reader.CreateVillainTeams(reader.ExtractVillainTeam(row.Skip(1).Take(15)));

            Assert.AreEqual(26, villain.First().VillainId); // 26 is Fright Train
            Assert.AreEqual(4, villain.Count);
            Assert.IsTrue(villain.First().Flipped);
            Assert.AreEqual(8, villain.Last().VillainId); // 8 is Baron Blade: Vengeance Five
            Assert.IsTrue(villain.First().VillainTeamGame);
        }

        [TestMethod]
        public void EnvironmentsUsedIsSuccessfulWithOneEnvironment()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var row = values[3];

            var environmentsUsed = reader.CreateEnvironmentsUsed(row[27].ToString());

            Assert.AreEqual(24, environmentsUsed.First().GameEnvironmentId);
        }
    }
}


