namespace PlaylistService.Models;

public class SongNode
{
    public string SongTitle { get; set; }
    public int Duration { get; set; }
    public string? NextSongTitle { get; set; }
    public string? PrevSongTitle { get; set; }
}