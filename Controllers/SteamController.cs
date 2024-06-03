using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using ApiGAMES;
using Microsoft.AspNetCore.Mvc;

namespace ApiGAMES
{
    [ApiController]
    [Route("[controller]")]
    public class SteamController : ControllerBase
    {
        private readonly string _connectionString;
        static private List<Game> favoriteGames = new List<Game>();

        public SteamController()
        {
            // Підключення до бази даних SQLite
            string databaseName = "games.db";
            _connectionString = $"Data Source={databaseName}";

            // Створення таблиці для зберігання ігор, якщо її ще не існує
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var dropTableQuery = "DROP TABLE IF EXISTS Games";
                using (var command = new SQLiteCommand(dropTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                var createTableQuery = @"
    CREATE TABLE IF NOT EXISTS Games (
        Id INTEGER PRIMARY KEY NOT NULL,
        Name TEXT NOT NULL,
        Price TEXT,
        Type TEXT,
        ImageUrl TEXT,
        ReleaseDate TEXT,
        SteamUrl TEXT
    )";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Занесення даних ігор до бази даних
                var gamesToAdd = new List<Game>
            {
                 new Game { Id = 730, Name = "Counter-Strike 2", Price = "Free to Play", Type = "shooting game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/730/header.jpg?t=1683566799", SteamUrl = "https://store.steampowered.com/app/730/CounterStrike_Global_Offensive/", ReleaseDate = "21.08.2012" },
new Game { Id = 570, Name = "Dota 2", Price = "Free to Play", Type = "MOBA", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/570/header.jpg?t=1614889216", SteamUrl = "https://store.steampowered.com/app/570/Dota_2/", ReleaseDate = "09.07.2013" },
new Game { Id = 552520, Name = "Far Cry 5", Price = "$59.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/552520/header.jpg?t=1624297985", SteamUrl = "https://store.steampowered.com/app/552520/Far_Cry_5/", ReleaseDate = "27.03.2018" },
new Game { Id = 381210, Name = "Dead by Daylight", Price = "$19.99", Type = "asymmetric survival horror game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/381210/header.jpg?t=1648767773", SteamUrl = "https://store.steampowered.com/app/381210/Dead_by_Daylight/", ReleaseDate = "14.06.2016" },
new Game { Id = 578080, Name = "PLAYERUNKNOWN'S BATTLEGROUNDS", Price = "$29.99", Type = "battle royale game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/578080/header.jpg?t=1649277575", SteamUrl = "https://store.steampowered.com/app/578080/PLAYERUNKNOWNS_BATTLEGROUNDS/", ReleaseDate = "21.12.2017" },
new Game { Id = 271590, Name = "Grand Theft Auto V", Price = "$29.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/271590/header.jpg?t=1645057617", SteamUrl = "https://store.steampowered.com/app/271590/Grand_Theft_Auto_V/", ReleaseDate = "14.04.2015" },
new Game { Id = 203160, Name = "Tomb Raider", Price = "$19.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/203160/header.jpg?t=1638552705", SteamUrl = "https://store.steampowered.com/app/203160/Tomb_Raider/", ReleaseDate = "05.03.2013" },
new Game { Id = 1174180, Name = "Red Dead Redemption 2", Price = "$59.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1174180/header.jpg?t=1648446613", SteamUrl = "https://store.steampowered.com/app/1174180/Red_Dead_Redemption_2/", ReleaseDate = "05.11.2019" },
new Game { Id = 292030, Name = "The Witcher 3: Wild Hunt", Price = "$39.99", Type = "action role-playing game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg?t=1640947066", SteamUrl = "https://store.steampowered.com/app/292030/The_Witcher_3_Wild_Hunt/", ReleaseDate = "18.05.2015" },
new Game { Id = 391220, Name = "Rise of the Tomb Raider", Price = "$29.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/391220/header.jpg?t=1602210095", SteamUrl = "https://store.steampowered.com/app/391220/Rise_of_the_Tomb_Raider/", ReleaseDate = "28.01.2016" },
new Game { Id = 50130, Name = "Mafia II", Price = "$29.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/50130/header.jpg?t=1649489929", SteamUrl = "https://store.steampowered.com/app/50130/Mafia_II/", ReleaseDate = "23.08.2010" },
new Game { Id = 1030840, Name = "Mafia: Definitive Edition", Price = "$39.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1030840/header.jpg?t=1639647413", SteamUrl = "https://store.steampowered.com/app/1030840/Mafia_Definitive_Edition/", ReleaseDate = "25.09.2020" },
new Game { Id = 10180, Name = "Call of Duty: Modern Warfare II", Price = "$19.99", Type = "first-person shooter", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/10180/header.jpg?t=1572924010", SteamUrl = "https://store.steampowered.com/app/10180/Call_of_Duty_Modern_Warfare_2/", ReleaseDate = "10.11.2009" },
new Game { Id = 1172620, Name = "Sea of Thieves", Price = "$39.99", Type = "action-adventure game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1172620/header.jpg?t=1649283811", SteamUrl = "https://store.steampowered.com/app/1172620/Sea_of_Thieves/", ReleaseDate = "20.03.2018" },
new Game { Id = 1172470, Name = "Apex Legends", Price = "Free to Play", Type = "battle royale game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1172470/header.jpg?t=1648473633", SteamUrl = "https://store.steampowered.com/app/1172470/Apex_Legends/", ReleaseDate = "04.02.2019" },
new Game { Id = 252490, Name = "Rust", Price = "$39.99", Type = "survival game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/252490/header.jpg?t=1648841213", SteamUrl = "https://store.steampowered.com/app/252490/Rust/", ReleaseDate = "08.02.2018" },
new Game { Id = 1091500, Name = "Cyberpunk 2077", Price = "$39.99", Type = "action role-playing game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg?t=1648898184", SteamUrl = "https://store.steampowered.com/app/1091500/Cyberpunk_2077/", ReleaseDate = "10.12.2020" },
new Game { Id = 550, Name = "Left 4 Dead 2", Price = "$9.99", Type = "first-person shooter", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/550/header.jpg?t=1648907747", SteamUrl = "https://store.steampowered.com/app/550/Left_4_Dead_2/", ReleaseDate = "17.11.2009" },
new Game { Id = 4000, Name = "Garry's Mod", Price = "$9.99", Type = "sandbox game", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/4000/header.jpg?t=1648727571", SteamUrl = "https://store.steampowered.com/app/4000/Garrys_Mod/", ReleaseDate = "29.11.2006" },
new Game { Id = 1238810, Name = "Battlefield 1", Price = "$19.99", Type = "first-person shooter", ImageUrl = "https://cdn.cloudflare.steamstatic.com/steam/apps/1238810/header.jpg?t=1646288971", SteamUrl = "https://store.steampowered.com/app/1238810/Battlefield_1/", ReleaseDate = "21.10.2016" }

            };

                foreach (var game in gamesToAdd)
                {
                    var insertQuery = @"
        INSERT INTO Games (Id, Name, Price, Type, ImageUrl, ReleaseDate, SteamUrl)
        VALUES (@Id, @Name, @Price, @Type, @ImageUrl, @ReleaseDate, @SteamUrl)";

                    using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Id", game.Id);
                        insertCommand.Parameters.AddWithValue("@Name", game.Name);
                        insertCommand.Parameters.AddWithValue("@Price", game.Price);
                        insertCommand.Parameters.AddWithValue("@Type", game.Type);
                        insertCommand.Parameters.AddWithValue("@ImageUrl", game.ImageUrl);
                        insertCommand.Parameters.AddWithValue("@ReleaseDate", game.ReleaseDate);
                        insertCommand.Parameters.AddWithValue("@SteamUrl", game.SteamUrl);

                        insertCommand.ExecuteNonQuery();
                    }
                }


            }
        }

        // Метод для отримання інформації про гру за назвою
        [HttpGet("GetInformationByName")]
        public IActionResult GetGameByName(string name)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var selectQuery = "SELECT * FROM Games WHERE Name = @Name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var game = new Game
                            {
                                Id = reader.GetInt32(0), // Змінено з reader.GetInt32(reader.GetOrdinal("Id"))
                                Name = reader.GetString(1), // Змінено з reader.GetString(reader.GetOrdinal("Name"))
                                Price = reader.GetString(2), // Змінено з reader.GetString(reader.GetOrdinal("Price"))
                                Type = reader.GetString(3), // Змінено з reader.GetString(reader.GetOrdinal("Type"))
                                ImageUrl = reader.GetString(4), // Змінено з reader.GetString(reader.GetOrdinal("ImageUrl"))
                                ReleaseDate = reader.GetString(5), // Змінено з reader.GetString(reader.GetOrdinal("ReleaseDate"))
                                SteamUrl = reader.GetString(6) // Змінено з reader.GetString(reader.GetOrdinal("SteamUrl"))
                            };

                            return Ok(game);
                        }
                    }
                }
            }

