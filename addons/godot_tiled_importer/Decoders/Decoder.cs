using System;

public abstract class Decoder 
{
    public abstract LayerData Decode(int mapWidth, int mapHeidth, string encodedString);
}
