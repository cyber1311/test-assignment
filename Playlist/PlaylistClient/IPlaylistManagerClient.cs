using System.Collections.Generic;
using System.Threading.Tasks;
using Playlist;

namespace PlaylistClient;

public interface IPlaylistManagerClient
{
    Task<string> Play();
    Task<string> Pause();
    Task<string> AddSong(Song song);
    Task<string> DeleteSong(string songTitle);
    Task<string> Next();
    Task<string> Prev();
    
    Task<string> ClearPlaylist();
    
    Task<List<string>> GetPlaylist();
}