using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace website.Models.databaseModels
{
    [Table("GameDetails", Schema = "statistics")]
    public class GameDetail
    {
        public int ID { get; set; }

        //FKey for Games, 1 to 1 relation.
        public int GameId { get; set; }
        public Game Game { get; set; }

        //Players Perceieved Difficulty, 1-5 (how hard they felt the game was after the fact)
        [Range(0,5)]
        public int PerceivedDifficulty { get; set; }


        [Required]
        public GameModes GameMode { get; set; }//Normal, Advanced, Challenge, Ultimate
        [Required]
        public SelectionMethods SelectionMethod { get; set; }//Randomizer App, Thematic, Player Choice, Other
        [Required]
        public Platforms Platform { get; set; }//Physical, Steam, iOS, Android, TableTopSimulator, Other
        [Required]
        public GameEndConditions GameEndCondition { get; set; }//Enum - see below, lots of options
        [Required]
        public GameTimeLengths GameTimeLength { get; set; }//ZeroToFifteenMin, SixteenToThirtyMin, ThirtyOneToFortyFiveMin, FortySixToSixtyMin, OneToFiveHours, FivePlusHours, Other
        public HouseRules HouseRule { get; set; } //None, BGGGloomWeaver, TwoHeroes, Sidekicks, DuplicateHeroes, SixPlayers, Other

        //Different than number of Heroes used. Number of actual people playing.
        [Range(1,6)]
        public int NumberOfPlayers { get; set; }

        //Nullable because the user may not remember
        public int? Rounds { get; set; }
        [Required]
        public bool Win { get; set; }

        [StringLength(5000)]
        public string UserComment { get; set; }

        //Concated String of Other descriptions from the enum cattegories above, created by the controller
        [StringLength(1000)]
        public string OtherSelections { get; set; }

        //Recoreded at time of creation based on the NaiveBayes and stored here for reference. 
        public double CalculatedDifficulty { get; set; }


        //FKey collections - The one side of the 1 to many
        public ICollection<HeroTeam> HeroTeams { get; set; }
        public ICollection<VillainTeam> VillainTeams { get; set; }
        public ICollection<EnvironmentUsed> EnvironmentsUsed { get; set; }

        

        //Enums for above

        public enum Platforms
        {
            Physical, 
            Steam,
            iOS,
            Android,
            TableTopSimulator,
            Other
        }

        public enum SelectionMethods
        {
            PlayerChoice, //Normal - Default
            Random, //Any randomizer - In App, Website, helper app, rolling dice, picking blind...
            Difficulty, //By a specific difficulty rating
            Thematic, //By a team or other theme
            Rematch, //Replay of previous game
            Other //Concated into another field, gathered by the Controller
        }

        public enum GameModes
        {
            Normal, //Default, no additional official mechanics used.
            Advanced, //Using only the advanced rules on printed on the Villan Card
            Challenge, //Using the Challenge Mode Document only
            Ultimate //Using both the Advanced Rules and the Challenge Mode Doc
        }

        public enum GameEndConditions
        {
            IncapVillain, //Villan was defeated in the normal manner of things - default win
            IncapHeroes, //Heroes Loose by being soundly defeated - default loss
            // Specific villain win/loss conditions - alphabetized by villain common name
            TerraLunarImpulsionBeam, //Baron Blade
            WorldBurns, //Deadline
            DidNotProtectHer, //Dreamer
            RelicVictory, //Gloomweaver
            ConsumedFromWithin, //Gloomweaver, Skinwalker
            MinionOverrun, //Grand Warlord Voss
            CrowdTurns, //Kaargra Warfang
            OmnitronLivesOn, //Omnitron's Devices killed the players after Omnitron was defeated
            WagerWin, //Wager Master special win condition (multiple)
            WagerLoss, //Wager Master special loss condition (multiple)
            //OblivAeon
            OblivAeonBetrayed, //Voss took over and had to be defeated.
            // Specific Environment loss/win conditions - alphabetized by environment common name
            Sentanced, //Celestial Tribunal
            EnginesFailed, //Mobile Defense Platform
            PortalClosed, //Silver Gulch
            SelfDestruct, //Wagner Mars Base
            // Other random conditions that are kind of interesting to track
            Environment, //Environment card killed the villain
            Destroyed, //A "Destroy" card was used (such as Sucker Punch)
            IncapAbility, //A Hero Incap ability finished the game.
            Other //Prompt to describe in Another field, by the controller.
        }

        public enum GameTimeLengths
        {
            //Enum - 0-15mins, 16-30mins, 31-45mins, 45-1h, 1hr-5hrs, 5+hrs
            ZeroToFifteenMin,
            SixteenToThirtyMin,
            ThirtyOneToFortyFiveMin,
            FortySixToSixtyMin,
            OneToFiveHours,
            FivePlusHours,
            Other

        }

        public enum HouseRules
        {
            None,
            BGGGloomWeaver, //BGG Rules to increase Gloomweaver
            TwoHeroes, //Only using 2 hero decks
            Sidekicks, //2 Hero decks but with a 3rd or more incapped characters
            DuplicateHeroes, //More than one instance of a hero deck (ie: 2 legacy)
            SixPlayers, //More than Five players
            Other //In Concated string.
        }

    }
}
