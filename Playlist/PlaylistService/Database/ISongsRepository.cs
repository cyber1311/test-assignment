using PlaylistService.Models;

namespace PlaylistService.Database;

public interface ISongsRepository
{
    Task<StatusCode> AddSong(Song song);

    Task<StatusCode> DeleteSong(string songTitle);

    Task<SongNode> GetSongNode(string songTitle);
    Task<SongNode> GetFirstSong();

    Task<IEnumerable<string>> GetPlaylist();
    
    Task<StatusCode> ClearPlaylist();
}