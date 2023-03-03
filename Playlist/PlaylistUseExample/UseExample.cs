using Playlist;
using PlaylistClient;
using Xunit.Abstractions;

namespace UseExample;

public class UseExample
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UseExample(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async void Example()
    {
        var playlistManagerClient = new PlaylistManagerClient();
        await playlistManagerClient.ClearPlaylist();
        var rnd = new Random();
        
        await playlistManagerClient.ClearPlaylist();
        for (int i = 1; i < 11; i++)
        {
            var result = await playlistManagerClient.AddSong(new Song()
            {
                Title = $"Song {i}",
                Duration =  rnd.Next(1000,4000)
            });
            _testOutputHelper.WriteLine(result);
            
        }
        
        var playlist = await playlistManagerClient.GetPlaylist();
        var count = playlist.Count;
        Assert.Equal(10, count);
        _testOutputHelper.WriteLine("Playlist:");
        foreach (var song in playlist)
        {
            _testOutputHelper.WriteLine(song);
        }
        
        await playlistManagerClient.DeleteSong("Song 1");
        await playlistManagerClient.DeleteSong("Song 5");
        
        playlist = await playlistManagerClient.GetPlaylist();
        count = playlist.Count;
        Assert.Equal("Song 2", playlist[0]);
        Assert.Equal(8, count);
        _testOutputHelper.WriteLine("Playlist:");
        foreach (var song in playlist)
        {
            _testOutputHelper.WriteLine(song);
        }
        
        playlistManagerClient.Play();
        playlistManagerClient.Pause();
        playlistManagerClient.Play();
        playlistManagerClient.Next();
        playlistManagerClient.Next();
        await playlistManagerClient.Prev();
    }
}