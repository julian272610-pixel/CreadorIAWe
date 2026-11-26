using System.Text;
using System.Text.Json;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => Results.Content("""
<!DOCTYPE html>
<html lang="es">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>CREADOR IA</title>
<style>
body {
    font-family: Arial, sans-serif;
    background: #101827;
    color: white;
    padding: 20px;
}
.contenedor {
    max-width: 700px;
    margin: 30px auto;
    background: #1e293b;
    padding: 25px;
    border-radius: 18px;
}
h1 { color: #38bdf8; text-align: center; }
p { text-align: center; color: #cbd5e1; }
label { display: block; margin: 20px 0 8px; }
select, textarea {
    width: 100%;
    box-sizing: border-box;
    padding: 13px;
    border-radius: 9px;
    background: #0f172a;
    color: white;
    border: 1px solid #475569;
    font-size: 16px;
}
textarea { min-height: 110px; }
button {
    width: 100%;
    margin-top: 18px;
    padding: 15px;
    border: 0;
    border-radius: 9px;
    background: #0284c7;
    color: white;
    font-size: 17px;
    font-weight: bold;
    cursor: pointer;
}
button:disabled { opacity: .6; }
.resultado {
    margin-top: 25px;
    padding: 18px;
    background: #0f172a;
    border-radius: 10px;
    min-height: 70px;
    white-space: pre-wrap;
    line-height: 1.6;
}
</style>
</head>
<body>
<div class="contenedor">
<h1>✨ CREADOR IA</h1>
<p>Tu estudio personal de contenido creativo</p>

<label>¿Qué deseas crear?</label>
<select id="tipo">
<option>Reflexión motivacional</option>
<option>Historia o frutinovela</option>
<option>Podcast</option>
<option>Guion viral para TikTok</option>
</select>

<label>Escribe tu tema:</label>
<textarea id="tema" placeholder="Escribe aquí tu tema..."></textarea>

<button id="boton" onclick="crear()">CREAR CONTENIDO</button>
<div class="resultado" id="resultado">Tu contenido aparecerá aquí.</div>
</div>

<script>
async function crear() {
    const tipo = document.getElementById('tipo').value;
    const tema = document.getElementById('tema').value.trim();
    const boton = document.getElementById('boton');
    const resultado = document.getElementById('resultado');

    if (!tema) {
        alert('Escribe un tema primero.');
        return;
    }

    boton.disabled = true;
    resultado.textContent = 'Gemini está creando tu contenido...';

    try {
        const respuesta = await fetch('/generar', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ tipo: tipo, tema: tema })
        });

        const datos = await respuesta.json();

        if (!respuesta.ok) {
            throw new Error(datos.detail || datos.error || 'Error del servidor');
        }

        resultado.textContent = datos.texto;
    } catch (error) {
        resultado.textContent = 'ERROR: ' + error.message;
    } finally {
        boton.disabled = false;
    }
}
</script>
</body>
</html>
""", "text/html; charset=utf-8"));

app.MapPost("/generar", async (Solicitud solicitud) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(solicitud.Tema))
        {
            return Results.BadRequest(new { error = "Escribe un tema." });
        }

        string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return Results.Problem(
                detail: "Falta configurar la clave de Gemini.",
                statusCode: 500);
        }

        string instrucciones = solicitud.Tipo switch
        {
            "Reflexión motivacional" =>
                "Escribe una reflexión motivadora de máximo 35 palabras para un video de 10 segundos. Emociona y termina invitando a seguir la página.",

            "Historia o frutinovela" =>
                "Crea una historia original en español con personajes, drama, conflicto y un final sorprendente. Usa diálogos naturales.",

            "Podcast" =>
                "Crea un guion de podcast en español con un gancho inicial, desarrollo interesante y una invitación a seguir la página. Respeta la duración que pida el usuario.",

            "Guion viral para TikTok" =>
                "Crea un guion para TikTok con un gancho poderoso, desarrollo rápido y una invitación a seguir la cuenta.",

            _ => "Crea contenido original en español."
        };

        string prompt = instrucciones +
            "\n\nTema: " + solicitud.Tema +
            "\n\nResponde solamente con el contenido solicitado en español.";

        var cuerpo = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        using var cliente = new HttpClient();

        using var peticion = new HttpRequestMessage(
            HttpMethod.Post,
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-3.1-flash-lite:generateContent");

        peticion.Headers.Add("x-goog-api-key", apiKey);

        peticion.Content = new StringContent(
            JsonSerializer.Serialize(cuerpo),
            Encoding.UTF8,
            "application/json");

        Console.WriteLine("Gemini está generando contenido...");

        using var respuesta = await cliente.SendAsync(peticion);

        string contenido = await respuesta.Content.ReadAsStringAsync();

        if (!respuesta.IsSuccessStatusCode)
        {
            Console.WriteLine("Error de Gemini: " + contenido);

            return Results.Problem(
                detail: "Error de Gemini: " + contenido,
                statusCode: 502);
        }

        using var documento = JsonDocument.Parse(contenido);

        if (!documento.RootElement.TryGetProperty("candidates", out var candidatos) ||
            candidatos.GetArrayLength() == 0)
        {
            return Results.Problem(
                detail: "Gemini no devolvió contenido. Respuesta: " + contenido,
                statusCode: 502);
        }

        string texto = candidatos[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString() ?? "Gemini no devolvió texto.";

        Console.WriteLine("Contenido generado correctamente.");

        return Results.Ok(new { texto });
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR AL GENERAR: " + ex.Message);

        return Results.Problem(
            detail: "No se pudo generar el contenido. " + ex.Message,
            statusCode: 500);
    }
});

app.Run();

public record Solicitud(string Tipo, string Tema);