using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Playlist;

namespace PlaylistClient;

public class PlaylistManagerClient : IPlaylistManagerClient
{
    
    private readonly Playlist.Playlist.PlaylistClient _client;

    public PlaylistManagerClient()
    {
        var channel = GrpcChannel.ForAddress("https://localhost:7003");
        _client = new Playlist.Playlist.PlaylistClient(channel);
    }

    public async Task<string> Play()
    {
        var statusResponse = await _client.PlayAsync(new Empty());
        await Task.Delay(100);
        return statusResponse.Code == 200 ? "Playing" : statusResponse.Message;
    }

    public async Task<string> Pause()
    {
        await _client.PauseAsync(new Empty());
        await Task.Delay(100);
        return "Pause";
    }

    public async Task<string> AddSong(Song song)
    {
        var request = new AddSongRequest()
        {
            Song = new Song()
            {
                Title = song.Title,
                Duration = song.Duration,
            }
        };
        var statusResponse =  await _client.AddSongAsync(request);
        await Task.Delay(500);
        return statusResponse.Code == 200 ? $"{song.Title} was added" : statusResponse.Message;
    }

    public async Task<string> DeleteSong(string songTitle)
    {
        var request = new DeleteSongRequest()
        {
            SongTitle = songTitle
        };
        var statusResponse = await _client.DeleteSongAsync(request);
        await Task.Delay(500);
        return statusResponse.Code == 200 ?  $"{songTitle} was deleted" : statusResponse.Message;
    }

    public async Task<string> Next()
    {
        var statusResponse = await _client.NextAsync(new Empty());
        await Task.Delay(100);
        return statusResponse.Code == 200 ? "Go to next song" : statusResponse.Message;
    }

    public async Task<string> Prev()
    {
        var statusResponse = await _client.PrevAsync(new Empty());
        await Task.Delay(100);
        return statusResponse.Code == 200 ? "Go to previous song" : statusResponse.Message;
    }

    public async Task<string> ClearPlaylist()
    {
        var statusResponse = await _client.ClearPlaylistAsync(new Empty());
        await Task.Delay(500);
        return statusResponse.Code == 200 ? "Playlist was cleaned" : statusResponse.Message;
    }

    public async Task<List<string>> GetPlaylist()
    {
        var playlistResponse = await _client.GetPlaylistAsync(new Empty());
        await Task.Delay(500);

        return playlistResponse.SongTitles.ToList();
    }
}