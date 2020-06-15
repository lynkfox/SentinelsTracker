using Google;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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

        [TestMethod]
        public void GameModeIsDiscoverable()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var normalRow = values[2].Skip(13).Take(2);
            var advancedRow = values[4].Skip(13).Take(2);
            var challengeRow = values[3].Skip(13).Take(2);
            var ultimateRow = values[5].Skip(13).Take(2);

            var normal = (GameDetail.GameModes)Enum.Parse(typeof(GameDetail.GameModes), "Normal");
            var advanced = (GameDetail.GameModes)Enum.Parse(typeof(GameDetail.GameModes), "Advanced");
            var challenge = (GameDetail.GameModes)Enum.Parse(typeof(GameDetail.GameModes), "Challenge");
            var ultimate = (GameDetail.GameModes)Enum.Parse(typeof(GameDetail.GameModes), "Ultimate");

            Assert.AreEqual(normal, reader.ExtractGameMode(normalRow));
            Assert.AreEqual(advanced, reader.ExtractGameMode(advancedRow));
            Assert.AreEqual(challenge, reader.ExtractGameMode(challengeRow));
            Assert.AreEqual(ultimate, reader.ExtractGameMode(ultimateRow));
        }

        [TestMethod]
        public void EndGameConditionConversion()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var expectedVillain = values[2][16].ToString();
            var expectedHero = values[4][16].ToString();
            var expectedSucker = values[6][16].ToString();

            var villainIncap = (GameDetail.GameEndConditions)Enum.Parse(typeof(GameDetail.GameEndConditions), "IncapVillain");
            var heroLoss = (GameDetail.GameEndConditions)Enum.Parse(typeof(GameDetail.GameEndConditions), "IncapHeroes");
            var suckerPunch = (GameDetail.GameEndConditions)Enum.Parse(typeof(GameDetail.GameEndConditions), "Destroyed");

            Assert.AreEqual(villainIncap, reader.ExtractEndCondition(expectedVillain));
            Assert.AreEqual(heroLoss, reader.ExtractEndCondition(expectedHero));
            Assert.AreEqual(suckerPunch, reader.ExtractEndCondition(expectedSucker));
        }

        [TestMethod]
        public void SelectionMethodConversion()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            var playerChoice = values[3][28].ToString();
            var selectedRandom = values[4][28].ToString();

            var expectedPlayerChoice = (GameDetail.SelectionMethods)Enum.Parse(typeof(GameDetail.SelectionMethods), "PlayerChoice");
            var expectedRandom = (GameDetail.SelectionMethods)Enum.Parse(typeof(GameDetail.SelectionMethods), "Random");

            Assert.AreEqual(expectedPlayerChoice, reader.ExtractSelectionMethod(playerChoice));
            Assert.AreEqual(expectedRandom, reader.ExtractSelectionMethod(selectedRandom));
        }

        [TestMethod]
        public void PerceievedDiffCanDealWithBlanks()
        {
            GoogleRead reader = new GoogleRead();

            string path = @"C:\Users\lynkf\Desktop\SentinelsTracker\website\wwwroot\Data\app_client_secret.json";
            reader.Init(path);

            IList<IList<object>> values = reader.GetValues(SpreadsheetId, "TestEntry").Values;

            Assert.AreEqual(0, reader.PerceivedDiff(values[2][29]));
            Assert.AreEqual(5, reader.PerceivedDiff(values[4][29]));
        }

    }
}


