namespace Presentation.Middleware;

// En una clase Helper o Extensions
public static class StringExtensionMiddleware
{
    public static string ExtraerNombreUsuario(this string correo, string dominioInstitucional = "@psm.edu.ve")
    {
        if (string.IsNullOrWhiteSpace(correo))
            return string.Empty;

        var correoLimpio = correo.Trim().ToLower();

        // Eliminar el dominio institucional si está presente
        if (correoLimpio.EndsWith(dominioInstitucional, StringComparison.OrdinalIgnoreCase))
        {
            return correoLimpio.Substring(0, correoLimpio.Length - dominioInstitucional.Length);
        }

        // Eliminar cualquier otro dominio (@gmail.com, @hotmail.com, etc.)
        if (correoLimpio.Contains('@'))
        {
            return correoLimpio.Split('@')[0];
        }

        // Si no tiene "@", devolver el valor original
        return correoLimpio;
    }

    public static string ConstruirCorreoInstitucional(this string nombreUsuario, string dominioInstitucional = "@psm.edu.ve")
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
            return string.Empty;

        return nombreUsuario.Trim().ToLower() + dominioInstitucional;
    }
}