using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Playlist;
using Song = PlaylistService.Models.Song;


namespace PlaylistService.Services
{
    public class PlaylistService : Playlist.Playlist.PlaylistBase
    {
        private readonly IPlaylistManager _playlistManager;
        

        public PlaylistService(IPlaylistManager playlistManager)
        {
            _playlistManager = playlistManager;
        }
        
        
        
        public override async Task<StatusResponse> AddSong(AddSongRequest request, ServerCallContext context)
        {
            var song = new Song()
            {
                Title = request.Song.Title,
                Duration = request.Song.Duration
            };
            
            var statusCode = await _playlistManager.AddSong(song);
            return await Task.FromResult(new StatusResponse
            {
                Code = statusCode.Code,
                Message = statusCode.Message,
            });
            
        } 
        
        public override async Task<StatusResponse> DeleteSong(DeleteSongRequest request, ServerCallContext context)
        {
            var statusCode = await _playlistManager.DeleteSong(request.SongTitle);
            return await Task.FromResult(new StatusResponse
            {
                Code = statusCode.Code,
                Message = statusCode.Message,
            });
        }
        
        public override async Task<PlaylistResponse> GetPlaylist(Empty request, ServerCallContext context)
        {
            var playlist = await _playlistManager.GetPlaylist();
            var response = new PlaylistResponse();
            foreach (var song in playlist)
            {
                response.SongTitles.Add(song);
            }
            return await Task.FromResult(response);
        }
        
        public override async Task<StatusResponse> Play(Empty request, ServerCallContext context)
        {
            var statusCode = await _playlistManager.Play();
            return await Task.FromResult(new StatusResponse
            {
                Code = statusCode.Code,
                Message = statusCode.Message,
            });
        }
        
        public override async Task<Empty> Pause(Empty request, ServerCallContext context)
        {
            _playlistManager.Pause();
            return await Task.FromResult(new Empty());
        }
        
        public override async Task<StatusResponse> Next(Empty request, ServerCallContext context)
        {
            var statusCode = await _playlistManager.Next();
            return await Task.FromResult(new StatusResponse
            {
                Code = statusCode.Code,
                Message = statusCode.Message,
            });
        }
        
        public override async Task<StatusResponse> Prev(Empty request, ServerCallContext context)
        {
            var statusCode = await _playlistManager.Prev();
            return await Task.FromResult(new StatusResponse
            {
                Code = statusCode.Code,
                Message = statusCode.Message,
            });
        }
        
        public override async Task<StatusResponse> ClearPlaylist(Empty request, ServerCallContext context)
        {
            var statusCode = await _playlistManager.ClearPlaylist();
            return await Task.FromResult(new StatusResponse
            {
                Code = statusCode.Code,
                Message = statusCode.Message,
            });
        }
    }
}

