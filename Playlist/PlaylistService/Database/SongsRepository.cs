using Dapper;
using Npgsql;
using PlaylistService.Models;

namespace PlaylistService.Database;

public class SongsRepository : ISongsRepository
{
    private readonly string _connectionString;
    
    private const string InsertCommand =
        @"insert into songs(song_title, duration, next_song_title, prev_song_title) VALUES(@songTitle, @duration, @nextSongTitle, @prevSongTitle)";

    private const string GetLastSongTitleCommand =
        @"select song_title from songs order by id desc limit 1;";
    
    private const string GetFirstSongCommand =
        @"select  song_title as songTitle, duration, next_song_title as nextSongTitle, prev_song_title as prevSongTitle from songs order by id asc limit 1;";
    
    private const string GetSongCommand =
        @"select song_title as songTitle, duration, next_song_title as nextSongTitle, prev_song_title as prevSongTitle from songs where song_title = @songTitle;";
    
    private const string GetAllSongsCommand =
        @"select song_title from songs order by id asc;";
    
    private const string UpdateNextSongTitleCommand = 
        @"update songs set next_song_title = @nextSongTitle where song_title = @songTitle;";
    
    private const string UpdatePrevSongTitleCommand = 
        @"update songs set prev_song_title = @prevSongTitle where song_title = @songTitle;";
    
    private const string DeleteSongCommand = 
        @"delete from songs where song_title = @songTitle;";
    
    private const string DeletePlaylistCommand = 
        @"delete from songs;";

    
    public SongsRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetValue<string>("Database:ConnectionString");
    }
    
    public SongsRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    
    public async Task<StatusCode> AddSong(Song song)
    {
        var statusCode = new StatusCode(code: 200, message: "OK");
        
        await using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        await using (var transaction = await connection.BeginTransactionAsync())
        {
            try
            {
                var lastSongTitle = await connection.QueryFirstOrDefaultAsync<string>(GetLastSongTitleCommand, transaction: transaction);

                string? next = null;
                
                if (lastSongTitle == null)
                {
                    string? prev = null;
                    await connection.ExecuteAsync(InsertCommand, new
                    {
                        songTitle = song.Title,
                        duration = song.Duration,
                        nextSongTitle = next,
                        prevSongTitle = prev,
                    }, transaction: transaction);
                }
                else
                {
                    await connection.ExecuteAsync(InsertCommand, new
                    {
                        songTitle = song.Title,
                        duration = song.Duration,
                        nextSongTitle = next,
                        prevSongTitle = lastSongTitle,
                    }, transaction: transaction);
                    
                    await connection.ExecuteAsync(UpdateNextSongTitleCommand, new
                    {
                        nextSongTitle = song.Title,
                        songTitle = lastSongTitle,
                    }, transaction: transaction);
                }
                
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                statusCode.Code = 500;
                statusCode.Message = "Operation failed";
            }
        }

        return statusCode;
    }

    public async Task<StatusCode> DeleteSong(string songTitle)
    {
        var statusCode = new StatusCode(code: 200, message: "OK");
        
        await using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        await using (var transaction = await connection.BeginTransactionAsync())
        {
            try
            {
                var song = await connection.QueryFirstOrDefaultAsync<SongNode>(GetSongCommand, new
                {
                  songTitle  
                }, transaction: transaction);
                
                
                if (song != null)
                {
                    await connection.ExecuteAsync(UpdateNextSongTitleCommand, new
                    {
                        nextSongTitle = song.NextSongTitle,
                        songTitle = song.PrevSongTitle,
                    }, transaction: transaction);
                    
                    await connection.ExecuteAsync(UpdatePrevSongTitleCommand, new
                    {
                        prevSongTitle = song.PrevSongTitle,
                        songTitle = song.NextSongTitle,
                    }, transaction: transaction);
                    
                    await connection.ExecuteAsync(DeleteSongCommand, new
                    {
                        songTitle,
                    }, transaction: transaction);
                }
                else
                {
                    statusCode.Code = 404;
                    statusCode.Message = "Song not found";
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                statusCode.Code = 500;
                statusCode.Message = "Operation failed";
            }
        }

        return statusCode;
    }

    public async Task<SongNode> GetSongNode(string songTitle)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();
        
        var song = await connection.QueryFirstOrDefaultAsync<SongNode>(GetSongCommand, new
        {
            songTitle
        });
        return song;
    }
    
    public async Task<SongNode> GetFirstSong()
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();
        
        return await connection.QueryFirstOrDefaultAsync<SongNode>(GetFirstSongCommand);
    }

    public async Task<IEnumerable<string>> GetPlaylist()
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        return await connection.QueryAsync<string>(GetAllSongsCommand);
    }

    public async Task<StatusCode> ClearPlaylist()
    {
        var statusCode = new StatusCode(code: 200, message: "OK");
        
        try
        {
            await using var connection = new NpgsqlConnection(_connectionString);

            await connection.OpenAsync();

            await connection.ExecuteAsync(DeletePlaylistCommand);
        }
        catch (Exception)
        {
            statusCode.Code = 500;
            statusCode.Message = "Operation failed";
        }

        return statusCode;

    }
}