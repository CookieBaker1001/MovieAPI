using MovieAPI.Models;

namespace MovieAPI.Data.Seed
{
    public static class DbSeeder
    {
        public static void Initialize(MovieAPIContext context)
        {
            // Kontrollera om databasen redan har data för att undvika dubbletter
            //if (context.Movies.Any())
            //{
            //    return;   // Databasen är redan seedad
            //}

            var movies = new List<Movie>
            {
                new Movie { Title = "De hänsynslösa (Reservoir Dogs)", Year = 1992, Duration = 5940 ,
                    Details = new MovieDetails {
                    Synopsis = "Efter ett misslyckat rån där polisen överraskar dem, flyr en grupp kriminella till en lagerlokal.",
                    Language = "Engelska",
                    Budget = 1250000,
                    Genre = "Thriller"
                    },
                    Actors = new List<Actor>
                    {
                        new Actor
                        {
                            Name = "Harvey Keitel",
                            BirthYear = 1939
                        },
                        new Actor
                        {
                            Name = "Tim Roth",
                            BirthYear = 1961
                        }
                    },
                    Reviews = new List<Review> {
                        new Review {
                            ReviewerName = "Adam Brannon",
                            Comment = "After last year’s bitterly disappointing Once Upon A Time in Hollywood bla bla.",
                            Rating = 4
                        }
                    }
                },
                new Movie { Title = "Monty Python's Life of Brian", Year = 1979, Duration = 5640,
                    Details = new MovieDetails {
                        Synopsis = "The film's themes of religious satire were controversial at the time of its release, " +
                        "drawing accusations of blasphemy and protests from some religious groups..",
                        Language = "Engelska",
                        Budget = 4000000,
                        Genre = "Comedy"
                    },

                    Actors = new List<Actor>
                    {
                    new Actor
                    {
                        Name = "Graham Chapman",
                        BirthYear = 1941
                    },
                    new Actor
                    {
                        Name = "John Cleese",
                        BirthYear = 1939
                    },
                    new Actor
                    {
                        Name = "Eric Idle",
                        BirthYear = 1943
                    }
                    },
                    Reviews = new List<Review> {
                        new Review {
                            ReviewerName = "Adam Brannon",
                            Comment = "After last year’s bitterly disappointing Once Upon A Time in Hollywood bla bla.",
                            Rating = 4
                        }
                    }
                },
                new Movie { 
                    Title = "Django Unchained", Year = 2012, Duration = 99000, 
                    Details = new MovieDetails {
                    Synopsis = "The film is a highly stylized, revisionist tribute to spaghetti Westerns, with its title referring particularly " 
                    + "to the 1966 Italian film Django by Sergio Corbucci. Development of Django Unchained began in 2007, when Tarantino was writing "
                    + "a book on Corbucci. By April 2011, Tarantino sent his final draft of the script to the Weinstein Company (TWC).",
                    Language = "Engelska",
                    Budget = 4000000,
                    Genre = "Crime"
                    },
                    Actors = new List<Actor>
                    {
                      new Actor
                      {
                        Name = "Jamie Foxx",
                        BirthYear = 1967
                      },
                      new Actor
                      {
                        Name = "Christoph Waltz",
                        BirthYear = 1956
                      }
                    },
                    Reviews = new List<Review> {
                        new Review {
                            ReviewerName = "Adam Brannon",
                            Comment = "After last year’s bitterly disappointing Once Upon A Time in Hollywood bla bla.",
                            Rating = 4
                        }
                    }
                },
            };

            context.Movies.AddRange(movies);
            context.SaveChanges();
        }
    }
}
