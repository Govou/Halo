using Microsoft.EntityFrameworkCore.Migrations;

namespace HaloBiz.Migrations
{
    public partial class addsStateAndLGAs : Migration 
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("SET IDENTITY_INSERT States ON"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES(1, 'Abia', 'Umuahia', 'Gods own State', 440001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (2, 'Adamawa', 'Yola', 'Highest peak of the nation', 640001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (3, 'Akwa Ibom', 'Uyo', 'Land of promise', 520001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (4, 'Anambra', 'Awka', 'Light of the nation', 420001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (5, 'Bauchi', 'Bauchi', 'Pearl of Tourism', 740001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (6, 'Bayelsa', 'Yenagoa'	,'The Glory of All Lands', 561001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (7, 'Benue', 'Makurdi', 'Food basket of the nation', 970001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (8, 'Borno', 'Maiduguri', 'Home of peace', 600001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (9, 'Cross River','Calabar', 'The peoples paradise', 540001 )"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (10, 'Delta', 'Asaba' , 'The Finger of God', 320001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (11, 'Ebonyi', 'Abakaliki', 'The salt of the nation', 840001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (12, 'Edo', 'Benin City', 'The heart beat of the nation', 300001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (13, 'Ekiti', 'Ado-Ekiti', 'Land of honour and integrity', 360001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (14, 'Enugu', 'Enugu', 'Coal city state', 400001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (15, 'FCT', 'Abuja', 'FCT', 900001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (16, 'Gombe', 'Gombe', 'Jewel in the Savannah', 760001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (17, 'Imo', 'Owerri', 'Eastern heartland', 460001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (18, 'Jigawa', 'Dutse', 'The New World', 720001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (19, 'Kaduna', 'Kaduna', 'Centere of learning', 800001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (20, 'Kano', 'Kano', 'Centre of Commerce', 700001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (21, 'Katsina', 'Katsina', 'State of Hospitality', 820001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (22, 'Kebbi', 'Birnin Kebbi', 'Land of Equity', 820001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (23, 'Kogi', 'Lokoja', 'The Confluence State', 260001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (24, 'Kwara', 'Ilorin', 'The state of Harmony', 240001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (25, 'Lagos', 'Ikeja', 'Center of Excellence', 101001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (26, 'Nasarawa', 'Lafia', 'Home of solid minerals', 962001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (27, 'Niger', 'Minna', 'The power State', 920001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (28, 'Ogun', 'Abeokuta', 'The gateway state', 110001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (29, 'Ondo', 'Akure', 'The sunshine State', 340001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (30, 'Osun', 'Oshogbo', 'Land of virtue', 230001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (31, 'Oyo', 'Ibadan', 'The pace setter State', 200001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (32, 'Plateau', 'Jos', 'Home of peace and tourism', 930001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (33, 'Rivers', 'Port Harcourt', 'Treasure base of the Nation', 500001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (34, 'Sokoto', 'Sokoto', 'Sit of the caliphate', 840001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (35, 'Taraba', 'Jalingo', 'Nature’s gift to the Nation', 660001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (36, 'Yobe', 'Damaturu', 'Pride of the Sahel', 320001)"); 
            migrationBuilder.Sql("INSERT INTO States (Id, Name, Capital, Slogan, StateCode) VALUES (37, 'Zamfara', 'Gusau', 'Farming is our pride', 860001)"); 


            
}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
