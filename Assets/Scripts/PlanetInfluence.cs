public struct PlanetInfluence {
    public object source; 
    public int heatEffect;
    public int humidityEffect;
    public int atmosphereEffect;

    public PlanetInfluence(object source, int heat, int humidity, int atmosphere) {
        this.source = source;
        heatEffect = heat;
        humidityEffect = humidity;
        atmosphereEffect = atmosphere;
    }
}