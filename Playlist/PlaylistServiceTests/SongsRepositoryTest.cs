using Microsoft.Extensions.Configuration;
using PlaylistService.Database;
using PlaylistService.Models;

namespace SongsRepositoryTests;

public class SongsRepositoryTest
{
    private const string ConnectionString = "User ID=;Password=;Host=localhost;Port=5432;Database=;";

    [Fact]
    public async void AddToDatabaseTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        
        await songRepository.ClearPlaylist();
        var playlist = await songRepository.GetPlaylist();
        var countBeforeAdding = playlist.Count();
        Assert.Equal(0, countBeforeAdding);
        
        await songRepository.AddSong(song);
        playlist = await songRepository.GetPlaylist();
        var countAfterAdding = playlist.Count();
        Assert.Equal(1, countAfterAdding);
    }
    
    [Fact]
    public async void DeleteFromDatabaseTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var songTitle = "Song 1";
        var song = new Song()
        {
            Title = songTitle,
            Duration = 1000
        };
        await songRepository.ClearPlaylist();
        
        
        await songRepository.AddSong(song);
        var playlist = await songRepository.GetPlaylist();
        var countBeforeDelete = playlist.Count();
        Assert.Equal(1, countBeforeDelete);
        
        await songRepository.DeleteSong(songTitle);
        playlist = await songRepository.GetPlaylist();
        var countAfterDelete = playlist.Count();
        Assert.Equal(0, countAfterDelete);
        
    }
    
    [Fact]
    public async void CheckReferencesToNextAndPrevSongsTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        await songRepository.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 1000
        };
        await songRepository.AddSong(song);

        var songNode1 = await songRepository.GetSongNode("Song 1");
        var songNode2 = await songRepository.GetSongNode("Song 2");
        var songNode3 = await songRepository.GetSongNode("Song 3");
        
        
        Assert.Null(songNode1.PrevSongTitle);
        Assert.Equal("Song 2", songNode1.NextSongTitle);
        
        Assert.Equal("Song 1", songNode2.PrevSongTitle);
        Assert.Equal("Song 3", songNode2.NextSongTitle);
        
        Assert.Equal("Song 2", songNode3.PrevSongTitle);
        Assert.Null(songNode3.NextSongTitle);

    }
    
    [Fact]
    public async void CheckReferencesToNextAndPrevSongsAfterDeleteTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        await songRepository.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 1000
        };
        await songRepository.AddSong(song);

        var songNode1 = await songRepository.GetSongNode("Song 1");
        var songNode2 = await songRepository.GetSongNode("Song 2");
        var songNode3 = await songRepository.GetSongNode("Song 3");
        
        
        Assert.Null(songNode1.PrevSongTitle);
        Assert.Equal("Song 2", songNode1.NextSongTitle);
        
        Assert.Equal("Song 1", songNode2.PrevSongTitle);
        Assert.Equal("Song 3", songNode2.NextSongTitle);
        
        Assert.Equal("Song 2", songNode3.PrevSongTitle);
        Assert.Null(songNode3.NextSongTitle);
        
        await songRepository.DeleteSong("Song 2");
        
        songNode1 = await songRepository.GetSongNode("Song 1");
        songNode3 = await songRepository.GetSongNode("Song 3");
        
        Assert.Null(songNode1.PrevSongTitle);
        Assert.Equal("Song 3", songNode1.NextSongTitle);

        Assert.Equal("Song 1", songNode3.PrevSongTitle);
        Assert.Null(songNode3.NextSongTitle);

    }
    
    [Fact]
    public async void GetFirstSongTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        await songRepository.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 1000
        };
        await songRepository.AddSong(song);

        var firstSongNode = await songRepository.GetFirstSong();
        Assert.Equal("Song 1", firstSongNode.SongTitle);
        
        await songRepository.DeleteSong("Song 1");
        firstSongNode = await songRepository.GetFirstSong();
        Assert.Equal("Song 2", firstSongNode.SongTitle);
        
    }
    
    [Fact]
    public async void GetPlaylistTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        await songRepository.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 1000
        };
        await songRepository.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 1000
        };
        await songRepository.AddSong(song);

        var playlist = await songRepository.GetPlaylist();
        var playlistList = playlist.ToList();
        Assert.Equal("Song 1", playlistList[0]);
        Assert.Equal("Song 2", playlistList[1]);
        Assert.Equal("Song 3", playlistList[2]);
        
    }
}