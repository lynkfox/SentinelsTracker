# Sentinels Tracker : The Sentinels Statistic Project Website and Tracker

## Aim of this Project

This project aims to create a proper sql database, website, and eventual mobile apps for use in adding information to the Sentinels Statistics Project. (Discussed in length here: https://greaterthangames.com/forum/topic/sentinels-statistics-project-2893 ) 

Currently the Statistics Project uses a google forms / sheets for storing the data. This is very problematic, for a number of reasons, not only the least of which is that the project has over 25k entries now. Doing anything with this data is incredibly difficult.

This project will have the following phases:

Follow along on the progress of this project at: https://trello.com/b/O7C7mVJx/sentinels-tracker

### Phase 1: The Database :

Construction of a CodeFirst, Entity Framework SQL database to store the information from the Statistics Project google form in a much more readily available, searchable, and processible format.

The conclusion of Phase 1 will come with a translator that will be able to move the data from the Google Sheets to the database, plus all the default data needed for dependencies in said database (ie: hero information in the Hero table to be used as a relation to the HeroTeams ect ect)

*Specifically:

+ CodeFirst proceedures
+ Microsoft SQL server setup
+ Google Forms translated to the database
+ Basic CRUD operations on the proto website


### Phase 2: The Website (Part 1) :

An ASP.Net, Razor and Bootstrap webpage with Docker Support that will provide a landing site, an entry form (for entering game information) and a statistics page to view various statistics. In addition, there will be a user system allowing users to track their own games and entries into the Statistics Project Database. 

At the completion of Phase 2 this project will be deployed to live servers. (Possibly Azure?)

*Specifically:
+ Entry form for entering new games
+ Entry validation
+ Statistics View
	+ Overall
	+ Indivdiual Characters/Entities
	+ By User
	+ By special categories (loss conditions, ect)
	+ Searchable
+ User register
+ User Email verification
+ User Login
+ User Profile/Image
+ User retrieve games from old forms
+ User validation
+ User see own games
+ User Profile Edit
+ Super User ability to modify existing entries for editing purposes.

### Phase 3 : Android App :

An android app that wil allow adding games to the database from the convience of your phone.

*Specifically
+ User Login
+ See Own Games
+ Enter new game
+ Edit Profile

### Phase 4 : The Website (Part 2) : 

Continue to upgrade the website with new services, such as randomizer, wiki integration, and difficulty calculator

+ Basic integration of the statistics with Wiki information ( https://sentinelswiki.com/ )
+ Difficulty Calculator
+ Random Game Generation, 
+ and other typical web services already created by many fans of the game. 

	These services will either be coded from scratch by this project team, or individual contributors who have already created apps of these nature are free to integrate them into this website

### Phase 5 : Additional Goals : 

It is unknown when these goals will be attained, but they are part of the eventual completed vision of this project. They are as follows, but not limited too:

+ An iOS app

	At this time the team (read individual) behind this project does not have any experience coding an iOS app, but that isn't enough to stop them (me!). Just that this is further down the road in terms of goals.
	
+ More advanced Wiki and Tracker integration

	With the ability for the Wiki to pull statistics information from the Tracker and the Tracker to pull from the wiki.
	
+ API - A way for others to tap into the database on request (without having direct access) to make use of the stastics data as they see fit

+ Integration of The Cauldron into the statistics database
	
	Once in a proper database, adding the Cauldron is much more simple than it was adding to the Google Forms.
	
+ House Rule Games
	
	Can be more easily excluded from the proper statistics (or seen with a click of a button)


## If you want to help: (please do!)

Feel free to fork this project and add whatever you think you can contribute before requesting a merge. Also, email Lynkfox or contact through reddit ( https://www.reddit.com/message/compose/?to=lynkfox ) to request access to the Trello board for tracking purposes. 

Specifically, anyone can contribute if you think you have something to add, but the following skills would be nice:

Looking for people with the following skills But if you don't have them, feel free to contribute anyways :)

- C# .net Core 
- ASP.Net 
- SQL database queries
- Java (for the android app)
- Bootstrap - Other CSS/HTML5 Design options
	**ESPECIALLY THIS: It will be functional but it won't necessarily be the prettiest page...**
	
- Javascript/Ajax queries

*Looking for in the future when we get that far:

- Swift (for the iOS app)
- REST API's

## This project uses:

+ Microsoft C# Core 3.1 (preview)
+ Microsoft C# Asp.net core 3.14
+ Microsoft SQL servers
+ Docker
+ Bootstrap (version ###)
+ Javascript (version ###)
+ Razor

+ Eventually...
	+ To be hosted on an Azure server (probably)
	+ Swift (version)
	+ (Java?C# for Mobile android?)
	+ Whatever comes about ...
	
	