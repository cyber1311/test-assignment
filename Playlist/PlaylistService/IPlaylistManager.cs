using PlaylistService.Models;
namespace PlaylistService;

public interface IPlaylistManager
{
    public Task<StatusCode> Play();
    public void Pause();
    public Task<StatusCode> Next();
    public Task<StatusCode> Prev();
    
    public Task<StatusCode> AddSong(Song song);

    public Task<StatusCode> DeleteSong(string songTitle);

    public Task<IEnumerable<string>> GetPlaylist();
    
    public Task<StatusCode> ClearPlaylist();
}