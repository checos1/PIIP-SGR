namespace DNP.Backbone.Comunes.Enums
{
    public enum EstadoAutorizacion
    {
        Ok = 1,
        UsuarioExiste = 2,
        UsuarioNoExiste = 3,
        AplicacionExiste = 4,
        AplicacionNoExiste = 5,
        UsuarioSinPermisosParaLaAplicacionUOpcion = 6,
        AutenticacionNoValidaDeLaAplicacionCliente = 7,
        ErrorIndefinido = 8,
        DefinaOpcion = 9,
        DefinaAplicacion = 10,
        OpcionExiste = 11,
        OpcionNoExiste = 12
    }
}
