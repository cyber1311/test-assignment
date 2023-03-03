using Playlist;
using PlaylistClient;

namespace PlaylistClientTests;

public class PlaylistManagerClientTest
{
    [Fact]
    public async void AddTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        
        await playlistManagerClient.ClearPlaylist();
        var playlist = await playlistManagerClient.GetPlaylist();
        var countBeforeAdding = playlist.Count();
        Assert.Equal(0, countBeforeAdding);
        
        await playlistManagerClient.AddSong(song);
        playlist = await playlistManagerClient.GetPlaylist();
        var countAfterAdding = playlist.Count();
        Assert.Equal(1, countAfterAdding);
    }
    
    
    [Fact]
    public async void DeleteTest()
    {
        
        var playlistManagerClient = new PlaylistManagerClient();
        
        var songTitle = "Song 1";
        var song = new Song()
        {
            Title = songTitle,
            Duration = 1000
        };
        await playlistManagerClient.ClearPlaylist();
        
        
        await playlistManagerClient.AddSong(song);
        var playlist = await playlistManagerClient.GetPlaylist();
        var countBeforeDelete = playlist.Count();
        Assert.Equal(1, countBeforeDelete);
        
        await playlistManagerClient.DeleteSong(songTitle);
        playlist = await playlistManagerClient.GetPlaylist();
        var countAfterDelete = playlist.Count;
        Assert.Equal(0, countAfterDelete);
        
    }
    
    [Fact]
    public async void GetPlaylistTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 1000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 1000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 1000
        };
        await playlistManagerClient.AddSong(song);

        var playlist = await playlistManagerClient.GetPlaylist();
        var playlistList = playlist.ToList();
        Assert.Equal("Song 1", playlistList[0]);
        Assert.Equal("Song 2", playlistList[1]);
        Assert.Equal("Song 3", playlistList[2]);
        
    }
    
    
    [Fact]
    public async void PlayTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        
        var result = await playlistManagerClient.Play();
        
        Assert.Equal("Playing", result);
        
    }
    
    [Fact]
    public async void PauseTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        
        
        playlistManagerClient.Play();
        await Task.Delay(1000);
        var result  = await playlistManagerClient.Pause();
        Assert.Equal("Pause", result);
        
    }
    
    
    [Fact]
    public async void NextTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        

        playlistManagerClient.Play();
        await Task.Delay(100);
        
        var result = await playlistManagerClient.Next();
        
        Assert.Equal("Not found", result);
    }
    
    [Fact]
    public async void PrevTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 2",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        song = new Song()
        {
            Title = "Song 3",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);
        
        playlistManagerClient.Play();
        playlistManagerClient.Next();
        var result = await playlistManagerClient.Prev();
        
        Assert.Equal("Not found", result);
    }
    
    [Fact]
    public async void PrevFailTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);

        playlistManagerClient.Play();
        
        var result = await playlistManagerClient.Prev();

        Assert.Equal("Not found", result);
        
    }
    
    [Fact]
    public async void NextFailTest()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        
        var song = new Song()
        {
            Title = "Song 1",
            Duration = 2000
        };
        await playlistManagerClient.AddSong(song);

        playlistManagerClient.Play();

        var result = await playlistManagerClient.Next();

        Assert.Equal("Not found", result);
        
    }
}