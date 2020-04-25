
// Classe estática para armazenar os estados do jogo entre as fases
public class GameState 
{
    public static float volume = 1f;
    public static int score;


    public void setvolume(float newvolume)
    {
        volume = newvolume;
    }

    public void setscore(int newscore)
    {
        score = newscore;
    }
}
