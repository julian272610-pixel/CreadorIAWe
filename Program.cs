
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
*{box-sizing:border-box}
body{margin:0;font-family:Arial,sans-serif;background:#f3f6fc;color:#172033}
.app{display:flex;min-height:100vh}
.sidebar{width:245px;background:#101d40;color:white;padding:25px 17px;flex-shrink:0}
.logo{font-size:23px;font-weight:900;padding:8px 10px 30px}
.logo span{color:#60a5fa}
.menu-titulo{color:#91a3c5;font-size:11px;font-weight:bold;letter-spacing:1.5px;padding:15px 12px 9px}
.nav{display:block;width:100%;padding:13px 12px;margin:5px 0;border:0;border-radius:10px;background:transparent;color:#dbeafe;text-align:left;font-size:14px;cursor:pointer}
.nav:hover,.nav.active{background:#263d70;color:white}
.sidebar-info{margin-top:35px;padding:15px;background:#1c315d;border-radius:13px;color:#dbeafe;font-size:12px;line-height:1.6}
.main{flex:1;min-width:0;padding:28px 35px 50px}
.topbar{display:flex;justify-content:space-between;align-items:center;margin-bottom:25px}
.topbar small{color:#64748b}
.menu-mobile{display:none;border:0;padding:10px 13px;border-radius:9px;background:#dbeafe;cursor:pointer}
.hero{padding:38px;border-radius:22px;color:white;background:linear-gradient(120deg,#1d4ed8,#6d28d9);box-shadow:0 12px 30px #3730a326}
.etiqueta{display:inline-block;background:#ffffff25;padding:7px 12px;border-radius:20px;font-size:12px}
.hero h1{font-size:clamp(27px,4vw,39px);margin:17px 0 10px}
.hero p{color:#e0e7ff;line-height:1.6;max-width:650px}
.seccion-titulo{display:flex;justify-content:space-between;align-items:center;margin:32px 0 16px}
.seccion-titulo h2{font-size:21px;margin:0}
.seccion-titulo p{color:#64748b;font-size:13px;margin:6px 0 0}
.tarjetas{display:grid;grid-template-columns:repeat(3,minmax(0,1fr));gap:16px}
.tarjeta{background:white;padding:21px;border-radius:16px;border:1px solid #e5eaf3;cursor:pointer;transition:.2s;text-align:left;color:#172033}
.tarjeta:hover{transform:translateY(-4px);box-shadow:0 10px 24px #17255412}
.icono{display:grid;place-items:center;width:48px;height:48px;border-radius:13px;font-size:24px;margin-bottom:15px}
.azul{background:#dbeafe}.morado{background:#ede9fe}.rosa{background:#fce7f3}.naranja{background:#ffedd5}.verde{background:#dcfce7}.amarillo{background:#fef3c7}
.tarjeta h3{font-size:16px;margin:0 0 8px}
.tarjeta p{color:#64748b;font-size:13px;line-height:1.5;margin:0}
.creador{margin-top:28px;background:white;border:1px solid #e5eaf3;border-radius:18px;padding:25px}
.creador h2{margin:0 0 8px}
.creador>p{color:#64748b;font-size:13px}
.campo-label{display:block;font-size:14px;font-weight:bold;margin:20px 0 9px}
select,textarea{width:100%;padding:14px;border:1px solid #d6deeb;border-radius:11px;background:#f8fafc;font-size:15px;color:#172033;outline:none}
select:focus,textarea:focus{border-color:#3b82f6}
textarea{min-height:115px;resize:vertical}
.generar{width:100%;margin-top:17px;padding:16px;border:0;border-radius:11px;color:white;font-size:16px;font-weight:bold;background:linear-gradient(100deg,#2563eb,#7c3aed);cursor:pointer}
.generar:disabled{opacity:.6;cursor:wait}
.resultado{display:none;margin-top:20px;padding:20px;border-radius:13px;background:#f1f5ff;border:1px solid #dbeafe;line-height:1.7;white-space:pre-wrap;overflow-wrap:anywhere}
.resultado button{display:block;margin-top:15px;padding:10px 15px;border:0;border-radius:8px;background:#dbeafe;color:#1d4ed8;font-weight:bold;cursor:pointer}
.banner{margin-top:25px;padding:23px;border-radius:16px;background:linear-gradient(100deg,#dbeafe,#ede9fe)}
.banner h3{margin:0 0 8px;color:#312e81}
.banner p{margin:0;color:#475569;line-height:1.5}
.biblioteca{margin-top:30px;background:white;padding:23px;border:1px solid #e5eaf3;border-radius:16px}
.biblioteca h2{margin-top:0;font-size:20px}
.item-guardado{padding:13px 0;border-bottom:1px solid #e5eaf3;font-size:14px;line-height:1.5;overflow-wrap:anywhere}
.item-guardado button{margin:8px 7px 0 0;padding:7px 10px;border:0;border-radius:7px;background:#e0e7ff;color:#3730a3;cursor:pointer}
.vacio{color:#64748b;font-size:13px}
@media(max-width:1000px){.tarjetas{grid-template-columns:repeat(2,minmax(0,1fr))}}
@media(max-width:700px){
.app{display:block}.sidebar{display:none;width:100%;padding:15px}
.sidebar.abierto{display:block}.main{padding:18px 14px 35px}
.menu-mobile{display:block}.hero{padding:25px 20px}
.tarjetas{grid-template-columns:1fr 1fr;gap:10px}
.tarjeta{padding:15px}.creador{padding:18px}
}
@media(max-width:390px){.tarjetas{grid-template-columns:1fr}}
</style>
</head>
<body>
<div class="app">
<aside class="sidebar" id="sidebar">
<div class="logo">👑 CREADOR <span>IA</span></div>
<div class="menu-titulo">MENÚ PRINCIPAL</div>
<button class="nav active" onclick="irAInicio()">🏠 Inicio</button>
<button class="nav" onclick="elegir('Reflexión motivacional')">💡 Reflexiones</button>
<button class="nav" onclick="elegir('Historia o frutinovela')">🎭 Historias y frutinovelas</button>
<button class="nav" onclick="elegir('Podcast')">🎙️ Podcasts</button>
<button class="nav" onclick="elegir('Guion viral para TikTok')">📱 Guiones para TikTok</button>
<button class="nav" onclick="elegir('Prompt para video')">🎬 Prompts de video</button>
<button class="nav" onclick="elegir('Prompt para imagen')">🎨 Prompts de imágenes</button>
<button class="nav" onclick="irABiblioteca()">📚 Biblioteca</button>
<div class="sidebar-info">✨ Crea contenido original con inteligencia artificial y desarrolla tus ideas.</div>
</aside>
<main class="main">
<div class="topbar">
<button class="menu-mobile" onclick="document.getElementById('sidebar').classList.toggle('abierto')">☰ Menú</button>
<div><strong>Mi espacio creativo</strong><br><small>Tu estudio de creación con IA</small></div>
<div>✨ <strong>IA</strong></div>
</div>
<section class="hero" id="inicio">
<span class="etiqueta">✨ TU CREATIVIDAD, CON INTELIGENCIA ARTIFICIAL</span>
<h1>¡Bienvenido a CREADOR IA!</h1>
<p>Convierte tus ideas en reflexiones, historias, podcasts y contenido para tus redes sociales. ¡Empieza a crear hoy!</p>
</section>
<section>
<div class="seccion-titulo"><div><h2>¿Qué quieres crear hoy?</h2><p>Elige una herramienta para comenzar.</p></div></div>
<div class="tarjetas">
<button class="tarjeta" onclick="elegir('Reflexión motivacional')"><div class="icono azul">💡</div><h3>Reflexiones</h3><p>Mensajes de motivación, superación y fe.</p></button>
<button class="tarjeta" onclick="elegir('Historia o frutinovela')"><div class="icono rosa">🎭</div><h3>Historias y frutinovelas</h3><p>Historias con drama, emoción y giros sorprendentes.</p></button>
<button class="tarjeta" onclick="elegir('Podcast')"><div class="icono morado">🎙️</div><h3>Podcast</h3><p>Guiones para hablar y conectar con tu audiencia.</p></button>
<button class="tarjeta" onclick="elegir('Guion viral para TikTok')"><div class="icono naranja">📱</div><h3>Guiones para TikTok</h3><p>Ganchos y guiones para videos que atrapen.</p></button>
<button class="tarjeta" onclick="elegir('Prompt para video')"><div class="icono verde">🎬</div><h3>Prompts de video</h3><p>Ideas y escenas para generar videos con IA.</p></button>
<button class="tarjeta" onclick="elegir('Prompt para imagen')"><div class="icono amarillo">🎨</div><h3>Prompts de imágenes</h3><p>Descripciones para crear imágenes originales.</p></button>
</div>
</section>
<section class="creador" id="zona-creador">
<h2>✨ Crear contenido</h2>
<p>Selecciona el tipo de contenido y escribe tu idea.</p>
<label class="campo-label" for="tipo">¿Qué deseas crear?</label>
<select id="tipo">
<option>Reflexión motivacional</option>
<option>Historia o frutinovela</option>
<option>Podcast</option>
<option>Guion viral para TikTok</option>
<option>Prompt para video</option>
<option>Prompt para imagen</option>
</select>
<label class="campo-label" for="tema">Escribe tu tema o idea:</label>
<textarea id="tema" placeholder="Ejemplo: Nunca te rindas, Dios tiene algo grande preparado para ti..."></textarea>
<button class="generar" id="boton" onclick="crear()">✨ GENERAR CONTENIDO</button>
<div class="resultado" id="resultado"></div>
</section>
<section class="banner">
<h3>🌟 Cada gran proyecto comienza con una idea.</h3>
<p>No dejes de crear. Comparte tus mensajes, inspira a otras personas y sigue trabajando por tus sueños.</p>
</section>
<section class="biblioteca" id="biblioteca">
<h2>📚 Mis creaciones recientes</h2>
<div id="lista-creaciones"><p class="vacio">Tus contenidos generados aparecerán aquí.</p></div>
</section>
</main>
</div>
<script>
function elegir(tipo){
document.getElementById('tipo').value=tipo;
document.getElementById('zona-creador').scrollIntoView({behavior:'smooth'});
document.getElementById('sidebar').classList.remove('abierto');
}
function irAInicio(){
document.getElementById('inicio').scrollIntoView({behavior:'smooth'});
document.getElementById('sidebar').classList.remove('abierto');
}
function irABiblioteca(){
document.getElementById('biblioteca').scrollIntoView({behavior:'smooth'});
document.getElementById('sidebar').classList.remove('abierto');
}
function cargarBiblioteca(){
const lista=document.getElementById('lista-creaciones');
let guardados=[];
try{guardados=JSON.parse(localStorage.getItem('creadorIA')||'[]')}catch{guardados=[]}
if(!guardados.length){lista.innerHTML='<p class="vacio">Todavía no tienes creaciones guardadas.</p>';return}
lista.innerHTML='';
guardados.forEach((item,indice)=>{
const div=document.createElement('div');div.className='item-guardado';
const titulo=document.createElement('strong');titulo.textContent=item.tipo;
const contenido=document.createElement('div');contenido.textContent=item.texto;
const copiar=document.createElement('button');copiar.textContent='📋 Copiar';copiar.onclick=()=>copiarTexto(item.texto);
const borrar=document.createElement('button');borrar.textContent='Eliminar';borrar.onclick=()=>eliminarCreacion(indice);
div.append(titulo,contenido,copiar,borrar);lista.appendChild(div);
});
}
function guardarCreacion(tipo,texto){
let guardados=[];
try{guardados=JSON.parse(localStorage.getItem('creadorIA')||'[]')}catch{guardados=[]}
guardados.unshift({tipo,texto});guardados=guardados.slice(0,15);
localStorage.setItem('creadorIA',JSON.stringify(guardados));cargarBiblioteca();
}
function eliminarCreacion(indice){
let guardados=JSON.parse(localStorage.getItem('creadorIA')||'[]');
guardados.splice(indice,1);localStorage.setItem('creadorIA',JSON.stringify(guardados));cargarBiblioteca();
}
async function copiarTexto(texto){
try{await navigator.clipboard.writeText(texto);alert('Texto copiado.')}
catch{alert('No se pudo copiar automáticamente. Selecciona el texto y cópialo.')}
}
async function crear(){
const tipo=document.getElementById('tipo').value;
const tema=document.getElementById('tema').value.trim();
const boton=document.getElementById('boton');
const resultado=document.getElementById('resultado');
if(!tema){alert('Escribe un tema primero.');return}
boton.disabled=true;boton.textContent='⏳ CREANDO CONTENIDO...';
resultado.style.display='block';
resultado.textContent='La inteligencia artificial está creando tu contenido...';
try{
const respuesta=await fetch('/generar',{
method:'POST',headers:{'Content-Type':'application/json'},
body:JSON.stringify({tipo:tipo,tema:tema})
});
const datos=await respuesta.json();
if(!respuesta.ok)throw new Error(datos.detail||datos.error||'Error del servidor');
resultado.textContent=datos.texto;
const copiar=document.createElement('button');
copiar.textContent='📋 COPIAR CONTENIDO';
copiar.onclick=()=>copiarTexto(datos.texto);
resultado.appendChild(copiar);
guardarCreacion(tipo,datos.texto);
}catch(error){resultado.textContent='ERROR: '+error.message}
finally{boton.disabled=false;boton.textContent='✨ GENERAR CONTENIDO'}
}
cargarBiblioteca();
</script>
</body>
</html>
""", "text/html; charset=utf-8"));

app.MapPost("/generar", async (Solicitud solicitud) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(solicitud.Tema))
            return Results.BadRequest(new { error = "Escribe un tema." });

        string? apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        if (string.IsNullOrWhiteSpace(apiKey))
            return Results.Problem(detail: "Falta configurar la clave de Gemini.", statusCode: 500);

        string instrucciones = solicitud.Tipo switch
        {
            "Reflexión motivacional" =>
                "Escribe una reflexión motivadora en español, emotiva y fácil de entender. Si el usuario pide una duración, respétala. Para 10 segundos usa máximo 35 palabras. Termina invitando a seguir la página cuando corresponda.",

            "Historia o frutinovela" =>
                "Crea una historia original en español con personajes, drama, conflicto, diálogos naturales y un final sorprendente. Divide en escenas si el usuario lo solicita.",

            "Podcast" =>
                "Crea un guion de podcast en español con un gancho inicial, desarrollo interesante y cierre que invite a seguir la página. Respeta la duración que pida el usuario.",

            "Guion viral para TikTok" =>
                "Crea un guion para TikTok en español con un gancho poderoso en los primeros segundos, desarrollo rápido y un cierre que invite a seguir la cuenta.",

            "Prompt para video" =>
                "Crea un prompt detallado para generar un video con inteligencia artificial. Incluye descripción visual, personajes, acciones, ambiente, movimientos de cámara, iluminación, duración y formato vertical 9:16 cuando sea apropiado. Incluye audio y diálogos si el usuario los solicita.",

            "Prompt para imagen" =>
                "Crea un prompt detallado para generar una imagen con inteligencia artificial. Describe personajes, escenario, composición, iluminación, colores, estilo visual y formato. Si el usuario pide texto dentro de la imagen, incluye las palabras exactas.",

            _ => "Crea contenido original en español, claro, atractivo y útil."
        };

        string prompt = instrucciones +
            "\n\nTema del usuario: " + solicitud.Tema +
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
            JsonSerializer.Serialize(cuerpo), Encoding.UTF8, "application/json");

        Console.WriteLine("Gemini está generando contenido...");
        using var respuesta = await cliente.SendAsync(peticion);
        string contenido = await respuesta.Content.ReadAsStringAsync();

        if (!respuesta.IsSuccessStatusCode)
        {
            Console.WriteLine("Error de Gemini: " + contenido);
            return Results.Problem(detail: "Error de Gemini: " + contenido, statusCode: 502);
        }

        using var documento = JsonDocument.Parse(contenido);

        if (!documento.RootElement.TryGetProperty("candidates", out var candidatos) ||
            candidatos.GetArrayLength() == 0)
            return Results.Problem(detail: "Gemini no devolvió contenido. Respuesta: " + contenido, statusCode: 502);

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
        return Results.Problem(detail: "No se pudo generar el contenido. " + ex.Message, statusCode: 500);
    }
});

app.Run();

public record Solicitud(string Tipo, string Tema);
