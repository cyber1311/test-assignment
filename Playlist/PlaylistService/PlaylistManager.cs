using System.Diagnostics;
using PlaylistService.Database;
using PlaylistService.Models;

namespace PlaylistService;

public class PlaylistManager : IPlaylistManager
{
    private readonly ISongsRepository _songsRepository;
    private Stopwatch _stopwatch;
    private SongNode? _currentSong;
    private int _pauseMoment;
    private CancellationTokenSource? _cancellationTokenSource;
    private readonly ILogger<PlaylistManager>? _logger;

    public PlaylistManager(ISongsRepository songsRepository, ILogger<PlaylistManager> logger)
    {
        _songsRepository = songsRepository;
        _logger = logger;
        _stopwatch = new Stopwatch();
        _currentSong = null;
        _pauseMoment = 0;
        _cancellationTokenSource = null;
    }
    
    public PlaylistManager(ISongsRepository songsRepository)
    {
        _songsRepository = songsRepository;
        _stopwatch = new Stopwatch();
        _currentSong = null;
        _pauseMoment = 0;
        _cancellationTokenSource = null;
    }

    public async Task<StatusCode> Play()
    {
        var statusCode = new StatusCode(code: 200, message: "OK");
        
        _currentSong ??= await _songsRepository.GetFirstSong();
        
        if (_currentSong != null)
        { 
            _cancellationTokenSource = new CancellationTokenSource();
            var duration = _currentSong.Duration - _pauseMoment;
            _logger?.LogInformation($"Play {_currentSong.SongTitle}");
            _stopwatch = Stopwatch.StartNew();
            await Task.Delay(duration, _cancellationTokenSource.Token);
            
            if (_currentSong.NextSongTitle != null)
            {
                _currentSong = await _songsRepository.GetSongNode(_currentSong.NextSongTitle);
                await Play();
            }
            else
            {
                _logger?.LogInformation("Stop playing. End of playlist");
            }
        }
        else
        {
            statusCode.Code = 404;
            statusCode.Message = "Not found";
            _logger?.LogError("Playlist is empty");
        }

        return statusCode;
    }

    public void Pause()
    {
        if (_cancellationTokenSource == null) return;
        _stopwatch.Stop();
        _logger?.LogInformation("Pause");
        _cancellationTokenSource.Cancel();
        _pauseMoment = (int) (_currentSong.Duration - _stopwatch.ElapsedMilliseconds);
        _cancellationTokenSource= null;
    }

    

    public async Task<StatusCode> Next()
    {
        var statusCode = new StatusCode(code: 200, message: "OK");
        Pause();
        _pauseMoment = 0;
        if (_currentSong?.NextSongTitle != null)
        {
            _logger?.LogInformation("Go to next song");
            _currentSong =await _songsRepository.GetSongNode(_currentSong.NextSongTitle);
            await Play();
        }
        else
        {
            statusCode.Code = 404;
            statusCode.Message = "Not found";
            _logger?.LogInformation("Next song doesn't exist");
        }
        
        return statusCode;
    }

    public async Task<StatusCode> Prev()
    {
        var statusCode = new StatusCode(code: 200, message: "OK");
        
        Pause();
        _pauseMoment = 0;
        if (_currentSong?.PrevSongTitle != null)
        {
            _logger?.LogInformation("Go to previous song");
            _currentSong = await _songsRepository.GetSongNode(_currentSong.PrevSongTitle);
            await Play();
        }else
        {
            statusCode.Code = 404;
            statusCode.Message = "Not found";
            _logger?.LogInformation("Previous song doesn't exist");
        }
        
        return statusCode;
        
    }

    public async Task<StatusCode> AddSong(Song song)
    {
        var statusCode = await _songsRepository.AddSong(song);
        if(statusCode.Code == 200) _logger?.LogInformation($"{song.Title} was added");
        else _logger?.LogError("Operation failed");
        return statusCode;
    }
    
    public async Task<StatusCode> DeleteSong(string songTitle)
    {
        StatusCode statusCode;
        if (_currentSong?.SongTitle == songTitle)
        {
            if (_cancellationTokenSource?.Token == null)
            {
                if (_currentSong?.NextSongTitle != null)
                {
                    _currentSong =await _songsRepository.GetSongNode(_currentSong.NextSongTitle);
                }else if (_currentSong?.PrevSongTitle != null)
                {
                    _currentSong = await _songsRepository.GetSongNode(_currentSong.PrevSongTitle);
                }
                else _currentSong = null;
                
                statusCode = await _songsRepository.DeleteSong(songTitle);
                if(statusCode.Code == 200) _logger?.LogInformation($"{songTitle} was deleted");
                else _logger?.LogError("Operation failed");
                return statusCode;
            } 
            _logger?.LogError("You cannot delete song during play");
            return new StatusCode(code: 400, message: "You cannot delete song during play");
        } 
        
        statusCode = await _songsRepository.DeleteSong(songTitle);
        if(statusCode.Code == 200) _logger?.LogInformation($"{songTitle} was deleted");
        else _logger?.LogError("Operation failed");
        return statusCode;
    }

    public Task<IEnumerable<string>> GetPlaylist()
    {
        return _songsRepository.GetPlaylist();
    }

    public async Task<StatusCode> ClearPlaylist()
    {
        var statusCode = await _songsRepository.ClearPlaylist();
        if(statusCode.Code == 200) _logger?.LogInformation("Playlist was cleaned");
        else _logger?.LogError("Operation failed");
        return statusCode;
    }

    public SongNode? GetCurrentSong()
    {
        return _currentSong;
    }
}