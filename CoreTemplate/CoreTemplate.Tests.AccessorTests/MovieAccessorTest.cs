﻿using AutoMapper;
using CoreTemplate.Accessors.Accessors;
using CoreTemplate.Accessors.Interfaces;
using CoreTemplate.Accessors.Models.DTO;
using CoreTemplate.Accessors.Models.EF;
using CoreTemplate.Tests.AccessorTests.Config;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace CoreTemplate.Tests.AccessorTests
{
    [Collection("Accessors")]
    public class MovieAccessorTest
    {
        private IMovieAccessor _movieAccessor;
        private DatabaseHelper _databaseHelper;

        //This is method is called before the start of every test in this class
        public MovieAccessorTest()
        {
            _databaseHelper = new DatabaseHelper();
            _movieAccessor = new MovieAccessor(_databaseHelper.Context);
        }

        [Fact]
        public void Get()
        {
            //Arrange
            var expectedMovie = _databaseHelper.SeedMovies().First();

            //Act
            var actualMovieDto = _movieAccessor.Get(expectedMovie.Id);

            //Assert
            var actualMovie = Mapper.Map<Movie>(actualMovieDto);

            actualMovie.ShouldBeEquivalentTo(expectedMovie);
        }

        [Fact]
        public void GetAll()
        {
            //Arrange
            var expectedMovies = _databaseHelper.SeedMovies(10);

            //Act
            var actualMovieDtos = _movieAccessor.GetAll();

            //Assert
            Assert.NotNull(actualMovieDtos);

            Assert.Equal(expectedMovies.Count, actualMovieDtos.Count);

            foreach (var expectedMovie in expectedMovies)
            {
                var actualMovieDto = actualMovieDtos.Single(x => x.Id == expectedMovie.Id);

                var actualMovie = Mapper.Map<Movie>(actualMovieDto);

                actualMovie.ShouldBeEquivalentTo(expectedMovie);
            }
        }

        [Fact]
        public void Save()
        {
            //Arrange
            var expectedMovie = _databaseHelper.SeedMovies().First();

            var originalName = expectedMovie.Name;

            expectedMovie.Name = Guid.NewGuid().ToString();

            var newName = expectedMovie.Name;

            var expectedMovieDto = Mapper.Map<MovieDTO>(expectedMovie);

            //Act
            _movieAccessor.Save(expectedMovieDto);

            //Assert
            var actualMovie = _databaseHelper.Context.Movies.Single(x => x.Id == expectedMovie.Id);

            Assert.NotEqual(originalName, actualMovie.Name);

            Assert.Equal(newName, actualMovie.Name);

            actualMovie.ShouldBeEquivalentTo(expectedMovie);
        }
    }
}