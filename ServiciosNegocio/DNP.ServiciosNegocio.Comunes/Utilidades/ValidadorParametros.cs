namespace DNP.ServiciosNegocio.Comunes.Utilidades
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class ValidadorParametros
    {
        public static bool ValidarString(string valor)
        {
            return !(string.IsNullOrEmpty(valor) || string.IsNullOrWhiteSpace(valor));
        }
    }
}