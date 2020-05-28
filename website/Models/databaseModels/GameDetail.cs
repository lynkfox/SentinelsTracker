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


        //FKey for Games
        public int GameId { get; set; }
        public Game Game { get; set; }

        //FKey for VillainTeams (1 to many)
        public int VillainTeamId { get; set; }
        public VillainTeam VillainTeam { get; set; }


        //Fkey for HeroTeams (1 to many)
        public int HeroTeamId { get; set; }
        public HeroTeam HeroTeam { get; set; }

        //Fkey for GameEnvironments (1 to many)
        [ForeignKey("Environment")]
        [Display(Name="Environment: ")]
        public GameEnvironment Environment { get; set; }

        //These two fields are flags for what positions were incapped, and when. May also be used for Oblivaeon?
        [StringLength(6)]
        public string HeroPositionsIncap { get; set; }
        [StringLength(5)]
        public string VillainPostionsIncap { get; set; }

        //Players Perceieved Difficulty, 1-5 (how hard they felt the game was after the fact)
        public int PerceivedDifficulty { get; set; }

        //Eenum - Normal, Advanced, Challenge, Ultimate
        public GameModes GameMode { get; set; }

        //Enum - Randomizer App, Thematic, Player Choice, Other
        public SelectionMethods SelectionMethod { get; set; }

        //Enum - Physical, Steam, iOS, Android, TableTopSimulator, Other
        public Platforms Platform { get; set; }
        
        //Enum - 0-15mins, 16-30mins, 31-45mins, 45-1h, 1hr-5hrs, 5+hrs
        public string GameTimeLength { get; set; }

        public int NumberOfPlayers { get; set; }

        //Were any House rules used - Describe in Comment
        public bool HouseRules { get; set; }

        //Nullable because the user may not remember
        public int? Rounds { get; set; }
        public bool Win { get; set; }

        //Enum - see below, lots of options
        public GameEndConditions GameEndCondition { get; set; }

        [StringLength(1000)]
        public string UserComment { get; set; }

        //Calculated field? Or Record the calculation at time of entry...
        public double CalculatedDifficulty { get; set; }

        


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
            Other
        }

        public enum GameModes
        {
            Normal,
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
            // Specific Environment loss/win conditions - alphabetized by environment common name
            Sentanced, //Celestial Tribunal
            EnginesFailed, //Mobile Defense Platform
            PortalClosed, //Silver Gulch
            SelfDestruct, //Wagner Mars Base
            // Other random conditions that are kind of interesting to track
            Environment, //Environment card killed the villain
            Destroyed, //A "Destroy" card was used (such as Sucker Punch)
            IncapAbility, //A Hero Incap ability finished the game.
            Other //Prompt to describe in Comments
        }
    }
}