            return NotFound($"Game with name '{name}' not found.");
        }

        // Метод для отримання посилання на Steam за назвою гри
        [HttpGet("GetSteamLinkByName")]
        public IActionResult GetSteamLinkByName(string name)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var selectQuery = "SELECT SteamUrl FROM Games WHERE Name = @Name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    var steamUrl = command.ExecuteScalar();

                    if (steamUrl != null)
                    {
                        return Ok(steamUrl.ToString());
                    }
                }
            }

            return NotFound($"Game with name '{name}' not found.");
        }

        // Метод для додавання гри до списку обраних
        [HttpPost("AddFavoriteGame")]
        public IActionResult AddFavoriteGame(string name)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                var selectQuery = "SELECT * FROM Games WHERE Name = @Name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var game = new Game
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = Convert.ToString(reader["Name"]),
                                Price = Convert.ToString(reader["Price"]),
                                Type = Convert.ToString(reader["Type"]),
                                ImageUrl = Convert.ToString(reader["ImageUrl"]),
                                ReleaseDate = Convert.ToString(reader["ReleaseDate"]),
                                SteamUrl = Convert.ToString(reader["SteamUrl"])
                            };

                            favoriteGames.Add(game);
                            return Ok($"Game '{name}' added to favorites.");
                        }
                    }
                }
            }

            return NotFound($"Game with name '{name}' not found.");
        }

        // Метод для видалення гри зі списку обраних
        [HttpDelete("RemoveFavoriteGame")]
        public IActionResult RemoveFavoriteGame(string name)
        {
            var gameToRemove = favoriteGames.FirstOrDefault(g => g.Name.Equals(name));
            if (gameToRemove != null)
            {
                favoriteGames.Remove(gameToRemove);
                return Ok($"Game '{name}' removed from favorites.");
            }

            return NotFound($"Game with name '{name}' not found in favorites.");
        }

        // Метод для отримання списку обраних ігор
        [HttpGet("GetFavoriteGames")]
        public IActionResult GetFavoriteGameNames()
        {
            var favoriteGameNames = favoriteGames.Select(g => g.Name).ToList();
            return Ok(favoriteGameNames);
        }
    }
}
