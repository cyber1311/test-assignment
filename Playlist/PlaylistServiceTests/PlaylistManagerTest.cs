using System.Diagnostics;
using PlaylistService;
using PlaylistService.Database;
using PlaylistService.Models;

namespace SongsRepositoryTests;

public class PlaylistManagerTest
{
    private const string ConnectionString = "User ID=;Password=;Host=localhost;Port=5432;Database=;";

    
    [Fact]
    public async void AddToDatabaseTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        
        await playlistManager.ClearPlaylist();
        var playlist = await playlistManager.GetPlaylist();
        var countBeforeAdding = playlist.Count();
        Assert.Equal(0, countBeforeAdding);
        
        await playlistManager.AddSong(song);
        playlist = await playlistManager.GetPlaylist();
        var countAfterAdding = playlist.Count();
        Assert.Equal(1, countAfterAdding);
    }
    
    
    [Fact]
    public async void DeleteFromDatabaseTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        
        var songTitle = "Song 1";
        var song = new Song()
        {
            Title = songTitle,
            Duration = 1000
        };
        await playlistManager.ClearPlaylist();
        
        
        await playlistManager.AddSong(song);
        var playlist = await playlistManager.GetPlaylist();
        var countBeforeDelete = playlist.Count();
        Assert.Equal(1, countBeforeDelete);
        
        await playlistManager.DeleteSong(songTitle);
        playlist = await playlistManager.GetPlaylist();
        var countAfterDelete = playlist.Count();
        Assert.Equal(0, countAfterDelete);
        
    }
    
    [Fact]
    public async void GetPlaylistTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 1000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 1000
        };
        await playlistManager.AddSong(song);

        var playlist = await playlistManager.GetPlaylist();
        var playlistList = playlist.ToList();
        Assert.Equal("Song 1", playlistList[0]);
        Assert.Equal("Song 2", playlistList[1]);
        Assert.Equal("Song 3", playlistList[2]);
        
    }
    
    
    [Fact]
    public async void PlayTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);

        var task = playlistManager.Play();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);
        await Task.Delay(2000);
        
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        await Task.Delay(2000);
        
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 3", currentSong.SongTitle);
        task.Wait();
    }
    
    [Fact]
    public async void PauseTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);

        var stopwatch = new Stopwatch();
        
        stopwatch.Start();
        var task = playlistManager.Play();
        await Task.Delay(1000);
        playlistManager.Pause();
        stopwatch.Stop();
        var timeBeforePause = stopwatch.ElapsedMilliseconds;
        
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);
        
        task = playlistManager.Play();
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);
        
        await Task.Delay((int)(2100-timeBeforePause));
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        
        task.Wait();
    }
    
    
    [Fact]
    public async void NextTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);

        var task = playlistManager.Play();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);

        task = playlistManager.Next();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        task = playlistManager.Next();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 3", currentSong.SongTitle);

        task.Wait();
    }
    
    [Fact]
    public async void PrevTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);
        var task = playlistManager.Play();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);

        task = playlistManager.Next();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        task = playlistManager.Next();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 3", currentSong.SongTitle);
        await Task.Delay(100);
        task = playlistManager.Prev();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        task = playlistManager.Prev();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);
        await Task.Delay(2000);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        
        task.Wait();
    }
    
    [Fact]
    public async void DeleteFromDatabaseWhilePlayingTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);

        var task = playlistManager.Play();
        await Task.Delay(100);
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 1", currentSong.SongTitle);
        
        var statusCode = await playlistManager.DeleteSong("Song 1");
        Assert.Equal(400, statusCode.Code);
        Assert.Equal("You cannot delete song during play", statusCode.Message);
        
        playlistManager.Pause();
        statusCode = await playlistManager.DeleteSong("Song 1");
        Assert.Equal(200, statusCode.Code);
        Assert.Equal("OK", statusCode.Message);
        
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 2", currentSong.SongTitle);
        
        statusCode = await playlistManager.DeleteSong("Song 2");
        Assert.Equal(200, statusCode.Code);
        Assert.Equal("OK", statusCode.Message);
        
        currentSong = playlistManager.GetCurrentSong();
        Assert.Equal("Song 3", currentSong.SongTitle);
        
        statusCode = await playlistManager.DeleteSong("Song 3");
        Assert.Equal(200, statusCode.Code);
        Assert.Equal("OK", statusCode.Message);
        
        currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);
        
    }
    
    [Fact]
    public async void PrevFailTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
       
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);

        playlistManager.Play();
        
        await Task.Delay(100);

        var statusCode = await playlistManager.Prev();

        Assert.Equal(404, statusCode.Code);
        Assert.Equal("Not found", statusCode.Message);
        
    }
    
    [Fact]
    public async void NextFailTest()
    {
        var songRepository = new SongsRepository(ConnectionString);
        var playlistManager = new PlaylistManager(songRepository);
        await playlistManager.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManager.AddSong(song);
       
        
        var currentSong = playlistManager.GetCurrentSong();
        Assert.Null(currentSong);

        playlistManager.Play();
        
        await Task.Delay(100);

        var statusCode = await playlistManager.Next();

        Assert.Equal(404, statusCode.Code);
        Assert.Equal("Not found", statusCode.Message);
        
    }
}